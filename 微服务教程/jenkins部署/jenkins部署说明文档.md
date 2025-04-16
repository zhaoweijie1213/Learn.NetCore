# 🐳 Jenkins 容器部署手册（docker run 方式）

适用于快速启动 Jenkins 并手动在容器中安装 Docker CLI 和证书支持。

------

## 📦 1. 使用 `docker run` 启动 Jenkins

```bash
docker run -u root -d \
  --privileged \
  -p 8080:8080 \
  -p 50000:50000 \
  --restart=always \
  -v /data/jenkins_home:/var/jenkins_home \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v /etc/ssl/certs:/usr/local/share/ca-certificates:ro \
  --name jenkins \
  jenkins/jenkins:lts-jdk17
```

### 参数说明：

| 参数                                                    | 说明                                              |
| ------------------------------------------------------- | ------------------------------------------------- |
| `-u root`                                               | 以 root 用户运行 Jenkins，便于安装包与管理 Docker |
| `--privileged`                                          | 给予容器完全权限，支持 Docker 构建                |
| `-v /var/run/docker.sock:/var/run/docker.sock`          | 宿主机 Docker 控制权限挂载                        |
| `-v /etc/ssl/certs:/usr/local/share/ca-certificates:ro` | 挂载根证书                                        |
| `--restart=always`                                      | 开机自启，异常自动重启                            |

------

## 🧭 2. 进入 Jenkins 容器

```bash
docker exec -it -u root jenkins bash
```

------

## 🐳 3. 安装 Docker CLI（容器内部）

> 注意：这种方式使用的是 Debian 官方源中的 `docker.io`，依赖较多，有时可能失败。

```bash
apt-get update
apt-get install -y docker.io
```

------

## 🔐 4. 更新根证书（容器内部）

如果你挂载了公司私有 CA 或 HTTPS 证书：

```bash
update-ca-certificates
```

------

## ✅ 5. 验证

在容器内执行以下命令，确认是否可用：

```bash
docker version
docker ps
```

如果能看到宿主机的容器列表，则配置成功。

------

## 🚨 注意事项

| 问题                             | 描述                                                         |
| -------------------------------- | ------------------------------------------------------------ |
| `apt install docker.io` 安装失败 | 原因可能是依赖冲突、基础镜像太精简                           |
| 没有 buildx 命令                 | `docker.io` 默认不带 `buildx`，建议改用 `docker-ce-cli` 或挂载宿主 CLI 插件 |
| 无法识别证书                     | 需确保证书路径正确并执行 `update-ca-certificates`            |

------

## 💡 建议升级方案

虽然这种方式部署快速，但**不适合长期维护和 CI 扩展**，推荐升级为以下方式：

- 使用 `docker-compose.yml` 管理多个挂载项和服务
- 使用 `Dockerfile` 安装 `docker-ce-cli`，支持 `buildx`
- 镜像统一管理，可复用和发布

👉 参考文档：[基于 Docker Compose 搭建支持 Docker CLI 的 Jenkins 环境](https://chatgpt.com/g/g-p-6768c3e3490c819181643bc3dab167fc-net/c/67ff5395-5548-8012-ab11-c21b2bd0e046#)

# 🚀 基于 Docker Compose 搭建支持 Docker CLI 的 Jenkins 环境

本指南提供一整套流程，帮助你部署 Jenkins，并支持：

- 容器内直接运行 `docker buildx build`、`docker push` 等命令
- 自动安装 `docker-ce-cli`，避免安装失败
- 自动更新根证书
- 支持构建和推送镜像的 CI 流水线任务

------

## 📁 目录结构

```text
jenkins/
├── docker-compose.yml
├── Dockerfile.jenkins
└── README.md（本文件）
```

------

## 🧱 1. Dockerfile.jenkins

用于构建 Jenkins 镜像，自动安装 `docker-ce-cli`、更新证书：

```Dockerfile
# Dockerfile.jenkins
FROM jenkins/jenkins:latest

USER root

# 安装依赖及 Docker 官方源
RUN apt-get update && apt-get install -y \
    ca-certificates \
    curl \
    gnupg \
    lsb-release \
    software-properties-common

# 添加 Docker GPG key
RUN mkdir -p /etc/apt/keyrings && \
    curl -fsSL https://download.docker.com/linux/debian/gpg | \
    gpg --dearmor -o /etc/apt/keyrings/docker.gpg

# 添加 Docker 官方 APT 源
RUN echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] \
  https://download.docker.com/linux/debian $(lsb_release -cs) stable" \
  > /etc/apt/sources.list.d/docker.list

# 安装 Docker CLI（不安装 daemon）
RUN apt-get update && apt-get install -y docker-ce-cli

# 更新系统证书
RUN update-ca-certificates

# 切换回 jenkins 用户
USER jenkins
```

------

## ⚙️ 2. docker-compose.yml

用于启动 Jenkins 服务容器，具备构建权限与 Docker socket 共享：

```yaml
version: '3.8'
services:
  jenkins:
    container_name: jenkins
    build:
      context: .
      dockerfile: Dockerfile.jenkins
    user: 0:0  # 使用 root 用户启动 Jenkins（可选，便于权限）
    privileged: true
    environment:
      - TZ=Asia/Shanghai
    volumes:
      - /data/jenkins_home:/var/jenkins_home
      - /etc/ssl/certs:/usr/local/share/ca-certificates
      - /var/run/docker.sock:/var/run/docker.sock
      - /usr/bin/docker:/usr/bin/docker
      - /usr/libexec/docker/cli-plugins:/usr/libexec/docker/cli-plugins
    network_mode: host
    restart: always
```

------

## 🛠️ 3. 启动流程

### 第一次构建镜像：

```bash
docker compose build
```

### 启动 Jenkins：

```bash
docker compose up -d
```

------

## ✅ 4. 验证 buildx 和 Docker CLI 是否正常

进入 Jenkins 容器内部：

```bash
docker exec -it -u root jenkins bash
```

验证命令：

```bash
docker --version
docker buildx version
docker buildx ls
```

如果都正常输出，说明环境已配置完成 🎉

------

## 📦 5. 可选 Jenkinsfile 示例片段（构建镜像）

```groovy
stage('构建 Docker 镜像') {
    steps {
        sh './jenkins/scripts/docker-image-build.sh'
    }
}
```

示例 `docker-image-build.sh`（分支判断用 `--load` 或 `--push`）：

```bash
#!/bin/bash
set -e

BRANCH_NAME=$(echo "${GIT_BRANCH}" | sed 's|origin/||')
echo "[INFO] 当前分支：$BRANCH_NAME"

if [[ "$BRANCH_NAME" == "test" ]]; then
    docker buildx build --load -t ${SERVICE_NAME}:v${BUILD_NUMBER} -f GameDataStatistics.Host/Dockerfile .
elif [[ "$BRANCH_NAME" == "develop" || "$BRANCH_NAME" == "master" ]]; then
    docker buildx build --push -t your-registry/${SERVICE_NAME}:v${BUILD_NUMBER} -f GameDataStatistics.Host/Dockerfile .
else
    docker buildx build --load -t ${SERVICE_NAME}:v${BUILD_NUMBER} -f GameDataStatistics.Host/Dockerfile .
fi
```

------

## 🔚 总结

| 功能             | 说明                              |
| ---------------- | --------------------------------- |
| Docker CLI       | 使用 `docker-ce-cli` 安装         |
| Buildx 支持      | 通过宿主挂载插件目录              |
| 根证书更新       | `update-ca-certificates` 自动完成 |
| Compose 一键部署 | 支持 `buildx build --load/--push` |