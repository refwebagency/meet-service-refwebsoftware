using System;
using System.ComponentModel.DataAnnotations;
using MeetService.Models;

namespace MeetService.Dtos
{
    public class CreateMeetDTO
    {   
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int UserId   { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}