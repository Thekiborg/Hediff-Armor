using Verse;
using RimWorld;
using UnityEngine;

namespace Thek.HediffArmor{
    ///<summary>
    /// Makes an apparel force any apparel and weapon defined in the XML
    ///</summary>
    public class HediffComp_ArmorBehavior : HediffComp{
        ///<summary>
        /// Method to force the apparel on the pawn, replace the existing apparel and prevent it from dropping
        ///</summary>
        void forceWearApparel(Apparel apparel, bool dropReplacedApparel, bool locked){//Crea el metodo forceWearApparel con los argumentos del método Wear, decompilado de rimworld 
            Pawn.apparel.Wear(apparel, true, true); //Llama el método Wear que existe ya en rimworld
        }
        ///<summary>
        /// Method to force the pawn to drop their currently equipped weapon, and then equips the desired one.
        ///</summary>
        void forceWearWeapon(ThingWithComps weapon){ //Crea el metodo forceWearWeapon, con el argumento que usan los métodos MakeRoomFor y AddEquipment, decompilado de rimworld
            Pawn.equipment.MakeRoomFor(weapon); //Llama el método MakeRoomFor que existe ya en rimworld
            Pawn.equipment.AddEquipment(weapon); //Llama el método AddEquipment que existe ya en rimworld
        }
        public override void CompPostTick(ref float SeverityAdjustment){
            if (base.Pawn.IsHashIntervalTick(60)){
                float currentSeverity = parent.Severity; //Crea la variable currentSeverity y se la asigna 
                foreach (HediffCompProperties_ArmorBehavior.armorSettingsComposite thing in Props.armorSettings){ //Comprueba cada cosa en la clase armorSettings, donde están las opciones del XML
                    if (thing.SeverityToForce >= lastSeverity && thing.SeverityToForce <= currentSeverity){//Comprueba si la opción SeverityToForce es mayor o igual que la última registrada y menor o igual que la actual, para saber si ha llegado a la severidad necesaria para aplicarlo
                        foreach (ThingDef apparel in thing.ApparelForced){ //Va por cada entrada de la lista ApparelForced, definida en XML
                            Thing spawnedApparel = ThingMaker.MakeThing(apparel); //Instances apparel form the def
                            if (spawnedApparel is Apparel a){ //Comprueba que spawnedApparel es un Apparel y le asigna la variable a
                                forceWearApparel(a, true, true); //Llama al method forceWearApparel con a como el apparel a forzar
                            }
                        }
                    }
                }
                foreach (HediffCompProperties_ArmorBehavior.armorSettingsComposite thing in Props.armorSettings){ //Comprueba cada cosa en la clase armorSettings, donde están las opciones del XML
                    if (thing.SeverityToForce >= lastSeverity && thing.SeverityToForce <= currentSeverity){ //Comprueba si la opción SeverityToForce es mayor o igual que la última registrada y menor o igual que la actual, para saber si ha llegado a la severidad necesaria para aplicarlo
                        foreach (ThingDef weapon in thing.WeaponForced){ //Va por cada entrada de la lista WeaponForced, definida en XML
                            if (Pawn.equipment.Primary == null || Pawn.equipment.Primary.def != weapon){ //Comprueba que el def del arma equipada es la misma que el def del arma definida en WeaponForced en el XML
                                Thing spawnedWeapon = ThingMaker.MakeThing(weapon); //Instances weapon from the def
                                if (spawnedWeapon is ThingWithComps w){ //Comprueba que spawnedWeapon es un ThingWithComps y le asigna la variable w
                                    forceWearWeapon(w); //Llama al method forceWearWeapon con w como el arma a asignar
                                }
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