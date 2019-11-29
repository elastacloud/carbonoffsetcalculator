using Elastacloud.CarbonCalculator.Manifest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elastacloud.CarbonCalculator
{
   public class CoreCalculator
   {
      public CarbonResponses Calculate(string json, int month, int year, string application)
      {
         var services = new UnpackManifest(json).Unplug();

         List<AzureBatchCalculator> batches = new List<AzureBatchCalculator>();
         foreach(var batch in services.AzureBatches)
         {
            batches.Add(new AzureBatchCalculator(batch.Minutes, batch.TimePeriod, new PueCalculator(), batch.Measure));
         }
         var batchesCalc = new AzureBatchCollection(new DateTime(year, month, 1)).CoreHoursDCvsCloudSavingsByPue(batches.ToArray());
         var carbonBatches = CalculateCarbonOffset(batchesCalc.Item1, batchesCalc.Item2);
         var carbBatches = new CarbonResponse(services.AzureBatches[0].ToString(), batchesCalc.Item1, batchesCalc.Item2, batchesCalc.Item3,
            carbonBatches.Item1, carbonBatches.Item2, month, year, application);

         List<SqlDwCalculator> sqlDw = new List<SqlDwCalculator>();
         foreach (var dw in services.AzureSqlDws)
         {
            sqlDw.Add(new SqlDwCalculator(dw.Minutes, dw.TimePeriod, new PueCalculator(), dw.Measure));
         }
         var sqlDwCalc = new SqlDwCollection(new DateTime(year, month, 1)).CoreHoursDCvsCloudSavingsByPue(sqlDw.ToArray());
         var carbonDw = CalculateCarbonOffset(sqlDwCalc.Item1, sqlDwCalc.Item2);
         var carbDw = new CarbonResponse(services.AzureSqlDws[0].ToString(), sqlDwCalc.Item1, sqlDwCalc.Item2, sqlDwCalc.Item3,
            carbonDw.Item1, carbonDw.Item2, month, year, application);

         List<AzureDatabricksCalculator> databricks = new List<AzureDatabricksCalculator>();
         foreach (var db in services.AzureDatabricks)
         {
            databricks.Add(new AzureDatabricksCalculator(db.Minutes, db.TimePeriod, new PueCalculator(), db.Measure));
         }
         var dbCalc = new AzureDatabricksCollection(new DateTime(year, month, 1)).CoreHoursDCvsCloudSavingsByPue(databricks.ToArray());
         var carbonDb = CalculateCarbonOffset(dbCalc.Item1, dbCalc.Item2);
         var carbDb = new CarbonResponse(services.AzureDatabricks[0].ToString(), dbCalc.Item1, dbCalc.Item2, dbCalc.Item3,
            carbonDb.Item1, carbonDb.Item2, month, year, application);
         // return all of the doubles in a structure
         return new CarbonResponses(carbBatches, carbDw, carbDb);
      }

      private (double, double) CalculateCarbonOffset(double coreOnPrem, double coreCloud)
      {
         // https://www.confusedaboutenergy.co.uk/index.php/climate-and-the-environment/142-global-warming/751-carbon-emissions-per-kwh-for-various-fuels
         // work from 0.367 from the UK grid kg / co2e/kwh
         double carbonX = coreOnPrem * 0.00085 /* watts */ * 0.367;
         double carbonY = coreCloud * 0.00085 /* watts */ * 0.367;
         return (carbonX, carbonY); 
      }
   }

   public class CarbonResponse
   {
      public CarbonResponse(string serviceName, double onPrem, double inCloud, 
         double ratio, double carbPrem, double carbCloud, int month, int year, string application)
      {
         ServiceName = serviceName;
         CoreHoursOnPrem = Convert.ToInt32(onPrem);
         CoreHoursInCloud = Convert.ToInt32(inCloud);
         PercentageSaving = ratio;
         CarbonOnPrem = carbPrem;
         CarbonInCloud = carbCloud;
         Month = month;
         Year = year;
         Application = application;
         Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
      }

      public string ServiceName { get; private set; }
      public int CoreHoursOnPrem { get; private set; }
      public int CoreHoursInCloud { get; private set; }
      public double PercentageSaving { get; private set; }
      public double CarbonOnPrem { get; private set; }
      public double CarbonInCloud { get; private set; }
      public int Month { get; private set; }
      public int Year { get; private set; }
      public string Application { get; private set; }
      public string Timestamp { get; private set; }
   }
}
