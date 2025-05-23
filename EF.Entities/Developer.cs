using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.Entities
{
    //[Table("Developers")]
    //[Index(nameof(Developer.Name), Name =("IX_Developers_Name"), IsUnique = true)] //Indice unico
    public class Developer
    {
        public int Id { get; set; }

        //[Required(ErrorMessage ="The field {0} is required")]
        //[StringLength(50,ErrorMessage="The field {0} must be between {2} and {1}",MinimumLength = 3)]
        public string Name { get; set; } = null!;
        public DateOnly FoundationDate { get; set; }

        //[Required(ErrorMessage = "The field {0} is required")]
        //[StringLength(50, ErrorMessage = "The field {0} must be between {2} and {1}", MinimumLength = 3)]
        public string Country { get; set; } = null!;

        public ICollection<Game>? Games { get; set; }

        public override string ToString()
        {
            return $"{Name.ToUpper()} - {Country.ToUpper()}";
        }
    }
}
