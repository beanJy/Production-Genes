using DDJY;
using RimWorld;
using System.Text;
using Verse;

public class Comp_ShowMyHediffInfo : ThingComp
{
    public override string CompInspectStringExtra()
    {
        StringBuilder stringBuilder = new StringBuilder();
        if (this.parent is Pawn pawn)
        {

            Hediff lactating = pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.Lactating);
            if (lactating != null)
            {
                var comp = lactating.TryGetComp<HediffComp_MilkableHuman>();
                if (comp != null)
                {
                    stringBuilder.AppendLine(comp.CompInspectStringExtra());
                }
            }

            Hediff hairProduction = pawn.health?.hediffSet?.GetFirstHediffOfDef(DDJY_HediffDefOf.DDJY_HairProductionHediff);
            if (hairProduction != null)
            {
                var comp = hairProduction.TryGetComp<HediffComp_HairHuman>();
                if (comp != null)
                {
                    stringBuilder.AppendLine(comp.CompInspectStringExtra());
                }
            }

        }
        return stringBuilder.ToString().TrimEnd();
    }
}