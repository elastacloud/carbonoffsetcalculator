using Elastacloud.CarbonCalculator.Manifest;
using System;

namespace Elastacloud.CarbonCalculator
{
   public class AzureDatabricksCollection
   {
        private DateTime TimePoint;

        public AzureDatabricksCollection(DateTime timePoint)
        {
            this.TimePoint = timePoint;
        }

        public (double, double, double) CoreHoursDCvsCloudSavingsByPue(AzureDatabricksCalculator[] calculators)
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
         return (AzureDatabricksCalculator.MaxNodesInCollection * HoursInMonth, cloudValue, Math.Round((cloudValue / (AzureDatabricksCalculator.MaxNodesInCollection * HoursInMonth)) * 100, 2));
      }

      private int HoursInMonth => DateTime.DaysInMonth(TimePoint.Year, TimePoint.Month) * 24;
   }
}
