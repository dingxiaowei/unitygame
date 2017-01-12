MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle_KBE");

local M = GlobalNS.Class(GlobalNS.NetCmdDispHandle_KBE);
M.clsName = "GameEventHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.isGetTime = false;
end

function M:dtor()
	GCtx.mNetCmdNotify_KBE:removeParamHandle("Client_onHelloCB", self, self.handleTest);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("Client_notifyReliveSeconds", self, self.Client_notifyReliveSeconds);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("handleSendAndGetMessage", self, self.handleSendAndGetMessage);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyTop10RankInfoList", self, self.notifyTop10RankInfoList);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyGameLeftSeconds", self, self.notifyGameLeftSeconds);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyResultRankInfoList", self, self.notifyResultRankInfoList);
end

function M:init()
	GCtx.mNetCmdNotify_KBE:addParamHandle("Client_onHelloCB", self, self.handleTest);
    GCtx.mNetCmdNotify_KBE:addParamHandle("Client_notifyReliveSeconds", self, self.Client_notifyReliveSeconds);
    GCtx.mNetCmdNotify_KBE:addParamHandle("handleSendAndGetMessage", self, self.handleSendAndGetMessage);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyTop10RankInfoList", self, self.notifyTop10RankInfoList);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyGameLeftSeconds", self, self.notifyGameLeftSeconds);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyResultRankInfoList", self, self.notifyResultRankInfoList);
end

function M:dtor()
    
end

function M:handleTest(cmd)
    
end

function M:handleSendAndGetMessage(params)
    local msgName = params[0];
    if not self:filterMessage(msgName) then
        if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIConsoleDlg) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIConsoleDlg);
            if nil ~= form and form.mIsReady then
                form:onSetLogText(msgName);
            end
        end
    end    
end

function M:filterMessage(msgname) --消息过滤
    if string.find(msgname, "Client_onUpdateBasePosXZ") ~= nil or
       string.find(msgname, "Baseapp_onUpdateDataFromClient") ~= nil or
       string.find(msgname, "Client_onUpdateData_xyz") ~= nil or
       string.find(msgname, "Baseapp_onClientActiveTick") ~= nil or
       string.find(msgname, "Client_onAppActiveTickCB") ~= nil
    then
        return true;
    else
        return false;
    end
end

function M:Client_notifyReliveSeconds(params)
    local reliveTime = params[0]; --param是C#的数组，从0开始
    local enemyName = params[1];
    GCtx.mGameData.reliveTime = reliveTime;
    GCtx.mGameData.enemyName = enemyName;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRelivePanel);
end

function M:notifyTop10RankInfoList(params)
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUITopXRankPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITopXRankPanel);
        if nil ~= form and form.mIsReady then            
            form:onSetRankInfo(params);
        end
    end
end

function M:notifyGameLeftSeconds(params)
    local leftseconds = params[0];
    if not self.isGetTime then
        local leftseconds = params[0];
        GCtx.mGameData:setGameTime(leftseconds);
        self.isGetTime = true;
    end
end

function M:notifyResultRankInfoList(params)
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIOptionPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUITopXRankPanel);

    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRankListPanel);
    GCtx.mGameData:setRankInfoList(params);
end

return M;