using Verse;
using RimWorld;
using System.Collections.Generic;

namespace Thek.HediffArmor{
	public class HediffCompProperties_ArmorBehavior : HediffCompProperties{
		public List<armorSettingsComposite> armorSettings = new List<armorSettingsComposite>();
		public class armorSettingsComposite{
			public List<ThingDef> ArmorSet = new List<ThingDef>();
			public float minSeverityForArmor; // Severity needed for the armor to spawn
		}

	    public HediffCompProperties_ArmorBehavior(){
	    	compClass = typeof(HediffComp_ArmorBehavior);
	    }
    }
}