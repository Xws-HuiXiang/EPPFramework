require "FSM/FSMStateBase"

ExampleState3 = FSMStateBase:New(nil);

--- 构建状态对象
--- @param fsm table 有限状态机
--- @returns 返回创建的状态对象
function ExampleState3:New(fsm)
    local o = {
        stateID = FSMSystem.StateID.ExampleState3,
        fsm = fsm
    };
    setmetatable(o, {__index = self});

    return o;
end

function ExampleState3:DoBeforeEntering()
    FDebugger.Log("状态3进入");
end

function ExampleState3:DoBeforeLeaving()
    FDebugger.Log("状态3退出");
end

function ExampleState3:Reason()
    FDebugger.Log("状态3Reason函数执行");
end

function ExampleState3:CarryOut()
    FDebugger.Log("状态3CarryOut函数执行");
end