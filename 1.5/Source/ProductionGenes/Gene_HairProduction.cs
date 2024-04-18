using UnityEngine;
using Verse;

namespace DDJY
{
    public class Gene_HairProduction : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            CompHairHuman compHairHuman = pawn.TryGetComp<CompHairHuman>();
            if (compHairHuman != null && !Overridden)
            {
                compHairHuman.AddGene(this);
            }
        }
        public override void PostRemove()
        {
            base.PostRemove();
            CompHairHuman compHairHuman = pawn.TryGetComp<CompHairHuman>();
            if (compHairHuman != null && !Overridden)
            {
                compHairHuman.RemoveGene(this);
            }
        }
        public override bool Active
        {
            get
            {
                DefModExtension_Gene ext = def.GetModExtension<DefModExtension_Gene>();
                if (base.Active && (!ext.femaleOnly || pawn.gender == Gender.Female))
                {
                    return true;
                }
                return false;
            }
        }
    }


}
