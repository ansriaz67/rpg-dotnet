using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_DOTNET.Models
{
    public class Character
    {
        public int Id { get; set; } 
        public string Name { get; set; } = "Frodo";
        public int HitPoits { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;

    }
}