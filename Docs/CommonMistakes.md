# GameplayLab Common Mistakes

用于记录反复出现的问题。只有当同类问题在代码检查中重复出现，或明显值得长期提醒时再追加。

## 已观察到的早期倾向

- 不要把移动速度、旋转速度、冲刺速度混用成同一个变量。
- 不要用 `speed += runSpeed` 这类累加方式处理按住状态，优先使用 `isRunning` 这类状态变量决定当前速度。
- 不要在高频 `Update` 中反复调用 `Camera.main`。
- 高频输入或移动逻辑中的临时 `Debug.Log` 不应长期保留。

## 2026-05-26 前五题复盘补充

- 命名要表达 Gameplay 含义，例如 `maxCla` 应改成类似 `maxPitchAngle`，以后读代码时才能立刻知道它限制的是摄像机俯仰角。
- 练习完成后的旧代码注释块要适时清理，否则一个玩家控制器里同时堆旧输入系统和新输入系统，会影响后续排查问题。
- 调试输出只服务于验证阶段，例如 `print(isRunning)` 验证完冲刺状态后应删除或临时注释。
- 同一个脚本职责开始变多时，要警惕后续继续把动画、技能、交互都塞进去；练习阶段可以先集中，项目阶段要逐步拆分。

## 2026-05-26 第六题复盘补充

- 输入 action 命名需要贴合 Gameplay 语义。第 5 题冲刺曾经出现过命名不合适的问题，第 6 题又把跳跃绑定到 `Interact`，后续应新增 `Jump` action，而不是复用无关 action。
- 已经挂上 Rigidbody 的动态物体，不建议长期继续用 `transform.Translate` 处理水平移动。练习阶段可以先过渡，但正式控制器应统一移动方案。
- 组件依赖要尽量显式。脚本依赖 Rigidbody 时，可以考虑 `[RequireComponent(typeof(Rigidbody))]`，避免 Inspector 漏挂组件后运行时报错。

## 2026-06-01 第七题复盘补充

- 物理检测参数不要写得太极限。Capsule 中心点向下检测时，距离刚好等于半高容易因为浮动、碰撞体尺寸或起点误差导致检测失败。
- Gameplay 状态最好持续更新，而不是只在输入触发时临时判断。`isGrounded` 后续应在帧循环中维护。
- Layer 配置属于项目数据的一部分。Editor 里改完 Layer 和对象 Layer 后，要确认 Project Settings / Scene 已保存并进入 Git。

## 2026-06-01 第七题优化补充

- 射线检测距离不要过度依赖“刚好能用”的数值。`1.01` 对当前 Capsule 可以工作，但仍属于接近边界的参数。
- 临时调试注释验证完要清理，否则 PlayerController 会逐渐堆积历史实验痕迹。

## 2026-06-03 第八题复盘补充

- 使用 `CharacterController.Move()` 时要统一单位：速度最后乘一次 `Time.deltaTime`，不要把“已乘 deltaTime 的位移”和“未乘 deltaTime 的速度”混加。
- `CharacterController` 和 `Rigidbody` 是两条不同角色控制路线，测试对象上不应同时依赖 Rigidbody。
- `CharacterController` 自带胶囊碰撞能力，通常不需要再额外挂一个 `CapsuleCollider`。
- 手动使用生成的 Input Actions 类时，场景上的 `PlayerInput` 组件可能是多余的，要避免输入接入方式混杂。
