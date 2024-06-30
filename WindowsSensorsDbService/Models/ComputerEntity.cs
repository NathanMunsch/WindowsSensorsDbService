using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsSensorsDbService.Models
{
    public class ComputerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DateMeasurementEntity> DateMeasurementEntities { get; set; }
    }
}
