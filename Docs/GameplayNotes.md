# GameplayLab Gameplay Notes

用于记录已经形成固定方案、后续可以复用的 Unity Gameplay 写法。

## Input System

- 持续性输入，例如移动方向，适合缓存为状态变量，例如 `Vector2 moveInput`。
- 一次性输入，例如跳跃、交互、攻击，适合在 `performed` 时触发。
- 按住状态，例如冲刺、瞄准，适合用 `performed` 设置 `true`，用 `canceled` 设置 `false`。
- 输入命名应贴合 Gameplay 语义，例如 `Sprint` 比 `Interact` 更适合作为冲刺动作名。

## Player Controller

- 移动速度、旋转速度、鼠标灵敏度、冲刺速度、跳跃力度应拆成独立配置项。
- 玩家水平旋转和摄像机上下俯仰应该分开处理。
- 摄像机俯仰角应使用 Clamp 限制，避免翻转。
- `OnEnable` 中订阅输入事件，`OnDisable` 中取消订阅输入事件，是 Input System 组件化写法里非常重要的生命周期习惯。
- 练习阶段可以把移动、视角、冲刺放在一个脚本中理解整体流程；当加入动画、交互、技能后，应考虑按职责拆分。

## 下一阶段重点

- Rigidbody 跳跃：使用物理组件处理竖直方向速度或冲量。
- Grounded 检测：限制跳跃触发条件，避免无限空中跳。
- CharacterController：理解非 Rigidbody 角色控制器的常见项目写法。

## Rigidbody 与 ForceMode

- 一次性跳跃更适合使用 `ForceMode.Impulse` 或 `ForceMode.VelocityChange`，因为它们会在一次调用中产生明显速度变化。
- `ForceMode.Force` 和 `ForceMode.Acceleration` 更适合持续施力，单次按钮触发通常效果很小。
- `Force` 和 `Impulse` 会受到 Rigidbody mass 影响；`Acceleration` 和 `VelocityChange` 不受 mass 影响。
- `AddForce` 的参数在不同 `ForceMode` 下含义不同，不是 Unity 自动把同一个力拆成多份。
- Drag / Linear Damping 更像速度衰减，不是会把物体反向推回去的固定阻力。

## Grounded 检测

- Raycast 地面检测要同时关注起点、方向、距离和 LayerMask。
- Capsule 角色从中心点向下检测时，距离应略大于脚底距离，避免刚好贴边导致检测不稳定。
- `isGrounded` 应作为持续维护的 Gameplay 状态，而不是只在跳跃输入触发时临时计算。
- `LayerMask.GetMask("Ground")` 适合理解概念，后续更推荐 `[SerializeField] private LayerMask groundLayer;` 由 Inspector 配置。
- `OnJump` 应读取 grounded 状态并触发跳跃，不应承担地面检测职责。
- 持续维护的 grounded 状态可以被跳跃、动画、下落、落地反馈、二段跳重置等多个系统复用。
- 刚起跳瞬间理论上仍可能被射线检测为 grounded，后续可通过缩短检测距离、脚底检测点、速度方向判断或离地缓冲来处理。

## CharacterController

- `CharacterController` 不使用 Rigidbody 自动重力；水平速度、竖直速度和重力需要脚本自己维护。
- `CharacterController.Move()` 接收的是“本帧位移”，如果当前变量表示速度，就在传入 Move 前统一乘 `Time.deltaTime`。
- 地面贴合常见写法是在 grounded 且竖直速度为负时，把 `velocity.y` 重置为一个小负数，例如 `-2f`。
- `CharacterController` 适合做可控性强的角色移动；Rigidbody 更接近物理驱动，两条路线要分清。
- 一个测试角色通常保留 `CharacterController` 即可，不需要额外 `Rigidbody` 或多余 `CapsuleCollider`。
- 使用目标跳跃高度控制 CharacterController 跳跃时，可用 `velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity)` 计算起跳初速度。公式来自最高点竖直速度为 0 的运动学关系。
- `jumpHeight` 表示期望高度，`jumpSpeed` 表示起跳初速度。直接写 `velocity.y = jumpSpeed` 也可以，但参数含义和调参方式不同。
- 起跳后持续执行 `velocity.y += gravity * Time.deltaTime`，竖直速度会经历正数、0、负数，分别对应上升、最高点和下落。
- 普通跳跃通常应作为一次性输入请求处理；如果把它长期保存为按住状态，角色可能在落地时自动再次起跳。
