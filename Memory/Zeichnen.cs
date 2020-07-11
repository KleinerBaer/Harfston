using System;
using System.Collections.Generic;
using System.Threading;

namespace Harfston
{
    public static class Zeichnen
    {
        private static int Bremse => 300;
        private static readonly int kartenBreite = 10, kartenHöhe = 6;
        private static void WriteAt(string s, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(s);
        }
        private static void Rahmen(int x, int y, bool zeichnen = true)
        {
            string oben, seite, unten;
            if (zeichnen)
            {
                oben = "\u2581";
                seite = "\u2503";
                unten = "\u2594";
            }
            else
            {
                oben = seite = unten = " ";
                Thread.Sleep(Bremse);
            }
            y--;
            for (int i = 0; i < kartenBreite + 2; i++)
                WriteAt(oben, x++, y);
            y++;
            for (int i = 0; i < kartenHöhe + 2; i++)
                WriteAt(seite, x, y++);
            x--;
            for (int i = 0; i < kartenBreite + 2; i++)
                WriteAt(unten, x--, y);
            y--;
            for (int i = 0; i < kartenHöhe + 2; i++)
                WriteAt(seite, x, y--);
        }
        public static void Held(Spieler s)
        {

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            int posX = 102, posY = s.Nummer == 1 ? 52 : 2, tempX = posX, tempY = posY, size = 4;

            for (int i = 0; i < size; i++)
            {
                WriteAt("\u2571", tempX--, tempY++);
            }
            tempX++;
            for (int i = 0; i < size; i++)
            {
                WriteAt("\u2572", tempX++, tempY++);
            }
            tempY--;
            for (int i = 0; i < size; i++)
            {
                WriteAt("_", tempX++, tempY);
            }
            for (int i = 0; i < size; i++)
            {
                WriteAt("\u2571", tempX++, tempY--);
            }
            tempX--;
            for (int i = 0; i < size; i++)
            {
                WriteAt("\u2572", tempX--, tempY--);
            }
            for (int i = 0; i < size; i++)
            {
                WriteAt("_", tempX--, tempY);
            }
            WriteAt($"{s.held.Leben,2}", posX + size / 2, posY + size - 1);
        }
        public static void Markieren(Spieler spieler, int figur, bool zeichnen = true)
        {
            int posY,posX;
            if (figur == -1)
            {
                posY = spieler.Nummer==1? 52:2;
                posX = 100;
            }
            else
            {
                posY = spieler.Nummer == 1 ? 33 : 21;
                posX = (-10 * spieler.tischDiener.Count + 110) + figur * 20;
            }
            Console.ForegroundColor = (spieler == Spieler.AktiverSpieler) ? ConsoleColor.White : ConsoleColor.DarkRed;
            Rahmen(posX, posY, zeichnen);
        }
    }
}
