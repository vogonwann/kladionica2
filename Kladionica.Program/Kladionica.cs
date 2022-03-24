using System;
using System.Collections.Generic;
using System.Linq;
using Kladionica.Models;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable CheckNamespace
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace Kladionica.Program
{
    class Kladionica
    {
        public static string Takmicenje { get; set; }
        public static List<UplatnoMesto> UplatnaMesta { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Unesite naziv takmičenja: ");
            Takmicenje = Console.ReadLine();

            UplatnaMesta = new List<UplatnoMesto>();
            UplatnaMesta.Add(UcitajUplatnoMesto());
            UplatnaMesta.Add(UcitajUplatnoMesto());
            
            DajPodatke();
        }

        public static void DajPodatke()
        {
            var ukupanBrojIgraca = 0;
            UplatnaMesta.ForEach(um => { ukupanBrojIgraca += um.SpisakIgraca.Count; }); // izracunaj ukupan broj igraca na svim uplatnim mestima
            Console.WriteLine($"{Takmicenje}, ukupan broj igraca {ukupanBrojIgraca}");
            UplatnaMesta.ForEach(um => // za svako uplatno mesto
            {
                Console.WriteLine($"\t{um.DajPodatke()}"); // ispisi podatke za uplatno mesto
                um.SpisakIgraca.ForEach(i => { Console.WriteLine($"\t\t{i.DajPodatke()}"); }); // za svakog igraca unutar "um", ispisi podatke
            });
            Console.WriteLine("Broj igraca sa najvise uplata: {0}", DajNajviseUplata());
        }

        public static UplatnoMesto UcitajUplatnoMesto()
        {
            Console.WriteLine("Molimo unesite grad:");
            string grad = Console.ReadLine();

            Console.WriteLine("Molimo unesite adresu:");
            string adresa = Console.ReadLine();

            UplatnoMesto novoUplatnoMesto = new UplatnoMesto(adresa, grad);

            Console.WriteLine("Unesite broj igraca:");
            string brojString = Console.ReadLine(); // unesi broj igraca za uplatno mesto
            
            int broj = int.Parse(brojString);
            for (int i = 0; i < broj; i++) // for od 0 do broja igraca koje smo uneli
            {
                Igrac noviIgrac = novoUplatnoMesto.UcitajIgraca(); // napravi novog igraca za uplatno mesto
                novoUplatnoMesto.DodajIgraca(noviIgrac); // dodaj ga u SpisakIgraca za uplatno mesto
            }

            return novoUplatnoMesto;
        }

        private static int DajNajviseUplata()
        {
            int brojac = 0;
            UplatnaMesta.ForEach(um =>
            {
                var brojUplata = um.SpisakIgraca
                    .GroupBy(igrac => new
                    {
                        igrac.Ime,
                        igrac.Prezime,
                        igrac.DatumRodjenja
                    })
                    .Count(grp => grp.Count() > 1);

                brojac += brojUplata;
            });

            return brojac;
        }
    }
}
