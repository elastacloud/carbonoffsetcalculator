using Elastacloud.CarbonCalculator;
using Elastacloud.CarbonCalculator.Manifest;
using System;
using Xunit;

namespace CarbonCalculatorTests
{
   public class PueCalculatorTest
   {
      string json = @"{
                          'AzureBatches': [
                           {
                              'Minutes': 30,
                              'TimePeriod': 'Hour',
                              'Measure': 3,
                              'Service' : 'AzureBatch'
                           },
                           {
                              'Minutes': 150,
                              'TimePeriod': 'Month',
                              'Measure': 3,
                              'Service' : 'AzureBatch'
                           }
                          ],
                          'AzureDatabricks': [
                           {
                              'Minutes': 30,
                              'TimePeriod': 'Hour',
                              'Measure': 3
                           },
                           {
                              'Minutes': 150,
                              'TimePeriod': 'Month',
                              'Measure': 3
                           }
                          ],
                          'AzureSqlDws': [
                           {
                              'Minutes': 30,
                              'TimePeriod': 'Hour',
                              'Dwu': 100
                           },
                           {
                              'Minutes': 150,
                              'TimePeriod': 'Month',
                              'Dwu': 200
                           }
                          ]
                        }";

      [Fact]
      public void Calculate_SingleNumber_ReturnsLessThanOriginal()
      {
         ICalculator calculator = new PueCalculator();
         double output = calculator.Calculate(1D);
         Assert.True(output < 1D);
      }

      [Fact]
      public void Calculate_AzureBatchCalculator_MaxNodes()
      {
         var calculator1 = new AzureBatchCalculator(32, TimePeriod.Hour, new PueCalculator(), 29);
         var calculator2 = new AzureBatchCalculator(60 * 9, TimePeriod.Month, new PueCalculator(), 106);

         Assert.Equal(106, AzureBatchCalculator.MaxNodesInCollection);
      }

      [Fact]
      public void Calculate_AzureBatchCalculator_CoreHoursSaving()
      {
         var calculator1 = new AzureBatchCalculator(32, TimePeriod.Hour, new PueCalculator(), 29);
         var calculator2 = new AzureBatchCalculator(540, TimePeriod.Month, new PueCalculator(), 106);

         var calcs = new AzureBatchCollection(new DateTime(2019, 04, 01)).CoreHoursDCvsCloudSavingsByPue(new[] { calculator1, calculator2 });

         Assert.Equal(76320, calcs.Item1);
         Assert.Equal(8060, Math.Round(calcs.Item2, 0));
         Assert.Equal(10.56, calcs.Item3);
      }

      [Fact]
      public void Calculate_SqlDwCalculator_MaxNodes()
      {
         var calculator1 = new SqlDwCalculator(60, TimePeriod.Hour, new PueCalculator(), 600);
         var calculator2 = new SqlDwCalculator(30, TimePeriod.Month, new PueCalculator(), 1200);

         Assert.Equal(12, SqlDwCalculator.MaxNodesInCollection);
      }

      [Fact]
      public void Calculate_SqlDwCalculator_CoreHoursSaving()
      {
         var calculator1 = new SqlDwCalculator(60, TimePeriod.Hour, new PueCalculator(), 200);
         var calculator2 = new SqlDwCalculator(30, TimePeriod.Month, new PueCalculator(), 1000);

         var calcs = new SqlDwCollection(new DateTime(2019, 06, 01)).CoreHoursDCvsCloudSavingsByPue(new[] { calculator1, calculator2 });

         Assert.Equal(8640, calcs.Item1);
         Assert.Equal(2884, Math.Round(calcs.Item2, 0));
         Assert.Equal(33.38, calcs.Item3);
      }

      [Fact]
      public void Calculate_AzureDatabricksCalculator_MaxNodes()
      {
         var calculator1 = new AzureDatabricksCalculator(102, TimePeriod.Month, new PueCalculator(), 352);

         Assert.Equal(352, AzureDatabricksCalculator.MaxNodesInCollection);
      }

      [Fact]
      public void Calculate_AzureDatabricksCalculator_CoreHoursSaving()
      {
         var calculator1 = new AzureDatabricksCalculator(102, TimePeriod.Month, new PueCalculator(), 352);

         var calcs = new AzureDatabricksCollection(new DateTime(2019, 04, 01)).CoreHoursDCvsCloudSavingsByPue(new[] { calculator1 });

         Assert.Equal(253440, calcs.Item1);
         Assert.Equal(399, Math.Round(calcs.Item2, 0));
         Assert.Equal(0.16, calcs.Item3);
      }

      [Fact]
      public void Manifest_UnpackAzureBatches_TwoInCollection()
      {   
         var manifest = new UnpackManifest(json);
         var services = manifest.Unplug();
         Assert.Equal(2, services.AzureDatabricks.Length);
      }

      [Fact]
      public void Manifest_UnpackAzureDatabricks_TwoInCollection()
      {
         var manifest = new UnpackManifest(json);
         var services = manifest.Unplug();
         Assert.Equal(2, services.AzureDatabricks.Length);
      }

      [Fact]
      public void Manifest_UnpackAzureSqlDw_TwoInCollection()
      {
         var manifest = new UnpackManifest(json);
         var services = manifest.Unplug();
         Assert.Equal(2, services.AzureSqlDws.Length);
      }

   }
}
