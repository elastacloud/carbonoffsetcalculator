using Elastacloud.CarbonCalculator.Manifest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elastacloud.CarbonCalculator
{
   public class AzureBatchCalculator : IServiceCalculator, ICalculator, ICompute
   {
      public static int MaxNodesInCollection { get; private set; }

      public AzureBatchCalculator(int minutes, TimePeriod period, ICalculator calculator, int nodes)
      {
         Minutes = minutes;
         Period = period;
         Calculator = calculator;
         CoreCount = nodes;
         MaxNodesInCollection = MaxNodesInCollection < nodes ?
            MaxNodesInCollection = nodes :
            MaxNodesInCollection;
      }

      public int Minutes { get; private set; }

      public TimePeriod Period { get; private set; }

      public ICalculator Calculator { get; private set; }

      public int CoreCount { get; private set; }
      
      /// <summary>
      /// The calculate method here takes the time taken so that the number of core hours can be determined
      /// </summary>
      /// <param name="inputValue">Time taken in minutes</param>
      /// <returns>Modifier time saved in core hours</returns>
      public double Calculate(double inputValue)
      {
         if(Calculator == null)
         {
            throw new ApplicationException("Calculator not set - did you want to use standard PUE?");
         }

         return Calculator.Calculate(inputValue);
      }
   }
}
