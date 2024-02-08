/*
name: null
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/Nation/CoreNation.cs
//cs_include Scripts/Nation/AssistingCragAndBamboozle[Mem].cs
using Skua.Core.Interfaces;
using Skua.Core.Options;
using Skua.Core.Models.Items;

public class CoreVHL
{
    // [Can Change]
    // True = 
    // False = It will automatically check if you got the things, but if you want to turn this off either way, heres the option.
    // Recommended: true
    public bool UseSparrowMethod = true;

    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public static CoreBots sCore => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreAdvanced Adv = new();
    public CoreDailies Daily = new();
    public CoreNation Nation = new();
    public AssistingCragAndBamboozle ACAB = new();

    public string OptionsStorage = "VoidHighLordOptions";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new()
    {
        new Option<bool>("SparrowMethod", "Use Sparrow's Blood Method",
            "When possible, it will use \"Assisting Crag and Bamboozle\" to get an additional Elders' Blood per day.\n" +
            "Needs Crag and Bamboozle and is Legend-Only.\n" +
            "Will not be done if you don't meed the conditions\n" +
            "Recommended setting: True", true),
        CoreBots.Instance.SkipOptions
    };

    public void ScriptMain(IScriptInterface bot)
    {
        Core.RunCore();
    }

    public void GetVHL(bool rankUpClass = true)
    {
        if (Core.CheckInventory("Void Highlord"))
            return;

        VHLChallenge(15);
        VHLCrystals();

        Adv.BuyItem("tercessuinotlim", 1355, "Void Highlord");

        if (rankUpClass)
            Adv.RankUpClass("Void Highlord");
    }

    public void VHLChallenge(int quant)
    {
        if (Core.CheckInventory("Roentgenium of Nulgath", quant))
            return;

        Core.Logger("Getting Void HighLord Challenge prerequisites");
        Farm.Experience(80);
        Core.AddDrop(Nation.bagDrops);
        if (!Core.CheckInventory(ChallengeRewards, toInv: false))
            Core.AddDrop(ChallengeRewards);
        Core.AddDrop("Roentgenium of Nulgath");

    Continue:
        Core.KillMonster("tercessuinotlim", "m4", "Right", "Shadow of Nulgath", "Hadean Onyx of Nulgath", isTemp: false);

        Core.FarmingLogger("Roentgenium of Nulgath", quant);
        int CurrentRoent = Bot.Inventory.GetQuantity("Roentgenium of Nulgath");
        while (!Bot.ShouldExit && !Core.CheckInventory("Roentgenium of Nulgath", quant))
        {
            Core.EnsureAccept(5660);

            if (!Core.CheckInventory("Elders' Blood", ((quant - (Bot.Inventory.Items.FirstOrDefault(x => x.Name == "Elders' Blood" && x.Name != null)?.Quantity ?? 0)) > 5 ? 5 : (quant - (Bot.Inventory.Items.FirstOrDefault(x => x.Name == "Elders' Blood" && x.Name != null)?.Quantity ?? 0)))))
            {
                Daily.EldersBlood();
                _SparrowMethod((quant - (Bot.Inventory.Items.FirstOrDefault(x => x.Name == "Elders' Blood"
                && x.Name != null)?.Quantity ?? 0)) > 5 ? 5 : (quant - (Bot.Inventory.Items.FirstOrDefault(x => x.Name == "Elders' Blood" && x.Name != null)?.Quantity ?? 0)));
            }


            Nation.FarmVoucher(false);
            Farm.BlackKnightOrb();
            Adv.BuyItem("citadel", 44, 38316, shopItemID: 22367);
            Adv.BuyItem("yulgar", 16, "Aelita's Emerald");
            Nation.FarmUni13(1);
            Nation.FarmGemofNulgath(20);
            Nation.EmblemofNulgath(20);
            Nation.EssenceofNulgath(50);
            Nation.SwindleBulk(100);
            Nation.ApprovalAndFavor(300, 300);


            Core.EnsureComplete(5660);
            Bot.Wait.ForPickup("Roentgenium of Nulgath");
            Core.ToBank(ChallengeRewards);

            if (Core.CheckInventory("Elders' Blood") && !Core.CheckInventory("Roentgenium of Nulgath", quant))
                goto Continue;
            else if (!Core.CheckInventory("Elders' Blood") && !Core.CheckInventory("Roentgenium of Nulgath", quant))
            {
                Core.Logger($"Not enough \"Elders' Blood\", please do the daily {2 - (Bot.Inventory.Items.FirstOrDefault(x =>
                    x.Name == "Elders' Blood" &&
                    x.Name != null)?.Quantity ?? 0)}");
                FarmExtra();
            }
            else return;
        }
    }
    private readonly string[] ChallengeRewards = { "Void Highlord Armor", "Helm of the Highlord", "Highlord's Void Wrap" };

    public void VHLCrystals()
    {
        if (Core.CheckInventory("Void Crystal A") && Core.CheckInventory("Void Crystal B"))
            return;

        Core.Logger("Obtaining Void Crystal A & Void Crystal B");
        Core.AddDrop(Nation.bagDrops);

        if (!Core.CheckInventory("Elders' Blood", 2))
            Daily.EldersBlood();
        _SparrowMethod(2);

        Nation.FarmUni13(1);
        Nation.FarmUni10(200);
        Nation.FarmGemofNulgath(150);
        Nation.FarmDarkCrystalShard(200);
        Nation.FarmDiamondofNulgath(200);
        Nation.FarmBloodGem(30);
        Nation.FarmTotemofNulgath(15);
        Nation.SwindleBulk(200);

        if (!Core.CheckInventory("Elders' Blood", 2))
            Core.Logger($"Not enough \"Elders' Blood\", please do the daily {2 - Bot.Inventory.GetQuantity("Elders' Blood")} more times (not today)", messageBox: true, stopBot: true);

        Adv.BuyItem("tercessuinotlim", 1355, "Void Crystal A");
        Adv.BuyItem("tercessuinotlim", 1355, "Void Crystal B");
    }


    private void FarmExtra(int quant = 15)
    {
        if (!Core.CheckInventory("Roentgenium of Nulgath", quant))
        {
            int quantity = Bot.Inventory.Items.FirstOrDefault(x => x.Name == "Roentgenium of Nulgath")?.Quantity ?? 0;

            Core.Logger($"Roentgenium of Nulgath: ({quantity}/{quant})", "Not Enough Roent");
            Core.Logger("Not enough \"Roentgenium of Nulgath\"\n" +
                "maxing mats so it's easier tomorrow\n" +
                "you can just leave this running");

            // Farm Mats for Tomarrow.
            Nation.FarmVoucher(false, true);
            Farm.BlackKnightOrb();
            Adv.BuyItem("citadel", 44, 38316, shopItemID: 22367);
            Adv.BuyItem("yulgar", 16, "Aelita's Emerald");
            Nation.FarmUni13(13);
            Nation.FarmGemofNulgath(1000);
            Nation.EmblemofNulgath(500);
            Nation.EssenceofNulgath(60);
            Nation.SwindleBulk(1000);
            Nation.ApprovalAndFavor(5000, 5000);

            Core.Logger("Materials max out! You should be good for tomorrow.", messageBox: true, stopBot: true);
        }
    }
    private void _SparrowMethod(int EldersBloodQuant)
    {
        if (!Bot.Config!.Get<bool>("SparrowMethod") || !Core.IsMember || !Core.CheckInventory(Nation.CragName) || Core.CheckInventory("Elders' Blood", EldersBloodQuant))
            return;

        Core.Logger("Sparrow Method is enabled, the bot will now max out Totems, BloodGems, Uni19 and Vouchers in order to get another Elders' Blood. This may take a while");

        ItemBase item = Core.EnsureLoad(7551).Rewards.Find(x => x.ID == 57446) ?? new ItemBase();

        Core.AddDrop("Totem of Nulgath", "Blood Gem of Nulgath", "Voucher of Nulgath", "Voucher of Nulgath (non-mem)");
        Nation.FarmTotemofNulgath();
        Nation.FarmBloodGem();
        if (!Core.CheckInventory("Unidentified 19"))
        {
            Nation.SwindleReturn(item.Name, 6);
            Adv.BuyItem("tercessuinotlim", 1951, "Unidentified 19");
        }
        Nation.FarmVoucher(false);
        Nation.FarmVoucher(true);
        ACAB.AssistingCandB();
    }
}
