using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Appointment_Scheduler.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [DisplayName("Full Name")]
        public string Name { get; set; }
        [DisplayName("Scheduled Date")]
        public string ScheduledDate { get; set; }
        [DisplayName("Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [DisplayName("End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public int Cust_Id { get; set; } //Optional
        public DateTime Created { get; set; }
        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        public Appointment()
        {

        }

    }
  
}
