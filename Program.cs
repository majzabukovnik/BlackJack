using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, double> denar = new Dictionary<string, double>();//hranjenje denarja, ki ga je igralec pridobil skozi igre
            do
            {
                List<string> talon = Talon();//inicializacija talona s klicom funkcije Talon()
                List<Igralec> igralci = Inicializacija(talon, denar);//klic metode, ki izve koliko igralcev bo igralo igro, njihova imena ter ustvari list v katerem so sharnjeni vsi igralci
                int index = igralci.Count + 1;
                PrikazStanja(igralci);//izpis stanja kart in njihovih vrednosti
                Odlocitev(igralci, talon, index);
                igralci[0].karteList.Add(talon[1]);//dodajanje manjkajoče karte dealerju
                PrikazStanja(igralci);
                while (igralci[0].IzracunajRezultat() < 17)//če je vrednost dealerjevih kart manjša od 17 mora dealer jemati karte dokler ne doseže ali preseže 17
                {
                    igralci[0].karteList.Add(talon[index]);
                    Console.WriteLine($"Dealer je dobil {talon[index]}. Vrednost njegovih kart je {igralci[0].IzracunajRezultat()}.\n");
                    index++;
                }

                int ig = 0;
                foreach (Igralec igr in igralci)//izpis ali je posameznik zmagal ali izgubil ta krog
                {
                    if (ig != 0)
                    {
                        igr.Zmaga(igralci[0].IzracunajRezultat());
                        Console.WriteLine($"{igr.ime} vaše stanje na računu je {igr.denar}.");
                    }
                    ig++;
                }

                denar.Clear();
                foreach (Igralec igr in igralci)
                {

                    denar.Add(igr.ime, Convert.ToDouble(igr.denar));
                }

                Console.Write("Želite iti novo igro? y/n: ");//pogoj, ki preveri, če si igralci želijo še en krog
                if (Console.ReadLine().ToLower() != "y") break;
                Console.Clear();
            }
            while (true);

            Console.ReadKey();
        }
        static List<string> Talon()
        {
            Random random = new Random();//ranodm knjižnica
            List<string> talon = new List<string>();//deklaracija talona kart
            string[] posebneKarte = { "A", "J", "Q", "K" };

            for (int i = 0; i < 4; i++)//zapis vseh 52 kart v list z imenom talon
            {
                for (int card = 2; card < 11; card++)
                {
                    talon.Add(Convert.ToString(card));
                }
                foreach (string k in posebneKarte)
                {
                    talon.Add(k);
                }
            }
            var Talon = talon.OrderBy(item => random.Next());//premeša karte
            talon = Talon.ToList();//pretvori iz tipa var v tip List
            return talon;
        }
        static List<Igralec> Inicializacija(List<string> talon_, IDictionary<string, double> denarP)
        {
            string[] talon = talon_.ToArray();
            List<Igralec> igralci = new List<Igralec>() { };
            int stIgralcev;

            Console.Title = "Blackjack";
            Console.WriteLine("Dobrodošli v igro Blackjack!\n____________________________\n");
            do//preverjanje pravilnosti vnosa števila igralcev
            {
                Console.Write("Vnesite število igralcev: ");
                stIgralcev = int.Parse(Console.ReadLine());
            }
            while (stIgralcev > 8 || stIgralcev < 1);

            Igralec igralec0 = new Igralec();//inicializacija dealerja
            igralec0.ime = "Dealer";
            igralec0.denar = 10000000;
            igralci.Add(igralec0);

            for (int i = 1; i <= stIgralcev; i++)//inicializacija ostalih igralcev
            {
                Console.Write($"Vnesite ime {i}. igralca: ");
                Igralec igralec1 = new Igralec();
                igralec1.ime = Console.ReadLine();
                igralci.Add(igralec1);
            }

            int index = 0;
            foreach (Igralec i in igralci)//dodajanje kart igralcem
            {
                if (index != 0)
                {
                    i.karteList.Add(talon[index]);
                    i.karteList.Add(talon[index + 1]);
                    if (denarP.ContainsKey(i.ime)) { i.denar = denarP[i.ime]; } //vnos stanja denarja iz prejšnje igre
                    else { i.denar = 1000; }//če igralca ni bilo v prejšnji igri dobi začetnih 1000

                    double stava;
                    do
                    {
                        Console.Write($"{i.ime}, na voljo imate {i.denar} žetonov. Vnesite svojo stavo: ");
                        stava = Convert.ToDouble(Console.ReadLine());
                    }
                    while(i.denar < stava);
                    i.Stavi(stava);
                }
                else i.karteList.Add(talon[index]);
                index += 2;
            }

            return igralci;
        }
        static void PrikazStanja(List<Igralec> igralci)
        {
            Console.WriteLine();
            foreach (Igralec i in igralci)//izpis katere karte imajo igralci in kolikšna je njihova vrednost
            {
                Console.Write($"{i.ime} ime karte ");
                foreach (string karta in i.karteList)
                {
                    Console.Write($"{karta}, ");
                }
                Console.WriteLine($"torej je vrednost kart {i.IzracunajRezultat()}.");
            }
            Console.WriteLine();
        }
        static void Odlocitev(List<Igralec> _igralci, List<string> _talon, int index)
        {
            byte ig = 0;
            foreach (Igralec i in _igralci)//če igralec še ni dosegel seštevka 21 se lahko odloči, da bo vzel še 1 karto
            {
                if (ig == 0) { ig++; continue; }
                while (i.IzracunajRezultat() < 21)
                {
                    Console.Write($"{i.ime}, želite novo karto? y/n: ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        i.karteList.Add(_talon[index]);
                        Console.WriteLine($"\n{i.ime}, dobili ste {_talon[index]}, torej je vaš nov seštevek kart {i.IzracunajRezultat()}!\n");
                        index++;
                    }
                    else break;
                }
                if (i.IzracunajRezultat() > 21) { Console.WriteLine($"{i.ime}, vrednost vaših kart je presegla 21, torej ste izgubili in ne morete zahtevati novih kart :(\n"); continue; }
            }
        }
    }
}
