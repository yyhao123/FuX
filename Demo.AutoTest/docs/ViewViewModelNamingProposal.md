# Demo.AutoTest View / ViewModel 命名整理建议

> 目标：统一 `View` 与 `ViewModel` 的命名规则，降低维护成本，避免同名/错名导致的理解和导航困难。

## 1. 现状问题（基于当前代码）

1. **文件名与类型名不一致**
   - `ViewModels/Module/AcquireModuleViewModel.cs` 中实际类型为 `AcquireModuleDataInfo`，不是 `AcquireModuleViewModel`。
2. **同名 ViewModel 重复且语义冲突**
   - `ViewModels/userControls/AcquireModuleViewModel.cs` 中存在真正的 `AcquireModuleViewModel`。
   - 与上面的文件名冲突，导致“一个名字两层含义”。
3. **大小写/风格不统一**
   - `TopLeftBarView`、`TopRightBarView`、`LeftBottomBarView`、`RightBottomBarView` 采用小写开头，未使用 PascalCase。
4. **拼写不明确**
   - `RightParamView` / `RightParamViewModel` 中 `Pram` 疑似 `Param` 的拼写简写，不利于团队理解和搜索。
5. **目录语义与命名不对齐**
   - 已有 `view/Module` + `ViewModels/Module`，也有 `view/userControls` + `ViewModels/userControls`；
   - 但类型命名有时体现“模块”（Module），有时体现“控件”（View），粒度不一致。

## 2. 推荐命名规范（建议统一执行）

### 2.1 通用规则

- **类名使用 PascalCase**。
- **View 以 `View` 结尾**，例如 `MainView`、`TopLeftBarView`。
- **ViewModel 以 `ViewModel` 结尾**，且与 View 一一对应。
- **纯数据/状态模型**不放在 `ViewModel` 命名里，使用 `State` / `Model` / `DataInfo` 后缀。
- **文件名 = 主类型名**（强制）。

### 2.2 目录建议

- `view/Module/*View.xaml`
- `ViewModels/Module/*ViewModel.cs`
- `view/userControls/bars/*View.xaml`
- `ViewModels/userControls/bars/*ViewModel.cs`
- `ViewModels/state/*State.cs`（承载页面状态）

## 3. 具体重命名建议（可直接落地）

### 3.1 明确区分“页面逻辑”与“状态模型”

- `ViewModels/Module/AcquireModuleViewModel.cs`（当前类型：`AcquireModuleDataInfo`）
  - **改为**：`ViewModels/state/AcquireModuleState.cs`
  - 类型名：`AcquireModuleState`
- 在 `ViewModels/userControls/AcquireModuleViewModel.cs` 中：
  - `private AcquireModuleDataInfo _model` -> `private AcquireModuleState _state`
  - `public AcquireModuleDataInfo Model` -> `public AcquireModuleState State`

### 3.2 bars 子视图统一 PascalCase

- `TopLeftBarView` -> `TopLeftBarView`
- `TopRightBarView` -> `TopRightBarView`
- `LeftBottomBarView` -> `LeftBottomBarView`
- `RightBottomBarView` -> `RightBottomBarView`

对应 ViewModel（保持对称）：

- `TopLeftBarViewModel` -> `TopLeftBarViewModel`（可选）
- `TopRightBarViewModel` -> `TopRightBarViewModel`
- `LeftBottomBarViewModel` -> `LeftBottomBarViewModel`
- `RightBottomBarViewModel` -> `RightBottomBarViewModel`

### 3.3 `Pram` 统一为 `Param`（或 `Parameter`）

- `RightParamView` -> `RightParamView`（或 `RightParameterView`）
- `RightParamViewModel` -> `RightParamViewModel`（或 `RightParameterViewModel`）
- 同步替换变量、字段、方法中 `Pram`。

## 4. 渐进式迁移策略（避免一次性大改）

1. **第一阶段：新增别名并兼容**
   - 新建规范命名类型，旧类型保留（可加 `[Obsolete]`）。
2. **第二阶段：批量替换引用**
   - 优先替换构造注入、XAML `x:Class`、`DataContext` 绑定、DI 注册。
3. **第三阶段：删除旧命名**
   - 清理旧文件与旧类型，确保文件名与类型名一致。
4. **第四阶段：加入约束**
   - 在代码评审规范中增加“命名一致性”检查项；必要时引入 Roslyn Analyzer。

## 5. 建议优先级

- **P0（马上改）**：
  - 解决 `AcquireModuleViewModel.cs` 与 `AcquireModuleDataInfo` 的错位命名。
- **P1（尽快改）**：
  - `sub*View` PascalCase 化。
- **P2（持续改进）**：
  - `Pram` -> `Param` 全量规范化。

---

如果你愿意，我可以在下一步按“**P0 最小可行改动**”直接提交一版可编译的重构补丁（含重命名和引用更新）。
