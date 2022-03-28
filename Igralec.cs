using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Igralec
    {
        public string ime;
        public List<string> karteList = new List<string>();
        public double denar = 1000.0;//vrednost žetonov, ki jih igralec lahko zastavi
        public double zacetnaStava = 0;
        public string Status()//metoda, ki igralcu pove katere karte ima in kolikšna je njihova vrednost
        {
            string kartevRoki = "";
            for (int i = 0; i < karteList.Count; i++)
            {
                kartevRoki += $"{karteList[i]} ";
            }
            return $"{ime} ima v roki karte {kartevRoki}, seštevek kart znaša {IzracunajRezultat()}.";
        }
        public int IzracunajRezultat()//metoda, ki izračuna vrednost kart, ki jih ima igralec v rokah
        {
            int n, stAsov = 0, rezultat = 0;
            foreach (string i in karteList)
            {
                if (int.TryParse(i, out n) == false)
                {
                    if (i == "A")//as je vreden 11 točk
                    {
                        stAsov++;
                        rezultat += 11;
                    }
                    else rezultat += 10;//kralj, kraljica in pob so vredni 10 točk
                }
                else rezultat += int.Parse(i);//ostale karte imajo isto vrednost kot število na njih
            }

            while (stAsov != 0 && rezultat > 21)//as je lahko vreden tudi le 1 točko, če rezultat preseže 21 točk se 10 odšteje od rezultata pod pogojem, da ima igralec v rokah kakšnega asa
            {
                rezultat -= 10;
                stAsov--;
            }
            return rezultat;
        }
        public void Stavi(double _stava)
        {
            denar -= _stava;
            zacetnaStava += _stava;
        }
        public void Zmaga(int dealer)//izračun izkupička denarja, ki ga zmagovalec dobi
        {
            int rezultat = IzracunajRezultat();
            if (rezultat <= 21)
            {
                if (rezultat == 21 && karteList.Count == 2 && dealer != 21) denar += (2.5 * zacetnaStava);
                else if (rezultat == dealer) denar += zacetnaStava;
                else if (dealer >= 22) denar += (2 * zacetnaStava);
                else if (dealer < rezultat) denar += (2 * zacetnaStava);
            }
            zacetnaStava = 0;
        }
    }
}
