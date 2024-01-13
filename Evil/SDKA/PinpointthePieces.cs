/*
name: PinpointthePieces
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/Evil/SDKA/CoreSDKA.cs
using System.Collections.Generic;
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Options;

public class PinpointthePieces_Any
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreSDKA SDKA = new CoreSDKA();

    public string OptionStorage = "Pinpoint_the_Pieces";

    public List<IOption> Options = new()
    {
        new Option<PinpointIDs>("questID", "Quest ID", "ID of the desired Pinpoint quest to do.", PinpointIDs.Dagger)
    };
    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        Pinpoint();

        Core.SetOptions(false);
    }

    public void Pinpoint()
    {
        Core.EquipClass(ClassType.Farm);

        PinpointIDs questID = Bot.Config?.Get<PinpointIDs>("questID") ?? default;
        ItemBase[] QuestRewards = Core.EnsureLoad((int)questID).Rewards.ToArray();

        foreach (ItemBase reward in QuestRewards)
        {
            while (!Bot.ShouldExit && !Core.CheckInventory(reward.ID, reward.MaxStack))
                SDKA.PinpointthePieces((int)questID, new[] { reward.Name }, new[] { reward.MaxStack });
        }

    }

    public enum PinpointIDs
    {
        Dagger = 2181,
        Blade = 2182,
        Broadsword = 2183,
        Scythe = 2184,
        Mace = 2185,
        Bow = 2186
    }
}
