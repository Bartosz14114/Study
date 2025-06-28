class Program
{
    static void Main()
    {
        Console.WriteLine("Tworzenie pierwszej macierzy:");
        var m1 = WczytajMacierz();

        Console.WriteLine("\nTworzenie drugiej macierzy:");
        var m2 = WczytajMacierz();

        Console.WriteLine("\nMacierz 1:");
        Console.WriteLine(m1);

        Console.WriteLine("Macierz 2:");
        Console.WriteLine(m2);

        Console.WriteLine("\nMacierze są " + (m1 == m2 ? "takie same." : "różne."));
        Console.ReadKey();
    }

    static Macierz<int> WczytajMacierz()
    {
        int w = WczytajLiczbe("Podaj liczbę wierszy: ");
        int k = WczytajLiczbe("Podaj liczbę kolumn: ");
        var m = new Macierz<int>(w, k);

        for (int i = 0; i < w; i++)
        {
            while (true)
            {
                Console.Write($"Podaj {k} liczb dla wiersza {i + 1} (oddzielone spacją): ");
                string[] wejscie = Console.ReadLine().Split();

                if (wejscie.Length != k)
                {
                    Console.WriteLine("Błąd: zła liczba wartości. Spróbuj ponownie.");
                    continue;
                }

                bool poprawne = true;
                for (int j = 0; j < k; j++)
                {
                    if (!int.TryParse(wejscie[j], out int liczba))
                    {
                        Console.WriteLine("Błąd: tylko liczby całkowite są dozwolone.");
                        poprawne = false;
                        break;
                    }
                    m[i, j] = liczba;
                }

                if (poprawne)
                    break;
            }
        }

        return m;
    }

    static int WczytajLiczbe(string komunikat)
    {
        while (true)
        {
            Console.Write(komunikat);
            string tekst = Console.ReadLine();

            if (!int.TryParse(tekst, out int liczba) || liczba <= 0)
                Console.WriteLine("Błąd: wpisz liczbę całkowitą większą od zera.");
            else
                return liczba;
        }
    }
}

public class Macierz<T> : IEquatable<Macierz<T>>
{
    private readonly T[,] dane;
    public int Wiersze { get; }
    public int Kolumny { get; }

    public Macierz(int wiersze, int kolumny)
    {
        if (wiersze <= 0 || kolumny <= 0)
            throw new ArgumentException("Wymiary muszą być większe od zera.");

        // ✅ Sprawdzenie czy T jest porównywalne
        if (!typeof(IEquatable<T>).IsAssignableFrom(typeof(T)) &&
            !typeof(IComparable<T>).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException($"Typ {typeof(T).Name} musi implementować IEquatable<{typeof(T).Name}> lub IComparable<{typeof(T).Name}>.");
        }

        Wiersze = wiersze;
        Kolumny = kolumny;
        dane = new T[wiersze, kolumny];
    }


    public T this[int i, int j]
    {
        get => dane[i, j];
        set => dane[i, j] = value;
    }

  
   
    public bool Equals(Macierz<T>? other)
    {
        if (other is null || Wiersze != other.Wiersze || Kolumny != other.Kolumny)
            return false;

        for (int i = 0; i < Wiersze; i++)
            for (int j = 0; j < Kolumny; j++)
                if (!Equals(this[i, j], other[i, j]))
                    return false;

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as Macierz<T>);

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(Wiersze, Kolumny);
        foreach (var el in dane)
            if (el != null) hash ^= el.GetHashCode();
        return hash;
    }

    public static bool operator ==(Macierz<T>? a, Macierz<T>? b)
    {
        if (a is null) return b is null;
        return a.Equals(b);
    }

    public static bool operator !=(Macierz<T>? a, Macierz<T>? b) => !(a == b);

    public override string ToString()
    {
        string wynik = "";
        for (int i = 0; i < Wiersze; i++)
        {
            for (int j = 0; j < Kolumny; j++)
                wynik += dane[i, j]?.ToString()?.PadLeft(4) ?? "null";
            wynik += "\n";
        }
        return wynik;
    }
}
