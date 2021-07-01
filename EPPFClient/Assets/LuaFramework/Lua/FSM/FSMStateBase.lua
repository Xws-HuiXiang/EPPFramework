FSMStateBase = {};
local this = FSMStateBase;

function FSMStateBase:New(fsm)
    local o = {
        stateID = FSMSystem.StateID.NullState,
        map = {},
        fsm = fsm
    }
    setmetatable(o, {__index = self})

    return o;
end

--- 保存所有转换条件的映射。数据格式：string(转换条件ID的字符串作为key):integer(转换的目标状态ID为value)
--self.map = {};
--- 当前持有该状态的FSMSystem
--self.fsm = nil;

--- 当前状态的唯一ID
--- @returns 返回当前状态的唯一ID
function FSMStateBase:GetStateID()
    return self.stateID;
end

--- 向条件-状态映射中添加状态转换条件
--- @param transID number 转换条件ID
--- @param targetStateID number 转换为哪个状态的ID
--- @returns 无返回值
function this:AddTransition(transID, targetStateID)
    if(transID == nil)then
        return;
    end
    if(targetStateID == nil)then
        return;
    end

    --不能添加空转换条件
    if(transID == FSMSystem.Transition.NullTransition)then
        FDebugger.LogError("尝试向条件-状态映射中添加空转换条件");

        return;
    end
    --不能添加空状态ID
    if(targetStateID == FSMSystem.StateID.NullState)then
        FDebugger.LogError("尝试向条件-状态映射中添加空状态ID");

        return;
    end

    --不能重复添加
    for k, v in pairs(self.map) do
        if(k == transID)then
            FDebugger.LogWarningFormat("添加转换映射时，转换条件：[{0}]重复添加。当前StateID：[{1}]准备添加的StateID：[{2}]", {transID, v, targetStateID});

            return;
        end
    end

    --成功添加一个转换条件
    local stringKey = this:GetTransitionStringID(transID);
    self.map[stringKey] = targetStateID;
end

--- 从条件-状态映射中移除转换条件
--- @param transID number 转换条件ID
--- @returns 无返回值
function this:DeleteTransition(transID)
    if(transID == nil)then
        return;
    end

    --不能移除空转换条件
    if(transID == FSMSystem.Transition.NullTransition)then
        FDebugger.LogError("尝试移除空转换条件");

        return;
    end

    local index = 1;
    local canRemove = false;
    local stringKey = this.GetTransitionStringID(transID);
    for k, v in pairs(self.map) do
        if(k == transStringID)then
            --找到了需要移除的转化条件ID，记录下这个索引
            canRemove = true;
            break;
        end

        index = index + 1;
    end
    if(canRemove)then
        table.remove(self.map, index);
    else
        --没有找到对应的转换条件
        FDebugger.LogWarningFormat("没有找到转换条件：{0}", {transID});
    end
end

--- 指定一个转换条件，返回这个条件可以转换为的StateID
--- @param transID number 转换条件ID
--- @returns 从map中查找指定条件的状态ID并返回
function this:GetOutputState(transID)
    if(transID == nil)then
        return;
    end

    local stringKey = this:GetTransitionStringID(transID);
    for k, v in pairs(self.map) do
        if(k == stringKey)then
            return v;
        end
    end

    --没有找到对应的转换条件
    FDebugger.LogWarning("查找转换ID对应的状态时，没有找到转换条件：" .. transID .. "，当前状态为：" .. self.stateID);

    return FSMSystem.StateID.NullState;
end

--- 当进入这个状态时会调用这个方法
function this:DoBeforeEntering()

end

--- 当离开这个状态时会调用这个方法
function this:DoBeforeLeaving()

end

--- 这个函数用来判断当前是否需要转换状态（自己手动调用，帧函数）
function this:Reason()

end

--- 这个函数用来实现在当前状态下需要执行什么操作（自己手动调用，帧函数）
function this:CarryOut()

end

--- 返回在map中的key的值
function this:GetTransitionStringID(transID)
    if(transID ~= nil)then
        return "_" .. transID;
    end
end
