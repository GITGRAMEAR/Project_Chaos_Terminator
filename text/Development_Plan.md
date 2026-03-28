## 《Chaos Terminator》1个月开发计划（可执行版）

### 使用方式（每天照着做）

- **当日目标**：只追求“可验收”的最小增量（能跑、能点、能结算）
- **文件落点**：本计划默认脚本位于 `Assets/TempAssets/_Scripts/`
- **场景落点**：建议新建 `Assets/TempAssets/Scenes/MVP.unity`
- **验收标准**：每个条目都写了“今天结束时必须看到什么”

---

## 周计划（1个月 MVP 可试玩）

### 第1周：闭环原型（选房→进房→战斗→结算→再选房）

- **交付物**：
  - `TRoomFlowManager` + `TRoomSelectionController` + `RoomFactory` 串通
  - 3类房间（普通/特殊/遗物）占位能进入并结算
  - 最小战斗：玩家移动 + 敌人追击 + 击杀判定
  - 最小升级：经验满 → 3选1（先改一个数值即可）
  - 最小存档：JSON 写入（runId / 房间数 / 秩序混沌占位）
- **关键指标**：一局连续跑 **≥ 5 个房间** 不报错

### 第2周：核心机制落地（秩序/混沌 + 联动 + 失败选项）

- **交付物**：
  - 秩序值：胜利获取、预览消耗、UI展示
  - 混沌：奋力一搏叠加、负面永久附着、胜利清除、后续减弱+奖励
  - 联动：特殊联动房匹配/不匹配分支、胜利进化奖励
  - 失败选项：放弃/奋力一搏全链路（并能回到循环）
- **关键指标**：联动与混沌规则 **可复现**，并能对核心函数做单元测试

### 第3周：爽感与内容最小集（波次/对象池/遗物策略）

- **交付物**：
  - 对象池（敌人/投射物/特效，至少敌人）
  - 敌人波次/刷怪（房间内节奏）
  - 3种敌人差异（速度/血量/攻击方式任意三种差异）
  - 技能长按范围（`TSkillCaster2D`）接入真实伤害圈
  - 遗物 3 个（影响秩序获取/伤害/移速等）
- **关键指标**：同屏 **80-120** 敌人稳定（目标值，可逐步靠近）

### 第4周：打磨与交付（留存导向）

- **交付物**：
  - 新手引导（第1-2房 UI 提示）
  - 难度曲线参数表（出现率、血量、刷新节奏）
  - 10-20人 Playtest（收集理解点/挫败点/退出点）
  - Bug 分级与修复、构建流程与 README 补完
- **关键指标**：首局玩家能说清 **秩序预览/混沌代价/联动匹配**；崩溃 0

---

## 现有脚本清单

- **GameFlow**：
  - `TRoomFlowManager.cs`：流程入口
  - `TRoomSelectionController.cs`：预览与选择
  - `RoomFactory.cs`：房间生成（已补齐骨架）
  - `FailChoiceController.cs`：失败选项事件
  - `TSkillRoomLinkageResolver.cs`：联动解析
- **Core**：
  - `TOrderChaosState.cs`：秩序/混沌状态模型
  - `TOrderChaosRules.cs`：规则函数（建议用于单元测试）
- **Data**：
  - `TRoomDefinition.cs` `TSkillDefinition.cs` `TRelicDefinition.cs`
- **Player**：
  - `TPlayerController2D.cs`：移动
  - `TSkillCaster2D.cs`：长按范围（待接入战斗）
- **AI**：
  - `EnemyAI.cs`：追击骨架
- **Persistence**：
  - `SaveDataModels.cs` `SaveSystem.cs`

---

## 每日计划

> 说明：如果你还没建场景/Prefab，按表格里“Prefab/UI”列创建占位（空物体+脚本也算）。

### 第1周（闭环原型）


| 天   | 今日目标（最小可验收）                    | 需要改/新增的文件                                                                      | Prefab / UI                                               | 今日验收标准                                                                    |
| --- | ------------------------------ | ------------------------------------------------------------------------------ | --------------------------------------------------------- | ------------------------------------------------------------------------- |
| D1  | 场景里把“流程三件套”跑起来（预览事件能触发）        | `TRoomFlowManager.cs` `TRoomSelectionController.cs` `RoomFactory.cs`           | `MVP.unity`：`GameFlow`物体挂3脚本；做一个临时按钮触发 `RequestPreview()` | 点击按钮后 `OnPreviewGenerated` 能触发（日志或UI显示两张房间卡）完成                            |
| D2  | 选择房间 → 进入房间（先用日志占位）            | `TRoomSelectionController.cs`（在 `ChooseEnter` 调用 `TRoomFlowManager.EnterRoom`） | UI：两张卡 + Enter按钮（占位）                                      | 选中某卡并点击进入后，`TRoomFlowManager.EnterRoom` 被调用且 `RoomInstance.Begin()` 执行 完成 |
| D3  | 3类房间“进入即表现”占位（普通/特殊/遗物）        | `TRoomFlowManager.cs`（GoToNextRoomPreview 先实现简单流转） `TRoomDefinition.cs`        | UI：房间标题/类型文本                                              | 进入不同 `RoomType` 时，UI能看到类型变化；遗物房进入后弹出“三选一占位”                               |
| D4  | 战斗最小集：玩家移动 + 1种敌人追击 + 击杀（触发胜利） | `TPlayerController2D.cs` `EnemyAI.cs`（锁定玩家target）                              | Prefab：`Player`、`Enemy`（CircleCollider2D等）                | 房间中能刷出敌人并追玩家；击杀条件满足时能触发“房间胜利”事件/调用 `FinishRoom(true)`                     |
| D5  | 经验/升级 3选1（只改一个数值即可）            | （新增）`ExpSystem.cs`（或放 `GameFlow/Combat`）                                       | UI：升级面板（三按钮）                                              | 经验满弹出三选一，选一个后玩家某个数值变化（如伤害+10%）并关闭面板                                       |
| D6  | 存档最小集（写入/读取跑通）                 | `SaveDataModels.cs` `SaveSystem.cs`（接入 runId/roomCount/order/chaos）            | UI：存档按钮（占位）                                               | 点击保存后本地 JSON 生成；重启运行可读取到 roomCount/order/chaos                            |
| D7  | 冒烟测试 + 小重构（把“临时硬编码”集中）         | 视情况整理：`RoomFactory.cs`、`TRoomFlowManager.cs`                                   | 无                                                         | 连跑 ≥5房稳定；关键数据只在一个地方生成（不散落硬编码）                                             |


### 第2周（秩序/混沌 + 联动 + 失败选项）


| 天   | 今日目标（最小可验收）                | 需要改/新增的文件（建议）                                                     | Prefab / UI（建议）            | 今日验收标准                                                 |
| --- | -------------------------- | ----------------------------------------------------------------- | -------------------------- | ------------------------------------------------------ |
| D8  | 秩序值：胜利获得 + UI 展示           | `TOrderChaosState.cs` `TOrderChaosRules.cs` `TRoomFlowManager.cs` | UI：Order/Chaos 面板（文本）      | 胜利后 orderValue 增加（联动房加成更高），UI数值刷新                      |
| D9  | 预览消耗秩序：点击预览扣值并展示候选         | `TRoomSelectionController.cs`                                     | UI：预览按钮 + 费用提示             | 点击预览按钮后 orderValue 下降；候选房刷新                            |
| D10 | 联动接入结算：特殊房匹配/不匹配分支生效       | `TSkillRoomLinkageResolver.cs` `TRoomFlowManager.cs`              | UI：联动结果提示（Matched/NoMatch） | 特殊房胜利时：匹配则“进化提示+额外秩序”；不匹配则出现负面提示                       |
| D11 | 失败弹窗：放弃/奋力一搏入口跑通           | `FailChoiceController.cs` `TRoomFlowManager.cs`                   | UI：失败弹窗（2按钮）               | 房间失败时弹窗出现；点放弃结束本局流程；点奋力一搏继续流程                          |
| D12 | 混沌附着：奋力一搏后永久负面挂载           | `TOrderChaosState.cs` `TOrderChaosRules.cs` `TRoomFlowManager.cs` | UI：负面列表（文本占位）              | 选择奋力一搏后 chaosValue 增加，attachedNegatives 增加一条并在后续房间仍存在  |
| D13 | 胜利清除全部负面 + 后续减弱+奖励（先做数值乘子） | `TOrderChaosRules.cs` `TOrderChaosState.cs`                       | UI：清除提示/奖励提示               | 任意房间胜利后 attachedNegatives 清空；随后新增负面强度变弱，同时给正向加成（如伤害乘子） |
| D14 | 单元测试覆盖规则纯函数（关键用例）          | （新增）`Assets/TempAssets/Tests/`* 或 `Assets/Tests/`*                | 无                          | 秩序收益、混沌叠加、衰减函数、联动解析关键用例通过                              |


### 第3周（爽感与内容最小集）


| 天   | 今日目标（最小可验收）          | 需要改/新增的文件（建议）                                        | Prefab / UI（建议）            | 今日验收标准                                  |
| --- | -------------------- | ---------------------------------------------------- | -------------------------- | --------------------------------------- |
| D15 | 对象池（先做敌人池）           | （新增）`EnemyPool.cs`（或 `ObjectPool.cs`）                | Prefab：Enemy               | 刷怪不再频繁 Instantiate/Destroy（运行中可观察计数/日志） |
| D16 | 房间波次刷怪 + 胜利条件（清波次）   | `TRoomFlowManager.cs`（或新增 `RoomBattleController.cs`） | UI：波次数显示（可选）               | 每房间按波次刷怪，清完触发胜利结算                       |
| D17 | 3种敌人差异化              | `EnemyAI.cs`（派生/参数化）                                 | Prefab：EnemyA/B/C          | 三种敌人行为或参数明显不同（速度/攻击/远程）                 |
| D18 | 技能长按范围接入战斗伤害圈        | `TSkillCaster2D.cs`（接入命中/伤害）                         | Prefab：技能范围特效（Sprite/Line） | 长按能改变范围，范围内敌人受伤；特殊房匹配时反馈更强              |
| D19 | 遗物 3 个 + 遗物房 3选1真实生效 | `TRelicDefinition.cs` `TRoomFlowManager.cs`          | UI：遗物选择面板                  | 选择遗物后对全局参数生效（秩序获取/伤害/移速等）               |
| D20 | 音效与关键反馈（房间切换/联动/击杀）  | （新增）`AudioRouter.cs`（可选）                             | AudioSource 管理物体           | 不同事件触发不同音效（至少3类事件）                      |
| D21 | 平衡一轮（参数表驱动）          | （新增）`BalanceTable.asset` 或 JSON/ScriptableObject     | 无                          | 第5房出现隐藏机制；胜率曲线接近目标（可手测记录）               |


### 第4周（打磨与交付）


| 天   | 今日目标（最小可验收）            | 需要改/新增的文件（建议）                                  | Prefab / UI（建议）   | 今日验收标准                          |
| --- | ---------------------- | ---------------------------------------------- | ----------------- | ------------------------------- |
| D22 | 新手引导（第1-2房提示）          | （新增）`TutorialController.cs`                    | UI：提示气泡/高亮        | 玩家在第1-2房能被引导完成“预览→进房→升级→结算”     |
| D23 | 房间库扩充（每类≥3配置）          | `TRoomDefinition.cs`（创建更多 ScriptableObject 资产） | RoomDefinition 资产 | 候选房不再重复单一；至少各3种                 |
| D24 | Playtest 1（10-20人）与记录表 | （新增）`PlaytestNotes.md`                         | 无                 | 收集“理解点/挫败点/退出点/爽点”并归类           |
| D25 | 修复致命/严重Bug + 性能优化      | 视问题定位                                          | 无                 | 崩溃 0；卡顿明显减少（同屏目标接近）             |
| D26 | Playtest 2（回归验证改动）     | 同上                                             | 无                 | 核心机制理解率提升；首局完成率提升               |
| D27 | 构建发布与版本记录              | （新增）`CHANGELOG.md`（可选）                         | 无                 | 可打包 Windows 可运行版本；README 更新运行方式 |
| D28 | 最终打磨封版（MVP）            | 全局整理                                           | 无                 | 可试玩MVP：闭环稳定、反馈清晰、可连续游玩10-15分钟   |


---

## 备注：每天最该看的“关键指标”

- **闭环稳定性**：连续跑房不崩
- **机制可解释性**：玩家能理解秩序/混沌/联动
- **爽感反馈**：击杀/联动/房间切换的反馈强度足够

