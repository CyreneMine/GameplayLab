# GameplayLab Learning Progress

## 2026-05-26 项目学习记录初始化

### 当前题目
阶段初始化：整理前五题学习状态，准备进入第六题。

### 本题目标
建立长期学习记录系统，用于后续每次代码检查后追加记录学习进展、问题、改进方向和下一阶段建议。

### 我做得好的地方
- 已经完成从旧输入系统到 Unity Input System 的基础过渡。
- 已经练习过移动、旋转、鼠标视角、冲刺等玩家控制器基础功能。
- 已经开始形成 `performed/canceled`、输入状态缓存、`isRunning` 状态变量等 Gameplay 思维。

### 当前存在的问题
- 跳跃、地面检测、物理移动、CharacterController 等角色控制核心模块还未开始系统练习。
- 当前记录只来自阶段总结，后续代码检查必须基于项目中的真实 `.cs` 文件。

### 推荐改进方向
- 下一题开始进入 Rigidbody 跳跃，先理解物理驱动和 Transform 直接移动的区别。
- 后续逐步加入 grounded 检测，避免空中无限跳。
- 再进入 CharacterController，比较它和 Rigidbody 在角色控制上的使用场景差异。

### 当前阶段总结
当前处于 Unity Gameplay / 客户端开发基础复习阶段。已掌握基础 Transform 控制、Input System 事件输入、鼠标视角和冲刺状态，准备进入更接近真实角色控制器的物理与控制器模块。

### 下一阶段建议
第六题建议继续做：Rigidbody 跳跃。

## 2026-05-26 前五题阶段汇总与代码评价

### 当前题目
第 1~5 题阶段复盘：WASD 移动、Q/E 旋转、Unity Input System、鼠标视角控制、Shift 冲刺。

### 本题目标
基于项目真实脚本 `Assets/Scripts/Lesson01.cs` 汇总前五题学习成果，判断当前玩家控制器代码是否已经具备继续进入 Rigidbody 跳跃、grounded 检测和 CharacterController 的基础。

### 我做得好的地方
- 已经把旧输入系统练习迁移到 Unity Input System，并使用 `InputAction.CallbackContext` 接收 Move、Run、Rotate、Look 输入。
- 移动输入已经从按键判断升级为 `Vector2 moveInput` 状态缓存，这是正式玩家控制器常见思路。
- 冲刺没有继续使用 `speed += runSpeed` 这类累加写法，而是通过 `isRunning` 和 `currentSpeed` 控制当前速度，方向是对的。
- 鼠标视角已经拆成角色 Y 轴旋转和摄像机 X 轴俯仰，并使用 `Mathf.Clamp` 限制俯仰角。
- 输入订阅和取消订阅都写在 `OnEnable` / `OnDisable`，已经开始接近 Unity 项目里组件生命周期管理的写法。

### 当前存在的问题
- `Lesson01.cs` 里仍然混有旧版输入系统注释代码，阶段学习时可以保留，但正式项目里会降低脚本可读性。
- 字段命名风格还不统一，例如 `WalkSpeed` 是 public PascalCase，`runSpeed` 是 private camelCase，`maxCla` 命名也不够明确。
- `OnRun` 中仍保留 `print(isRunning)`，冲刺这种高频输入调试日志完成后应移除。
- 鼠标水平旋转使用了 `sensitivity * lookInput.x * Time.deltaTime`，Mouse Delta 本身通常已经是每帧变化量，是否乘 `Time.deltaTime` 要结合手感验证，后续需要形成稳定方案。
- 玩家控制器职责开始变多：移动、旋转、鼠标视角、光标锁定、冲刺都集中在一个类里。当前练习阶段可以接受，但后续进入动画、技能、交互时需要逐步拆分。
- `Lesson02.cs` 目前是空脚本，可以作为第六题 Rigidbody 跳跃的新练习脚本，或者后续清理不用的空脚本。

### 推荐改进方向
- 进入第六题前，先明确玩家对象上是否准备使用 Rigidbody，并确认移动方式暂时仍用 Transform，跳跃先只处理竖直方向。
- 把 `jumpForce`、`Rigidbody rb`、`Jump` 输入单独加入，先做“一次按下触发一次向上冲量”。
- 后续在第七题再做 grounded 检测，不要在第六题一次性塞太多功能。
- 逐渐把公开字段改成 `[SerializeField] private`，让 Inspector 可调，同时减少外部脚本随意改值。

### 当前阶段总结
前五题已经完成了从 Transform 基础控制到 Input System 玩家控制器雏形的过渡。当前代码不像“最终正式项目”，但已经像一个可继续演进的练习型 PlayerController：输入状态、冲刺状态、摄像机俯仰、生命周期订阅这些关键点都已经出现。下一阶段应该进入物理和地面判断，开始理解 Unity 角色控制的两条路线：Rigidbody 与 CharacterController。

### 下一阶段建议
第六题继续做 Rigidbody 跳跃。重点不是写很多代码，而是理解：跳跃为什么交给物理组件、`AddForce` 和直接改 `transform.position` 的差异、一次性输入和持续性输入的处理方式差异。
