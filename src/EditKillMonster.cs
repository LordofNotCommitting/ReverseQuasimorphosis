
using HarmonyLib;
using MGSC;

using UnityEngine;


namespace ReverseQuasimorphosis
{


    [HarmonyPatch(typeof(CreatureSystem), nameof(CreatureSystem.KillMonster))]
    public class EditKillMonster
    {



        // Navigation - proxy company
        static int Kill_Human_QGain_Mult_Perc = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Kill_Human_QGain_Mult_Perc", 200);
        static int Kill_Quasi_QGain_Mult_Perc = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Kill_Quasi_QGain_Mult_Perc", 60);
        static float Kill_Human_QGain_Mult = ((float)Kill_Human_QGain_Mult_Perc) / 100f;
        static float Kill_Quasi_QGain_Mult = ((float)Kill_Quasi_QGain_Mult_Perc) / 100f;
        
        public static void Prefix(Statistics statistics, Scenarios scenarios, RaidMetadata raidMetadata, Creatures creatures, QmorphosController qmorphosController, CombatLog combatLog, LocationMetadata locationMetadata, ref Creature creature, float releaseDelay)
        {
            if (creature.CreatureData.CreatureClass == CreatureClass.Quasimorph) {
                creature.CreatureData.QuazimorphosisReward = (int)(creature.CreatureData.QuazimorphosisReward * Kill_Quasi_QGain_Mult);
            }
            if (creature.CreatureData.CreatureClass == CreatureClass.Human)
            {
                creature.CreatureData.QuazimorphosisReward = (int)(creature.CreatureData.QuazimorphosisReward * Kill_Human_QGain_Mult);
            }

        }
    }

}