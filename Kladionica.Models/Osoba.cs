using System;

namespace Kladionica.Models
{
    public class Osoba
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }

        public Osoba()
        {
        }

        public Osoba(string ime, string prezime, DateTime datumRodjenja)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
        }

        public string DajPodatke()
        {
            return $"{Ime} {Prezime} {DatumRodjenja:dd.MM.yyyy.}";
        }
    }
}