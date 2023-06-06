using Verse;
using RimWorld;
using UnityEngine;

namespace Thek.HediffArmor{
    ///<summary>
    /// Makes an apparel force any apparel and weapon defined in the XML
    ///</summary>
    public class HediffComp_ArmorBehavior : HediffComp{
        ///<summary>
        /// Makes the pawn wear the desired apparel
        ///</summary>
        void forceWearApparel(Apparel apparel, bool dropReplacedApparel, bool locked){
            Pawn.apparel.Wear(apparel, true, true);
        }
        ///<summary>
        /// Makes the pawn drop the current weapon, if any, and equip the desired one
        ///</summary>
        void forceWearWeapon(ThingWithComps weapon){
            Pawn.equipment.MakeRoomFor(weapon);
            Pawn.equipment.AddEquipment(weapon);
        }
        public override void CompPostTick(ref float SeverityAdjustment){ // Comprueba la severidad cada segundo
            if (base.Pawn.IsHashIntervalTick(60)){
                float currentSeverity = parent.Severity;
                foreach (HediffCompProperties_ArmorBehavior.armorSettingsComposite thing in Props.armorSettings){
                    if (thing.minSeverityForArmor >= lastSeverity && thing.minSeverityForArmor <= currentSeverity){
                        foreach (ThingDef apparel in thing.ArmorSet){
                            Thing spawnedThing = ThingMaker.MakeThing(apparel);
                            if (spawnedThing is Apparel realApparel){
                                forceWearApparel(realApparel, true, true);
                            }
                            else if (spawnedThing is ThingWithComps realWeapon){
                                forceWearWeapon(realWeapon);
                            }
                             
                        }
                    }
                }
                lastSeverity = currentSeverity;
            }
        }
        public float lastSeverity = 0f;
        public override void CompExposeData() // Guarda el ultimo valor de severidad en cache
        {
            base.CompExposeData();
            Scribe_Values.Look(ref lastSeverity, "lastCheckSeverity", 0);
        }

        public HediffCompProperties_ArmorBehavior Props => (HediffCompProperties_ArmorBehavior)props;
    }
}