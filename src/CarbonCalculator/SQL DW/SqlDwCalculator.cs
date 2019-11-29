using Elastacloud.CarbonCalculator.Manifest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elastacloud.CarbonCalculator
{
   public class SqlDwCalculator : IServiceCalculator, ICalculator, ICompute
   {
      public static int MaxNodesInCollection { get; private set; }

      public SqlDwCalculator(int minutes, TimePeriod period, ICalculator calculator, int dwu)
      {
         CoreCount = dwu / 100;
         Minutes = minutes;
         Period = period;
         Calculator = calculator;
         MaxNodesInCollection = MaxNodesInCollection < CoreCount ?
            MaxNodesInCollection = CoreCount :
            MaxNodesInCollection;
      }

      public int CoreCount { private set; get; }

      public int Minutes { private set; get; }

      public TimePeriod Period { private set; get; }

      public ICalculator Calculator { private set; get; }

      public double Calculate(double inputValue)
      {
         if (Calculator == null)
         {
            throw new ApplicationException("Calculator not set - did you want to use standard PUE?");
         }

         return Calculator.Calculate(inputValue);
      }
   }
}
