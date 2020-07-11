using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Harfston
{
    public class Spieler
    {
        void ZeichnenHand(Spieler spieler)
        {
            foreach (Karte item in spieler.handKarten)
            {
                item.KarteZeichnen();
            }
        }
        void ZeichnenTisch(Spieler spieler)
        {
            foreach (Diener item in spieler.tischDiener)
            {
                item.KarteZeichnen();
            }
        }
        /// <summary>
        /// Speichert die Spieler, damit man auf den anderen zugreifen kann
        /// </summary>
        public static Spieler[] spieler = new Spieler[2];
        /// <summary>
        /// Speichert die Zahl der Spieler, um jedem eine Nummer zu geben
        /// </summary>
        static int Anzahl { get; set; }
        /// <summary>
        /// Gibt die Nummer des aktiven Spielers zurück
        /// </summary>
        public static int AktivNummer { get; set; }
        /// <summary>
        /// Gibt den aktiven Spieler zurück
        /// </summary>
        public static Spieler AktiverSpieler { get; private set; }
        /// <summary>
        /// Hat Wert 0 solange niemand gewonnen hat, sonst 1 bzw. 2
        /// </summary>
        public static int sieger = 0;
        /// <summary>
        /// int Wert 1 bzw. 2 des Spielers
        /// </summary>
        public int Nummer { get; set; }
        /// <summary>
        /// zählt die Runden
        /// </summary>
        public int Runde { get; set; }
        /// <summary>
        /// Hat der Spieler einen Diener, der angreifen kann?
        /// </summary>
        public bool KannAngreifen
        {
            get
            {
                foreach (Diener diener in tischDiener)
                {
                    if (diener.Warten == 0)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Hat der Spieler noch Karten?
        /// </summary>
        public bool KannSpielen => stapel.Count > 0 || handKarten.Count > 0 || tischDiener.Count > 0;
        /// <summary>
        /// gibt den anderen Spieler zurück
        /// </summary>
        public Spieler Gegenspieler => spieler[0] == this ? spieler[1] : spieler[0];
        /// <summary>
        /// Liste der Karten im Stapel
        /// </summary>
        public List<Karte> stapel = new List<Karte>();
        /// <summary>
        /// Liste der Karten auf der Hand
        /// </summary>
        public List<Karte> handKarten = new List<Karte>();
        /// <summary>
        /// Liste der Karten die ausgespielt wurden
        /// </summary>
        public List<Diener> tischDiener = new List<Diener>();
        /// <summary>
        /// Der Held des Spielers
        /// </summary>
        public Held held = new Held();
        /// <summary>
        /// Fügt die Karten dem Stapel hinzu
        /// </summary>
        public Spieler()
        {
            spieler[Anzahl] = this;
            Anzahl++;
            Nummer = Anzahl;
            
        }
        /// <summary>
        /// mischt den Stapel des Spielers
        /// </summary>
        public void Mischen()
        {
            List<Karte> neu = new List<Karte>();
            Random rand = new Random();
            int randIndex;
            int k = stapel.Count;
            for (int i = 0; i < k; i++)
            {
                randIndex = rand.Next(0, stapel.Count);
                neu.Add(stapel[randIndex]);
                stapel.RemoveAt(randIndex);
            }
            stapel = neu;
        }
        public void ZugBeginnt()
        {
            if (sieger != 0)
                return;
            Runde++;
            AktiverSpieler = this;
            AktivNummer = Nummer;
            if (tischDiener.Count != 0)
            {
                foreach (Diener diener in tischDiener)
                {
                    if (diener.Warten > 0)
                        diener.Warten--;
                }
            }
            //Nachziehen();
        }
        public void Nachziehen()
        {
            if (stapel.Count != 0)
            {
                while (handKarten.Count < 3)
                {
                    Karte k = stapel[stapel.Count - 1];
                    handKarten.Add(k);
                    stapel.RemoveAt(stapel.Count - 1);
                    k.MeinOrt = Karte.Ort.Hand;
                    k.KarteZeichnen();
                }
            }
            //AktionWählen();
        }
        public void AktionWählen()
        {
            AusspielenWählen();
            AngreifenWählen();
        }
        public void AusspielenWählen()
        {
            if (handKarten.Count == 0 || tischDiener.Count == 9)
                return;
            Menü.Ausspielen(this);
            int eingabe;
            while (true)
            {
                Menü.Fehler();
                eingabe = Console.ReadKey(true).KeyChar - '0';
                if (eingabe == 0)
                    break;
                if (eingabe > 0 && eingabe <= handKarten.Count)
                {
                    if (handKarten[eingabe - 1] is Diener diener)
                    {
                        if (diener.Angriff <= Runde)
                            break;
                        else
                            Menü.Fehler($"{diener.GetType().Name} kann noch nicht ausgespielt werden!");
                    }
                    else
                        break;
                }
            }
            Ausspielen(eingabe - 1);
            Menü.Ausspielen();
        }
        public void Ausspielen(int index)
        {
            if (index == -1)
                return;
            if (handKarten.Count != 0)
            {
                Diener d = handKarten[index] as Diener;
                tischDiener.Add(d);
                d.MeinOrt = Karte.Ort.Tisch;
                ZeichnenTisch(this);
                if (!(d.Ansturm))
                    d.Warten = 1;
                handKarten.RemoveAt(index);
                ZeichnenHand(this);
            }
        }
        public void AngreifenWählen()
        {
            while (KannAngreifen&&sieger==0)
            {
                int eingabe, dienerIndex, gegnerIndex;
                Menü.AngreifenDiener(this);
                while (true)
                {
                    Menü.Fehler();
                    eingabe = Console.ReadKey(true).KeyChar - '0';
                    if (eingabe == 0)
                        break;
                    if (eingabe >= 1 && eingabe <= tischDiener.Count)
                    {
                        if (tischDiener[eingabe - 1].Warten == 0)
                            break;
                        else
                        {
                            Menü.Fehler($"{tischDiener[eingabe - 1].GetType().Name} kann nicht angreifen!");
                        }
                    }
                }
                Menü.AngreifenDiener();
                if (eingabe == 0)
                    return;
                dienerIndex = eingabe - 1;
                Zeichnen.Markieren(this,dienerIndex);

                if (Gegenspieler.tischDiener.Count == 0)
                {
                    Zeichnen.Markieren(Gegenspieler, -1);
                    Zeichnen.Markieren(this, dienerIndex, false);
                    Zeichnen.Markieren(Gegenspieler, -1, false);
                    Angreifen(dienerIndex, -1);
                }
                else
                {
                    List<int> spots = new List<int>();
                    for (int index = 0; index < Gegenspieler.tischDiener.Count; index++)
                    {
                        if (Gegenspieler.tischDiener[index].Spott)
                            spots.Add(index);
                    }
                    while (true)
                    {
                        Menü.AngreifenGegner(Gegenspieler);
                        Menü.Fehler();
                        eingabe = Console.ReadKey(true).KeyChar - '0';
                        if (spots.Count != 0)
                        {
                            if (eingabe == 0)
                                Menü.Fehler($"Held kann nicht angegriffen werden. Spott");
                            if (eingabe >= 1 && eingabe <= Gegenspieler.tischDiener.Count)
                            {
                                if (Gegenspieler.tischDiener[eingabe - 1].Spott)
                                    break;
                                else
                                    Menü.Fehler($"Dieser Diener kann nicht angegriffen werden. Spott");
                            }
                        }
                        else if (eingabe >= 0 && eingabe <= Gegenspieler.tischDiener.Count)
                            break;
                    }
                    Menü.AngreifenGegner();
                    gegnerIndex = eingabe - 1;
                    Zeichnen.Markieren(Gegenspieler, gegnerIndex);
                    Zeichnen.Markieren(this, dienerIndex, false);
                    Zeichnen.Markieren(Gegenspieler, gegnerIndex, false);
                    Angreifen(dienerIndex, gegnerIndex);
                }
            }
        }
        public void Angreifen(int dienerIndex, int gegnerIndex)
        {
            Diener diener = tischDiener[dienerIndex];
            diener.Warten = 1;
            if (gegnerIndex < 0)
            {
                Gegenspieler.held.Leben -= diener.Angriff;
                Zeichnen.Held(Gegenspieler);
                if (Gegenspieler.held.Leben < 1)
                    SpielBeenden();
            }
            else
            {
                Diener gegner = Gegenspieler.tischDiener[gegnerIndex];
                Diener gegnerR = gegnerIndex != Gegenspieler.tischDiener.Count - 1? Gegenspieler.tischDiener[gegnerIndex + 1]:null;
                Diener gegnerL= gegnerIndex != 0? Gegenspieler.tischDiener[gegnerIndex - 1]:null;
                gegner.Leben -= diener.Angriff;
                if (diener.Prankenhiebe)
                {
                    if (gegnerL != null)
                        gegnerL.Leben--;
                    if (gegnerR != null)
                        gegnerR.Leben--;
                }
                if (gegner.Leben < 1 || (gegnerL != null && gegnerL.Leben < 1) || (gegnerR != null && gegnerR.Leben < 1))
                {
                    Gegenspieler.tischDiener.RemoveAt(gegnerIndex);
                    ZeichnenTisch(Gegenspieler);
                }
                diener.Leben -= gegner.Angriff;
                if (diener.Leben < 1)
                {
                    tischDiener.RemoveAt(dienerIndex);
                    ZeichnenTisch(this);
                }
            }
        }
        public void SpielBeenden()
        {
            Console.SetCursorPosition(100,30);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Spieler {Nummer} gewinnt!");
            Thread.Sleep(300);
            Console.SetCursorPosition(100, 30);
            Console.WriteLine("                  ");
            Thread.Sleep(300);
            Console.SetCursorPosition(100, 30);
            Console.WriteLine($"Spieler {Nummer} gewinnt!");
            sieger = Nummer;
            Console.ReadKey(true);
        }
    }
}