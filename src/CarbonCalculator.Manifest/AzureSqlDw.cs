namespace Elastacloud.CarbonCalculator.Manifest
{
   public class AzureSqlDw : ServiceType
   {
      public override ServiceList Service => ServiceList.AzureSqlDw;

      public override string ToString()
      {
         return "Azure SQL DW";
      }
   }
}
