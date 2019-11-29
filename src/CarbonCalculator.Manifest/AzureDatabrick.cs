namespace Elastacloud.CarbonCalculator.Manifest
{
   public class AzureDatabrick : ServiceType
   {
      public override ServiceList Service => ServiceList.AzureDatabricks;

      public override string ToString()
      {
         return "Azure Databricks";
      }
   }
}
