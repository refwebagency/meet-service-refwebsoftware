using System;
using System.ComponentModel.DataAnnotations;

namespace MeetService.Dtos
{
    public class ReadMeetDTO
    {   
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int UserId   { get; set; }

        public int ClientId { get; set; }
    }
}