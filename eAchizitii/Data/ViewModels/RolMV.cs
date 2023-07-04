using System.ComponentModel.DataAnnotations;

namespace eAchizitii.Data.ViewModels
{
    public class RolMV
    {
        [Required(ErrorMessage ="Denumirea este obligatorie")]
        public string Nume { get; set; }
    
    }
}
