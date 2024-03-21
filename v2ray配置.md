## v2ray配置

在CentOS、Ubuntu等常用Linux系统上，直接执行如下命令安装V2Ray（如果已安装则更新程序）：

```
bash <(curl -L https://raw.githubusercontent.com/v2fly/fhs-install-v2ray/master/install-release.sh)
```

安装完成后，配置文件为`/usr/local/etc/v2ray/config.json`，内容默认为空。粘贴下面模板内容至配置文件中：

```
{
  "inbounds": [{
    "port": 监听端口,
    "protocol": "vmess",
    "settings": {
      "clients": [
        {
          "id": "用户id，生成方法见下面说明"
        }
      ]
    }
  }],
  "outbounds": [{
    "protocol": "freedom",
    "settings": {}
  }]
}
```

配置文件中最重要的信息有两项：1. port（`监听端口`），建议是1024-65535中的任意一个数字，例如12345，6789等；2. clients中的id（`用户id`），可以运行命令 /`usr/local/bin/v2ray uuid` 得到。**这两个参数将在配置客户端时用到，而必须与服务端一致！**

配置好后，接下来防火墙放行监听的端口，设置开机启动并运行`V2Ray`：

```
# firewalld放行端口（适用于CentOS7/8）
firewall-cmd --permanent --add-port=123456/tcp # 23581改成你配置文件中的端口号
firewall-cmd --reload
# ufw放行端口（适用于ubuntu）
ufw allow 12345/tcp # 12345改成配置中的端口号
# iptables 放行端口（适用于CentOS 6/7）
iptables -I INPUT -p tcp --dport 12345 -j ACCEPT
# 设置开机启动
systemctl enable v2ray
# 运行v2ray
systemctl start v2ray
```

`ss -ntlp | grep v2ray` 命令可以查看v2ray是否正在运行。如果输出为空，大概率是被selinux限制了，解决办法如下：

\1. 禁用selinux：`setenforce 0`;

\2. 重启v2ray：`systemctl restart v2ray`

到此，服务端应该配置好了。如果服务器商层面还有防火墙（阿里云/Google/AWS购买的vps），请登录网页后台，放行v2ray的端口。





主机账号

```
#hk
root
o960kQA7sLowd0M1YX

#us
root
400L6wFQh4zmzsQYR3
```

