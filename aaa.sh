#!/bin/bash
# 统计每个作者的代码贡献（增加/删除行数）
# 使用当前目录下的 Git 仓库

git log --pretty='%aN' | sort -u | while read name; do
    echo -n "$name: "
    git log --author="$name" --pretty=tformat: --numstat \
        | awk '{ add += $1; subs += $2; loc += $1 - $2 } END { printf "增加 %s 行, 删除 %s 行, 贡献净行数 %s\n", add, subs, loc }' -
done
