using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace Kladionica.Models
{
    public class UplatnoMesto
    {
        public string Adresa { get; set; }
        public string Grad { get; set; }
        public List<Igrac> SpisakIgraca { get; set; }

        public UplatnoMesto() // konstruktor koji inicijalizuje listu igraca
        {
            SpisakIgraca = new List<Igrac>();
        } 

        public UplatnoMesto(string adresa, string grad) : this() // pozivanje konstruktora bez parametara koji inicijalizuje listu igraca
        {
            Adresa = adresa;
            Grad = grad;
        }

        public string DajPodatke()
        {
            var adresaSplit = Adresa.Split(" ");
            var adresaBezBroja = string.Join(" ", adresaSplit.Take(adresaSplit.Length - 1)); // uzmi sve sem posledenjeg elementa niza (broj) i Join sa space
            var samoglasnici = "[aeiou ]"; // pattern za regex, koji sklanja samoglasnike i space (Novi Sad = NVSD)
            var gradBezSamoglasnika = Regex.Replace(Grad, samoglasnici, "", RegexOptions.IgnoreCase).ToUpperInvariant(); // regexom zameni sve karaktere iz patterna za Grad praznim karakterom ("") i zanemari case
            var ukupnaUplata = SpisakIgraca.Sum(t => t.IznosUplate); // prodji kroz spisak igraca i Sum njihov IznosUplate
            return $"{adresaBezBroja} {gradBezSamoglasnika}, Ukupna uplata: {ukupnaUplata}";
        }

        public void DodajIgraca(Igrac noviIgrac)
        {
            if (!(DateTime.Now.Year - noviIgrac.DatumRodjenja.Year >= 18))
            {
                Console.WriteLine("Igrac mora biti punoletan!");
                return; // ako nije punoletan nemoj dodavati igraca
            }

            if (!(noviIgrac.IznosUplate >= 50 && noviIgrac.IznosUplate <= 100000))
            {
                Console.WriteLine("Uplata od {0} dinara nije korektna!", noviIgrac.IznosUplate);
                return; // ako nije korektna uplata nemoj dodavati igraca
            }

            int indexIgracaSaIstomZemljom = 0;
            for (var i = 0; i < SpisakIgraca.Count; i++)
            {
                var igrac = SpisakIgraca[i];
                if (noviIgrac.Ime == igrac.Ime &&
                    noviIgrac.Prezime == igrac.Prezime &&
                    noviIgrac.DatumRodjenja == igrac.DatumRodjenja &&
                    noviIgrac.OdabranaZemlja.Naziv == igrac.OdabranaZemlja.Naziv)
                {
                    indexIgracaSaIstomZemljom = i;
                    break;
                }
            }

            if (indexIgracaSaIstomZemljom != -1)
            {
                SpisakIgraca[indexIgracaSaIstomZemljom] = noviIgrac;
            }
            else
            {
                SpisakIgraca.Add(noviIgrac);
            }
        }

        public Igrac UcitajIgraca()
        {
            Console.WriteLine("Molimo unesite ime igraca:");
            var ime = Console.ReadLine();

            Console.WriteLine("Molimo unesite prezime igraca:");
            var prezime = Console.ReadLine();

            Console.WriteLine("Molimo unesite datum rodjenja igraca:");
            var datumString = Console.ReadLine();
            var datum = DateTime.ParseExact(datumString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            bool pogresanUnosUplate = true; // pretpostavljamo da je unos pogresan
            int uplata = 0; // out parametar TryParse metode
            while (pogresanUnosUplate) // vrti se while dokle pogresanUnosUplate nije false
            {
                Console.WriteLine("Molimo unesite uplatu igraca:");
                var uplataString = Console.ReadLine();
                if (int.TryParse(uplataString, out uplata)) // ako je unos moguce parsirati u string, upisi u "uplata" i izadji iz petlje
                {
                    pogresanUnosUplate = false;
                }
                else
                {
                    Console.WriteLine("Pogresan unos! Molimo pokusajte ponovo:"); // ako je unos pogresan, zatraziti ponovni unos
                }
            }

            Console.WriteLine("Molimo unesite zemlju:");
            string zemljaNaziv = Console.ReadLine();
            Console.WriteLine("Molimo unesite kontinent:");
            string kontinetNaziv = Console.ReadLine();
            Kontinent kontinent; // out parametar TryParse metode
            Enum.TryParse(kontinetNaziv, out kontinent); // pretvori string u Kontinetnt enum; u slucaju pogresnog unosa vratice prvi element enumeracije
            bool pogresanUnosKvote = true;
            double kvota = 0; // out parametar TryParse metode
            while (pogresanUnosKvote)
            {
                Console.WriteLine("Molimo unesite kvotu za zemlju:");
                var kvotaString = Console.ReadLine();
                if (double.TryParse(kvotaString, out kvota)) // ako je unos moguce parsirati u double, upisi u "kvota" i izadji iz petlje
                {
                    pogresanUnosKvote = false;
                }
                else
                {
                    Console.WriteLine("Pogresan unos! Molimo pokusajte ponovo:"); // ako je unos pogresan, zatraziti ponovni unos
                }
            }

            var zemlja = new Zemlja(zemljaNaziv, kvota, kontinent); // zemlja na koju se igrac kladi, parametar ctra klase Igrac
            
            return new Igrac(uplata, zemlja, ime, prezime, datum); // vraca novokreiranog igraca
        }
    }
}
