using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsSensorsDbService.Data;
using WindowsSensorsDbService.Helpers;
using WindowsSensorsDbService.Models;

namespace WindowsSensorsDbService
{
    public partial class Service : ServiceBase
    {
        private readonly DataContext _context;
        private Computer computer;
        private Timer timer;

        public Service()
        {
            InitializeComponent();
            _context = new DataContext();
        }

        protected override void OnStart(string[] args)
        {
            _context.Database.Initialize(force: true);

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

            int loggingIntervalSeconds = int.Parse(UserConfigManager.GetSetting("logging_interval_seconds"));
            timer = new Timer(RecordSensorDataToDatabase, null, TimeSpan.Zero, TimeSpan.FromSeconds(loggingIntervalSeconds));
        }

        protected override void OnStop()
        {
            timer.Dispose();
            computer.Close();
            _context.Dispose();
        }

        private void RecordSensorDataToDatabase(object state)
        {
            try
            {
                computer.Accept(new ComputerUpdateVisitor());

                // Get the computer name from the config file and check if it exists in the database or create a new one
                var computerName = UserConfigManager.GetSetting("computer_name");
                var computerEntity = _context.ComputerEntities.FirstOrDefault(c => c.Name == computerName);
                if (computerEntity == null)
                {
                    computerEntity = new ComputerEntity { Name = computerName };
                    _context.ComputerEntities.Add(computerEntity);
                }

                // Create a new DateMeasurementsModel and add it to the database
                var dateMeasurementEntity = new DateMeasurementEntity { Date = DateTime.Now, ComputerEntity = computerEntity };
                _context.DateMeasurementEntities.Add(dateMeasurementEntity);

                // Iterate through all the hardware and sensors and add them to the database
                foreach (var hardware in computer.Hardware)
                {
                    // Check if the hardware exists in the database or create a new one
                    HardwareEntity hardwareEntity = _context.HardwareEntities.FirstOrDefault(h => h.Name == hardware.Name);
                    if (hardwareEntity == null)
                    {
                        hardwareEntity = new HardwareEntity { Name = hardware.Name };
                        _context.HardwareEntities.Add(hardwareEntity);
                    }

                    // Iterate through all the sensors and add the measurements to the database
                    foreach (var sensor in hardware.Sensors)
                    {
                        string sensorValue = sensor.Value.HasValue ? sensor.Value.Value.ToString() : "N/A";
                        string unit = DetermineUnit(sensor);

                        _context.MeasurementEntities.Add(new MeasurementEntity
                        {
                            SensorName = sensor.Name,
                            MeasuredValue = sensorValue,
                            Unit = unit,
                            DateMeasurementEntity = dateMeasurementEntity,
                            HardwareEntity = hardwareEntity
                        });
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording sensor data: {ex.Message}");
            }
        }

        private string DetermineUnit(ISensor sensor)
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
