# GameplayLab Common Mistakes

用于记录反复出现的问题。只有当同类问题在代码检查中重复出现，或明显值得长期提醒时再追加。

## 已观察到的早期倾向

- 不要把移动速度、旋转速度、冲刺速度混用成同一个变量。
- 不要用 `speed += runSpeed` 这类累加方式处理按住状态，优先使用 `isRunning` 这类状态变量决定当前速度。
- 不要在高频 `Update` 中反复调用 `Camera.main`。
- 高频输入或移动逻辑中的临时 `Debug.Log` 不应长期保留。

