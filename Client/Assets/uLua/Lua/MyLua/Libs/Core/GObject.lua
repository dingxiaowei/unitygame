-- 所有的类的基类

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"

local M = GlobalNS.Class();
M.clsName = "GObject";
GlobalNS[M.clsName] = M;

--[[
-- 表访问
M.__index = M
M.__newindex = M
]]

-- 函数访问
function M.get(tbl, k)
    M.__index = nil;
    local ret = tbl[k];
    M.__index = M.get;
    return ret;
end

M.__index = M.get;

function M.set(tbl, key, value)
    M.__newindex = nil;  -- 删除 __newindex ，否则调用 tbl[key] = value 的时候会递归调用 set 函数
    tbl[key] = value;
    M.__newindex = M.set;
    
    M.checkAttrRedef(tbl, key, value);
end

M.__newindex = M.set;

function M.checkAttrRedef(tbl, key, value)
    --测试相同属性定义导致的属性覆盖
    if tbl.clsName ~= nil and tbl.clsName == 'Form' then
        
    end
end

return M;