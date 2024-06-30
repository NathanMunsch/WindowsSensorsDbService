using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsSensorsDbService.Models
{
    public class MeasurementEntity
    {
        public int Id { get; set; }
        public int DateMeasurementEntityId { get; set; }
        public string SensorName { get; set; }
        public string MeasuredValue { get; set; }
        public string Unit { get; set; }
        public virtual DateMeasurementEntity DateMeasurementEntity { get; set; }
    }
}
