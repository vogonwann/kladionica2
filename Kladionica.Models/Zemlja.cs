namespace Kladionica.Models
{
    public class Zemlja
    {
        public string Naziv { get; set; }
        public double Kvota { get; set; }

        public Kontinent Kontinent { get; set; }
        
        public Zemlja() {}

        public Zemlja(string naziv, double kvota, Kontinent kontinent)
        {
            Naziv = naziv;
            Kvota = kvota;
            Kontinent = kontinent;
        }
    }
}