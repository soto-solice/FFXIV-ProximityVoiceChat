using Dalamud.Plugin.Services;
using ECommons.Automation;
using ECommons.Logging;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Lumina.Text.ReadOnly;
using System.Collections.Generic;



namespace ProximityVoiceChat
{
    internal class InstanceJoiner
    {
        private readonly IGameGui gameGui;

        public InstanceJoiner(IGameGui gameGui) {
            this.gameGui = gameGui;
        }

        public unsafe void openAdventurerList() {
            
            var window = (AtkUnitBase*)gameGui.GetAddonByName("ContentMemberList", 1);
            if (window != null && window->IsVisible) {
                //run command twice if window is already open to close & reopen it
                //var componentNode = (FFXIVClientStructs.FFXIV.Component.GUI.AtkComponentWindow*)window->UldManager.NodeList[1];
                //var buttonNode = (FFXIVClientStructs.FFXIV.Component.GUI.AtkComponentButton*)componentNode->UldManager.NodeList[6];
                //var collisionNode = (FFXIVClientStructs.FFXIV.Component.GUI.AtkCollisionNode*)buttonNode->UldManager.NodeList[0];
                //string[] clicks = ECommons.Automation.UIInput.ClickHelper.GetAvailableClicks();
                //ECommons.Automation.UIInput.ClickHelperExtensions.ClickAddonButton(*(collisionNode), window);
                PluginLog.Debug("cliccked");
            }
            Chat.ExecuteCommand("/search");
            return;
        }

        public unsafe void clearAtk() {
            // This might be the most bricked thing in this whole file this does just fuck with game memory but the array repopulates whenever you reload the player list
            var atkArrayDataHolder = RaptureAtkModule.Instance()->AtkArrayDataHolder;
            var array = atkArrayDataHolder.StringArrays[85];
            for (int i = 0; i < array->Size; i++){
                array->SetValue(i, null);
                }
            }

        public unsafe int getAdvListLength() {
            var cMemberList = (AtkUnitBase*)gameGui.GetAddonByName("ContentMemberList");
            if (cMemberList != null && cMemberList->IsVisible){
                var textNode = (AtkTextNode*)cMemberList->GetNodeById(20);
                if (textNode != null)
                {
                    var text = textNode->NodeText;
                    if (text.ToString() != null) {
                        if (int.TryParse(text.ToString().Split("/")[0], out int playerCount)){
                            PluginLog.Debug(playerCount.ToString());
                            return playerCount;
                        }
                        else
                        {
                            PluginLog.Debug("Could not find playercount");
                        }
                    }
                    PluginLog.Debug(text.ToString());
                }
            }

            return 0;
        }

        public unsafe string[] getAdventurerList() {
            var atkArrayDataHolder = RaptureAtkModule.Instance()->AtkArrayDataHolder;
            //Reads Atk Array Data from an OPEN player search window (specifically in instanced content)
            var array = atkArrayDataHolder.StringArrays[85];
            List<string> ret = new List<string>();
            int retsize = 0;
            string str = "";
            var playerCount = getAdvListLength();
            for (int i = 0; (i< array->Size && i < playerCount*4); i++) {
                var isNull = (nint)array->StringArray[i] == 0;
                if (isNull) { continue; }
                if (i % 4 == 0) {
                    str += (new ReadOnlySeStringSpan(array->StringArray[i])).ToString();
                    str += "@";
                }
                if (i % 4 == 1) {
                    str += (new ReadOnlySeStringSpan(array->StringArray[i])).ToString();
                    ret.Add(str);
                    str = "";
                    retsize++;
                }
            }
            foreach (string player in ret) {
                PluginLog.Debug($"{player}");
            }
            return ret.ToArray();

        }
    }
}
