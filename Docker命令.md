# Docker命令



##### 先移除掉exited状态的容器 ,然后删除dangling状态的镜像(悬虚镜像)

```shell
docker rm $(docker ps -q -f status=exited)
docker rmi $(docker images -q -f dangling=true)
```

##### 查看是否存在具有特定名称的Docker容器

```shell
if [ ! "$(docker ps -q -f name=mysql80)" ]; then  //判断该容器是否存在
    if [ "$(docker ps -aq -f status=exited -f name=mysql80)" ]; then
        # cleanup
        docker rm mysql80
    fi
    # run your container
    docker run -d -p 3306:3306 -p 33060:33060 --volume "$MYSQLFOLDERLOCATION":/var/lib/mysql --name mysql80 frostedflakez/php-mysql-webserver:0.9-beta.3-mysql-latest-8.0
fi
```

##### 删除所有容器

```shell
docker rm `docker ps -a -q`
```

##### 删除所有镜像

```shell
docker rmi `docker images -q`
```

##### 按条件删除镜像

###### 没有打标签

```shell
docker rmi "`docker images -q | awk '/^<none>/ { print $3 }'`"
```

###### 镜像名包含关键字

```shell
docker rmi --force `docker images | grep doss-api | awk '{print $3}'`    //其中doss-api为关键字
```

##### 清理镜像

我们在使用 Docker 一段时间后，系统一般都会残存一些临时的、没有被使用的镜像文件，可以通过以下命令进行清理

```shell
docker image prune
```

它支持的子命令有：

- `-a, --all`: 删除所有没有用的镜像，而不仅仅是临时文件；
- `-f, --force`：强制删除镜像文件，无需弹出提示确认；

另外，执行完 `docker image prune` 命令后，还是告诉我们释放了多少存储空间！

##### 删除所有Tag是v开头的镜像

```shell
docker images --format "{{.Repository}}:{{.Tag}}" | grep ":v" | awk '{system("docker rmi "$1)}'
```

##### 要启动所有已停止的容器，你可以使用以下步骤：

1. 使用以下命令列出所有已停止的容器的ID：

```
bashCopy code
docker ps -a -q --filter "status=exited"
```

1. 使用 `docker start` 命令结合上述命令启动所有已停止的容器：

```
bashCopy code
docker start $(docker ps -a -q --filter "status=exited")
```



# 清理docker

Docker 的容器、镜像、卷和网络可能会占用大量的磁盘空间。为了清理不再需要的 Docker 资源并释放磁盘空间，你可以使用以下命令：

1. **清理停止的容器**：

   ```
   docker container prune
   ```
   
2. **清理未使用的镜像**：

   - 删除悬挂的镜像（即未被任何容器使用的镜像）：

     ```
     docker image prune
     ```
     
   - 删除所有未被任何容器使用的镜像：
   
     ```
     docker image prune -a
     ```
   
3. **清理未使用的卷**：

   ```
   docker volume prune
   ```
   
4. **清理未使用的网络**：

   ```
   docker network prune
   ```
   
5. **一次性清理停止的容器、未使用的网络、悬挂的镜像和未使用的卷**：

   ```
   docker system prune
   ```
   
   如果你也想删除所有未被容器使用的镜像（而不仅仅是悬挂的镜像），可以添加 `-a` 选项：
   
   ```
   docker system prune -a
   ```

**注意**：在使用上述命令之前，请确保你确实不再需要这些资源，因为这些操作是不可逆的。特别是当你清理镜像或卷时，重新获取或创建它们可能需要一些时间。
