using System;

namespace Elastacloud.CarbonCalculator
{
   public class PueCalculator : ICalculator
   {
      public const double PUE_MODIFIER = 1.2D;
      public const double STANDARD_DATACENTRE_MODIFIER = 1.8D;
      /// <summary>
      /// This returns the number of core hours adjusted by the ratio of pue 
      /// this is a simple linear power conversion function
      /// </summary>
      /// <param name="inputValue">Input value of core hours</param>
      /// <returns>Adjusted core hours</returns>
      public double Calculate(double inputValue)
      {
         return (PUE_MODIFIER / STANDARD_DATACENTRE_MODIFIER) * inputValue;
      }
   }
}
