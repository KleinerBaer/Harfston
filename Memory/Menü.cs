using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Harfston
{
    public class Menü
    {
        static void DruckeMenü(params string[] stray)
        {
            int y = Spieler.AktivNummer == 1 ? 45 : 10;
            Console.SetCursorPosition(0, y);
            if (stray.Length == 0)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("                           ");
                Console.WriteLine("                                                                        ");
                Console.WriteLine("                                                                        ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($"Spieler {Spieler.AktivNummer} Aktion wählen:");
                foreach (string s in stray)
                {
                    Console.WriteLine(s);
                }
            }
        }
        public static void Fehler(string fehler = "                                                    ")
        {
            int y = Spieler.AktivNummer == 1 ? 48 : 13;
            Console.SetCursorPosition(0, y);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(fehler);
            Thread.Sleep(400);
        }
        public static void Ausspielen(Spieler spieler = null)
        {
            if (spieler == null)
            {
                DruckeMenü();
                return;
            }
            int nummer=0;
            string s;
            foreach (Diener diener in spieler.handKarten)
            {
                if (diener.Angriff <= spieler.Runde)
                {
                    nummer = spieler.handKarten.IndexOf(diener) + 1;
                    break;
                }
            }
            s = $"({nummer})";
            for (int i = nummer; i < spieler.handKarten.Count; i++)
            {
                if (spieler.handKarten[i] is Diener diener)
                {
                    if (diener.Angriff <= spieler.Runde)
                        s += $", ({i + 1})";
                }
            }
            DruckeMenü("Karte" + s + "ausspielen oder keine (0)");
        }
        public static void AngreifenDiener(Spieler spieler = null)
        {
            if (spieler == null)
            {
                DruckeMenü();
                return;
            }
            int nummer = 0;
            string s;
            foreach (Diener diener in spieler.tischDiener)
            {
                if (diener.Warten == 0)
                {
                    nummer = spieler.tischDiener.IndexOf(diener) + 1;
                    break;
                }
            }
            s = $"({nummer})";
            for (int i = nummer; i < spieler.tischDiener.Count; i++)
            {
                if (spieler.tischDiener[i].Warten == 0)
                    s += $", ({i + 1})";
            }
            DruckeMenü("Angreifen mit Diener " + s + "\n    oder Zug beenden (0)?");
        }
        public static void AngreifenGegner(Spieler spieler = null)
        {
            if (spieler == null)
            {
                DruckeMenü();
                return;
            }
            string s = "";
            if (spieler.tischDiener.Count != 0)
            {
                s += " oder Gegnerischen Diener (1)";
                for (int i = 1; i < spieler.tischDiener.Count; i++)
                {
                    s += $", ({i + 1})";
                }
            }
            DruckeMenü("Held (0)" + s + " angreifen");
        }
    }
}
