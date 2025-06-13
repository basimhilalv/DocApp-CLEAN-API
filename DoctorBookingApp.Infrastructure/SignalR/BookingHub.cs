using Microsoft.AspNetCore.SignalR;

namespace DoctorBookingApp.Infrastructure.SignalR
{
    public class BookingHub : Hub
    {
        public async Task NotifySlotBooked(Guid slotId, Guid doctorId)
        {
            await Clients.Group(doctorId.ToString()).SendAsync("SlotBooked", slotId);
        }
        public async Task JoinDoctorGroup(Guid doctorId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, doctorId.ToString());
        }
    }
}
