using LibreHardwareMonitor.Hardware;
using Microsoft.AspNetCore.SignalR;
using WindowsSensorsDbService.Helpers;
using WindowsSensorsDbService.SignalR;
using System.Text.Json;

namespace WindowsSensorsDbService;

public class LiveStatsWorker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHubContext<StatHub> _hubContext;
    private Computer computer;

    public LiveStatsWorker(ILogger<Worker> logger, IHubContext<StatHub> hubContext)
    {
        _logger = logger;
        _hubContext = hubContext;

        computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true,
            IsNetworkEnabled = true,
            IsStorageEnabled = true
        };

        computer.Open();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Live stats worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                computer.Accept(new ComputerUpdateVisitor());

                // Collection to hold all sensor data
                var sensorsData = new List<object>();

                // Iterate through all the hardware and sensors and add them to the collection
                foreach (var hardware in computer.Hardware)
                {
                    foreach (var sensor in hardware.Sensors)
                    {
                        string sensorValue = sensor.Value.HasValue ? sensor.Value.Value.ToString() : "N/A";
                        string unit = DetermineSensorUnit.DetermineUnit(sensor);

                        // Create an object with all the sensor data
                        var sensorData = new
                        {
                            HardwareName = hardware.Name,
                            SensorName = sensor.Name,
                            SensorValue = sensorValue,
                            SensorUnit = unit
                        };

                        sensorsData.Add(sensorData);
                    }
                }

                string jsonString = JsonSerializer.Serialize(sensorsData, new JsonSerializerOptions { WriteIndented = true });
                await _hubContext.Clients.All.SendAsync("ReceiveStats", jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording sensor data");
            }

            await Task.Delay(500, stoppingToken);
        }

        computer.Close();
    }
}
