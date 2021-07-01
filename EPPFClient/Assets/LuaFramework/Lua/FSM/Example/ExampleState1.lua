require "FSM/FSMStateBase"

ExampleState1 = FSMStateBase:New(nil);

--- 构建状态对象
--- @param fsm table 有限状态机
--- @returns 返回创建的状态对象
function ExampleState1:New(fsm)
    local o = {
        stateID = FSMSystem.StateID.ExampleState1,
        fsm = fsm
    };
    setmetatable(o, {__index = self});

    return o;
end

function ExampleState1:DoBeforeEntering()
    FDebugger.Log("状态1进入");
end

function ExampleState1:DoBeforeLeaving()
    FDebugger.Log("状态1退出");
end

function ExampleState1:Reason()
    FDebugger.Log("状态1Reason函数执行");
end

function ExampleState1:CarryOut()
    FDebugger.Log("状态1CarryOut函数执行");
end