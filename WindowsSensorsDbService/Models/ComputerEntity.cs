namespace WindowsSensorsDbService.Models
{
    public class ComputerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DateMeasurementEntity> DateMeasurementEntities { get; set; }
    }
}