namespace Appointment_Scheduler.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScheduledDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Cust_Id { get; set; } //Optional
        public DateTime Created { get; set; }

        public Appointment()
        {

        }

    }
  
}
