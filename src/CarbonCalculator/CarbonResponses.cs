namespace Elastacloud.CarbonCalculator
{
    public class CarbonResponses
    {
      public CarbonResponses(params CarbonResponse[] carbBatches)
        {
            this.Carbon = carbBatches;
        }
      public CarbonResponse[] Carbon { get; private set; }
   }
}