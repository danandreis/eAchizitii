using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace eAchizitii.Data.Enum
{
    public enum RolAngajatComanda
    {

        Emitent = 1,

        [Display(Name="Persoana contact")]
        Persoana_contact,

        [Display(Name ="Responsabil achizitii")]
        Responsabil_achizitii
    }
}
