# lazyiot2.0

 lazyiot2.0 相对于1.0进行了大规模的系统升级
 目前组态设计器是需要进行商业购买
 
一款开源的web组态，IOT，物联网产品 一款开源的web组态，IOT，物联网产品 ,改开源系统采用分布式集群部署，支持多采集站/数据中心模式，前端采用web显示 开发语言全部是.net/C# 最新版本2021-10-10

目前整个解决方案采用vs2017进行编译，编译后的所有程序放置在系统目录下的Publish,在Publish目录下包含IOCenter和IOStation 两个文件夹。 IOCenter 是中心服务器的应用程序 IOStation 是采集站的应用程序，在该程序下包含三个exe可执行文件：IOManager.exe 采集站工程管理器,IOMonitor.exe负责采集站上的数据采集与转存，ScadaFlowDesign.exe是组态设计器。

服务器端部署： 1 首先编译发布web端程序，发布工程中Scada.WebServer/UI/SCADA.Web网站工程，将发布的文件拷贝到要部署的电脑或者是服务器上。 2 在发布的电脑上安装iis7以上版本，.net4.5 版本，部署web工程 3 打开部署电脑的web工程目录，在该目录下有ScadaCenterServer文件（如果没有请建立同名的文件夹），打开后将publish目录下的IOCenter文件里的所有文件拷贝到ScadaCenterServer文件夹内（全部拷贝）。 4 设置ScadaCenterServer文件夹下的IOProject\IOCenterServer.station的权限，分配networkservices,eveny 权限给IOCenterServer.station文件。 5 在谷歌或者360极速浏览器下输入对应的网址，打开网站，账号和密码:admin/123456 如果输入密码成功则表示web和服务器端程序部署成功，如果登录失败说明IOCenterServer.station文件没有设置权限 6 执行ScadaCenterServer/ScadaCenterServer.exe 文件启动中心服务器 登录账号admin/密码123456 采集站部署： 1 首先将Publish/IOStation整个文件夹拷贝到您要部署的服务器上，如果采集站服务器和中心服务器是同一台机器，则直接启动IOMonitor.exe可执行文件。 2 执行IOManager.exe文件进行IO工程的管理和编辑工作，在这里可以建立通讯，IO 采集点， 3 当IOManager工程都编辑成功并发布了，则进行组态设计，点击ScadaFlowDesign.exe 文件打开设计器 4 采集站上的所有IP地址是您部署的服务器的IP地址，登录账号admin/密码123456。 5 如果要部署多个采集站，则将Publish/IOStation 文件拷贝到对应的采集站服务器上，然后进行io工程编辑和io工程管理,同时发布IO采集站工程

感谢大家地支持，帮助文档会逐渐发布到github的wiki里。截止到2021.4.27 日发现部分bug,目前已经修改并将代码发布到开源网站了，请大家及时更新。相关视频教程已经发布到百度网盘；后续会逐步增加 系统开源地址: gitee:https://gitee.com/ningxia-zhongzhi/lazyiot

github：https://github.com/nxzzkj/lazyiot

相关教程视频： 链接：https://pan.baidu.com/s/1bH_IDuYXUUMKxV5nAS64Tw 提取码：lazy

QQ：249250126 QQ技术交流群:89226196 微信:18695221159

2021年5月19日更新内容： 1--------------大屏展示部分增加了环状排名图，柱状排名图，锥形排名图，动态水池图、进度条图，飞线图。 2--------------修改了组态工程中增加关系数据源后编辑后出现重复的信息bug。 3--------------修改了组态中环图无法读取数据库中值得bug。 4--------------修改了用户点击部分图元组件出现弹出“”保存成功的“”的异常错误。 5--------------重新将部分对象进行引用，缺失的dll放置到lib文件下，主要是sybase,aceclient。 6--------------修改了由于javascript $.post 异步引起的对象丢失问题。 7--------------应微信用户leo2013 用户要求增加了一个读取本地日期的图元，该图元静态显示当前电脑的时间。 8--------------目前所有工程编译可采用64位，唯独组态设计器工程需要编译成32位，主要是疑问WebKit c#库目前只找到32位的，如果全部编译成64位，部分组态控件将无法显示。 8--------------由于github上传后再LazyScadaCode\ScadaProject\ScadaWeb\目录下有个packages1文件，请将文件夹名称改成"packages" 去掉1，主要是github无法上传原packages文件目录，所以改成别名，，请下载后将此文件夹改回原名。否则web工程编译将出现错误。 2021年7月14日更新内容 1 修改了实时值传递到数据中心错误 2 修改了报警值传递到数据中心的错误 3 增加了MQTT驱动和DEMO的示例 4 修改了部分界面显示参数的错误 5 增加了报警值组合报警的判断 6 修改了数据中心无法保存报警的错误

2021年10月10日更新内容：1修改了数据上传机制，采用缓存机制，取消了之前的接收到数据立即上传的方式，而是将接收到的数据统一存储在缓存中，采用批量从缓存数据上传到数据中心，极大的提高了数据传输效率。提高系统并发处理能力。数据中心采用缓存模式将接收到的数据存储缓存，在指定时间内批量将缓存数据提交的influxdb.取消了之前的接收到采集数据就提交influxdb的模式。减小了influxdb的压力，提高数据并发入库能力。 2 增加了定时系统内存回收机制，屏蔽系统睡眠和锁屏模式。此前发现系统如果在锁屏模式下或者是睡眠模式下内存一直不回收导致系统内存不断增大。


如果想咨询MQTT的DEMO运行，请家微信进微信群！
