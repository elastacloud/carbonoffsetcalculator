namespace Elastacloud.CarbonCalculator.Manifest
{
   public class AzureBatch : ServiceType
   {
      public override ServiceList Service { get => ServiceList.AzureBatch; }

      public override string ToString()
      {
         return "Azure Batch";
      }
   }
}
