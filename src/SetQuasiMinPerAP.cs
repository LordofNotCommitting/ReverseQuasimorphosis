
using HarmonyLib;
using MGSC;
using System.Reflection;
using UnityEngine;


namespace ReverseQuasimorphosis
{


    [HarmonyPatch(typeof(QmorphosController), nameof(QmorphosController.ProcessActionPoint))]
    [HarmonyPatch(new System.Type[] { })]
    public class SetQuasiMinPerAP
    {
        static int saved_qval;


        static bool Quasi_MaxReduction_IsOn = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Quasi_MaxReduction_IsOn", false);
        static int Quasi_MaxReduction_Amount = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Quasi_MaxReduction_Amount", 50);
        static int Quasi_MaxReduction_Cost = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Quasi_MaxReduction_Cost", 100);


        static bool Passive_Default_QGain_Reverted = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Passive_Default_QGain_Reverted", false);
        static int Passive_AdditDefault_QGain = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Passive_AdditDefault_QGain", -3);
        static int Passive_Effect_QGain_Cap_Min = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Passive_Effect_QGain_Cap_Min", -3);
        static int Passive_Effect_QGain_Cap_Max = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Passive_Effect_QGain_Cap_Max", 1);

        static string temp_recordByLevel;


        public static bool Prefix(ref QmorphosController __instance)
        {
            if (Quasi_MaxReduction_IsOn && !__instance._raidMetadata.QMorphosBaronSpawned && __instance._raidMetadata.QMorphosMinLevel == __instance._raidMetadata.QMorphosLevel && __instance._raidMetadata.QMorphosMinLevel > 0 && __instance._raidMetadata.QMorphosMinLevel < 1000)
            {
                __instance._raidMetadata.QMorphosMinLevel = Mathf.Max(__instance._raidMetadata.QMorphosMinLevel - Quasi_MaxReduction_Amount, 0);
                __instance.QmorphosValue = Mathf.Min(__instance._raidMetadata.QMorphosMinLevel + Quasi_MaxReduction_Cost, 1000);
                __instance.InvokeRefreshStage(__instance.Now, true);
            }
            saved_qval = __instance.QmorphosValue;
            //Plugin.Logger.Log("saved quasi" + saved_qval);
            return true;
        }
        public static void Postfix(QmorphosController __instance)
        {

            int default_multi = (Passive_Default_QGain_Reverted ? -1 : 1);
            int weightedIndex = DropManager.GetWeightedIndex(Data.Global.QmorphosPerTurnWeights);
            int num = Mathf.Clamp(Mathf.RoundToInt(__instance._creatures.Player.CreatureData.EffectsController.SumEffectsValue<WoundEffectQmorph>((WoundEffectQmorph w) => (float)w.QmorphValue)), Passive_Effect_QGain_Cap_Min, Passive_Effect_QGain_Cap_Max);
            int num2 = Mathf.RoundToInt((float)((Data.Global.QmorphosPerTurnPoints[weightedIndex] * default_multi) + num + Passive_AdditDefault_QGain) * __instance._difficulty.Preset.QmorphLevelGrowth);


            //Plugin.Logger.Log("loaded quasi" + saved_qval);
            //loaded save or such? so this value could be uninitialized.
            if (saved_qval == 0)
            {
                saved_qval = __instance.QmorphosValue;
            }

            //QmorphosRecord recordByLevel = __instance.GetRecordByLevel(saved_qval, __instance._raidMetadata.BramfaturaId);
            saved_qval += num2;
            QmorphosRecord recordByLevel2 = __instance.GetRecordByLevel(saved_qval, __instance._raidMetadata.BramfaturaId);

            //__instance.QmorphosValue = 0;
            //need to re-proc... quasi behaviors, since setting it to value did not exactly  (implication unknown)
            __instance.QmorphosValue = saved_qval;

            //Plugin.Logger.Log("hunt data" + temp_recordByLevel);
            //Plugin.Logger.Log("hunt data" + recordByLevel2);

            // no idea what the default behavior is for Descent. but it is not global aggro I think
            if (__instance._missions.Get(__instance._raidMetadata).ProcMissionType != ProceduralMissionType.BramfaturaInvasion ) {
                //temp_recordByLevel = recordByLevel2.ToString();
                //seeing if this fix derpy quasis
                //make them behave as usual upon the correct conditions.
                if (__instance.QmorphosValue >= 800)
                {
                    //Plugin.Logger.Log("proccing hunt");
                    __instance.SetGlobalEndlessHunt(true);
                }
                else
                {
                   // Plugin.Logger.Log("un-proccing hunt");
                    __instance.SetGlobalEndlessHunt(false);
                }
            }
            

        }

        /*
        public static bool Prefix(QmorphosController __instance)
        {
            // something wrong with turnable call when defending against quasis.
            // so this function is not usable.
            MethodInfo baseMethod = AccessTools.Method(typeof(Turnable), "ProcessActionPoint");
            baseMethod.Invoke(__instance, null);
            
            //squash max quasimorphosis if it is on
            
            if (Quasi_MaxReduction_IsOn && !__instance._raidMetadata.QMorphosBaronSpawned && __instance._raidMetadata.QMorphosMinLevel == __instance._raidMetadata.QMorphosLevel && __instance._raidMetadata.QMorphosMinLevel > 0 && __instance._raidMetadata.QMorphosMinLevel < 1000)
            {
                __instance._raidMetadata.QMorphosMinLevel = Mathf.Max(__instance._raidMetadata.QMorphosMinLevel - Quasi_MaxReduction_Amount, 0);
                __instance.QmorphosValue = Mathf.Min(__instance._raidMetadata.QMorphosMinLevel + Quasi_MaxReduction_Cost, 1000);
                __instance.InvokeRefreshStage(__instance.Now, true);
            }
            
            // --- ORIGINAL METHOD BODY EXACT COPY ---

            if (__instance._raidMetadata.StationVisit)
            {
                __instance.IsProcessing = false;
                return false; // skip original
            }

            //slightly altered logic
            int default_multi = (Passive_Default_QGain_Reverted ? -1 : 1);
            int weightedIndex = DropManager.GetWeightedIndex(Data.Global.QmorphosPerTurnWeights);
            int num = Mathf.Clamp(Mathf.RoundToInt(__instance._creatures.Player.CreatureData.EffectsController.SumEffectsValue<WoundEffectQmorph>((WoundEffectQmorph w) => (float)w.QmorphValue)), Passive_Effect_QGain_Cap_Min, Passive_Effect_QGain_Cap_Max);
            int num2 = Mathf.RoundToInt((float)((Data.Global.QmorphosPerTurnPoints[weightedIndex] * default_multi) + num + Passive_AdditDefault_QGain) * __instance._difficulty.Preset.QmorphLevelGrowth);


            __instance.QmorphosValue += num2;

            __instance.TryEccolapseAliveMonsters();
            __instance.TryMutateAliveMonsters();

            __instance.IsProcessing = false;

            return false;
        }
        */

    }
}
