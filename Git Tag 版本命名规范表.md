**企业级 Git Tag 版本命名规范表**，适用于多分支（test / develop / master）、多环境（内网 / 镜像测试 / 正式发布）场景，特别为 CI/CD 构建与镜像打包而设计。

------

# 📘 Git Tag 版本命名规范表（标准化 CI/CD 版本控制）

| 环境类型       | 所属分支    | 示例 Tag      | 用途说明                            | 镜像推送建议地址                                 |
| -------------- | ----------- | ------------- | ----------------------------------- | ------------------------------------------------ |
| ✅ 内网开发环境 | `test`      | `v1.2.3-test` | 内网环境调试、联调、日常开发        | `your-registry/test/your-service:v1.2.3-test`    |
| 🧪 镜像测试环境 | `develop`   | `v1.2.3-beta` | 功能基本完成，进入镜像测试          | `your-registry/dev/your-service:v1.2.3-beta`     |
| 🚥 灰度预发布   | `release/*` | `v1.2.3-rc.1` | Release Candidate，用于预发布、灰度 | `your-registry/release/your-service:v1.2.3-rc.1` |
| 🚀 正式生产环境 | `master`    | `v1.2.3`      | 稳定版，正式发布版本                | `your-registry/prod/your-service:v1.2.3`         |

------

## ✅ 命名规则说明

- 所有版本以 `v` 开头，遵循语义化版本号格式：`v<主>.<次>.<补丁>`
- 各分支分别加后缀标识环境：
  - `-test`：test 内网开发环境
  - `-beta`：develop 镜像环境
  - `-rc.N`：release 候选发布版本
  - 无后缀：master 正式发布版本

------

## 🧠 Git Tag 命名建议

| 类型         | 示例          | 说明                    |
| ------------ | ------------- | ----------------------- |
| 开发调试 tag | `v1.0.0-test` | 仅供 test 分支使用      |
| 镜像测试 tag | `v1.0.0-beta` | 供 develop 分支镜像测试 |
| 候选发布 tag | `v1.0.0-rc.1` | 用于 release 分支       |
| 正式版本 tag | `v1.0.0`      | 仅在 master 分支创建    |

------

## 🛠️ Jenkins `gitParameter` 推荐配置

| 分支      | `branchFilter`   | `tagFilter`            |
| --------- | ---------------- | ---------------------- |
| `test`    | `origin/test`    | `*-test`               |
| `develop` | `origin/develop` | `*-beta`               |
| `master`  | `origin/master`  | `v[0-9]*.[0-9]*.[0-9]` |

------

## 🧱 镜像命名规范建议

镜像 tag 应直接与 Git tag 对应，例如：

```
镜像名：game-data-statistics:v1.2.3-beta
镜像仓库地址：queyouquan.tencentcloudcr.com/dev/game-data-statistics:v1.2.3-beta
```

------

## 📦 总结命名模板

```
# 内网环境 (test)
v1.2.3-test

# 镜像测试环境 (develop)
v1.2.3-beta

# 灰度发布 (release)
v1.2.3-rc.1

# 正式上线 (master)
v1.2.3
```

