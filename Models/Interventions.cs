using System;
using System.Collections.Generic;

namespace Rocket_REST_API
{
    public partial class Interventions
    {
        public long Id { get; set; }
        public int AuthorId { get; set; }
        public int CustomerId { get; set; }
        public int? BuildingId { get; set; }
        public int? BatteryId { get; set; }
        public int? ColumnId { get; set; }
        public int? ElevatorId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? InterventionStartDateTime { get; set; }
        public DateTime? InterventionEndDateTime { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string Report { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class InterventionDTO
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }

    public class CustomerPortalInterventionDTO
    {
        public string CustomerEmail { get; set; }
        public long Id { get; set; }
        public int BuildingId { get; set; }
        public int? BatteryId { get; set; }
        public int? ColumnId { get; set; }
        public int? ElevatorId { get; set; }
        public string Report { get; set; }


    }
}
