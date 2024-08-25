namespace WindowsSensorsDbService.Models
{
    public class DateMeasurementEntity
    {
        public int Id { get; set; }
        public int ComputerEntityId { get; set; }
        public DateTime Date { get; set; }
        public virtual ComputerEntity ComputerEntity { get; set; }
        public virtual ICollection<MeasurementEntity> MeasurementEntities { get; set; }
    }
}