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

        # 开启图片压缩（对后端传输图片时启用）
		gzip_proxied any;
		gzip_types image/jpeg image/png image/gif image/webp;
		gzip_min_length 1000;
		
		# 大文件优化
		proxy_buffering on;
		proxy_buffers 16 64k;
		proxy_buffer_size 128k;
		# 根据需要调节上传图片大小限制
		client_max_body_size 50M;
    }
}
