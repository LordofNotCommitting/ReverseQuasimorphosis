
using HarmonyLib;
using MGSC;

using UnityEngine;


namespace ReverseQuasimorphosis
{


    [HarmonyPatch(typeof(EccolapseBack), nameof(EccolapseBack.ProcessActionPoint))]
    [HarmonyPatch(new System.Type[] { })]
    public class FixEccolapseBack
    {

        static int Kill_Quasi_QGain_Mult_Perc = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Kill_Quasi_QGain_Mult_Perc", 60);

        static bool Quasi_Resist_Phasing_On = Plugin.ConfigGeneral.ModData.GetConfigValue<bool>("Quasi_Resist_Phasing_On", true);

        static float Kill_Quasi_QGain_Mult = ((float)Kill_Quasi_QGain_Mult_Perc) / 100f;
        public static void Postfix(EccolapseBack __instance)
        {
            if (Quasi_Resist_Phasing_On && __instance._turnTimer.APsLeft > 0)
            {
                if (UnityEngine.Random.Range(0f, 1f) <= 0.8)
                {
                    //PERISH, while leaving quasimorphosis.
                    __instance._owner.CreatureData.Health.ReasonOfDeath = HealthInfo.DeathReason.ReceiveDamage;
                    __instance._owner.Creature3dView.InvulnerabilityEffectView.SetActive(false);
                    __instance._owner.CreatureData.Health.SetInvulnerability(false);
                    __instance._owner.CreatureData.IsEccolapsing = false;
                    __instance._owner.CreatureData.Health.Value = 0;
                    if (Kill_Quasi_QGain_Mult_Perc != 0) {
                        __instance._owner.CreatureData.QuazimorphosisReward = (int)(__instance._owner.CreatureData.QuazimorphosisReward / Kill_Quasi_QGain_Mult);
                    }


                }

            }
        }
    }

}