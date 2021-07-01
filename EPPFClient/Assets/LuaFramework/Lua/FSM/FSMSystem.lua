--[[
    这个类是Lua版的有限状态机实现。
    亲测可用（
    目前项目里其实没有用到，只不过是之前面试时被问到“框架里有什么设计模式”之后，回来就想塞进来，仅此而已...
--]]

FSMSystem = {};
local this = FSMSystem;

function FSMSystem:New()
    local o = o or {};
    setmetatable(o, self);
    self.__index = self;

    return o;
end

--- 所有状态转换条件
this.Transition = {
    NullTransition = 0,
    OneToTwo = 1,
    TwoToThree = 2,
    ThreeToTwo = 3,
    TwoToOne = 4
}

--- 所有状态ID
this.StateID = {
    NullState = 0,
    ExampleState1 = 1,
    ExampleState2 = 2,
    ExampleState3 = 3,
}

--- 所有状态对象的列表
this.states = {};

local currentStateID = -1;
--- 获取当前状态的ID
--- @returns 返回状态对应的枚举ID
function this:GetCurrentStateID()
    return currentStateID;
end
local currentState = nil;
--- 获取当前状态的对象
--- @returns 返回当前状态的对象
function this:GetCurrentState()
    return currentState;
end

--- 向状态机中添加一个状态。如果是第一个添加的状态则设置为默认状态
--- @param state table 状态ID
--- @returns 无返回值
function this:AddState(state)
    if(state == nil)then
        return;
    end

    local stateID = state:GetStateID();

    --不能添加空状态
    if(stateID == FSMSystem.StateID.NullState)then
        FDebugger.LogError("不能向状态机中添加空状态");
    end

    --数量等于0说明还没有状态，设置为默认状态
    local stateAmount = #this.states;
    if(stateAmount == 0) then
        --设置为默认状态
        currentState = state;
        currentStateID = state:GetStateID();
        table.insert(this.states, state);
    else
        --不是默认状态，判断是否添加重复
        for k, v in pairs(this.states) do
            if(v:GetStateID() == stateID)then
                --ID有重复的情况，给出警告
                FDebugger.LogWarningFormat("尝试向状态机中添加重复的状态。StateID为：{0}", {stateID})

                return;
            end
        end

        --确认没有重复，添加到表中
        table.insert(this.states, state);
    end
end

--- 从状态机中移除一个状态
--- @param stateID number 状态ID
--- @returns 无返回值
function this:DeleteState(stateID)
    if(stateID == nil)then
        return;
    end

    --不能移除空状态
    if(stateID == FSMSystem.StateID.NullState)then
        FDebugger.LogError("不能移除默认的空状态");
    end

    local index = 1;
    local canRemove = false;
    for k, v in pairs(this.states) do
        if(v:GetStateID() == stateID)then
            canRemove = true;
            break;
        end

        index = index + 1;
    end
    if(canRemove)then
        table.remove(this.states, index);
    end
end

--- 执行状态转换
--- @param transID number 转换条件ID
--- @returns 无返回值
function this:PreformTransition(transID)
    if(transID == nil)then
        return;
    end

    --空转换不可以用在实际转换中
    if(transID == FSMSystem.Transition.NullTransition)then
        FDebugger.LogError("空转换不可以用在实际转换中");

        return;
    end

    local stateID = FSMSystem:GetCurrentState():GetOutputState(transID);
    if(stateID == FSMSystem.StateID.NullState) then
        --没有指定的转换条件
        FDebugger.LogError("状态StateID："..FSMSystem:GetCurrentStateID().."在转换时指定的转换条件为："..transID.."  没有对应的目标状态")

        return;
    else
        --调用状态离开的方法和新状态的进入状态方法
        currentStateID = stateID;
        --先找到对应状态的对象，然后调用离开和进入状态的方法
        for k, v in pairs(this.states)do
            if(v:GetStateID() == this:GetCurrentStateID())then
                this:GetCurrentState():DoBeforeLeaving();
                currentState = v;
                this:GetCurrentState():DoBeforeEntering();

                break;
            end
        end
    end
end
