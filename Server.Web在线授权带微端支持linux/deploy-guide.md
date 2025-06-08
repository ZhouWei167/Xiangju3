# Server.Web Ubuntu 20.04部署指南

本指南将帮助您在Ubuntu 20.04服务器上部署Server.Web应用程序，所有操作均使用国内镜像源以提高下载速度。

## 前提条件

1. 一台运行Ubuntu 20.04的服务器
2. 具有sudo权限的用户账号
3. 服务器开放了5000端口

## 部署步骤

### 1. 安装Docker和Docker Compose

```bash
# 更新apt包索引
sudo apt-get update

# 安装必要的依赖
sudo apt-get install -y apt-transport-https ca-certificates curl gnupg-agent software-properties-common

# 添加阿里云Docker的GPG密钥
curl -fsSL https://mirrors.aliyun.com/docker-ce/linux/ubuntu/gpg | sudo apt-key add -

# 添加阿里云Docker软件源
sudo add-apt-repository "deb [arch=amd64] https://mirrors.aliyun.com/docker-ce/linux/ubuntu $(lsb_release -cs) stable"

# 更新apt包索引
sudo apt-get update

# 安装Docker
sudo apt-get install -y docker-ce docker-ce-cli containerd.io

# 配置Docker国内镜像加速
sudo mkdir -p /etc/docker
sudo tee /etc/docker/daemon.json <<-'EOF'
{
  "registry-mirrors": [
    "https://registry.docker-cn.com",
    "https://hub-mirror.c.163.com",
    "https://mirror.baidubce.com"
  ]
}
EOF

# 重启Docker服务
sudo systemctl daemon-reload
sudo systemctl restart docker

# 安装Docker Compose (方法1 - 使用apt安装)
sudo apt update
sudo apt install -y docker-compose

# 如果上面的方法不可用，可以尝试以下方法安装Docker Compose (方法2)
# sudo curl -L "https://get.daocloud.io/docker/compose/releases/download/v2.18.1/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
# sudo chmod +x /usr/local/bin/docker-compose
# sudo ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose

# 启动Docker服务并设置开机自启
sudo systemctl enable docker
sudo systemctl start docker

# 将当前用户添加到docker组（避免每次都需要sudo）
sudo usermod -aG docker $USER
# 注意：添加用户到组后需要重新登录才能生效
```

### 2. 准备应用程序

1. 将整个项目代码复制到服务器上

```bash
# 使用scp从本地复制
scp -r /path/to/your/project user@your-server:/opt/server-web

# 或者使用git克隆（如果使用国内Gitee等平台）
git clone your-repository-url /opt/server-web
```

2. 进入项目目录

```bash
cd /opt/server-web
```

### 3. 创建或修改docker-compose.yml

如果项目中已有docker-compose.yml文件，请确认其中的版本设置与您安装的Docker Compose版本兼容。

检查您的Docker Compose版本：

```bash
docker-compose --version
```

根据您的Docker Compose版本，使用兼容的配置：

#### 对于Docker Compose 1.x版本（如1.25.0）：

```yaml
version: '2.2'
services:
  server-web:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    restart: always
    volumes:
      - ./logs:/app/logs
```

#### 对于Docker Compose 2.x或更高版本：

```yaml
version: '3.3'
services:
  server-web:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    restart: always
    volumes:
      - ./logs:/app/logs
```

如果您遇到版本兼容性错误，请将`version`值降低到您的Docker Compose支持的版本，常用的兼容版本有：`'1'`, `'2'`, `'2.2'`, `'3'`, `'3.3'`。

### 4. 使用Docker Compose部署

1. 进入Server.Web目录

```bash
cd Server.Web
```

2. 验证Docker Compose是否安装成功

```bash
docker-compose --version
```

如果显示版本信息，则表示安装成功。如果提示命令未找到，请使用上面的方法2安装Docker Compose。

3. 构建并启动容器

```bash
docker-compose up -d --build
```

这将构建Docker镜像并在后台启动容器。

4. 检查容器是否正常运行

```bash
docker-compose ps
```

如果看到`server-web`容器的状态为`Up`，则表示部署成功。

### 5. 访问应用程序

现在，您可以通过以下URL访问应用程序：

```
http://your-server-ip:5000
```

### 6. 管理应用程序

- 查看应用程序日志：

```bash
docker-compose logs -f
```

- 停止服务：

```bash
docker-compose down
```

- 重启服务：

```bash
docker-compose up -d
```

### 7. 更新应用程序

当需要更新应用程序时，请按照以下步骤操作：

1. 拉取最新代码（如果使用Git管理）

```bash
git pull
```

2. 重新构建并启动容器

```bash
docker-compose up -d --build
```

## 故障排除

### 1. 容器无法启动

检查Docker日志：

```bash
docker-compose logs
```

### 2. 无法访问应用程序

- 确认容器正在运行：

```bash
docker ps
```

- 检查Ubuntu防火墙设置，确保5000端口已开放：

```bash
sudo ufw status
sudo ufw allow 5000/tcp
sudo ufw enable
```

- 如果使用云服务器，请检查安全组设置，确保已开放5000端口

### 3. Docker Compose命令未找到

如果遇到`docker-compose: command not found`错误，请尝试以下解决方案：

```bash
# 方法1：使用apt安装
sudo apt update
sudo apt install -y docker-compose

# 方法2：手动安装最新版本
sudo curl -L "https://get.daocloud.io/docker/compose/releases/download/v2.18.1/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
sudo ln -s /usr/local/bin/docker-compose /usr/bin/docker-compose
```

### 4. Docker Compose版本兼容性错误

如果遇到以下错误：

```
ERROR: Version in "./docker-compose.yml" is unsupported.
```

请修改docker-compose.yml文件中的版本号，使用与您的Docker Compose兼容的版本：

```bash
# 查看当前Docker Compose版本
docker-compose --version

# 编辑docker-compose.yml文件
nano docker-compose.yml

# 将version字段修改为兼容的版本，如：
# version: '2.2'  # 适用于Docker Compose 1.x
# 或
# version: '1'    # 适用于更旧的版本
```

Docker Compose版本与文件格式版本对应关系：
- Docker Compose 1.x 支持文件格式版本 '1', '2', '2.1', '2.2'
- Docker Compose 2.x 支持文件格式版本 '2', '2.x', '3', '3.x'
- Docker Compose 3.x 支持文件格式版本 '3', '3.x'

### 5. 找不到Dockerfile错误

如果遇到以下错误：

```
ERROR: Cannot locate specified Dockerfile: Server.Web/Dockerfile
```

这是因为docker-compose.yml文件中指定的Dockerfile路径不正确。请检查项目结构和Dockerfile位置，然后修改docker-compose.yml文件：

```bash
# 首先查看当前目录结构
ls -la

# 查找Dockerfile的位置
find . -name "Dockerfile"

# 编辑docker-compose.yml文件
nano docker-compose.yml
```

根据您的项目结构，修改Dockerfile路径。常见的修复方法有：

1. 如果Dockerfile在当前目录：

```yaml
version: '2.2'  # 适用于Docker Compose 1.25.0
services:
  server-web:
    build:
      context: .
      dockerfile: Dockerfile  # 修改为正确的路径
    # 其他配置保持不变
```

2. 如果Dockerfile在上级目录的Server.Web文件夹中，但您当前已经在Server.Web目录中：

```yaml
version: '2.2'  # 适用于Docker Compose 1.25.0
services:
  server-web:
    build:
      context: .  # 修改为当前目录
      dockerfile: Dockerfile  # 修改为正确的路径
    # 其他配置保持不变
```

3. 确保context路径和dockerfile路径的组合能够正确指向Dockerfile文件

### 6. 构建时找不到项目文件

如果在构建过程中遇到类似以下错误：

```
COPY failed: file not found in build context or excluded by .dockerignore: stat Server.Web/在线授权系统.csproj: file does not exist
```

这表明Dockerfile中引用的文件路径不正确，或者文件确实不存在。请按照以下步骤解决：

1. 检查项目结构，确认项目文件的实际位置：

```bash
# 查看当前目录结构
ls -la

# 如果您在server-web目录中，查看上级目录
ls -la ..

# 查找.csproj文件
find .. -name "*.csproj"
```

2. 检查Dockerfile内容，了解它期望的文件结构：

```bash
# 查看Dockerfile内容
cat Dockerfile  # 如果在当前目录
# 或
cat ../Server.Web/Dockerfile  # 如果在上级目录
```

3. 修改Dockerfile或调整目录结构：

方法一：修改Dockerfile中的COPY指令，使其指向正确的文件路径：

```bash
# 编辑Dockerfile
nano Dockerfile
```

将类似这样的行：
```dockerfile
COPY ["Server.Web/在线授权系统.csproj", "Server.Web/"]
```

修改为正确的路径，例如：
```dockerfile
COPY ["在线授权系统.csproj", "Server.Web/"]  # 如果文件在当前目录
# 或
COPY ["../在线授权系统.csproj", "Server.Web/"]  # 根据实际情况调整
```

方法二：创建正确的目录结构，确保文件位于Dockerfile期望的位置：

```bash
# 例如，如果需要在上级目录创建Server.Web目录并复制项目文件
mkdir -p ../Server.Web
cp 在线授权系统.csproj ../Server.Web/
```

4. 修改docker-compose.yml文件中的context路径，确保它指向包含所有必要文件的正确目录：

```bash
nano docker-compose.yml
```

确保build.context指向正确的目录：
```yaml
build:
  context: ..  # 如果需要包含上级目录的文件
  # 或
  context: .   # 如果所有文件都在当前目录及其子目录中
```

5. 重新构建容器：

```bash
docker-compose up -d --build
```

### 7. 镜像下载速度慢

如果仍然遇到镜像下载速度慢的问题，可以尝试其他国内镜像源：

```bash
sudo tee /etc/docker/daemon.json <<-'EOF'
{
  "registry-mirrors": [
    "https://docker.mirrors.ustc.edu.cn",
    "https://dockerhub.azk8s.cn",
    "https://reg-mirror.qiniu.com"
  ]
}
EOF

sudo systemctl daemon-reload
sudo systemctl restart docker
```

## 注意事项

1. 在生产环境中，建议配置Nginx反向代理和HTTPS
2. 定期备份数据库和应用程序数据
3. 设置服务器自动安全更新

如有任何问题，请联系技术支持团队。 

cd /opt/server-web
docker-compose down
docker-compose up -d