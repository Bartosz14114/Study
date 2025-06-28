using System.Globalization;
using System.Text.RegularExpressions;

public class Osoba
{
    private string imię;
    public string Nazwisko;
    public DateTime? DataUrodzenia = null;
    public DateTime? DataŚmierci = null;

    public Osoba(string imięNazwisko)
    {
        ImięNazwisko = imięNazwisko;
    }

    public string Imię
    {
        get => imię;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Imię nie może być puste.");
            imię = value;
        }
    }

    public string ImięNazwisko
    {
        get => $"{imię} {Nazwisko}".Trim();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                imię = "";
                Nazwisko = "";
                return;
            }

            var parts = value.Trim().Split(' ');
            if (parts.Length == 1)
            {
                imię = parts[0];
                Nazwisko = "";
            }
            else
            {
                imię = parts[0];
                Nazwisko = string.Join(" ", parts, 1, parts.Length - 1);
            }
        }
    }

    public TimeSpan? Wiek
    {
        get
        {
            if (DataUrodzenia == null)
                return null;

            DateTime endDate = DataŚmierci ?? DateTime.Now;
            return endDate - DataUrodzenia.Value;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Podaj imię: ");
            string imie = Console.ReadLine();
            if (ContainsDigits(imie))
            {
                Console.WriteLine("\nBłąd: Imię i nazwisko nie mogą zawierać cyfr.\nSpróbuj ponownie.\n");
                continue;
            }

            Console.Write("Podaj nazwisko: ");
            string nazwisko = Console.ReadLine();


            if (ContainsDigits(nazwisko))
            {
                Console.WriteLine("\nBłąd: Imię i nazwisko nie mogą zawierać cyfr.\nSpróbuj ponownie.\n");
                continue;
            }


            Console.Write("Podaj datę urodzenia (rrrr-mm-dd): ");
            string dataInput = Console.ReadLine();

            DateTime dataUrodzenia;

            while (!DateTime.TryParseExact(dataInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataUrodzenia))
            {
                Console.Write("Błędny format! Wprowadź datę jako rrrr-mm-dd: ");
                dataInput = Console.ReadLine();
            }

            var osoba = new Osoba($"{imie} {nazwisko}")
            {
                DataUrodzenia = dataUrodzenia
            };

            Console.WriteLine("\n--- Dane Osoby ---");
            Console.WriteLine($"Imię: {osoba.Imię}");
            Console.WriteLine($"Nazwisko: {osoba.Nazwisko}");
            Console.WriteLine($"Imię i Nazwisko: {osoba.ImięNazwisko}");
            Console.WriteLine($"Wiek (w latach): {(osoba.Wiek?.Days / 365)}");

            break;
        }


        static bool ContainsDigits(string input)
        {
            return Regex.IsMatch(input, @"\d");
        }
    }
}
