using Dalamud;
using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Logging;
using Dalamud.Plugin.Services;
using ECommons;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.Logging;
using ECommons.Throttlers;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using FFXIVClientStructs.STD.ContainerInterface;
using FFXIVClientStructs.STD.Helper;
using Lumina.Text.ReadOnly;
using ProximityVoiceChat.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


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

        public unsafe string[] getAdventurerList() {
            var atkArrayDataHolder = RaptureAtkModule.Instance()->AtkArrayDataHolder;
            //Reads Atk Array Data from an OPEN player search window (specifically in instanced content)
            var array = atkArrayDataHolder.StringArrays[85];
            List<string> ret = new List<string>();
            int retsize = 0;
            string str = "";
            for (int i = 0; i< array->Size; i++) {
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
