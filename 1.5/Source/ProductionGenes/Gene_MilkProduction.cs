using Verse;

namespace DDJY
{
    public class Gene_MilkProduction : Gene
    {
        public override void PostAdd() 
        {
            base.PostAdd();
            CompMilkableHuman compMilkableHuman = pawn.TryGetComp<CompMilkableHuman>();
            if (compMilkableHuman != null)
            {
                compMilkableHuman.AddGene(this);
            }
        }
        public override void PostRemove()
        {
            base.PostRemove();
            CompMilkableHuman compMilkableHuman = pawn.TryGetComp<CompMilkableHuman>();
            if (compMilkableHuman != null)
            {
                compMilkableHuman.RemoveGene(this);
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
