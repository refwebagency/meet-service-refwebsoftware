using System;
using System.ComponentModel.DataAnnotations;

namespace MeetService.Dtos
{
    public class ReadMeetDTO
    {   
        public int Id { get; set; }

        public DateTime Date { get; set; }
    }
}