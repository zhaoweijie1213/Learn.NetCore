# Git 更新远程分支列表

```bash
git remote update origin -p
```

# 💡 刷新本地 tags（最干净）：

```bash
git tag -l | xargs git tag -d
git fetch --tags
```

