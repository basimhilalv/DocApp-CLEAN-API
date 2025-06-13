namespace DoctorBookingApp.Application.DTOs.TimeSlotDto
{
    public class SetScheduleDto
    {
        public List<DayOfWeek>? DaysAvailable { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDurationInMinutes { get; set; }
        public int WeeksToGenerate { get; set; }
    }
}
