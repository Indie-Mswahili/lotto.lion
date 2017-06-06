## dotnet publish

```
$ sudo supervisorctl stop lot.webapi
$ sudo dotnet publish -c Release -o /var/lot.webapi
$ sudo supervisorctl start lot.webapi
```

## Let’s Encrypt 

$ sudo mkdir /var/www/letsencrypt && sudo chgrp www-data letsencrypt

$ sudo nano /etc/letsencrypt/configs/lottoapi.odinsoftware.co.kr.conf

```
domains = lottoapi.odinsoftware.co.kr
rsa-key-size = 4096
server = https://acme-v01.api.letsencrypt.org/directory
email = help@odinsoftware.co.kr
text = True
authenticator = webroot
webroot-path = /var/www/letsencrypt/
```

$ sudo nano /etc/nginx/sites-available/lottoapi.odinsoftware.co.kr

```
 	server 
	{
		listen *:80;
		server_name lottoapi.odinsoftware.co.kr;

		location /.well-known/acme-challenge 
		{
			root /var/www/letsencrypt;
		}
    }
 
 ``

$ sudo nginx -t && sudo nginx -s reload

$ sudo certbot --config /etc/letsencrypt/configs/lottoapi.odinsoftware.co.kr.conf certonly

$ crontab -e

```
@weekly certbot renew --quiet --post-hook "systemctl reload nginx"
```

## nginx

```
$ sudo nano /etc/nginx/sites-available/lottoapi.odinsoftware.co.kr
```

```
 server 
 {
	listen 443 ssl;
	server_name lottoapi.odinsoftware.co.kr;

	ssl on;
    ssl_certificate /etc/letsencrypt/live/lottoapi.odinsoftware.co.kr/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/lottoapi.odinsoftware.co.kr/privkey.pem;

    location / 
	{
		proxy_pass http://localhost:5127;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
	}
 }

server 
{
	listen 80;
	server_name lottoapi.odinsoftware.co.kr;

    return 301 https://$host$request_uri;
}
```

```
$ sudo nginx_ensite /etc/nginx/sites-available/lottoapi.odinsoftware.co.kr
```

## supervisor

```
$ sudo nano /etc/supervisor/conf.d/lot.webapi.conf
```

```
[program:lot.webapi]
command=/usr/bin/dotnet /var/lot.webapi/lot.webapi.dll --server.urls:http://*:5127
directory=/var/lot.webapi/
autostart=true
autorestart=true
stderr_logfile=/var/log/lot.webapi.err.log
stdout_logfile=/var/log/lot.webapi.out.log
environment=ASPNETCORE_ENVIRONMENT=Production
environment=HOME=/home/odinsoft
user=www-data
stopsignal=INT
```


## logrotate

```
$ sudo nano /etc/logrotate.d/lot.webapi
```

```
/var/log/lot.webapi.out.log {
        rotate 4
        weekly
        missingok
        create 640 root adm
        notifempty
        compress
        delaycompress
        postrotate
                /usr/sbin/service supervisor reload > /dev/null
        endscript
}

/var/log/lot.webapi.err.log {
        rotate 4
        weekly
        missingok
        create 640 root adm
        notifempty
        compress
        delaycompress
}
```