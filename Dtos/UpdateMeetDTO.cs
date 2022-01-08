using System;
using System.ComponentModel.DataAnnotations;

namespace MeetService.Dtos
{
    public class UpdateMeetDTO
    {   
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int UserId   { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}