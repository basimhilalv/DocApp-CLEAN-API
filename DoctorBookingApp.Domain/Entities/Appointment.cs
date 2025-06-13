namespace DoctorBookingApp.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid TimeSlotId { get; set; }
        public TimeSlot? TimeSlot { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //payment details
        public string? PaymentStatus { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public string? StripeTransactionId { get; set; }

        //fee details
        public decimal FeeAmount { get; set; } = 100;
    

    }
}
