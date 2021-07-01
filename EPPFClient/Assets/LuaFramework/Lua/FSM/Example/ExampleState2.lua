require "FSM/FSMStateBase"

ExampleState2 = FSMStateBase:New(nil);

--- 构建状态对象
--- @param fsm table 有限状态机
--- @returns 返回创建的状态对象
function ExampleState2:New(fsm)
    local o = {
        stateID = FSMSystem.StateID.ExampleState2,
        fsm = fsm
    };
    setmetatable(o, {__index = self});

    return o;
end

function ExampleState2:DoBeforeEntering()
    FDebugger.Log("状态2进入");
end

function ExampleState2:DoBeforeLeaving()
    FDebugger.Log("状态2退出");
end

function ExampleState2:Reason()
    FDebugger.Log("状态2Reason函数执行");
end

function ExampleState2:CarryOut()
    FDebugger.Log("状态2CarryOut函数执行");
end