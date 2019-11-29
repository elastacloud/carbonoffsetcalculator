using Elastacloud.CarbonCalculator.Manifest;
using System;

namespace Elastacloud.CarbonCalculator
{
   public class AzureBatchCollection
   {
        public AzureBatchCollection(DateTime timepoint)
        {
            Timepoint = timepoint;
        }

      public (double, double, double) CoreHoursDCvsCloudSavingsByPue(AzureBatchCalculator[] calculators)
      {
         double cloudValue = 0D;
         foreach (var calculator in calculators)
         {
            double inputValue = 0D;
            switch (calculator.Period)
            {
               case TimePeriod.Hour:
                  inputValue = ((double)calculator.Minutes / 60D) * (double)HoursInMonth;
                  break;
               case TimePeriod.Month:
                  inputValue = (double)calculator.Minutes / 60D;
                  break;
               default:
                  inputValue = 0;
                  break;
            }
            cloudValue += calculator.Calculate(calculator.CoreCount * inputValue);
         }
         return (AzureBatchCalculator.MaxNodesInCollection * HoursInMonth, cloudValue, Math.Round((cloudValue / (AzureBatchCalculator.MaxNodesInCollection * HoursInMonth)) * 100, 2));
      }

      private int HoursInMonth => DateTime.DaysInMonth(Timepoint.Year, Timepoint.Month) * 24;

        public DateTime Timepoint { get; }
    }
}
