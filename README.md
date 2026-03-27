# 《Chaos Terminator》

## 一、项目概述  
本文档整合了《Chaos Terminator》的玩法架构、开发计划、代码框架等内容，便于后续参考。
① 项目概述
游戏类型：2D 上帝俯视角（Top-Down）末世混沌割草生存游戏
核心 slogan：“在混沌房间中抉择，用秩序终结末世”
痛点对标：传统肉鸽重复地图、技能联动单一、策略反馈不清晰
开发周期：1 个月（MVP 可试玩）
团队配置：Solo 开发核心玩法+架构；美术/音效使用免费资源与轻量自制

## 二、文字内容  
- [设计文档](text/Chaos_Terminator_Design_Doc.md)：包含核心玩法、系统框架、美术音效等内容。  
- [开发计划](text/Development_Plan.md)：包含1个月开发周期的周目标、每天目标。  

## 三、代码内容  
- [TempAssent](Assets/TempAssets/_Scripts/_Core/TOrderChaosRules.cs)：角色移动脚本（Top-Down用Rigidbody2D）。

## 四、图片内容  
② 核心玩法详解
1. 用户旅程图（核心循环）
核心体验不是“刷到强就赢”，而是每一步都要为下一步的风险收益付出“秩序值/混沌值”的代价。
- ![核心循环流程图](User_Journey_Map.png)：核心玩法循环（进入房间→清怪→升级→胜利→秩序值→选下一房间）。  
## 五、扩展说明  
- 代码中的`// TODO:`标记：需后续添加多技能系统、遗物房等功能。  
- 图片中的`PLACEHOLDER`：需后续替换为真实资源（如美术素材、音效）。