# DistributedServer

.Net 7.0
Orleans 7.0.0
Unity 2022.3.8f1c1
LiteNetLib 1.1.0

两种分布式实践：
1. 手动实现传统分布式；
2. Orleans架构分布式；
使Unity客户端在无感知的情况下，分别连接两种服务器。

服务器承载估算：
商业在线人数：
2003|热血传奇|65W
2009|WOW|1300W
2021|热血4STEAM|80W
2022|王者|200W
2023|CS:GO|150W
2023|LOL|750W
2023|OW2|＜10W
- 服务器拆分后，单服功能性单一，测试每个API独立处理请求耗时。
## CPU瓶颈（木桶短板）
- 单位时间（1s） / API耗时（0.0000025s） = 40w次并发/秒。
- API执行频率 = 1s / tickrate（32次）= 0.03125s/次
- 40w次 * 0.03125s/次 = 1.5w个房间（单进程）
## 内存瓶颈
## 带宽瓶颈
- 所需带宽 = 结构体体积 * 并发量
byte  	1字节	128
short 	2字节	65536
int		4字节
long 	8字节
struct input //=5字节
{
	//ushort/ulong userid;
	byte seatid; //seatid
	ushort tick;
	ushort operation; //压缩后的上/下/左/右/跳跃/攻击
}
5字节 * 30次 * 6人= 900字节
1MB带宽（=1024KB=2^20字节） ≈ 1048576 / 900 = 1165人
2MB ≈ 2300人