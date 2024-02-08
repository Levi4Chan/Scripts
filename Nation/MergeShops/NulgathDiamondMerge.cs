/*
name: Nulgath Diamond Merge
description: This bot will farm the items belonging to the selected mode for the Nulgath Diamond Merge [456] in /evilwarnul
tags: nulgath, diamond, merge, evilwarnul, abyssal, priest, loyalty, nation, oversoul, witch, witchs, soulstealing, paladin, bearded, dire, monk, cleric, head, golden, hanzo, void, katana, katanas, horns, cyber, crystal, phoenix, blood, cloak, horned, worshipper, face, ranger, sheath, bow, morph, quiver, storm, knight, molten, dragonspear, formal, suit, circlet, cane
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Nation/CoreNation.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/Story/BattleUnder.cs
//cs_include Scripts/Good/BLoD/CoreBLOD.cs
//cs_include Scripts/Nation/Various/TarosManslayer.cs
//cs_include Scripts/Nation/Various/PurifiedClaymoreOfDestiny.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Options;

public class NulgathDiamondMerge
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreAdvanced Adv = new();
    public CoreDailies Dailies = new();
    public static CoreAdvanced sAdv = new();
    public CoreNation Nation = new();
    public TarosManslayer Taro = new();
    public CoreBLOD BLOD = new();

    public List<IOption> Generic = sAdv.MergeOptions;
    public string[] MultiOptions = { "Generic", "Select" };
    public string OptionsStorage = sAdv.OptionsStorage;
    // [Can Change] This should only be changed by the author.
    //              If true, it will not stop the script if the default case triggers and the user chose to only get mats
    private bool dontStopMissingIng = false;

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        BuyAllMerge();

        Core.SetOptions(false);
    }

    public void BuyAllMerge(string? buyOnlyThis = null, mergeOptionsEnum? buyMode = null)
    {
        //Only edit the map and shopID here
        Adv.StartBuyAllMerge("evilwarnul", 456, findIngredients, buyOnlyThis, buyMode: buyMode);

        #region Dont edit this part
        void findIngredients()
        {
            ItemBase req = Adv.externalItem;
            int quant = Adv.externalQuant;
            int currentQuant = req.Temp ? Bot.TempInv.GetQuantity(req.Name) : Bot.Inventory.GetQuantity(req.Name);
            if (req == null)
            {
                Core.Logger("req is NULL");
                return;
            }

            switch (req.Name)
            {
                default:
                    bool shouldStop = !Adv.matsOnly || !dontStopMissingIng;
                    Core.Logger($"The bot hasn't been taught how to get {req.Name}." + (shouldStop ? " Please report the issue." : " Skipping"), messageBox: shouldStop, stopBot: shouldStop);
                    break;
                #endregion

                case "Diamond of Nulgath":
                    Nation.FarmDiamondofNulgath(quant);
                    break;

                case "Archfiend's Favor":
                    Nation.ApprovalAndFavor(0, quant);
                    break;

                case "Nulgath's Approval":
                    Nation.ApprovalAndFavor(quant, 0);
                    break;

                case "Taro's Manslayer":
                    Taro.GuardianTaro(true);
                    break;

                case "Dark Crystal Shard":
                    Nation.FarmDarkCrystalShard(quant);
                    break;

                case "Tainted Gem":
                    Nation.FarmTaintedGem(quant);
                    break;

                case "Blade of Holy Might":
                    Core.EquipClass(ClassType.Solo);
                    Core.HuntMonster("northlands", "Aisha's Drake", "Blade of Holy Might", isTemp: false);
                    break;

                case "Blood Gem of the Archfiend":
                    Nation.FarmBloodGem(quant);
                    break;

                case "Unidentified 13":
                    Nation.FarmUni13(quant);
                    break;

                case "Iron":
                    BLOD.UnlockMineCrafting();
                    Dailies.MineCrafting(new[] { "Iron" }, quant);
                    break;

                case "Gem of Nulgath":
                    Nation.FarmGemofNulgath(quant);
                    break;

                case "Totem of Nulgath":
                    Nation.FarmTotemofNulgath(quant);
                    break;

                case "Bone Dust":
                    Farm.BattleUnderB("Bone Dust", quant);
                    break;

                case "Cloak of Nulgath":
                    Core.BuyItem("tercessuinotlim", 4667, "Cloak of Nulgath");
                    break;

                case "Staff of Imp Fire":
                    Core.EquipClass(ClassType.Farm);
                    Core.HuntMonster("bludrut2", "Fire Elemental", "Staff of Imp Fire", isTemp: false);
                    break;

                case "Cool Head":
                    Core.BuyItem("tercessuinotlim", 4826, "Cool Head");
                    break;

                case "Primal Dread Fang":
                    Nation.Supplies("Primal Dread Fang", quant);
                    break;

                case "Random Weapon of Nulgath":
                    Nation.Supplies("Random Weapon of Nulgath", quant);
                    break;

                case "Voucher of Nulgath (non-mem)":
                    Nation.FarmVoucher(false);
                    break;

                case "Unidentified 27":
                    Core.FarmingLogger(req.Name, quant);
                    Core.EnsureAccept(584);
                    Nation.Supplies("Unidentified 26");

                    Core.ResetQuest(7551);
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
                            if (!Bot.Player.InCombat)
                                Core.Sleep();  // Use the built-in delay
                            Bot.Combat.Attack("*");
                            if (Core.CheckInventory("Dark Makai Sigil"))
                                break;
                        }
                    }
                    Bot.Wait.ForPickup("Dark Makai Sigil");
                    Core.EnsureComplete(584);
                    Bot.Wait.ForPickup(req.Name);
                    break;

                case "Crystal Phoenix Blade of Nulgath":
                    Core.FarmingLogger(req.Name, quant);
                    Nation.FarmDiamondofNulgath(13);
                    Nation.FarmDarkCrystalShard(50);
                    Nation.FarmTotemofNulgath(3);
                    Nation.FarmGemofNulgath(20);
                    Nation.FarmVoucher(false);
                    Nation.SwindleBulk(50);

                    Core.EnsureAccept(837);
                    Core.HuntMonster("underworld", "Undead Bruiser", "Undead Bruiser Rune");
                    Core.EnsureComplete(837, req.ID);
                    Bot.Wait.ForPickup(req.Name);
                    break;

                case "Dragon Scale":
                    Core.FarmingLogger(req.Name, quant);
                    Core.EquipClass(ClassType.Farm);
                    while (!Bot.ShouldExit && !Core.CheckInventory(11475, quant))
                        Core.KillMonster("lair", "Hole", "Center", "*", isTemp: false, log: false);
                    break;

                case "Gold Star of Avarice":
                    Core.FarmingLogger(req.Name, quant);
                    Adv.BuyItem("tercessuinotlim", 1951, "Gold Star of Avarice");
                    break;
            }
        }
    }

    public List<IOption> Select = new()
    {
       new Option<bool>("13347", "Abyssal Priest of Nulgath", "Mode: [select] only\nShould the bot buy \"Abyssal Priest of Nulgath\" ?", false),
        new Option<bool>("25215", "Loyalty Blade of the Nation", "Mode: [select] only\nShould the bot buy \"Loyalty Blade of the Nation\" ?", false),
        new Option<bool>("25239", "Oversoul Witch of Nulgath", "Mode: [select] only\nShould the bot buy \"Oversoul Witch of Nulgath\" ?", false),
        new Option<bool>("25240", "OverSoul Witch's Hat", "Mode: [select] only\nShould the bot buy \"OverSoul Witch's Hat\" ?", false),
        new Option<bool>("25198", "Staff of Soul-Stealing", "Mode: [select] only\nShould the bot buy \"Staff of Soul-Stealing\" ?", false),
        new Option<bool>("25241", "OverSoul Paladin", "Mode: [select] only\nShould the bot buy \"OverSoul Paladin\" ?", false),
        new Option<bool>("25252", "Bearded Axe of Nulgath", "Mode: [select] only\nShould the bot buy \"Bearded Axe of Nulgath\" ?", false),
        new Option<bool>("25251", "Dual Bearded Axe of Nulgath", "Mode: [select] only\nShould the bot buy \"Dual Bearded Axe of Nulgath\" ?", false),
        new Option<bool>("25266", "Oversoul Dire Monk", "Mode: [select] only\nShould the bot buy \"Oversoul Dire Monk\" ?", false),
        new Option<bool>("25264", "OverSoul Cleric Cape", "Mode: [select] only\nShould the bot buy \"OverSoul Cleric Cape\" ?", false),
        new Option<bool>("25263", "OverSoul Witch Cape", "Mode: [select] only\nShould the bot buy \"OverSoul Witch Cape\" ?", false),
        new Option<bool>("25256", "OverSoul Cleric Locks", "Mode: [select] only\nShould the bot buy \"OverSoul Cleric Locks\" ?", false),
        new Option<bool>("25267", "OverSoul Dire Monk Head", "Mode: [select] only\nShould the bot buy \"OverSoul Dire Monk Head\" ?", false),
        new Option<bool>("25265", "OverSoul Paladin Head", "Mode: [select] only\nShould the bot buy \"OverSoul Paladin Head\" ?", false),
        new Option<bool>("25255", "Oversoul Cleric of Nulgath", "Mode: [select] only\nShould the bot buy \"Oversoul Cleric of Nulgath\" ?", false),
        new Option<bool>("27849", "Golden Hanzo Void Katana", "Mode: [select] only\nShould the bot buy \"Golden Hanzo Void Katana\" ?", false),
        new Option<bool>("27850", "Dual Golden Hanzo Void Katanas", "Mode: [select] only\nShould the bot buy \"Dual Golden Hanzo Void Katanas\" ?", false),
        new Option<bool>("27843", "Golden Hanzo Void", "Mode: [select] only\nShould the bot buy \"Golden Hanzo Void\" ?", false),
        new Option<bool>("27851", "Golden Hanzo Void Mask", "Mode: [select] only\nShould the bot buy \"Golden Hanzo Void Mask\" ?", false),
        new Option<bool>("27852", "Golden Hanzo Void Horns", "Mode: [select] only\nShould the bot buy \"Golden Hanzo Void Horns\" ?", false),
        new Option<bool>("28031", "Golden Hanzo Katana Cape", "Mode: [select] only\nShould the bot buy \"Golden Hanzo Katana Cape\" ?", false),
        new Option<bool>("27848", "Golden Hanzo Void Cape", "Mode: [select] only\nShould the bot buy \"Golden Hanzo Void Cape\" ?", false),
        new Option<bool>("30208", "Cyber Crystal Phoenix Blade", "Mode: [select] only\nShould the bot buy \"Cyber Crystal Phoenix Blade\" ?", false),
        new Option<bool>("344", "Blood Cloak", "Mode: [select] only\nShould the bot buy \"Blood Cloak\" ?", false),
        new Option<bool>("52883", "Horned Worshipper Face of Nulgath", "Mode: [select] only\nShould the bot buy \"Horned Worshipper Face of Nulgath\" ?", false),
        new Option<bool>("52884", "Horned Worshipper Visage of Nulgath", "Mode: [select] only\nShould the bot buy \"Horned Worshipper Visage of Nulgath\" ?", false),
        new Option<bool>("69968", "Blood Ranger", "Mode: [select] only\nShould the bot buy \"Blood Ranger\" ?", false),
        new Option<bool>("69973", "Blood Ranger Sword and Sheath", "Mode: [select] only\nShould the bot buy \"Blood Ranger Sword and Sheath\" ?", false),
        new Option<bool>("69972", "Blood Ranger Bow", "Mode: [select] only\nShould the bot buy \"Blood Ranger Bow\" ?", false),
        new Option<bool>("69969", "Blood Ranger Morph", "Mode: [select] only\nShould the bot buy \"Blood Ranger Morph\" ?", false),
        new Option<bool>("69971", "Blood Ranger Cape", "Mode: [select] only\nShould the bot buy \"Blood Ranger Cape\" ?", false),
        new Option<bool>("69970", "Blood Ranger Quiver", "Mode: [select] only\nShould the bot buy \"Blood Ranger Quiver\" ?", false),
        new Option<bool>("70001", "Storm Knight", "Mode: [select] only\nShould the bot buy \"Storm Knight\" ?", false),
        new Option<bool>("70002", "Storm Knight Helm", "Mode: [select] only\nShould the bot buy \"Storm Knight Helm\" ?", false),
        new Option<bool>("69152", "Molten DragonSpear of Nulgath", "Mode: [select] only\nShould the bot buy \"Molten DragonSpear of Nulgath\" ?", false),
        new Option<bool>("83410", "Formal Nation Suit", "Mode: [select] only\nShould the bot buy \"Formal Nation Suit\" ?", false),
        new Option<bool>("83411", "Formal Nation Hair", "Mode: [select] only\nShould the bot buy \"Formal Nation Hair\" ?", false),
        new Option<bool>("83412", "Formal Nation Locks", "Mode: [select] only\nShould the bot buy \"Formal Nation Locks\" ?", false),
        new Option<bool>("83413", "Formal Nation Mask", "Mode: [select] only\nShould the bot buy \"Formal Nation Mask\" ?", false),
        new Option<bool>("83414", "Formal Nation Circlet", "Mode: [select] only\nShould the bot buy \"Formal Nation Circlet\" ?", false),
        new Option<bool>("83415", "Formal Nation Morph", "Mode: [select] only\nShould the bot buy \"Formal Nation Morph\" ?", false),
        new Option<bool>("83416", "Formal Nation Visage", "Mode: [select] only\nShould the bot buy \"Formal Nation Visage\" ?", false),
        new Option<bool>("83417", "Formal Nation Mask Morph", "Mode: [select] only\nShould the bot buy \"Formal Nation Mask Morph\" ?", false),
        new Option<bool>("83418", "Formal Nation Circlet Visage", "Mode: [select] only\nShould the bot buy \"Formal Nation Circlet Visage\" ?", false),
        new Option<bool>("83419", "Formal Nation Blade", "Mode: [select] only\nShould the bot buy \"Formal Nation Blade\" ?", false),
        new Option<bool>("83420", "Formal Nation Blades", "Mode: [select] only\nShould the bot buy \"Formal Nation Blades\" ?", false),
        new Option<bool>("83421", "Formal Nation Cane", "Mode: [select] only\nShould the bot buy \"Formal Nation Cane\" ?", false),
        new Option<bool>("83422", "Formal Nation Cane Sheath", "Mode: [select] only\nShould the bot buy \"Formal Nation Cane Sheath\" ?", false),
    };
}
