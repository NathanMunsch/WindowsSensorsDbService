namespace WindowsSensorsDbService.Models
{
    public class MeasurementEntity
    {
        public int Id { get; set; }
        public int DateMeasurementEntityId { get; set; }
        public int HardwareEntityId { get; set; }
        public string SensorName { get; set; }
        public string MeasuredValue { get; set; }
        public string Unit { get; set; }
        public virtual DateMeasurementEntity DateMeasurementEntity { get; set; }
        public virtual HardwareEntity HardwareEntity { get; set; }
    }
}