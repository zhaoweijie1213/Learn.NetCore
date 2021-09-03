### 安装Xray

自行部署Xray服务端需要你有基本linux技巧，能使用vim/nano等编辑器。官方提供了[大多数Linux系统的一键脚本](https://github.com/XTLS/Xray-install)，可以直接使用：

```
bash <(curl -L https://github.com/XTLS/Xray-install/raw/main/install-release.sh) install
```

> 新手推荐使用 [Xray一键脚本](https://tlanyan.pp.ua/go.php?key=xray-script)

官方脚本安装的文件符合[FHS规范](https://en.wikipedia.org/wiki/Filesystem_Hierarchy_Standard)，可执行文件xray在 `/usr/local/bin` 目录下，配置文件位于 `/usr/local/etc/xray`目录内。

\4. 官方脚本安装的配置文件内容为空，可参考 [Xray-examples](https://github.com/XTLS/Xray-examples) 中提供的模板编辑配置文件。例如使用VLESS+TCP+XTLS的配置文件为：

```
{
    "log": {
        "loglevel": "info"
    },
    "inbounds": [
        {
            "port": 443, # 可以换成其他端口
            "protocol": "vless",
            "settings": {
                "clients": [
                    {
                        "id": "", // 填写UUID，可以使用xray uuid生成
                        "flow": "xtls-rprx-direct",
                        "level": 0
                    }
                ],
                "decryption": "none",
                "fallbacks": [
                    {
                        "dest": 80 // 回落配置，可以直接转到其他网站，例如"www.baidu.com:80"
                    }
                ]
            },
            "streamSettings": {
                "network": "tcp",
                "security": "xtls",
                "xtlsSettings": {
                    "alpn": [
                        "http/1.1"
                    ],
                    "certificates": [
                        {
                            "certificateFile": "/path/to/fullchain.crt", // 换成你的证书，绝对路径
                            "keyFile": "/path/to/private.key" // 换成你的私钥，绝对路径
                        }
                    ]
                }
            }
        }
    ],
    "outbounds": [
        {
            "protocol": "freedom"
        }
    ]
}
```

XTLS需要证书，因此需要一个域名并申请证书。域名不需要备案，国内和国外买的都可以。域名购买可参考：[Namesilo域名注册和使用教程](https://tlanyan.pp.ua/namesilo-domain-tutorial/) 或从 [适合国人的域名注册商推荐](https://tlanyan.pp.ua/domain-register-for-mainland/) 选购。域名申请证书可参考[从Let’s Encrypt获取免费证书](https://tlanyan.pp.ua/use-lets-encrypt-certificate/) 或 [从阿里云获取免费SSL证书](https://tlanyan.pp.ua/get-free-ssl-certificates-from-aliyun/)。

> fallback选项以及ALPN等设置请参考：[VLESS协议的fallback参数介绍](https://tlanyan.pp.ua/vless-fallback-object/)

\5. 配置完毕后，可通过 `systemctl start xray` 运行 xray，`systemctl stop xray` 停止xray，`systemctl restart xray` 重启，`journalctl -xe --no-pager -u xra`y 查看运行日志。

最后，记得放行防火墙。如果是阿里云、腾讯云、AWS/GCP等大厂的服务器，还需要到网页后台的安全组放行端口。

### 配置Xray客户端

服务端配置好后，接下来是配置客户端。目前有如下客户端支持Xray：

**Xray Windows客户端**：

- V2rayN：3.28版本起支持xray，只需要下载[Xray-core](https://github.com/XTLS/Xray-core/releases)，将解压的文件放到V2rayN-Core文件夹下即可。需要注意的是V2rayN 4.0版本移除了PAC，改用路由规则，会给习惯了PAC的用户带来困扰。习惯Qv2ray的网友应该乐于接受这个改变；
- winXray：winXray是Windows系统上简洁稳定的[Xray/V2Ray](https://tlanyan.pp.ua/v2ray-clients-download/)、[Shadowsocks](https://tlanyan.pp.ua/shadowsock-clients/)、[Trojan](https://tlanyan.pp.ua/trojan-go-clients-download/) 通用客户端，可自动检测并连接**访问速度最快的** 代理服务器。该项目原作者删库后出现了一些同名库，安全性未知，因此本站托管的依然是旧版；
- Qv2ray：Qv2ray是一个基于Qt框架开发的v2ray客户端，可通过插件支持SS、SSR、VMESS、VLESS、trojan等多种协议。

**Xray安卓客户端**：

- V2rayNG：V2rayNG可以说是最跟随Xray步伐的[V2ray客户端](https://tlanyan.pp.ua/v2ray-clients-download/)了，Xray发布新版本后会在第一时间更新，推荐使用。

**Xray Mac客户端**：

- Qv2ray：Qv2ray是一个基于Qt框架开发的跨平台[v2ray客户端](https://tlanyan.pp.ua/v2ray-clients-download/)，因此支持MacOS系统。实际上，自V2rayU作者删库不更新后，Qv2ray算得上Mac系统上支持VLESS协议的独苗，但可能会出现设置系统代理无效的bug。

**Xray苹果客户端：**

- Shadowrocket/小火箭：小火箭目前是ios系统上更新最频繁的[V2ray客户端](https://tlanyan.pp.ua/v2ray-clients-download/)，价格也不贵，支持多种协议，推荐使用。