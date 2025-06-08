#!/bin/bash

# 确保脚本在出错时停止执行
set -e

# 显示彩色输出
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# 打印带颜色的信息
info() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

warn() {
    echo -e "${YELLOW}[WARN]${NC} $1"
}

error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# 检查是否安装了.NET SDK
if ! command -v dotnet &> /dev/null; then
    error ".NET SDK未安装，请先安装.NET SDK 7.0或更高版本"
    exit 1
fi

# 获取脚本所在目录的绝对路径
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PROJECT_DIR="$SCRIPT_DIR"
PUBLISH_DIR="$PROJECT_DIR/publish"

# 显示部署信息
info "开始部署Server.Web应用程序"
info "项目目录: $PROJECT_DIR"
info "发布目录: $PUBLISH_DIR"

# 清理旧的发布文件
if [ -d "$PUBLISH_DIR" ]; then
    info "清理旧的发布文件..."
    rm -rf "$PUBLISH_DIR"
fi

# 创建发布目录
mkdir -p "$PUBLISH_DIR"

# 发布应用程序
info "正在发布应用程序..."
cd "$PROJECT_DIR"
dotnet publish -c Release -o "$PUBLISH_DIR" --self-contained false

# 检查发布是否成功
if [ $? -ne 0 ]; then
    error "应用程序发布失败"
    exit 1
fi

info "应用程序发布成功"

# 创建systemd服务文件
SERVICE_FILE="/etc/systemd/system/server-web.service"
info "创建systemd服务文件: $SERVICE_FILE"

# 需要root权限
if [ "$EUID" -ne 0 ]; then
    warn "需要root权限来创建systemd服务文件"
    warn "请手动创建服务文件或以root身份运行此脚本"
    warn "服务文件内容如下:"
    echo "
[Unit]
Description=Server.Web ASP.NET Core Application
After=network.target

[Service]
WorkingDirectory=$PUBLISH_DIR
ExecStart=/usr/bin/dotnet $PUBLISH_DIR/在线授权系统.dll
Restart=always
# 重启服务前等待10秒
RestartSec=10
# 使用专门的用户运行服务
User=www-data
# 设置环境变量
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://*:5000

[Install]
WantedBy=multi-user.target
"
else
    # 创建服务文件
    cat > "$SERVICE_FILE" << EOF
[Unit]
Description=Server.Web ASP.NET Core Application
After=network.target

[Service]
WorkingDirectory=$PUBLISH_DIR
ExecStart=/usr/bin/dotnet $PUBLISH_DIR/在线授权系统.dll
Restart=always
# 重启服务前等待10秒
RestartSec=10
# 使用专门的用户运行服务
User=www-data
# 设置环境变量
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://*:5000

[Install]
WantedBy=multi-user.target
EOF

    # 重新加载systemd配置
    systemctl daemon-reload

    # 启用并启动服务
    info "启用并启动服务..."
    systemctl enable server-web.service
    systemctl start server-web.service

    # 检查服务状态
    info "检查服务状态..."
    systemctl status server-web.service --no-pager
fi

info "部署完成!"
info "您可以通过以下URL访问应用程序: http://localhost:5000"
info "如果需要从外部访问，请确保防火墙已开放5000端口"

exit 0 