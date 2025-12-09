
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


        public static bool Prefix(QmorphosController __instance)
        {
            // --- SAFE CALL TO base.ProcessActionPoint() ---
            // Equivalent to: base.ProcessActionPoint();
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



        /*
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
            if (saved_qval == 0) {
                saved_qval = __instance.QmorphosValue;
            }

            QmorphosRecord recordByLevel = __instance.GetRecordByLevel(saved_qval, __instance._raidMetadata.BramfaturaId);
            saved_qval += num2;
            QmorphosRecord recordByLevel2 = __instance.GetRecordByLevel(saved_qval, __instance._raidMetadata.BramfaturaId);

            __instance.QmorphosValue = saved_qval;

            if (recordByLevel != recordByLevel2)
            {
                __instance.InvokeRefreshStage(recordByLevel2, true);
            }

        }
        */
    }
}