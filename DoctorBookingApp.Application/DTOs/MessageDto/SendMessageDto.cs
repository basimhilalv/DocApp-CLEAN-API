using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorBookingApp.Application.DTOs.MessageDto
{
    public class SendMessageDto
    {
        public Guid ReceiverId { get; set; }
        public string? Message { get; set; }
    }
}
