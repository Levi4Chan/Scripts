/*
name: Nulgath Demands Work
description: This bot will do the Nulgath Demands Work quest untill you have a uni35 and the equipment.
tags: archfiend, doomlord, ADFL, nulgath, demands, work, unidentified, uni, 35, essence, fragment
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Nation/CoreNation.cs
//cs_include Scripts/Nation/AFDL/WillpowerExtraction.cs
//cs_include Scripts/Nation/Various/GoldenHanzoVoid.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;

public class NulgathDemandsWork
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreNation Nation = new();
    public GoldenHanzoVoid GHV = new();
    public WillpowerExtraction WillpowerExtraction = new();
    public CoreAdvanced Adv = new();

    public string[] NDWItems =
    {
        "DoomLord's War Mask",
        "ShadowFiend Cloak",
        "Locks of the DoomLord",
        "Doomblade of Destruction",
    };

    public void ScriptMain(IScriptInterface bot)
    {
        Core.BankingBlackList.AddRange(Nation.bagDrops);
        Core.BankingBlackList.AddRange(NDWItems);
        Core.BankingBlackList.AddRange(new[] { "Archfiend Essence Fragment", "Unidentified 35" });
        Core.SetOptions();

        DoNulgathDemandsWork();

        Core.SetOptions(false);
    }

    public void DoNulgathDemandsWork()
    {
        NDWQuest(NDWItems);
        NDWQuest(new[] { "Unidentified 35" }, 300);
    }

    /// <summary>
    /// Complets "Nulgath Demands Work" until the Desired Items are gotten. 
    /// <param name="string[] items">The List of items to Get from the Quest</param>
    /// <param name="quant">Amount of the "item" [Mostly the Archfiend Ess and Uni 35]</param>
    /// </summary>
    public void NDWQuest(string[]? itemNames = null, int quant = 1)
    {
        itemNames ??= NDWItems.Select(item => item).ToArray() ?? Array.Empty<string>(); // Ensure itemNames is not null

        if (itemNames.Length == 0 || Core.CheckInventory(itemNames, quant)) return;

        ItemBase[] Rewards = Core.EnsureLoad(5259).Rewards.ToArray();

        Core.AddDrop(Nation.bagDrops);
        Core.AddDrop(Rewards.Select(x => x.Name).ToArray());

        foreach (string itemName in itemNames)
        {
            ItemBase item = Rewards.Find(x => x.Name == itemName) ?? new ItemBase(); // Ensure item is not null

            if (Core.CheckInventory(item.Name, quant))
            {
                Core.Logger($"{item.Name}, x[{quant}] owned, continuing.");
                continue;
            }

            Core.FarmingLogger(item.Name, quant);

            while (!Bot.ShouldExit && !Core.CheckInventory(item.Name, quant))
            {
                Core.EnsureAccept(5259);

                WillpowerExtraction.Unidentified34(10);
                Nation.FarmUni13(2);
                Nation.FarmBloodGem(2);
                Nation.FarmDiamondofNulgath(60);
                Nation.FarmDarkCrystalShard(45);
                Uni27();
                Nation.FarmGemofNulgath(15);
                Nation.SwindleBulk(50);
                GHV.GetGHV();
                Nation.FarmVoucher(true);

                if (item.Name == "Unidentified 35")
                {
                    while (!Bot.ShouldExit && Core.CheckInventory("Archfiend Essence Fragment", 9) && !Core.CheckInventory("Unidentified 35", quant))
                        Adv.BuyItem("tercessuinotlim", 1951, item.ID, shopItemID: 7912);

                    if (!Core.CheckInventory(NDWItems))
                    {
                        foreach (string NDWItem in NDWItems)
                            if (!Core.CheckInventory(NDWItem))
                                Core.EnsureCompleteChoose(5259, new[] { NDWItem });
                            else Core.Logger("all NDW items owned, completing quest without Selecting reward.\n" +
                            "(will still get \"Archfiend Essence Fragment\" and uni 35.)");
                    }
                    else Core.EnsureComplete(5259);
                }
                else Core.EnsureComplete(5259, item.Name == "Archfiend Essence Fragment" ? -1 : item.ID);
            }
        }
    }


    public void Uni27()
    {
        if (Core.CheckInventory("Unidentified 27"))
            return;

        Core.AddDrop("Unidentified 27");
        Nation.Supplies("Unidentified 26", 1);
        Core.EnsureAccept(584);
        Nation.ResetQuest(7551);
        while (!Bot.ShouldExit && !Core.CheckInventory("Dark Makai Sigil"))
        {
            // Define the maps with their corresponding indexes
            var maps = new[] { ("tercessuinotlim", "m1"), (Core.IsMember ? "Nulgath" : "evilmarsh", "Field1") };

            // Randomly select a map
            var randomMapIndex = new Random().Next(0, maps.Length);
            var selectedMap = maps[randomMapIndex];

            Core.Join(selectedMap.Item1, selectedMap.Item2, "Left");

            while (!Bot.ShouldExit &&
                (selectedMap.Item1 == "tercessuinotlim"
                    ? (Core.IsMonsterAlive(2, useMapID: true) || Core.IsMonsterAlive(3, useMapID: true))
                    : (Core.IsMonsterAlive(1, useMapID: true) || Core.IsMonsterAlive(2, useMapID: true))))
            {
                while (!Bot.ShouldExit && Bot.Player.Cell != selectedMap.Item2)
                {
                    Core.Jump(selectedMap.Item2);
                    Core.Sleep();
                    if (Bot.Player.Cell == selectedMap.Item2)
                        break;
                }

                if (!Bot.Player.InCombat)
                    Bot.Combat.Attack("*");
                if (Core.CheckInventory("Dark Makai Sigil"))
                    break;
            }
        }
        Bot.Wait.ForDrop("Dark Makai Sigil");
        Bot.Wait.ForPickup("Dark Makai Sigil");
        Core.EnsureComplete(584);
        Bot.Wait.ForDrop("Unidentified 27");
        Bot.Wait.ForPickup("Unidentified 27");
        Core.Logger("Uni 27 acquired");

    }
}
