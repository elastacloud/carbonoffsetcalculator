namespace CarbonCaptureFunctions
{
    public class CoreHoursMetrics
    {
        public double DataCenter { get; set; }
        public double Azure { get; set; }
        public double Ratio { get; set; }
    }
    public class CalculatedSavings
    {
        public CalculatedSavings()
        {
        }

        public CoreHoursMetrics AzureBatches { get; set; }
        public CoreHoursMetrics AzureDatabricks { get; set; }
        public CoreHoursMetrics SqlDw { get; set; }
    }
}