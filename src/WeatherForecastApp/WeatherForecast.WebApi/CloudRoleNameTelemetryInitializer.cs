﻿namespace WeatherForecast.WebApi
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    public class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string _roleName;

        public CloudRoleNameTelemetryInitializer(string roleName)
        {
            _roleName = roleName;
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = _roleName;
        }
    }
}