using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Harfston
{
    class Program
    {
        #region
        [DllImport("Kernel32.dll")]
        static extern IntPtr GetStdHandle(int nStdHandle);
        enum STD_HANDLE : int
        {
            STD_INPUT_HANDLE = -10,
            STD_OUTPUT_HANDLE = -11,
            STD_ERROR_HANDLE = -12
        }
        [DllImport("Kernel32.dll")]
        static extern bool SetConsoleDisplayMode(IntPtr hConsoleOutput, int dwFlags, IntPtr lpNewScreenBufferDimensions);
        enum CONSOLE_MODE : int
        {
            CONSOLE_FULLSCREEN_MODE = 1,
            CONSOLE_WINDOWED_MODE = 2
        }
        [DllImport("Kernel32.dll")]
        static extern int GetLastError();
        static bool ConsoleDisplayMode(CONSOLE_MODE mode)
        {
            IntPtr handle = GetStdHandle((int)STD_HANDLE.STD_OUTPUT_HANDLE);
            return SetConsoleDisplayMode(handle, (int)mode, IntPtr.Zero);
        }
        #endregion
        static void Main(string[] args)
        {
            //Konsolenfenster einrichten
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            if (!ConsoleDisplayMode(CONSOLE_MODE.CONSOLE_FULLSCREEN_MODE))
                Console.WriteLine("Error: " + GetLastError());
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.CursorVisible = false;
            
            //Spiel startet
            Spieler s1 = new Spieler();
            //List<Karte> k1 = s1.stapel;
            Spieler s2 = new Spieler();
            //List<Karte> k2 = s2.stapel;
            Zeichnen.Held(s1);
            Zeichnen.Held(s2);
            
            
            s1.stapel.Add(new Katze(s1));
            s1.stapel.Add(new Tiger(s1));
            s1.stapel.Add(new Papagei(s1));
            s1.stapel.Add(new Bär(s1));
            s1.stapel.Add(new Löwe(s1));
            s1.stapel.Add(new Papagei(s1));
            s1.stapel.Add(new Marder(s1));
            s1.stapel.Add(new Nashorn(s1));
            s1.stapel.Add(new Schwarzbär(s1));
            s1.stapel.Add(new Katze(s1));
            s1.Mischen();
            s2.stapel.Add(new Katze(s2));
            s2.stapel.Add(new Bär(s2));
            s2.stapel.Add(new Katze(s2));
            s2.stapel.Add(new Papagei(s2));
            s2.stapel.Add(new Marder(s2));
            s2.stapel.Add(new Nashorn(s2));
            s2.stapel.Add(new Schwarzbär(s2));
            s2.stapel.Add(new Tiger(s2));
            s2.stapel.Add(new Löwe(s2));
            s2.stapel.Add(new Papagei(s2));
            s2.Mischen();
            while (s1.KannSpielen && s2.KannSpielen && Spieler.sieger == 0)
            {
                s1.ZugBeginnt();
                s1.Nachziehen();
                s1.AusspielenWählen();
                s1.AngreifenWählen();
                s2.ZugBeginnt();
                s2.Nachziehen();
                s2.AusspielenWählen();
                s2.AngreifenWählen();
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
