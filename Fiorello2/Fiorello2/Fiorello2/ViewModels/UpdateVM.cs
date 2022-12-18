using System.ComponentModel.DataAnnotations;

namespace Fiorello2.ViewModels
{
    public class UpdateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
