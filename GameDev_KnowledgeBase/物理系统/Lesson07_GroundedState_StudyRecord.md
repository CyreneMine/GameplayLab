# Lesson07 学习记录 - Grounded 状态维护与连跳问题思考

日期：2026-06-01

## 问题1：为什么要把 Grounded 检测改成持续检测？

### 我的疑问

老师要求把 Grounded 检测改成持续维护状态，而不是在按下跳跃键时临时检测。

我的疑问：

- 为什么一定要每帧检测？
- 直接在按下跳跃键时检测一次不是更省性能吗？
- 每帧 Raycast 会不会造成性能浪费？

### AI 的回答

Grounded 是角色状态（State），不是只为跳跃服务。

未来很多系统都会依赖它：

- 动画状态机
- 下落状态
- 落地音效
- 落地特效
- 二段跳重置
- CharacterController

因此：

```csharp
isGrounded
```

应该被持续维护，而不是需要时临时计算。

### 关于性能

对于单个玩家角色：

- 60FPS ≈ 每秒60次 Raycast
- 开销极低
- 属于正常开发方式

真正需要担心的是大量 AI 同时进行物理检测。

---

## 问题2：为什么修改后连跳消失了？

### 我的发现

原本代码：

```csharp
if (Physics.Raycast(...))
{
    isGrounded = true;
}
```

修改后：

```csharp
private void CheckGrounded()
{
    isGrounded = Physics.Raycast(...);
}
```

然后在 Update 中调用：

```csharp
CheckGrounded();
```

结果发现：

原本会出现连跳。

修改后没有出现。

### 我的推测

我认为：

Update 执行速度太快。

角色刚起跳：

```csharp
isGrounded = false;
```

但人物还没离开射线范围。

于是下一帧：

```csharp
Physics.Raycast(...)
```

再次检测到地面。

导致：

```csharp
isGrounded = true;
```

覆盖掉跳跃时设置的 false。

### AI 的分析

我的方向接近正确。

但真正原因不是：

```text
封装成函数
```

而是：

```text
赋值逻辑发生变化
```

原本：

```csharp
if (Physics.Raycast(...))
{
    isGrounded = true;
}
```

等价于：

- 检测到地面 → true
- 检测不到地面 → 保持原值

修改后：

```csharp
isGrounded = Physics.Raycast(...);
```

等价于：

- 檢测到地面 → true
- 检测不到地面 → false

真正修复问题的是状态被持续同步。

---

## 问题3：理论上不还是可能连跳吗？

### 我的疑问

即使持续维护：

```csharp
isGrounded = Physics.Raycast(...);
```

角色刚起跳的前几帧：

- 已经离地
- 但还没离开射线范围

此时：

```csharp
Physics.Raycast(...)
```

仍然可能返回：

```csharp
true
```

如果手速足够快。

理论上仍然可能触发第二次跳跃。

### AI 的回答

这个判断是正确的。

理论上确实仍然存在这种可能。

因为：

```text
射线检测到地面
≠
角色已经稳定站在地面
```

---

## AI 提出的方案

### 方案一

起跳后短时间内：

```csharp
强制 isGrounded = false;
```

例如：

```text
0.1 秒
```

### 方案二

缩短：

```csharp
groundCheckDistance
```

让角色起跳后更快离开检测范围。

### 方案三

结合速度判断：

```csharp
rigidbody.velocity.y
```

只有：

- 检测到地面
- 速度向下

才算 grounded。

---

## 我的思考

我没有选择方案一。

因为考虑到未来需要扩展：

- 二段跳
- 跳跃次数统计
- 落地重置逻辑

如果加入：

```text
跳跃后 0.1 秒强制不检测地面
```

这种时间锁机制。

未来可能引入额外状态管理。

因此我选择：

## 方案二

通过调整：

```csharp
groundCheckDistance
```

解决问题。

---

## AI 对我选择的评价

AI认为：

对于当前学习阶段来说：

这个选择是合理的。

优点：

- 逻辑简单
- 不引入额外状态
- 不影响未来二段跳设计
- 容易理解

同时指出：

方案二本质属于参数调优。

未来模型高度、碰撞体大小、坡道等因素变化后，可能还需要进一步优化。

---

## 本次最大的收获

以前思考方式：

```text
功能能跑就行
```

本次开始思考：

```text
为什么这样设计
职责应该归谁负责
未来功能扩展会不会出问题
这个方案是否影响后续系统
```

特别是在讨论二段跳时。

在 AI 给出方案后。

我没有直接采用。

而是主动思考：

```text
如果以后扩展功能呢？
```

然后根据未来需求选择更适合自己的方案。

这是一次从：

```text
实现功能
```

向：

```text
设计 Gameplay 逻辑
```

思维方式的转变。
