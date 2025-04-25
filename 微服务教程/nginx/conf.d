# 自动跳转到 HTTPS
server {
    listen 80;
    server_name jenkins.acgsupply.com;

    # 强制跳转 HTTPS
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl http2;
    server_name jenkins.acgsupply.com;

    # TLS 证书
    ssl_certificate     /etc/nginx/ssl/jenkins.acgsupply.com.pem;
    ssl_certificate_key /etc/nginx/ssl/jenkins.acgsupply.com.key;

    # 推荐安全配置
    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_ciphers         HIGH:!aNULL:!MD5;

    # Jenkins 反代配置
    location / {
        proxy_pass http://127.0.0.1:8080;

        # 常规推荐头部
        proxy_set_header Host              $host;
        proxy_set_header X-Real-IP         $remote_addr;
        proxy_set_header X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        # Jenkins 兼容性建议
        proxy_http_version 1.1;
        proxy_request_buffering off;
    }
}
