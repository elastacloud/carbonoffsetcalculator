namespace Elastacloud.CarbonCalculator.Manifest
{
   public class AzureServices
   {
      public AzureBatch[] AzureBatches { get; set; }
      public AzureDatabrick[] AzureDatabricks { get; set; }
      public AzureSqlDw[] AzureSqlDws { get; set; }
   }

   public abstract class ServiceType
   {
      public int Minutes { get; set; }
      public TimePeriod TimePeriod { get; set; }
      public int Measure { get; set; } /* CoreCount or Dwu */
      public abstract ServiceList Service { get; }
   }
}
