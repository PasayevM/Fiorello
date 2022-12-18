using System.ComponentModel.DataAnnotations;

namespace Fiorello2.ViewModels
{
    public class LoginVM
    {
       
        [Required]
        public string UserName { get; set; }

       
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       
    }
}
