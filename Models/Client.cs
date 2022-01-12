using System.ComponentModel.DataAnnotations;

namespace MeetService.Models
{
    public class Client
    {
        [Key]
        [Required]
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }
    }
}