/*
name: Combat Trophy
description: This script will farm Combat Trophies in /bludrutbtawl
tags: combat, trophy, pvp, bludrut, brawl
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
using Skua.Core.Interfaces;

public class CombatTrophy
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreAdvanced Adv = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        DoCombatTrophy();

        Core.SetOptions(false);
    }

    public void DoCombatTrophy()
    {
        //Adv.BestGear(GenericGearBoost.dmgAll);
        Farm.BludrutBrawlBoss();
    }
}
