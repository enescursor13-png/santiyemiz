using System;

namespace SantiyeAPI.Models;

public class Santiye
{
    public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Konum { get; set; } = string.Empty;
        public bool AktifMi { get; set; } = true;

        // EF Core Sihri: Bir şantiyenin içinde BİRDEN FAZLA işçi olur.
        // İleride .Include(s => s.Isciler) dediğimizde bu liste dolacak!
        public List<Isci> Isciler { get; set; } = new List<Isci>();
}
