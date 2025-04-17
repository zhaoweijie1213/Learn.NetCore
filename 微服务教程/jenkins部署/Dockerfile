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