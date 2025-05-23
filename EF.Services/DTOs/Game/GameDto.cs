using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Services.DTOs.Game
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateOnly PublishDate { get; set; }
        public int DeveloperId { get; set; }
    }
}
