using System.ComponentModel.DataAnnotations;

namespace MeetService.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}