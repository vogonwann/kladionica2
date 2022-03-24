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

        public UplatnoMesto()
        {
            SpisakIgraca = new List<Igrac>();
        }

        public UplatnoMesto(string adresa, string grad) : this()
        {
            Adresa = adresa;
            Grad = grad;
        }

        public string DajPodatke()
        {
            var adresaSplit = Adresa.Split(" ");
            var adresaBezBroja = string.Join(" ", adresaSplit.Take(adresaSplit.Length - 1));
            var samoglasnici = "[aeiou]";
            var gradBezSamoglasnika = Regex.Replace(Grad, samoglasnici, "", RegexOptions.IgnoreCase).ToUpperInvariant();
            var ukupnaUplata = SpisakIgraca.Sum(t => t.IznosUplate);
            return $"{adresaBezBroja} {gradBezSamoglasnika}, Ukupna uplata: {ukupnaUplata}";
        }

        public void DodajIgraca(Igrac noviIgrac)
        {
            if (!(DateTime.Now.Year - noviIgrac.DatumRodjenja.Year >= 18))
            {
                Console.WriteLine("Igrac mora biti punoletan!");
                return;
            }

            if (!(noviIgrac.IznosUplate >= 50 && noviIgrac.IznosUplate <= 100000))
            {
                Console.WriteLine("Uplata od {0} dinara nije korektna!", noviIgrac.IznosUplate);
                return;
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

            bool pogresanUnosUplate = true;
            int uplata = 0;
            while (pogresanUnosUplate)
            {
                Console.WriteLine("Molimo unesite uplatu igraca:");
                var uplataString = Console.ReadLine();
                if (int.TryParse(uplataString, out uplata))
                {
                    pogresanUnosUplate = false;
                }
                else
                {
                    Console.WriteLine("Pogresan unos! Molimo pokusajte ponovo:");
                }
            }

            Console.WriteLine("Molimo unesite zemlju:");
            string zemljaNaziv = Console.ReadLine();
            Console.WriteLine("Molimo unesite kontinent:");
            string kontinetNaziv = Console.ReadLine();
            Kontinent kontinent;
            Enum.TryParse(kontinetNaziv, out kontinent);
            bool pogresanUnosKvote = true;
            double kvota = 0;
            while (pogresanUnosKvote)
            {
                Console.WriteLine("Molimo unesite kvotu za zemlju:");
                var kvotaString = Console.ReadLine();
                if (double.TryParse(kvotaString, out kvota))
                {
                    pogresanUnosKvote = false;
                }
                else
                {
                    Console.WriteLine("Pogresan unos! Molimo pokusajte ponovo:");
                }
            }

            var zemlja = new Zemlja(zemljaNaziv, kvota, kontinent);
            
            return new Igrac(uplata, zemlja, ime, prezime, datum);
        }
    }
}