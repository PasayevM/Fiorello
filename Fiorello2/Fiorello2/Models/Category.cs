using System.ComponentModel.DataAnnotations;

namespace Fiorello2.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Doldurulmasi Vacibdir")]
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
    }
}
