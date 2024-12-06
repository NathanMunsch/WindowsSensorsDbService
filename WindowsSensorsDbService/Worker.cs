using LibreHardwareMonitor.Hardware;
using Microsoft.EntityFrameworkCore;
using WindowsSensorsDbService.Data;
using WindowsSensorsDbService.Helpers;
using WindowsSensorsDbService.Models;

namespace WindowsSensorsDbService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DataContext _dbContext;
    private Computer computer;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _dbContext = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(ConnectionString.GetConnectionString()).Options);
        _dbContext.Database.Migrate();

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
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                computer.Accept(new ComputerUpdateVisitor());

                // Get the computer name from the config file find the corresponding computer entity and create a new one if it doesn't exist
                var computerName = UserConfigManager.GetSetting("computer_name");
                var computerEntity = _dbContext.ComputerEntities.FirstOrDefault(c => c.Name == computerName);
                if (computerEntity == null)
                {
                    computerEntity = new ComputerEntity { Name = computerName };
                    _dbContext.ComputerEntities.Add(computerEntity);
                }

                // Create a new DateMeasurementsModel and add it to the database
                var dateMeasurementEntity = new DateMeasurementEntity { Date = DateTime.Now, ComputerEntity = computerEntity };
                _dbContext.DateMeasurementEntities.Add(dateMeasurementEntity);

                // Iterate through all the hardware and sensors and add them to the database
                foreach (var hardware in computer.Hardware)
                {
                    // Iterate through all the sensors and add the measurements to the database
                    foreach (var sensor in hardware.Sensors)
                    {
                        string sensorValue = sensor.Value.HasValue ? sensor.Value.Value.ToString() : "N/A";
                        string unit = DetermineSensorUnit.DetermineUnit(sensor);

                        SensorDataToDatabase.SaveSensorDataToDatabase(dateMeasurementEntity, hardware.Name, sensor.Name, sensorValue, unit, _dbContext);
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error recording sensor data: {ex.Message}");
            }

            int loggingInterval = int.Parse(UserConfigManager.GetSetting("logging_interval"));
            await Task.Delay(loggingInterval, stoppingToken);
        }

        computer.Close();
        _dbContext.Dispose();
    }
}
