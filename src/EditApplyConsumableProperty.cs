
using HarmonyLib;
using MGSC;
using System.Collections.Generic;
using UnityEngine;


namespace ReverseQuasimorphosis
{


    [HarmonyPatch(typeof(ItemInteractionSystem), nameof(ItemInteractionSystem.ApplyConsumableProperty))]
    public class EditApplyConsumableProperty
    {

        static int Medical_QGain_Mult_Perc = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Medical_QGain_Mult_Perc", 50);
        static float Medical_QGain_Mult = ((float)Medical_QGain_Mult_Perc) / 100f;

        static int Consumables_QGain_Mult_Perc = Plugin.ConfigGeneral.ModData.GetConfigValue<int>("Consumables_QGain_Mult_Perc", 20);
        static float Consumables_QGain_Mult = ((float)Consumables_QGain_Mult_Perc) / 100f;

        static int temp_quasival = 100;
        static bool changed = false;

        public static void Prefix(ref ConsumableRecord record, Creature creature, QmorphosController qmorphosController)
        {
            if (record.QmorphosValue > 0 && record.Id != "quasi_medical_kit_1") {

                bool ismedical = false;
                temp_quasival = record.QmorphosValue;
                changed = true;
                foreach (string temp in record.Categories) {
                    if (temp == "Medical") {
                        ismedical = true;
                    }
                }
                if (ismedical) {
                    record.QmorphosValue = (int)(record.QmorphosValue * Medical_QGain_Mult);
                }
                else {
                    record.QmorphosValue = (int)(record.QmorphosValue * Consumables_QGain_Mult);
                }
                    
            }
            
        }

        public static void Postfix(ref ConsumableRecord record, Creature creature, QmorphosController qmorphosController)
        {
            if (changed)
            {
                //Plugin.Logger.Log("item " + record.Id + " quasi now set to " + temp_quasival);
                record.QmorphosValue = temp_quasival;
                changed = false;
            }
        }
    }

}
