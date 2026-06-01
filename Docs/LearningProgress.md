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

## 2026-05-26 第六题代码检查：Rigidbody 跳跃

### 当前题目
第 6 题：Rigidbody 跳跃。

### 本题目标
在现有玩家控制器 `Assets/Scripts/Lesson01.cs` 中，通过 Unity Input System 的按钮输入触发一次跳跃，并使用 `Rigidbody.AddForce` 实现竖直方向的物理跳跃，不直接修改 `transform.position`。

### 我做得好的地方
- 已经在场景中的玩家对象上添加了 `Rigidbody`，开始进入 Unity 物理控制思维。
- 在脚本中通过 `GetComponent<Rigidbody>()` 获取刚体，并使用 `AddForce(Vector3.up * jumpForce, ForceMode.Impulse)` 实现跳跃，符合本题“单次按下立刻起跳”的目标。
- `jumpForce` 使用 `[SerializeField] private`，这很接近正式项目习惯：Inspector 可调，但外部脚本不能随意访问。
- 继续保持了 Input System 的事件订阅 / 取消订阅流程，跳跃输入也放进了 `OnEnable` / `OnDisable` 生命周期管理。
- 注释中主动记录了 Force、Acceleration、Impulse、VelocityChange 的对比，并补充了 `GameDev_KnowledgeBase/物理系统/Unity_Rigidbody_ForceMode_学习记录.md`，这说明你不只是完成效果，也在追问物理模式背后的原因。

### 当前存在的问题
- 跳跃输入仍然绑定在 `Interact` action 上。第 5 题已经出现过“冲刺/功能输入命名不贴合语义”的问题，这次又用 `Interact` 来做 `Jump`，属于重复问题。Gameplay 项目里 action 名称应该表达玩家意图，建议新增 `Jump` action。
- `rigidBody` 在 `Start` 中获取，而输入订阅在 `OnEnable` 中完成。正常情况下问题不大，但生命周期上更稳的写法是把组件引用放在 `Awake`，或者加 `[RequireComponent(typeof(Rigidbody))]` 明确依赖。
- 当前玩家水平移动仍然使用 `transform.Translate`，同时玩家对象已经是非 kinematic Rigidbody。练习阶段可以接受，但正式项目中混用 Transform 位移和 Rigidbody 物理对象，容易出现碰撞、穿透或物理同步问题。
- `OnRun` 里仍保留 `print(isRunning)`，这是前面已经记录过的调试输出问题，后续要养成验证完就清理的习惯。
- 当前没有做 grounded 检测，空中连跳是本题允许范围，但下一题需要立刻补上这个限制。

### 推荐改进方向
- 在 Input Actions 中新增 `Jump` action，不要继续复用 `Interact`。
- 把 Rigidbody 依赖写得更明确：可以考虑在类上加 `[RequireComponent(typeof(Rigidbody))]`，并在 `Awake` 中缓存 `rigidBody`。
- 下一题先做 grounded 检测，只解决“是否允许跳跃”的问题，不急着重构完整控制器。
- 后续进入 Rigidbody 或 CharacterController 正式移动方案时，再统一处理水平移动和物理系统的关系。

### 当前阶段总结
第六题已经达成目标：你完成了 Input System 按钮触发 + Rigidbody 瞬时跳跃，并通过额外学习记录理解了 `ForceMode` 的差异。现在已经从纯 Transform 玩家控制进入了物理系统门口。接下来最重要的不是继续加功能，而是补上“什么时候可以跳”的判断，也就是 grounded 检测。

### 下一阶段建议
第七题建议做 Grounded 地面检测：判断玩家是否在地面上，只有 grounded 时才允许跳跃，避免无限空中跳。

### 检查后修正
- 已将 Input Actions 中原本复用的 `Interact` 改名为 `Jump`，并同步更新生成的 `PlayerSystem_Actions.cs`。
- `Lesson01.cs` 中跳跃订阅已改为 `playerController.Player.Jump.performed += OnJump`，取消订阅也同步改为 `Jump`。
- `Rigidbody` 引用已从 `Start` 移动到 `Awake`，组件引用初始化时机更稳。
- 这次修正解决了“输入 action 命名不贴合 Gameplay 语义”的重复问题，是一次很好的检查后迭代。

## 2026-06-01 第七题代码检查：Grounded 地面检测

### 当前题目
第 7 题：Grounded 地面检测。

### 本题目标
在 Rigidbody 跳跃基础上加入地面检测，让角色只有检测到地面时才能执行跳跃，避免空中无限跳。

### 我做得好的地方
- 已经使用 `Physics.Raycast` 从玩家位置向下检测地面，并把检测结果接入跳跃条件。
- 通过实际现象定位到“刚开始附近不能跳”不是输入问题，而是射线长度过短，并把距离从 `1f` 调整到 `1.2f`，这是很好的 Debug 过程。
- 使用 `LayerMask.GetMask("Ground")` 过滤地面，开始接触 LayerMask 思维，比不加筛选的射线更接近正式项目。
- 在 `OnJump` 中加入 `ctx.performed && isGrounded` 判断，并在跳跃后把 `isGrounded` 设为 `false`，已经能防止连续空中跳。

### 当前存在的问题
- 当前 grounded 检测只在按下跳跃键时执行。功能上可以阻止空中连跳，但还不算真正的“持续更新的 grounded 状态”。正式玩家控制器通常会在 `Update` 或 `FixedUpdate` 中持续检测。
- `isGrounded` 在射线未命中时没有直接赋值为 `false`，目前依赖跳跃后手动置 false。后续如果加入落地动画、下落状态、坡道、离地检测，这个状态会不够可靠。
- `LayerMask.GetMask("Ground")` 写在检测调用里，每次跳跃都会重新按字符串查找。练习阶段可以接受，正式写法更推荐 `[SerializeField] private LayerMask groundLayer;`。
- `Debug.DrawRay` 和 `Debug.Log` 已经注释掉，但长期保留较多调试注释会让控制器越来越乱，后续完成验证后应逐步清理。
- 从项目文件看，`ProjectSettings/TagManager.asset` 和 `SampleScene.unity` 中暂时没有记录 Ground Layer / 地面 Layer 的变更。如果你是在 Unity Editor 中配置好的，需要确认已经保存 Project Settings 和 Scene，否则 GitHub 上的项目可能缺少这部分配置。

### 推荐改进方向
- 下一步把地面检测拆成单独方法，例如 `CheckGrounded()`，在 `Update` 或 `FixedUpdate` 中持续更新 `isGrounded`。
- 把检测距离、ground layer 做成 `[SerializeField]` 字段，避免魔法数字和字符串散落在逻辑中。
- 可以继续先用 Raycast，但要理解脚底检测点比角色中心点更稳定；之后可以尝试 `CheckSphere` 作为更宽容的地面检测方案。
- 保存并确认 `Ground` Layer 与地面对象的配置进入 Git 版本记录。

### 当前阶段总结
第七题已经完成基础功能：跳跃不再是无条件触发，而是开始依赖地面检测。你也通过射线长度问题理解了“检测起点、检测距离、角色碰撞体尺寸”之间的关系。当前代码是一个可运行的第一版 grounded 判断，但还需要从“按键时检测一下”进化到“持续维护角色状态”。

### 下一阶段建议
建议先做第七题小优化：把地面检测整理成持续更新的 `CheckGrounded()`，并把 `groundLayer` / `groundCheckDistance` 暴露到 Inspector。完成后再进入 CharacterController。

## 2026-06-01 第七题优化检查：持续 Grounded 状态维护

### 当前题目
第 7 题优化：持续维护 `isGrounded` 状态。

### 本题目标
把地面检测从 `OnJump` 中拆出来，改为每帧持续更新 grounded 状态，让 `OnJump` 只负责响应跳跃输入并读取状态。

### 我做得好的地方
- 已经新增 `CheckGrounded()`，并在 `Update` 中持续调用，`isGrounded` 现在由 Raycast 结果直接赋值。
- `OnJump` 中已经不再执行 `Physics.Raycast`，职责变得更清楚：输入回调只判断 `ctx.performed && isGrounded` 并触发跳跃。
- `groundCheckDistance` 和 `groundMask` 已改为 `[SerializeField]` 字段，并在 Scene 中配置了 `groundCheckDistance: 1.01` 与 Ground LayerMask。
- `ProjectSettings/TagManager.asset` 已保存 `Ground` Layer，`SampleScene.unity` 中 Plane 已设置为 Ground Layer，说明这次 Inspector / Project Settings 配置进入了 Git。
- 额外整理了 `GameDev_KnowledgeBase/物理系统/Lesson07_GroundedState_StudyRecord.md`，记录了“持续状态”和“连跳理论风险”的完整思考过程。

### 当前存在的问题
- `groundCheckDistance` 从代码默认值 `1f` 到场景配置 `1.01` 仍然比较贴边。当前能工作，但如果角色高度、缩放、Collider 或地面起伏变化，可能再次出现检测不稳定。
- 当前仍从 `transform.position` 向下发射射线。对 Capsule 来说这是从中心点检测，后续更稳定的方式是使用脚底附近的 `groundCheck` 点，或改用 `CheckSphere`。
- `Update` 中仍保留了注释掉的 `Debug.DrawRay`，验证完成后可以清理，或在需要时用 Gizmos / 条件调试显示。
- `OnRun` 中的 `print(isRunning)` 仍然是之前遗留的高频调试输出问题，后续整理玩家控制器时应该删除。

### 推荐改进方向
- 暂时接受当前 Raycast 方案，进入 CharacterController 前可以先做一次小清理：删除无用调试注释，统一命名和公开字段风格。
- 后续学习 CharacterController 时，重点比较 `CharacterController.isGrounded` 与自定义 Raycast / CheckSphere grounded 的差异。
- 如果未来继续用 Rigidbody 路线，可以尝试加入“向上速度时不算稳定 grounded”或更短检测距离，解决刚起跳瞬间理论上仍可能检测到地面的边界问题。

### 当前阶段总结
第七题已经从“按跳跃时临时检测地面”升级为“持续维护 grounded 状态”。这一步很关键，因为 `isGrounded` 以后不只是跳跃条件，还会服务于动画、下落、落地反馈、二段跳重置和 CharacterController 学习。

### 下一阶段建议
下一题建议进入 CharacterController 基础移动：先实现不依赖 Rigidbody 的水平移动和重力处理，再对比当前 Rigidbody + Raycast 路线。
