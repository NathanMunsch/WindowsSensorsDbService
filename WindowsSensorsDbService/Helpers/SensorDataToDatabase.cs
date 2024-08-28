using WindowsSensorsDbService.Data;
using WindowsSensorsDbService.Models;

namespace WindowsSensorsDbService.Helpers
{
    public static class SensorDataToDatabase
    {
        public static void SaveSensorDataToDatabase(DateMeasurementEntity dateMeasurementEntity, string hardwareName, string sensorName, string sensorValue, string sensorUnit, DataContext dataBaseContext)
        {
            // Check if the hardware exists in the database and if not create a new one
            HardwareEntity hardwareEntity = dataBaseContext.HardwareEntities.FirstOrDefault(h => h.Name == hardwareName);
            if (hardwareEntity == null)
            {
                hardwareEntity = new HardwareEntity { Name = hardwareName };
                dataBaseContext.HardwareEntities.Add(hardwareEntity);
                // Save the changes to the database because it is needed for the next iteration,
                // or it will create a new hardware entity of the same type every time
                dataBaseContext.SaveChanges();
            }

            // Add the sensor data to the database
            dataBaseContext.MeasurementEntities.Add(new MeasurementEntity
            {
                SensorName = sensorName,
                MeasuredValue = sensorValue,
                Unit = sensorUnit,
                DateMeasurementEntity = dateMeasurementEntity,
                HardwareEntity = hardwareEntity
            });
        }
    }
}
