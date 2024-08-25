namespace WindowsSensorsDbService.Models
{
    public class HardwareEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MeasurementEntity> MeasurementEntities { get; set; }
    }
}