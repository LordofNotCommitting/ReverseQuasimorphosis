
using HarmonyLib;
using MGSC;

using UnityEngine;


namespace ReverseQuasimorphosis
{


    [HarmonyPatch(typeof(QmorphosController), nameof(QmorphosController.LowestQmorphosLevelWithoutSomnia))]
    [HarmonyPatch(new System.Type[] { })]
    public class ReduceQuasiMin
    {

        static bool Quasi_MaxReduction_IsOn = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Quasi_MaxReduction_IsOn", false);
        static int Quasi_MaxReduction_Amount = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Quasi_MaxReduction_Amount", 50);
        static int Quasi_MaxReduction_Cost = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Quasi_MaxReduction_Cost", 100);
        public static int Postfix(int __result)
        {
            int temp = __result;
            if (Quasi_MaxReduction_IsOn)
            {
                temp = (__result == 100 ? (100 + Quasi_MaxReduction_Amount) : __result);
            }
            return temp;
        }
    }

}
