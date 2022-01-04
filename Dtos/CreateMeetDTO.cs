using System;
using System.ComponentModel.DataAnnotations;

namespace MeetService.Dtos
{
    public class CreateMeetDTO
    {   
        [Required]
        public DateTime Date { get; set; }
    }
}