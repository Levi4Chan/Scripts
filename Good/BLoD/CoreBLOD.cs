/*
name: null
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreDailies.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Quests;
using Skua.Core.Models.Items;

public class CoreBLOD
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreDailies Daily = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.RunCore();
    }

    public string[] BLoDItems =
    {
        "Blinding Light of Destiny",
        "Get Your Blinding Light of Destiny",
        // Coppers
        "Copper",
        "Celestial Copper",
        "Celestial Copper of Destiny",
        // Maces
        "Mace of Destiny",
        "Bright Mace of Destiny",
        "Blinding Mace of Destiny",
        // Silvers
        "Silver",
        "Sanctified Silver",
        "Sanctified Silver of Destiny",
        // Bows
        "Bow of Destiny",
        "Bright Bow of Destiny",
        "Blinding Bow of Destiny",
        // Bariums
        "Barium",
        "Blessed Barium",
        "Blessed Barium of Destiny",
        // Blades
        "Blade of Destiny",
        "Bright Blade of Destiny",
        "Blinding Blade of Destiny",
        // Weapon Kits
        "Basic Weapon Kit",
        "Advanced Weapon Kit",
        "Ultimate Weapon Kit",
        // Merge misc.
        "Bone Dust",
        "Undead Essence",
        "Undead Energy",
        "Blinding Light Fragments",
        "Spirit Orb",
        "Loyal Spirit Orb",
        "Bright Aura",
        "Brilliant Aura",
        "Blinding Aura",
        // Misc.
        "Blinding Light of Destiny Handle",
        "Bonegrinder Medal",
        "Zardman's StoneHammer",
        "WolfClaw Hammer",
        "Great Ornate Warhammer"
    };

    public void DoAll()
    {
        if (Core.CheckInventory("Blinding Light of Destiny"))
            return;

        if (Core.CheckInventory("Get Your Blinding Light of Destiny"))
        {
            Core.BuyItem("battleon", 1415, "Blinding Light of Destiny");
            return;
        }

        Core.AddDrop(BLoDItems);

        UnlockMineCrafting();
        BlindingMace();
        BlindingBow();
        BlindingBlade();
        BlindingScythe();
        TheBlindingLightofDestiny();

        Core.BuyItem("battleon", 1415, "Blinding Light of Destiny");
    }

    public void UnlockMineCrafting()
    {
        if (Bot.Quests.IsUnlocked(2091))
            return;

        //Reforging the Blinding Light
        if (!Core.isCompletedBefore(2066))
        {
            Core.Logger("Doing Quest: [2066] \"Reforging the Blinding Light\"");
            Core.BuyItem("doomwood", 276, "Blinding Light of Destiny Handle");
            Core.ChainComplete(2066);
        }

        //Secret Order of Undead Slayers
        if (!Core.isCompletedBefore(2067))
        {
            Core.Logger("Doing Quest: [2067] \"Secret Order of Undead Slayers\"");
            Farm.Gold(15000);
            Core.BuyItem("doomwood", 276, "Bonegrinder Medal");
            Core.ChainComplete(2067);
        }

        //Essential Essences
        if (!Core.isCompletedBefore(2082))
        {
            Core.Logger("Doing Quest: [2082] \"Essential Essences\"");
            Farm.BattleUnderB("Undead Essence", 25);
            Core.ChainComplete(2082);
        }

        //Bust some Dust
        if (!Core.isCompletedBefore(2083))
        {
            Core.Logger("Doing Quest: [2083] \"Bust some Dust\"");
            Farm.BattleUnderB("Bone Dust", 40);
            Core.ChainComplete(2083);
        }

        //A Loyal Follower        
        if (!Core.isCompletedBefore(2084))
        {
            Core.Logger("Doing Quest: [2084] \"A Loyal Follower\"");
            SpiritOrb(100);
            Core.EnsureAccept(2084);
            Core.HuntMonster("timevoid", "Ephemerite", "Celestial Compass");
            Core.EnsureComplete(2084);
            Bot.Drops.Pickup("Loyal Spirit Orb");
        }
    }

    public void SpiritOrb(int quant = 10500)
    {
        if (Core.CheckInventory("Spirit Orb", quant))
            return;

        Core.AddDrop("Bone Dust", "Undead Essence", "Undead Energy", "Spirit Orb");
        Core.EquipClass(ClassType.Farm);
        Core.FarmingLogger("Spirit Orb", quant);

        Core.RegisterQuests(2082, 2083);
        while (!Bot.ShouldExit && !Core.CheckInventory("Spirit Orb", quant))
            Core.KillMonster("battleunderb", "Enter", "Spawn", "Skeleton Warrior", log: false);
        Core.CancelRegisteredQuests();
    }

    public void BasicWK(int quant = 1)
    {
        if (Core.CheckInventory("Basic Weapon Kit", quant))
            return;

        Core.EquipClass(ClassType.Farm);
        Core.FarmingLogger("Basic Wepon Kit", quant);

        Core.AddDrop("Basic Weapon Kit", "Zardman's StoneHammer");
        Core.RegisterQuests(2136);
        while (!Bot.ShouldExit && !Core.CheckInventory("Basic Weapon Kit", quant))
        {
            Core.KillMonster("forest", "Forest3", "Left", "*", "Zardman's StoneHammer", 1, false);
            Core.KillMonster("noobshire", "North", "Left", "Horc Noob", "Noob Blade Oil");
            Core.KillMonster("farm", "Crop1", "Right", "Scarecrow", "Burlap Cloth", 4);

            Bot.Quests.UpdateQuest(4614);
            Core.HuntMonster("pyramid", "Mummy", "Triple Ply Mummy Wrap", 7);
            Core.HuntMonster("pyramid", "Golden Scarab", "Golden Lacquer Finish");
            Core.HuntMonster("lair", "Bronze Draconian", "Bronze Brush");
            Core.HuntMonster("bloodtusk", "Rock", "Rocky Stone Sharpener");

            Bot.Wait.ForPickup("Basic Weapon Kit");
        }
        Core.CancelRegisteredQuests();
    }

    public void AdvancedWK(int quant = 1)
    {
        if (Core.CheckInventory("Advanced Weapon Kit", quant))
            return;
        Core.FarmingLogger("Advanced Weapon Kit", quant);

        Core.AddDrop("WolfClaw Hammer", "Advanced Weapon Kit");
        Core.RegisterQuests(2162);
        while (!Bot.ShouldExit && !Core.CheckInventory("Advanced Weapon Kit", quant))
        {
            Core.EquipClass(ClassType.Solo);
            Core.HuntMonster("hachiko", "Dai Tengu", "Superior Blade Oil");
            Core.HuntMonster("airstorm", "Lightning Ball", "Shining Lacquer Finish");
            Core.HuntMonster("faerie", "Cyclops Warlord", "Brass Awl");
            Core.HuntMonster("darkoviaforest", "Lich of the Stone", "Slate Stone Sharpener");

            Core.EquipClass(ClassType.Farm);
            Core.KillMonster("safiria", "c3", "Left", "Chaos Lycan", "WolfClaw Hammer", 1, false);
            Core.KillMonster("lycan", "r4", "Left", "Chaos Vampire Knight", "Silver Brush");
            Core.KillMonster("sandport", "r3", "Right", "Tomb Robber", "Leather Case");
            Core.KillMonster("pines", "Path1", "Left", "Leatherwing", "Leatherwing Hide", 10);

            Bot.Wait.ForPickup("Advanced Weapon Kit");
        }
    }

    public void UltimateWK(string item = "Ultimate Weapon Kit", int quant = 1)
    {
        if (Core.CheckInventory(item, quant))
            return;

        Core.AddDrop("Ultimate Weapon Kit", "Blinding Light Fragments", "Bright Aura", "Spirit Orb", "Loyal Spirit Orb", "Great Ornate Warhammer");

        Core.FarmingLogger(item, quant);

        Core.RegisterQuests(2163);
        while (!Bot.ShouldExit && !Core.CheckInventory(item, quant))
        {
            Core.EquipClass(ClassType.Farm);
            Core.KillMonster("dragonplane", "r2", "Right", "Earth Elemental", "Great Ornate Warhammer", 1, false);

            Core.EquipClass(ClassType.Solo);
            Core.KillMonster("greendragon", "Boss", "Left", "Greenguard Dragon", "Greenguard Dragon Hide", 3);
            Core.KillMonster("sandcastle", "r7", "Left", "Chaos Sphinx", "Gold Brush", publicRoom: true);
            Core.KillMonster("crashsite", "Boss", "Left", "ProtoSartorium", "Non-abrasive Power Powder");
            Core.KillMonster("kitsune", "Boss", "Left", "Kitsune", "No. 1337 Blade Oil", publicRoom: true);
            Core.KillMonster("citadel", "m14", "Left", "Grand Inquisitor", "Blinding Lacquer Finish");
            Core.HuntMonster("djinn", "Harpy", "Suede Travel Case");
            Core.KillMonster("roc", "Enter", "Spawn", "Rock Roc", "Sharp Stone Sharpener");

            Bot.Wait.ForPickup(item);
        }
    }

    public void BlindingMace()
    {
        if (Core.CheckInventory("Blinding Mace of Destiny"))
        {
            Core.Logger("Mace found, skipping.");
            return;
        }

        if (!Core.CheckInventory(new[] { "Mace of Destiny", "Bright Mace of Destiny", "Blinding Mace of Destiny" }, any: true))
        {
            if (!Core.CheckInventory("Celestial Copper of Destiny"))
            {
                if (!Core.CheckInventory("Celestial Copper"))
                {
                    Core.FarmingLogger("Celestial Copper", 1);
                    Core.EnsureAccept(2107);
                    Farm.BattleUnderB("Undead Energy", 25);
                    Daily.MineCrafting(new[] { "Copper" });
                    if (!Core.CheckInventory(12297))
                        Core.Logger("Can't complete Celestial Copper Enchantment (Missing Copper).", messageBox: true, stopBot: true);
                    SpiritOrb(5);
                    Core.HuntMonster("arcangrove", "Seed Spitter", "Paladaffodil", 25);
                    Core.EnsureComplete(2107);
                    Bot.Wait.ForPickup("Celestial Copper");
                }

                BrightAuraMerge(2);
                LoyalSpiritOrbMerge(5);
                Core.BuyItem("dwarfhold", 434, "Celestial Copper of Destiny");
                Bot.Wait.ForPickup("Celestial Copper of Destiny");
            }

            if (!Core.isCompletedBefore(2136))
            {
                Core.Logger("Unlocking Weapon Kit quests");
                Core.EnsureAccept(2133);
                Core.HuntMonster("dwarfhold", "Albino Bat", "Forge Key", isTemp: false);
                Core.EnsureComplete(2133);
            }

            Core.Logger("Farming for Mace of Destiny");
            Farm.BattleUnderB("Undead Energy", 7);
            BasicWK();
            AdvancedWK();
            UltimateWK("Loyal Spirit Orb");
            UltimateWK("Spirit Orb", 20);
            LightMerge("Mace of Destiny");
        }

        if (Core.CheckInventory("Mace of Destiny"))
        {
            Core.FarmingLogger("Mace of Destiny", 1);
            AdvancedWK();
            UltimateWK("Bright Aura", 2);
            LightMerge("Bright Mace of Destiny");
        }

        if (Core.CheckInventory("Bright Mace of Destiny"))
        {
            Core.Logger("Blinding Mace of Destiny");
            UltimateWK();
            LightMerge("Blinding Mace of Destiny");
        }
    }

    //Weapon Upgrades can be optimized with functions that upgrade the shit, as they are basically the same

    public void BlindingBow()
    {
        if (Core.CheckInventory("Blinding Bow of Destiny"))
        {
            Core.Logger("Bow found, skipping");
            return;
        }

        if (!Core.CheckInventory("Blinding Mace of Destiny"))
            BlindingMace();

        if (!Core.CheckInventory(new[] { "Blinding Bow of Destiny", "Bright Bow of Destiny", "Bow of Destiny" }, any: true))
        {
            if (!Core.CheckInventory("Sanctified Silver of Destiny"))
            {
                if (!Core.CheckInventory("Sanctified Silver"))
                {
                    Core.FarmingLogger("Sanctified Silver", 1);
                    Core.EnsureAccept(2108);
                    Farm.BattleUnderB("Undead Energy", 25);
                    Daily.MineCrafting(new[] { "Silver" });
                    if (!Core.CheckInventory("Silver"))
                        Core.Logger("Can't complete Sanctified Silver Enchantment (Missing Silver).", messageBox: true, stopBot: true);
                    UltimateWK("Spirit Orb", 5);
                    Core.HuntMonster("arcangrove", "Seed Spitter", "Paladaffodil", 25);
                    Core.EnsureComplete(2108);
                }
                Core.FarmingLogger("Sanctified Silver of Destiny", 1);
                UltimateWK("Loyal Spirit Orb", 5);
                UltimateWK("Bright Aura", 2);
                Core.BuyItem("dwarfhold", 434, "Sanctified Silver of Destiny");
            }
            Core.FarmingLogger("Bow of Destiny", 1);
            FindingFragmentsMace();
            Farm.BattleUnderB("Undead Energy", 17);
            UltimateWK("Loyal Spirit Orb");
            UltimateWK("Spirit Orb", 13);
            BasicWK();
            LightMerge("Bow of Destiny");
        }

        if (Core.CheckInventory("Bow of Destiny"))
        {
            Core.FarmingLogger("Bright Bow of Destiny", 1);
            UltimateWK("Loyal Spirit Orb", 3);
            AdvancedWK();
            LightMerge("Bright Bow of Destiny");
        }

        if (Core.CheckInventory("Bright Bow of Destiny"))
        {
            Core.FarmingLogger("Blinding Bow of Destiny", 1);
            UltimateWK();
            LightMerge("Blinding Bow of Destiny");
        }
    }

    public void BlindingBlade()
    {
        if (Core.CheckInventory("Blinding Blade of Destiny"))
        {
            Core.Logger("Blade found, skipping.");
            return;
        }

        if (!Core.CheckInventory("Blinding Bow of Destiny"))
            BlindingBow();

        if (!Core.CheckInventory(new[] { "Blinding Blade of Destiny", "Bright Blade of Destiny", "Blade of Destiny" }, any: true))
        {
            if (!Core.CheckInventory("Blessed Barium of Destiny"))
            {
                if (!Core.CheckInventory("Blessed Barium"))
                {
                    Core.FarmingLogger("Blessed Barium", 1);
                    Core.EnsureAccept(2104);
                    Farm.BattleUnderB("Undead Energy", 25);
                    Daily.MineCrafting(new[] { "Barium" });
                    if (!Core.CheckInventory("Barium"))
                        Core.Logger("Can't complete Sanctified Barium Enchantment (Missing Barium).", messageBox: true, stopBot: true);
                    UltimateWK("Spirit Orb", 5);
                    Core.HuntMonster("arcangrove", "Seed Spitter", "Paladaffodil", 25);
                    Core.EnsureComplete(2104);
                    Bot.Wait.ForPickup("Blessed Barium");
                }
                Core.FarmingLogger("Blessed Barium of Destiny", 1);
                FindingFragmentsBow(2);
                UltimateWK("Loyal Spirit Orb", 5);
                Core.BuyItem("dwarfhold", 434, "Blessed Barium of Destiny");
                Bot.Wait.ForPickup("Blessed Barium of Destiny");
            }
            Core.FarmingLogger("Blade of Destiny", 1);
            FindingFragmentsMace();
            UltimateWK("Loyal Spirit Orb");
            UltimateWK("Spirit Orb", 15);
            BasicWK();
            LightMerge("Blade of Destiny");
        }

        if (Core.CheckInventory("Blade of Destiny"))
        {
            Core.FarmingLogger("Bright Blade of Destiny", 1);
            FindingFragmentsMace();
            AdvancedWK();
            LightMerge("Bright Blade of Destiny");
        }

        if (Core.CheckInventory("Bright Blade of Destiny"))
        {
            Core.FarmingLogger("Blinding Blade of Destiny", 1);
            UltimateWK();
            LightMerge("Blinding Blade of Destiny");
        }
    }

    public void BlindingBroadsword()
    {
        if (Core.CheckInventory("Blinding Broadsword of Destiny"))
        {
            Core.Logger("Broadsword found, skipping.");
            return;
        }

        if (!Core.CheckInventory("Blinding Bow of Destiny"))
            BlindingBow();

        if (!Core.CheckInventory(new[] { "Blinding Broadsword of Destiny", "Bright Broadsword of Destiny", "Broadsword of Destiny" }, any: true))
        {
            if (!Core.CheckInventory("Glorious Gold of Destiny"))
            {
                if (!Core.CheckInventory("Glorious Gold"))
                {
                    Core.FarmingLogger("Glorious Gold", 1);
                    Core.EnsureAccept(2105);
                    Farm.BattleUnderB("Undead Energy", 25);
                    Daily.MineCrafting(new[] { "Gold" });
                    if (!Core.CheckInventory("Gold"))
                        Core.Logger("Can't complete Glorious Gold Enchantment (Missing Gold).", messageBox: true, stopBot: true);
                    UltimateWK("Spirit Orb", 5);
                    Core.HuntMonster("arcangrove", "Seed Spitter", "Paladaffodil", 25);
                    Core.EnsureComplete(2105);
                    Bot.Wait.ForPickup("Glorious Gold");
                }
                Core.FarmingLogger("Glorious Gold of Destiny", 1);
                FindingFragmentsBow(2);
                UltimateWK("Loyal Spirit Orb", 5);
                Core.BuyItem("dwarfhold", 434, "Glorious Gold of Destiny");
                Bot.Wait.ForPickup("Glorious Gold of Destiny");
            }
            Core.FarmingLogger("Broadsword of Destiny", 1);
            FindingFragmentsMace();
            UltimateWK("Loyal Spirit Orb", 5);
            UltimateWK("Spirit Orb", 15);
            BasicWK();
            LightMerge("Broadsword of Destiny");
        }

        if (Core.CheckInventory("Broadsword of Destiny"))
        {
            Core.FarmingLogger("Bright Broadsword of Destiny", 1);
            FindingFragmentsMace();
            AdvancedWK();
            LightMerge("Bright Broadsword of Destiny");
        }

        if (Core.CheckInventory("Bright Broadsword of Destiny"))
        {
            Core.FarmingLogger("Blinding Broadsword of Destiny", 1);
            UltimateWK();
            LightMerge("Blinding Broadsword of Destiny");
        }
    }

    public void BlindingScythe()
    {
        if (Core.CheckInventory("Blinding Scythe of Destiny"))
        {
            Core.Logger("Scythe found, skipping.");
            return;
        }

        if (!Core.CheckInventory("Blinding Bow of Destiny"))
            BlindingBow();

        if (!Core.CheckInventory(new[] { "Blinding Scythe of Destiny", "Bright Scythe of Destiny", "Scythe of Destiny" }, any: true))
        {
            if (!Core.CheckInventory("Immortal Iron of Destiny"))
            {
                if (!Core.CheckInventory("Immortal Iron"))
                {
                    Core.FarmingLogger("Immortal Iron", 1);
                    Core.EnsureAccept(2106);
                    Farm.BattleUnderB("Undead Energy", 25);
                    Daily.MineCrafting(new[] { "Iron" });
                    if (!Core.CheckInventory("Iron"))
                        Core.Logger("Can't complete Immortal Iron Enchantment (Missing Iron).", messageBox: true, stopBot: true);
                    UltimateWK("Spirit Orb", 5);
                    Core.HuntMonster("arcangrove", "Seed Spitter", "Paladaffodil", 25);
                    Core.EnsureComplete(2106);
                    Bot.Wait.ForPickup("Immortal Iron");
                }
                Core.FarmingLogger("Immortal Iron of Destiny", 1);
                FindingFragmentsBow(2);
                UltimateWK("Loyal Spirit Orb", 5);
                Core.BuyItem("dwarfhold", 434, "Immortal Iron of Destiny");
                Bot.Wait.ForPickup("Immortal Iron of Destiny");
            }
            Core.FarmingLogger("Scythe of Destiny", 1);
            UltimateWK("Loyal Spirit Orb", 2);
            UltimateWK("Spirit Orb", 10);
            BasicWK();
            LightMerge("Scythe of Destiny");
        }

        if (Core.CheckInventory("Scythe of Destiny"))
        {
            Core.FarmingLogger("Bright Scythe of Destiny", 1);
            FindingFragmentsMace(2);
            AdvancedWK();
            LightMerge("Bright Scythe of Destiny");
        }

        if (Core.CheckInventory("Bright Scythe of Destiny"))
        {
            Core.FarmingLogger("Blinding Scythe of Destiny", 1);
            UltimateWK();
            LightMerge("Blinding Scythe of Destiny");
        }
    }
    public void TheBlindingLightofDestiny()
    {
        if (Core.CheckInventory("Blinding Light of Destiny"))
            return;

        Core.Logger("Final part");
        FindingFragmentsBroadSword(500, 125);
        FindingFragmentsMace(75);
        FindingFragmentsBlade(0, 250);
        FindingFragments(Core.CheckInventory("Blinding Scythe of Destiny") ? 2177 : 2174, "Blinding Aura");
        UltimateWK();
        Core.ChainComplete(2180);
        Bot.Drops.Pickup("Get Your Blinding Light of Destiny");
    }

    public void FindingFragmentsMace(int quant = 1)
    {
        if (Core.CheckInventory("Brilliant Aura", quant))
            return;
        if (!Core.CheckInventory("Blinding Mace of Destiny"))
            BlindingMace();

        FindingFragments(2176, "Brilliant Aura", quant);
    }

    public void FindingFragmentsBow(int quant = 1)
    {
        if (Core.CheckInventory("Bright Aura", quant))
            return;

        if (!Core.CheckInventory("Blinding Bow of Destiny"))
            BlindingBow();

        FindingFragments(2174, "Bright Aura", quant);
    }

    public void FindingFragmentsBlade(int quantSO, int quantLSO)
    {
        if (Core.CheckInventory("Spirit Orb", quantSO) && Core.CheckInventory("Loyal Spirit Orb", quantLSO))
            return;
        if (!Core.CheckInventory("Blinding Blade of Destiny"))
            BlindingBlade();

        FindingFragments(2179, "Spirit Orb", quantSO);
        FindingFragments(2179, "Loyal Spirit Orb", quantLSO);
    }

    public void FindingFragmentsBroadSword(int quantSO, int quantBA)
    {
        if (Core.CheckInventory("Spirit Orb", quantSO) && Core.CheckInventory("Bright Aura", quantBA))
            return;
        if (!Core.CheckInventory("Blinding Scythe of Destiny"))
            BlindingBroadsword();

        FindingFragments(2178, "Spirit Orb", quantSO);
        FindingFragments(2178, "Bright Aura", quantBA);
    }

    public void FindingFragments(int quest = 0000, string item = null, int quant = 1)
    {
        if (Core.CheckInventory(item, quant) || item == null)
            return;

        Quest QuestData = Core.EnsureLoad(quest);
        ItemBase[] RequiredItems = QuestData.Requirements.ToArray();
        ItemBase[] QuestReward = QuestData.Rewards.ToArray();

        foreach (ItemBase Item in RequiredItems.Concat(QuestReward))
            Bot.Drops.Add(Item.ID);

        Core.FarmingLogger(item, quant);
        Core.EquipClass(ClassType.Farm);

        Core.RegisterQuests(quest);
        while (!Bot.ShouldExit && !Core.CheckInventory(item, quant))
            Core.KillMonster("battleunderb", "Enter", "Spawn", "*", "Blinding Light Fragments", 10, isTemp: false, log: false);
        Core.CancelRegisteredQuests();
    }

    // ------------------------------------------------------------------------------------------------------------------------------ //
    // Blinding Light of Destiny Extras Below
    // ------------------------------------------------------------------------------------------------------------------------------ //

    public void SanctifiedLightofDestiny()
    {
        if (Core.CheckInventory("Sanctified Light of Destiny"))
            return;

        DoAll();

        Core.AddDrop(new[] { "Sanctified Light of Destiny", "Pious Platinum" });

        Core.EnsureAccept(8112);

        if (!Core.CheckInventory("Glorious Gold of Destiny"))
        {
            UltimateWK("Loyal Spirit Orb", 5);

            while (!Bot.ShouldExit && !Core.CheckInventory("Bright Aura", 2))
            {
                FindingFragments(2174);
                Bot.Drops.Pickup("Bright Aura");
            }

            if (!Core.CheckInventory("Glorious Gold"))
            {
                Core.EnsureAccept(2105);

                Farm.BattleUnderB("Undead Energy", 25);

                if (!Core.CheckInventory("Gold"))
                    Daily.MineCrafting(new[] { "Gold" });

                UltimateWK("Spirit Orb", 5);
                Core.HuntMonster("Marsh", "Marsh Tree", "Paladaffodil", 25);

                Core.EnsureComplete(2105);
            }

            Core.BuyItem("Dwarfhold", 434, "Glorious Gold of Destiny");
        }

        if (!Core.CheckInventory("Pious Platinum of Destiny"))
        {
            UltimateWK("Loyal Spirit Orb", 5);

            while (!Bot.ShouldExit && !Core.CheckInventory("Bright Aura", 2))
            {
                FindingFragments(2174);
                Bot.Drops.Pickup("Bright Aura");
            }

            if (!Core.CheckInventory("Pious Platinum"))
            {
                Core.EnsureAccept(2109);

                Farm.BattleUnderB("Undead Energy", 25);
                UltimateWK("Spirit Orb", 5);

                if (!Core.CheckInventory("Platinum"))
                    Daily.MineCrafting(new[] { "Platinum" });

                if (!Core.CheckInventory("Platinum"))
                {
                    Core.Logger("This Daily has already been done today please re-run this script tomorrow.");
                    return;
                }

                Core.HuntMonster("Marsh", "Marsh Tree", "Paladaffodil", 25);

                Core.EnsureComplete(2109);
            }

            Core.BuyItem("Dwarfhold", 434, "Pious Platinum of Destiny");
        }

        Farm.BattleUnderB("Bone Dust", 300);
        UltimateWK("Loyal Spirit Orb", 25);
        Core.HuntMonster("Extinction", "Cyworg", "Refined Metal", 5);

        Core.EnsureComplete(8112);
    }

    void LightMerge(string item, int quant = 1) => Core.BuyItem("necropolis", 422, item, quant);

    public void LoyalSpiritOrbMerge(int quant)
    {
        if (Core.CheckInventory("Loyal Spirit Orb", quant))
            return;

        Core.FarmingLogger("Loyal Spirit Orb", quant);
        while (!Bot.ShouldExit && !Core.CheckInventory("Loyal Spirit Orb", quant))
        {
            SpiritOrb(100 * quant);
            Core.BuyItem("necropolis", 422, "Loyal Spirit Orb", quant);
        }
    }

    public void BrightAuraMerge(int quant)
    {
        if (Core.CheckInventory("Bright Aura", quant))
            return;

        Core.FarmingLogger("Bright Aura", quant);
        while (!Bot.ShouldExit && !Core.CheckInventory("Bright Aura", quant))
        {
            LoyalSpiritOrbMerge(50 * quant);
            Core.BuyItem("necropolis", 422, "Bright Aura", quant);
        }
    }

    public void BrilliantAuraMerge(int quant)
    {
        if (Core.CheckInventory("Brilliant Aura", quant))
            return;

        Core.FarmingLogger("Brilliant Aura", quant);
        while (!Bot.ShouldExit && !Core.CheckInventory("Brilliant Aura", quant))
        {
            BrightAuraMerge(25 * quant);
            Core.BuyItem("necropolis", 422, "Brilliant Aura", quant);
        }
    }
}
