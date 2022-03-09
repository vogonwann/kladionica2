using System;

namespace Kladionica.Models
{
    public class Igrac : Osoba
    {
        public int IznosUplate { get; set; }
        public double OcekivanaIsplata { get; set; }
        public Zemlja OdabranaZemlja { get; set; }

        public Igrac() : base()
        {
        }

        public Igrac(int iznosUplate, Zemlja odabranaZemlja, string ime, string prezime, DateTime datumRodjenja, double ocekivanaIsplata) : base(
            ime, prezime, datumRodjenja)
        {
            IznosUplate = iznosUplate;
            OdabranaZemlja = odabranaZemlja;
            OcekivanaIsplata = ocekivanaIsplata;
        }

        public string DajPodatke()
        {
            return $"{OdabranaZemlja.Naziv} {OdabranaZemlja.Kontinent.ToString()}, kvota: {OdabranaZemlja.Kvota}, uplata: {IznosUplate}, isplata: {OcekivanaIsplata:f2} {base.DajPodatke()}";
        }
    }
}