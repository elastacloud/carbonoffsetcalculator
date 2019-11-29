using Elastacloud.CarbonCalculator.Manifest;

namespace Elastacloud.CarbonCalculator
{
   public interface IServiceCalculator
   {
      int Minutes { get; }
      TimePeriod Period { get; }
      ICalculator Calculator { get; }
   }
}
