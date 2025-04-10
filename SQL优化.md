非常好，这份笔记内容相当全面，我帮你梳理一下，并加上一些补充，方便你今后复习和深入理解。你是主要做后端开发的，理解这些对于数据库性能调优很有帮助。

------

# MySQL 索引与 SQL 优化总结

## 一、B+树索引结构

> MySQL InnoDB 默认使用 **B+树**

- 非叶子节点只存储键（索引字段），不存储数据。
- 叶子节点是有序链表，存储数据行的完整或部分信息。
- B+树有序且支持范围查询，特别适合区间查找。
- 聚簇索引：主键索引，叶子节点存放完整行数据。
- 辅助索引：非主键索引，叶子节点存储主键值。

------

## 二、EXPLAIN 字段详解

### type（表连接类型，性能排序）

性能：system > const > eq_ref > ref > range > index > all

- **system**：系统表，只有一条记录
- **const**：通过主键或唯一索引查询到单行记录
- **eq_ref**：唯一索引，返回唯一行（如主键或唯一约束字段）
- **ref**：普通索引，返回匹配行（多行）
- **range**：范围查询，BETWEEN、>、< 等
- **index**：只遍历索引，不访问数据表（覆盖索引）
- **all**：全表扫描，最差

### key_len（索引长度）

- 判断复合索引是否被完全利用
- utf8：1字符=3字节

### ref（连接使用的列）

- 指出使用哪个列或常量与索引匹配。

### rows（扫描行数估算）

- 越小越好。

### Extra（额外信息）

- **Using filesort**：额外排序
- **Using temporary**：使用临时表，性能差
- **Using index**：覆盖索引，无需回表
- **Using where**：使用了WHERE过滤，需要回表
- **Impossible where**：where 条件恒为 false
- **Using join buffer**：连接缓存

------

## 三、两表连接优化建议

- **左连接**：尽量 **小表驱动大表**
- 索引要建在 **频繁查询** 的字段上。

------

## 四、避免索引失效

1. **复合索引顺序要正确**，遵循最佳左前缀法则
2. **不要对索引列进行操作**：如函数、计算、类型转换
3. **覆盖索引尽可能使用**：select 出的字段都包含在索引中
4. **LIKE 查询**：
   - `LIKE 'abc%'` 可用索引
   - `LIKE '%abc'` 索引失效
5. **避免隐式类型转换**：如 int 和 varchar 比较
6. **避免 OR**：OR 可能导致索引失效
7. **范围查询后索引失效**：比如 `WHERE age > 10 AND name = '张三'`，`name` 字段不走索引。

------

## 五、其他优化技巧

### 1. exist/in 使用建议

- 主查询大，用 **IN**
- 子查询大，用 **EXISTS**

### 2. order by 排序优化

- **MySQL 4.1 后默认单路排序**，读一次磁盘，Buffer 内部排序
- 数据量大时，Buffer 装不下，还是会多次 IO
- 调优建议：
  - 避免 `select *`
  - 充分利用复合索引
  - 调整 `max_length_for_sort_data`
  - 排序字段方向一致：全部升序或降序

------

## 六、SQL 慢查询日志排查

### 检查是否开启

```sql
show variables like '%slow_query_log%';
```

### 临时开启

```sql
set global slow_query_log = 1;
```

### 永久开启

编辑配置文件 `/etc/my.cnf`

```ini
[mysqld]
slow_query_log=1
slow_query_log_file=/var/lib/mysql/localhost-slow.log
long_query_time=3
```

重启服务：

```bash
service mysql restart
```

### 查看慢查询阈值

```sql
show variables like '%long_query_time%';
```

### 查询慢查询次数

```sql
show global status like '%slow_queries%';
```

------

## 七、分析海量数据时

```sql
show variables like '%log_bin_trust_function_creators%';
set global log_bin_trust_function_creators=1;
```

------

## 小建议 🌟

你可以结合你的 .NET 项目，配合 Dapper / EF Core 查询日志、慢查询分析，一起做性能优化。
 另外，可以结合工具，比如：

- **pt-query-digest**：慢查询日志分析
- **MySQL Workbench**：执行计划图形化查看
- **Navicat**：SQL 可视化优化建议

------

好！我帮你整理一份实用的 ✅ **.NET + MySQL 性能优化全套 Checklist + 附加讲解**
 你用的时候可以当成参考手册，出问题就按这个排查和优化。

------

# ✅ .NET + MySQL 性能优化 Checklist

## 一、数据库设计阶段

-  主键：优先使用 **自增 ID** 或短字符串，避免 UUID（空间浪费大，B+ 树页分裂严重）
-  索引设计：
  -  高频查询字段要建索引
  -  组合索引遵循最左前缀法则
  -  查询条件覆盖索引字段顺序与索引一致
-  避免过多索引，维护索引有成本（增删改都会维护索引）
-  表结构规范化，避免冗余字段，减少列数量

## 二、SQL 编写阶段

-  避免使用 `SELECT *`，明确列名
-  合理分页，避免超大页码 `LIMIT 10000, 10` 性能极差
-  避免在 where 中使用函数和计算：
  - 错误：`WHERE YEAR(create_time) = 2025`
  - 正确：`WHERE create_time >= '2025-01-01' AND create_time < '2026-01-01'`
-  避免隐式类型转换（比如：int 字段用引号当字符串比较）
-  尽量避免使用 `OR`，可以改成 `UNION ALL`
-  用 EXISTS / IN 合理优化子查询
-  使用 `EXPLAIN` 查看执行计划，避免全表扫描（type=all）

## 三、连接查询优化

-  小表驱动大表：手动控制连接顺序
-  确保 Join 条件有索引
-  使用 Inner Join 替代 Left Join，除非必须返回空记录

## 四、排序与分组

-  避免无索引的 `ORDER BY`
-  避免大数据量的临时表 `GROUP BY`，可拆分聚合或提前计算
-  确认排序方向一致（升序 / 降序），利于使用索引排序
-  调整 `sort_buffer_size` 缓存大小

## 五、慢查询日志排查

-  开启慢查询日志
-  设置合理的慢查询阈值：`long_query_time = 1` 秒以内
-  使用 `pt-query-digest` 工具分析日志

## 六、MySQL 参数优化

-  `innodb_buffer_pool_size` 设置为物理内存 60%-70%
-  `max_connections` 根据并发合理配置
-  `query_cache_size` 检查是否需要开启（注意：8.0 已废弃）
-  `tmp_table_size` 和 `max_heap_table_size` 增大以减少磁盘临时表

## 七、应用层（.NET）优化

-  使用连接池（默认开启，保持合理配置）
-  避免频繁创建和销毁数据库连接
-  使用批量操作，减少多次单条插入或更新（如：`BulkInsert`）
-  使用异步库，如 Dapper.Async / EF Core 异步方法
-  查询结果缓存热点数据，减少数据库压力
-  配置超时和重试策略，防止雪崩

------

# 🌳 附赠：B+ 树结构图（简化版）

```
B+ Tree (3阶示意)

          [30]-------[60]
          /           \
   [10,20]         [40,50]-----[70,80,90]

- 非叶子节点：只存储索引键值
- 叶子节点：链表结构，存储完整数据（或主键指针）

优点：
- 范围查询效率高
- 叶子节点天然有序，支持区间扫描
- IO 读写块级别，批量读取快
```

------

# 🌟 附赠：EXPLAIN 典型案例

SQL 示例：

```sql
EXPLAIN SELECT id, name FROM users WHERE age > 20 ORDER BY create_time DESC LIMIT 10;
```

结果解读：

| id   | select_type | table | type  | possible_keys | key          | key_len | ref  | rows | Extra                                    |
| ---- | ----------- | ----- | ----- | ------------- | ------------ | ------- | ---- | ---- | ---------------------------------------- |
| 1    | SIMPLE      | users | range | idx_age_time  | idx_age_time | 5       | NULL | 100  | Using where; Using index; Using filesort |

关键看：

- `type`: range，说明走了索引范围扫描
- `Extra`:
  - `Using where`：需要回表或过滤
  - `Using index`：覆盖索引
  - `Using filesort`：ORDER BY 触发文件排序

优化建议：

- 确保 `age` 和 `create_time` 在复合索引中，且顺序符合查询条件和排序要求
- 考虑增加覆盖索引

------

这样你今后优化项目的数据库性能就有一整套体系化参考啦！🚀

如果你需要，我还能帮你继续：

- ✅ 出一套 SQL 慢查询排查脚本合集
- ✅ .NET Dapper / EF Core 性能优化建议
- ✅ 实战案例：比如 分页优化、大表拆分、读写分离等场景讲解

要不要一起整合一下？这样你项目上线前可以直接做一轮完整性能自检！🔍