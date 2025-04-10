using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateOnly PublishDate { get; set; }
        public decimal Price { get; set; }
        public int DeveloperId { get; set; }
        public Developer? Developer { get; set; } //navigation property

        public override string ToString()
        {
            return $"{Title.ToUpper()} - ${Price}";
        }


    }
}
