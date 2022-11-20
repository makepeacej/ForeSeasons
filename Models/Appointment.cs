namespace Appointment_Scheduler.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int Cust_Id { get; set; } //Optional
        public DateTime Created { get; set; }

        public Appointment()
        {

        }
    }
}
