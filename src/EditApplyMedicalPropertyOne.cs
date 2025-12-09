
using HarmonyLib;
using MGSC;
using System.Collections.Generic;
using UnityEngine;


namespace ReverseQuasimorphosis
{


    [HarmonyPatch(typeof(HealthScreen), nameof(HealthScreen.ProcessMedkitSideEffects))]
    public class EditApplyMedicalPropertyOne
    {
        static int Medical_QGain_Mult_Perc = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Medical_QGain_Mult_Perc", 50);
        static float Medical_QGain_Mult = ((float)Medical_QGain_Mult_Perc) / 100f;

        static int temp_quasival = 50;
        static bool changed = false;
        static FixationMedicineRecord temp_record;
        public static void Prefix(ref HealthScreen __instance)
        {

            temp_record = __instance._medkitItem.Record<FixationMedicineRecord>();
            if (temp_record.QmorphosValue > 0 && temp_record.Id != "quasi_medical_kit_1") {
                temp_quasival = temp_record.QmorphosValue;
                changed = true;
                temp_record.QmorphosValue = (int)(temp_record.QmorphosValue * Medical_QGain_Mult);
            }
        }

        public static void Postfix(ref HealthScreen __instance)
        {
            if (changed)
            {
                temp_record.QmorphosValue = temp_quasival;
                changed = false;
            }
        }
    }

}
