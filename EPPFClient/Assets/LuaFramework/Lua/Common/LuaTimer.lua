LuaTimer = {};
local this = LuaTimer;

--表中存储一个表:{id="", timer = 0, time = 0, action}
local loopAction = {};

--- 添加间隔一定时间执行的函数
--- @param intervalTime number 函数执行的间隔时间
--- @param callback function 需要间隔执行的函数
--- @param runAwake boolean 开始计时前是否执行一次函数
--- @returns 无返回值
function this.AddLoopTimer(intervalTime, callback, runAwake)
    --首先判断update里面有没有添加LoopTimer函数
    if (not GameManager.Instance:ContainsUpdateAction(this.LoopTimer)) then
        GameManager.Instance:AddUpdateAction(this.LoopTimer);
    end

    --计时器开始时是否执行一次
    local timerTemp = 0;
    if (runAwake) then
       timerTemp =  intervalTime;
    end

    table.insert(loopAction, {
        id = #loopAction,
        timer = timerTemp,
        time = intervalTime,
        action = callback
    });
end

---移除间隔一定时间执行的函数
function this.RemoveLoopTimer(callback)
    for k, v in pairs(loopAction) do
        local fun = v["action"];
        if (fun == callback) then
            --移除
            table.remove(k);

            break;
        end
    end
end

---update执行函数 执行计时功能
function this.LoopTimer()
    for k, v in pairs(loopAction) do
        v["timer"] = v["timer"] + UnityEngine.Time.deltaTime;
        if (v["timer"] >= v["time"]) then
            v["action"]();

            v["timer"] = 0;
        end
    end
end
