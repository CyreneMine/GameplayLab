# GameplayLab

GameplayLab 是一个长期维护的 Unity Gameplay / 客户端开发练习项目。

这个仓库不是单纯用来存放课堂练习代码，而是用来把一个小型玩家控制器实验场，逐步扩展成更完整的 Gameplay 工程训练环境。项目会持续围绕输入、角色控制、摄像机、物理、状态机、动画、UI、技能和 AI 等客户端核心模块积累代码与记录。

## 当前目标

当前目标是沿着 Unity Gameplay / 客户端开发路线逐步成长。

现阶段重点放在玩家控制基础上：

- 理解 Transform 直接移动、Rigidbody 物理控制、CharacterController 控制之间的差异。
- 熟悉 Unity Input System 的输入配置、回调流程和 Gameplay 语义命名。
- 让玩家控制器从能跑起来，逐步变成结构清晰、可继续扩展的代码。
- 长期记录练习过程、常见问题和可复用的 Gameplay 写法。

## 当前已完成

项目已经完成第一阶段的玩家控制基础练习：

- 使用 `Input.GetKey` 和 `transform.Translate` 实现 WASD 移动
- 使用 `transform.Rotate` 实现 Q/E 旋转
- 配置并使用 Unity Input System
- 使用 `InputAction.CallbackContext` 接收输入回调
- 将移动输入缓存为 `Vector2`
- 使用 Mouse Delta 实现鼠标视角控制
- 区分角色水平旋转和摄像机上下俯仰
- 使用 Clamp 限制摄像机俯仰角
- 实现 FPS 风格的鼠标锁定
- 使用 Shift 实现冲刺
- 使用 `performed` / `canceled` 处理按住状态
- 使用 `isRunning` 状态控制冲刺速度
- 使用 `Rigidbody.AddForce` 实现基础跳跃
- 初步理解 `ForceMode.Impulse` 与其他 ForceMode 的差异

当前阶段主要练习脚本：

- `Assets/Scripts/Lesson01.cs`

## 后续计划

后续会从基础输入和 Transform 控制，逐步进入更接近正式项目的 Gameplay 系统：

- Grounded 地面检测
- CharacterController 移动
- 摄像机跟随
- Animator 动画参数
- FSM 状态机
- 交互系统
- 简单技能系统
- UI 血条 / 体力条
- 简单敌人 AI

下一阶段会继续练习 grounded 检测，并对比 CharacterController 的角色控制写法。

## 使用技术

- Unity
- C#
- Unity Input System
- Input Actions
- `InputAction.CallbackContext`
- Transform 移动与旋转
- Camera Pitch Clamp
- Rigidbody
- CharacterController

## 项目结构

- `Assets/`
  Unity 资源目录，包含场景、材质、输入配置和 Gameplay 脚本。
- `Assets/Scripts/`
  练习脚本和 Input System 生成的 C# 包装类。
- `Docs/`
  长期学习记录、常见问题记录和 Gameplay 笔记。
- `Docs/LearningProgress.md`
  按时间追加的学习进度记录。每次代码检查或阶段复盘后维护。
- `Docs/CommonMistakes.md`
  记录重复出现的问题和需要长期提醒的编码习惯。
- `Docs/GameplayNotes.md`
  记录已经形成固定方案、后续可以复用的 Gameplay / 客户端写法。
- `GameDev_KnowledgeBase/`
  记录围绕 Unity 机制、物理系统和 Gameplay 问题展开的深入学习过程。
- `AGENTS.md`
  项目专用的训练规则、文档维护规则和 Git 工作流规则。
- `Packages/`
  Unity 包配置。
- `ProjectSettings/`
  Unity 项目设置。

## 学习记录

[学习进度](Docs/LearningProgress.md)

[常见问题](Docs/CommonMistakes.md)

[Gameplay 笔记](Docs/GameplayNotes.md)

GameplayLab 会持续作为一个成长型仓库维护。每完成一个阶段，项目中都应该留下对应的代码、复盘记录和下一步方向，让这个工程逐渐从基础练习场变成一个真正可展示的 Unity Gameplay / 客户端开发作品集。
