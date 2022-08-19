## v2ray配置

在CentOS、Ubuntu等常用Linux系统上，直接执行如下命令安装V2Ray（如果已安装则更新程序）：

```
bash <(curl -sL https://raw.githubusercontent.com/hijkpw/scripts/master/goV2.sh)
```

安装完成后，配置文件为`/etc/v2ray/config.json`，cat命令可查看内容：`cat /etc/v2ray/config.json`。一个安装时自动生成的配置文件示例：

```
{
  "inbounds": [{
    "port": 23581,
    "protocol": "vmess",
    "settings": {
      "clients": [
        {
          "id": "ceb793e6-49cf-25d8-e4de-ae542e62748e",
          "level": 1,
          "alterId": 64
        }
      ]
    }
  }],
  "outbounds": [{
    "protocol": "freedom",
    "settings": {}
  },{
    "protocol": "blackhole",
    "settings": {},
    "tag": "blocked"
  }],
  "routing": {
    "rules": [
      {
        "type": "field",
        "ip": ["geoip:private"],
        "outboundTag": "blocked"
      }
    ]
  }
}
```

配置文件中”inbounds”下的这几项信息需要留意：port（`端口`）、clients中的id（`用户id`）和alterId（`额外id`），它们将在配置客户端时用到。

配置文件无需任何改动即可正常使用。接下来防火墙放行监听的端口，设置开机启动并运行`V2Ray`：

```
# firewalld放行端口（适用于CentOS7/8）
firewall-cmd --permanent --add-port=23581/tcp # 23581改成你配置文件中的端口号
firewall-cmd --reload
# ufw放行端口（适用于ubuntu）
ufw allow 23581/tcp # 23581改成你的端口号
# iptables 放行端口（适用于CentOS 6/7）
iptables -I INPUT -p tcp --dport 23581 -j ACCEPT
# 设置开机启动
systemctl enable v2ray
# 运行v2ray
systemctl start v2ray
```

`ss -ntlp | grep v2ray` 命令可以查看v2ray是否正在运行。如果输出为空，大概率是被selinux限制了，解决办法如下：

\1. 禁用selinux：`setenforce 0`;

\2. 重启v2ray：`systemctl restart v2ray`

到此，服务端应该配置好了。如果服务器商层面还有防火墙（阿里云/Google/AWS购买的vps），请登录网页后台，放行v2ray的端口。

接下来介绍v2ray客户端的配置和使用。





主机账号

root

N1YFz7oO9I5Rdpj47q

root

3q1LuWgj2IQ9Tb5Kh1