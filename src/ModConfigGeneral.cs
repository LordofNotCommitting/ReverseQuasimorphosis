using MGSC;
using ModConfigMenu.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReverseQuasimorphosis
{
    // Token: 0x02000006 RID: 6
    public class ModConfigGeneral
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002840 File Offset: 0x00000A40



        public ModConfigGeneral(string ModName, string ConfigPath)
        {
            this.ModName = ModName;
            this.ModData = new ModConfigData(ConfigPath);
            this.ModData.AddConfigHeader("STRING:General Settings", "general");
            this.ModData.AddConfigValue("general", "about_final", "STRING:<color=#f51b1b>The game must be restarted after setting then saving this config to take effect.</color>\n");

            this.ModData.AddConfigHeader("STRING:Qgain", "Quasimorphosis Gain");

            this.ModData.AddConfigValue("Quasimorphosis Gain", "about_qgain_1", "STRING:This mod by default reverse natural increase of quasimorphosis. Which is roughly <color=#f51b1b>0.7</color> per turn.\n");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Passive_Default_QGain_Reverted", true, "STRING:Revert Default Q Gain", "STRING:From positive on average 0.7 per turn quasimorphosis gain, turn it into -0.7 per turn quasimorphosis gain.");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Passive_AdditDefault_QGain", -3, -20, 10, "STRING:Addit. Passive Default Q Gain", "STRING:On top of natural 0.7 (or -0.7 if reverted) Quasimorphosis gain, additional Quasimorphosis Gain/loss per turn.");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Kill_Human_QGain_Mult_Perc", 200, 0, 1000, "STRING:Human Kill Multiplier %", "STRING:% Quasimorphosis gain multiplier for killing humans.");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Kill_Quasi_QGain_Mult_Perc", 60, 0, 1000, "STRING:Quasi Kill Multiplier %", "STRING:% Quasimorphosis gain multiplier for killing Quasimorphs.");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Consumables_QGain_Mult_Perc", 20, 0, 1000, "STRING:Consumable Multiplier %", "STRING:% Quasimorphosis gain multiplier for eating consumables.");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Medical_QGain_Mult_Perc", 50, 0, 1000, "STRING:Medical Multiplier %", "STRING:% Quasimorphosis gain multiplier for using Medical item.");

            this.ModData.AddConfigValue("Quasimorphosis Gain", "Passive_Effect_QGain_Cap_Min", -2, -20, -1, "STRING:Alternative Per turn Q-gain Min Cap", "STRING:Quasimorphosis gain per turn from implant or effect (gavakh addiction) Minimum cap.");
            this.ModData.AddConfigValue("Quasimorphosis Gain", "Passive_Effect_QGain_Cap_Max", 1, 0, 20, "STRING:Alternative Per turn Q-gain Max Cap", "STRING:Quasimorphosis gain per turn from implant or effect (gavakh addiction) Maximum cap.");

            this.ModData.AddConfigValue("General Gain", "Quasi_Resist_Phasing_On", true, "STRING:Quasi Resist Phasing", "STRING:Instead of phasing when below 100 quasimorphosis, most of quasimorphs will convert themselves at 100% quasimorphosis gain (instead to [Quasi Kill Multiplier %] used below) to let other quasimorphs stay in realspace.");

            this.ModData.AddConfigHeader("STRING:Qredux", "Max Quasimorphosis Reduction");
            this.ModData.AddConfigValue("Max Quasimorphosis Reduction", "about_max_1", "STRING:If enabled, Upon reaching Minimum non-zero quasimorphosis. Max quasimorphosis will be reduced by <color=#f51b1b>[Max Q Reduction Per Proc]</color> while quasimorphosis meter will be increased by <color=#f51b1b>[Q Reduction Cost Per Proc]</color> as a cost.\n");
            this.ModData.AddConfigValue("Max Quasimorphosis Reduction", "Quasi_MaxReduction_IsOn", true, "STRING:Max Q Reduction On", "STRING:Enable Max Quasimorphosis reduction");
            this.ModData.AddConfigValue("Max Quasimorphosis Reduction", "Quasi_MaxReduction_Amount", 50, 20, 100, "STRING:Max Q Reduction Per Proc", "STRING:Max Quasimorphosis reduction upon reaching minimum quasimorphosis");
            this.ModData.AddConfigValue("Max Quasimorphosis Reduction", "Quasi_MaxReduction_Cost", 100, 10, 100, "STRING:Q Reduction Cost Per Proc", "STRING:Quasimorphosis increase for the cost of decreasing max quasimorphosis");

            //this.ModData.AddConfigValue("general", "Debug_Log_On", false, "STRING:Debug Log", "STRING:For personal debugging. DO NOT TURN IT ON if you don't intend to.");

            this.ModData.RegisterModConfigData(ModName);
        }

        // Token: 0x04000011 RID: 17
        private string ModName;

        // Token: 0x04000012 RID: 18
        public ModConfigData ModData;

    }
}
