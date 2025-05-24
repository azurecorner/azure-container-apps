namespace WeatherForecast.Observability
{
    using System.Diagnostics.Metrics;

    public class WeatherMetrics
    {
        private readonly Counter<int> _serviceCalls;
        private readonly Counter<int> _temperatureChange;

        private readonly Counter<int> _businessMetriChange;
        public WeatherMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("OtelReferenceApp.WeatherForecast");
            _temperatureChange = meter.CreateCounter<int>("OtelReferenceApp.WeatherForecast.temperature_change", unit: "{celcuis}", description: "Value of temperature being changed through the WeatherForecast service.");

            _businessMetriChange = meter.CreateCounter<int>("OtelReferenceApp.WeatherForecast.onecloud_business_metric", unit: "{unit}", description: "Value of temperature being changed through the WeatherForecast service.");
        }

        public void TemperatureChange(int value)
        {
            _temperatureChange.Add(value);
        }
        public void BusinessMetriChange(int value)
        {
            _businessMetriChange.Add(value);
        }
    }
}