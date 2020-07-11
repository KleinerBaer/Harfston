using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Harfston
{
    public class Held
    {
        public int Angriff { get; set; }
        public int Leben { get; set; }
        public Held()
        {
            Angriff = 0;
            Leben = 20;
        }
    }
    public abstract class Karte
    {
        /// <summary>
        /// Letzte x- bzw. y-Position der Karte
        /// </summary>
        int x, y;
        /// <summary>
        /// Index der Liste in der die Karte sich befindet
        /// </summary>
        int MeinIndex
        {
            get
            {
                if (MeinOrt == Ort.Stapel)
                    return 0;
                if (MeinOrt == Ort.Hand)
                    return meinSpieler.handKarten.IndexOf(this);
                return meinSpieler.tischDiener.IndexOf(this as Diener);
            }
        }
        /// <summary>
        /// Count der Liste in der die Karte sich befindet
        /// </summary>
        int MeinCount
        {
            get
            {
                if (MeinOrt == Ort.Stapel)
                    return 0;
                if (MeinOrt == Ort.Hand)
                    return meinSpieler.handKarten.Count;
                return meinSpieler.tischDiener.Count;
            }
        }
        /// <summary>
        /// Wert für Layout
        /// </summary>
        readonly static int handLinks = 85, handAbstand = 15, tischMitte = 110, tischVersatz = -10, tischAbstand = 20, kartenBreite = 10, kartenHöhe = 6;
        /// <summary>
        /// Zeichen der Karte Zeilenweise als String-Array
        /// </summary>
        readonly static string[] leerKarte = StandardKarte(), löschkarte = Löschkarte();
        /// <summary>
        /// Spieler dem die Karte gehört
        /// </summary>
        protected Spieler meinSpieler;
        //protected Spieler Gegenspieler => Spieler.spieler[0] == meinSpieler ? Spieler.spieler[1] : Spieler.spieler[0];
        /// <summary>
        /// Mögliche Kartenorte
        /// </summary>
        public enum Ort {Stapel,Hand,Tisch };
        /// <summary>
        /// Aktueller Ort der Karte
        /// </summary>
        public Ort MeinOrt { get; set; } = Ort.Stapel;
        /// <summary>
        /// Aktuelle y-Position der Karte
        /// </summary>
        int Y
        {
            get
            {
                if (MeinOrt == Ort.Stapel)
                    return 0;
                if (MeinOrt == Ort.Hand)
                    return meinSpieler.Nummer == 1 ? 43 : 11;
                return meinSpieler.Nummer == 1 ? 33 : 21;
            }
        }
        /// <summary>
        /// Aktuelle x-Position der Karte
        /// </summary>
        int X
        {
            get
            {
                if (MeinOrt == Ort.Stapel)
                    return 0;
                return MeinOrt == Ort.Hand ? handLinks + MeinIndex * handAbstand : (tischVersatz * MeinCount + tischMitte) + MeinIndex * tischAbstand;
            }
        }
        /// <summary>
        /// Baut zu Beginn einmal die Karte mit Leerzeichen
        /// </summary>
        /// <returns>Leerzeichen zeilenweise</returns>
        private static string[] Löschkarte()
        {
            string[] s = new string[kartenHöhe+2];
            for (int i = 0; i < kartenHöhe+2; i++)
            {
                for (int k = 0; k < kartenBreite+2; k++)
                {
                    s[i] += ' ';
                }
            }
            return s;
        }
        /// <summary>
        /// Baut zu beginn einmal die Standardkarte ohne Inhalt
        /// </summary>
        /// <returns>Standardkarte zeilenweise</returns>
        private static string[] StandardKarte()
        {
            
            string[] s = new string[kartenHöhe + 2];
            int j = 0;
            s[j] = "\u250F";
            for (int i = 0; i < kartenBreite; i++)
                s[j] += '\u2501';
            s[j++] += '\u2513';
            for (int k = 0; k < kartenHöhe; k++)
            {
                s[j] += '\u2503';
                for (int i = 0; i < kartenBreite; i++)
                    s[j] += ' ';
                s[j++] += '\u2503';
            }
            s[j] += '\u2517';
            for (int i = 0; i < kartenBreite; i++)
                s[j] += '\u2501';
            s[j] += '\u251B';
            return s;
        }
        /// <summary>
        /// Radiert die alte Position und Zeichnet die Karte an der neuen
        /// </summary>
        public void KarteZeichnen()
        {
            Thread.Sleep(200);
            Radieren();
            if ((this as Diener).Leben > 0)
            {
                StandardKarteZeichnen();
                DatenZeichnen();
            }
        }
        /// <summary>
        /// Entfernt die Grafik der Karte an der alten Position
        /// </summary>
        public void Radieren()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            for (int i = 0; i < löschkarte.Length; i++)
            {
                Console.SetCursorPosition(x, y+i);
                Console.Write(löschkarte[i]);
            }
            x = X;
            y = Y;
        }
        /// <summary>
        /// Zeichnet die Standardkarte ohne Inhalt an der aktuellen Position der Karte
        /// </summary>
        public void StandardKarteZeichnen()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            for (int i = 0; i < leerKarte.Length; i++)
            {
                Console.SetCursorPosition(X, Y+i);
                Console.Write(leerKarte[i]);
            }
        }
        /// <summary>
        /// Zeichnet die aktuellen Daten der Karte ein
        /// </summary>
        public void DatenZeichnen()
        {
            if (this is Diener d)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                //Angriffwert und -symbol
                Console.SetCursorPosition(X + 1, Y + 1);
                Console.Write($"{d.Angriff}\u2694");
                //Lebenswert und -symbol
                Console.SetCursorPosition(X + kartenBreite - 1, Y + 1);
                Console.Write($"{d.Leben}\u2665");
                //Symbol für Ansturm oder Spott
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(X + kartenBreite / 2, Y + 1);
                if (d.Ansturm)
                    Console.Write("\u25B3");
                if (d.Spott)
                    Console.Write("\u21CA");
                if (d.Prankenhiebe)
                    Console.Write("\u222D");
                //Name
                string name = (this as Diener).GetType().Name;
                Console.SetCursorPosition(X + 1 + (kartenBreite - name.Length) / 2, Y + kartenHöhe / 2);
                Console.Write(name);
            }
        }
    }
    public abstract class Diener : Karte
    {
        //public Action LebenChanged;
        //protected virtual void OnLebenChanged()
        //{
        //    if (LebenChanged != null) LebenChanged();
        //}
        public int Angriff { get; set; }
        private int _leben;
        public int Leben { get { return _leben; } set { _leben = value; DatenZeichnen(); if (_leben < 1) Radieren(); } }
        /// <summary>
        /// Diener kann nach dem ausspielen direkt angreifen
        /// </summary>
        public bool Ansturm { get; set; } = false;
        /// <summary>
        /// Ist Spott aktiv können Diener nur diese Gegner angreifen
        /// </summary>
        public bool Spott { get; set; } = false;
        /// <summary>
        /// Greift der Diener einen Gegner an, nehmen auch die Diener links und rechts des Gegners Schaden
        /// </summary>
        public bool Prankenhiebe { get; set; } = false;
        public virtual int Warten { get; set; }
        public override string ToString()
        {
            return $"{this.GetType().Name}, Angriff: {Angriff}, Leben: {Leben}";
        }
    }
    class Katze : Diener
    {
        public Katze(Spieler spieler)
        {
            Angriff = 1;
            Leben = 9;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Papagei : Diener
    {
        public Papagei(Spieler spieler)
        {
            Angriff = 2;
            Leben = 3;
            Ansturm = true;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Marder : Diener
    {
        public Marder(Spieler spieler)
        {
            Angriff = 3;
            Leben = 2;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Bär : Diener
    {
        public Bär(Spieler spieler)
        {
            Angriff = 3;
            Leben = 5;
            Spott = true;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Schwarzbär : Diener
    {
        public Schwarzbär(Spieler spieler)
        {
            Angriff = 3;
            Leben = 4;
            Prankenhiebe = true;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Tiger : Diener
    {
        public Tiger(Spieler spieler)
        {
            Angriff = 5;
            Leben = 3;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Löwe : Diener
    {
        public Löwe(Spieler spieler)
        {
            Angriff = 4;
            Leben = 4;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }
    class Nashorn : Diener
    {
        public Nashorn(Spieler spieler)
        {
            Angriff = 6;
            Leben = 3;
            Ansturm = true;
            meinSpieler = spieler;
            //LebenChanged += DatenZeichnen;
        }
    }

}
