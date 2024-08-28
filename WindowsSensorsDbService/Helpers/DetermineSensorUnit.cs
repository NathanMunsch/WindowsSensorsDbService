using LibreHardwareMonitor.Hardware;

namespace WindowsSensorsDbService.Helpers
{
    public static class DetermineSensorUnit
    {
        public static string DetermineUnit(ISensor sensor)
        {
            switch (sensor.SensorType)
            {
                case SensorType.Temperature:
                    return "°C";
                case SensorType.Load:
                    return "%";
                default:
                    return string.Empty;
            }
        }
    }
}
