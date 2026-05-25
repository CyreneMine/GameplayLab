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
