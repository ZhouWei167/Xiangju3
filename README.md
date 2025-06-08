# 相聚传奇3使用说明
## 1. 合区工具
     两种使用方法：
    1. 将分区的Users.db改名后复制到服务端Database目录下作为源数据库，再行迁移。
    2. 将分区的Users.db、System.db、ClientSystem.db复制到同一目录下作为源数据库，再行迁移。

## 2. 授权工具
     需将License.dll和license.license同时放到服务端Server目录下。
## 3. 授权工具编译方法
    先编译License项目，再编译Server
## 4. 区服列表工具
    区服列表文件为ServerList.json。同时配备了加解密工具。。
    添加区服列表请严格按照里面的格式。
    修改时请先解密（有明文版忽略）
    修改完成后先使用加密工具加密，再放到服务器的FTP目录 或是微端的wwwroot目录  或者自行指定的地址
