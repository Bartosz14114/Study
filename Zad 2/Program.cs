using System;
using System.Linq;

public class Wektor
{
    private readonly double[] wspolrzedne;

    public double[] Wspolrzedne => wspolrzedne;

    public double Dlugosc => Math.Sqrt(IloczynSkalarny(this, this));

    public int Wymiar => wspolrzedne.Length;

    public Wektor(int wymiar)
    {
        if (wymiar <= 0)
            throw new ArgumentException("Wymiar musi być większy od zera.");
        wspolrzedne = new double[wymiar];
    }

    public Wektor(params double[] wspolrzedne)
    {
        if (wspolrzedne == null || wspolrzedne.Length == 0)
            throw new ArgumentException("Tablica współrzędnych nie może być pusta.");
        this.wspolrzedne = new double[wspolrzedne.Length];
        Array.Copy(wspolrzedne, this.wspolrzedne, wspolrzedne.Length);
    }

    public double this[int index]
    {
        get => wspolrzedne[index];
        set => wspolrzedne[index] = value;
    }

    public static double IloczynSkalarny(Wektor V, Wektor W)
    {
        if (V.Wymiar != W.Wymiar)
            return double.NaN;

        double suma = 0;
        for (int i = 0; i < V.Wymiar; i++)
            suma += V[i] * W[i];

        return suma;
    }

    public static Wektor Suma(params Wektor[] Wektory)
    {
        if (Wektory.Length == 0)
            throw new ArgumentException("Brak wektorów.");

        int wymiar = Wektory[0].Wymiar;

        foreach (var w in Wektory)
            if (w.Wymiar != wymiar)
                throw new ArgumentException("Wektory mają różne wymiary.");

        var wynik = new double[wymiar];
        foreach (var w in Wektory)
            for (int i = 0; i < wymiar; i++)
                wynik[i] += w[i];

        return new Wektor(wynik);
    }

    public static Wektor operator +(Wektor a, Wektor b)
    {
        if (a.Wymiar != b.Wymiar)
            throw new ArgumentException("Wektory mają różne wymiary.");

        var wynik = new double[a.Wymiar];
        for (int i = 0; i < a.Wymiar; i++)
            wynik[i] = a[i] + b[i];

        return new Wektor(wynik);
    }

    public static Wektor operator -(Wektor a, Wektor b)
    {
        if (a.Wymiar != b.Wymiar)
            throw new ArgumentException("Wektory mają różne wymiary.");

        var wynik = new double[a.Wymiar];
        for (int i = 0; i < a.Wymiar; i++)
            wynik[i] = a[i] - b[i];

        return new Wektor(wynik);
    }

    public static Wektor operator *(Wektor a, double skalar)
    {
        var wynik = new double[a.Wymiar];
        for (int i = 0; i < a.Wymiar; i++)
            wynik[i] = a[i] * skalar;

        return new Wektor(wynik);
    }

    public static Wektor operator *(double skalar, Wektor a) => a * skalar;

    public static Wektor operator /(Wektor a, double skalar)
    {
        if (skalar == 0)
            throw new DivideByZeroException("Nie można dzielić przez zero.");

        var wynik = new double[a.Wymiar];
        for (int i = 0; i < a.Wymiar; i++)
            wynik[i] = a[i] / skalar;

        return new Wektor(wynik);
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", wspolrzedne)}]";
    }

    public static void Main()
    {
        Console.Write("Podaj wymiar wektorów: ");
        if (!int.TryParse(Console.ReadLine(), out int wymiar) || wymiar <= 0)
        {
            Console.WriteLine("Nieprawidłowy wymiar.");
            return;
        }

        Console.WriteLine("Podaj współrzędne wektora 1, oddzielone spacją:");
        double[] wsp1 = WczytajWspolrzedne(wymiar);

        Console.WriteLine("Podaj współrzędne wektora 2, oddzielone spacją:");
        double[] wsp2 = WczytajWspolrzedne(wymiar);

        var w1 = new Wektor(wsp1);
        var w2 = new Wektor(wsp2);

        Console.WriteLine("Wektor 1: " + w1);
        Console.WriteLine("Wektor 2: " + w2);
        Console.WriteLine("Suma: " + (w1 + w2));
        Console.WriteLine("Różnica: " + (w1 - w2));
        Console.WriteLine("Iloczyn skalarny: " + Wektor.IloczynSkalarny(w1, w2));
        Console.WriteLine("Długość wektora 1: " + w1.Dlugosc);
        Console.WriteLine("Wektor 1 * 2: " + (w1 * 2));
        Console.WriteLine("Wektor 2 / 2: " + (w2 / 2));
        Console.WriteLine("Suma wielu (w1, w2): " + Wektor.Suma(w1, w2));
    }

    private static double[] WczytajWspolrzedne(int wymiar)
    {
        while (true)
        {
            string input = Console.ReadLine();
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != wymiar)
            {
                Console.WriteLine($"Podaj dokładnie {wymiar} liczb.");
                continue;
            }

            double[] wsp = new double[wymiar];
            bool sukces = true;

            for (int i = 0; i < wymiar; i++)
            {
                if (!double.TryParse(parts[i], out wsp[i]))
                {
                    Console.WriteLine("Błąd przy parsowaniu liczby. Spróbuj ponownie.");
                    sukces = false;
                    break;
                }
            }

            if (sukces)
                return wsp;
        }
    }
}
