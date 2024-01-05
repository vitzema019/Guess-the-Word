using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
const int defaultHelth = 12; //defaultní hodnota životů (používa se při resetování hry)
int helth = defaultHelth; //počet životů
string[] words = new string[]
{
    "programming", "database", "software", "developer", "algorithm", "coding", "debugging", "frontend", "backend", "server",
    "network", "web", "security", "encryption", "authentication", "framework", "user", "interface", "function",
    "method", "variable", "object", "class", "API", "library", "code", "repository", "git", "branch",
    "merge", "pull", "push", "commit", "bug", "feature", "release", "deployment", "version", "control", 
    "testing", "unit", "integration", "automation", "scripting", "codebase",
    "debugger", "console", "terminal", "server", "client", "protocol", "HTTP", "HTTPS", "CSS", "HTML",
    "JavaScript", "Python", "Java", "SQL", "MongoDB", "MySQL", "Firebase", "cloud", "container",
    "virtualization", "API", "REST", "JSON", "XML", "frontend", "backend", "fullstack",
    "responsive", "design", "mobile", "application", "framework", "responsive", "deployment",
    "pipeline", "automation", "scripting", "script", "shell", "PowerShell", "lambda"
};



var validCharacters = new Regex("^[a-z]$"); //nastavení validních znaků, které mohou být zadány
bool win = false; //proměnná, která určuje, zda-li hráč vyhrál hru
List<string> guessedLetters = new List<string>(); //list stringů, který obsahuje všechny již hádané znaky
while (true)
{
    Console.WriteLine("Vítejte ve hře Hádej slovo");
    Console.WriteLine("Vaším cílem je zijsit slovo podle jeho délky, které je vypsáno podtržítky");
    Console.WriteLine($"Máte {defaultHelth} životů, za každé špatné hádání se vám počet životů sníží o 1");
    Console.WriteLine("Všechna slova jsou z terminologie IT oblasti a jsou v anglickém jazyce");
    Console.WriteLine("Hodně štěstí");
    Console.WriteLine("Hra začíná");
    Console.WriteLine(string.Empty);
    string selectedWord = SelectWord(words); //slovo, které je aktuálně hádáno
    string[] secret = Secret(selectedWord); //array stringů naplněný podtržítky, které budou reprezentovat neodhalené znaky slova (postupně se budou nahrazovat odhalenými znaky)
    string[] result = Split(selectedWord); //array stringů naplněný znak kompletně odhaleného slova
    while (helth != 0 || helth > 0) //cyklus, který se opakuje dokud má hráč životy
    {
        Console.WriteLine("---------------------------------");
        Console.Write($"Počet životů: {helth}\n");
        Console.Write("Hádané slovo: ");
        foreach (string item in secret) //vypsání aktuálního stavu hádaného slova
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine("\n---------------------------------");
        Console.WriteLine(string.Empty);
        Console.Write("Zadejte písmeno: ");
        var keyInfo = Console.ReadKey(); //přečtení klávesy
        if(keyInfo.Key == ConsoleKey.Escape) //pokud je stisknuta klávesa escape, hra se ukončí (přidáno z důvodu zláštního chování programu při stisknutí klávesy escape (loop))
        {
            Environment.Exit(0); //ukončení programu
        }
        var key =keyInfo.Key.ToString().ToLower(); //převedení klávesy na string a převedení na malá písmena
        Console.WriteLine(string.Empty);
        if (!validCharacters.IsMatch(key)) //pokud je klávesa nevalidní, program se vrátí na začátek cyklu
        {
            Warning("Zdávejte pouze písmena abecedy");
            continue; //pokračování na začátek cyklu
        }
        else if (guessedLetters.Contains(key)) //pokud je klávesa již hádaná, program se vrátí na začátek cyklu
        {
            Warning("Toto písmeno jste již zkusili");
            continue; //pokračování na začátek cyklu
        }
        else
        {
            guessedLetters.Add(key); //přidání hádaného písmena do listu
        }
        if (result.Contains(key)) //pokud je hádané písmeno v hádaném slově, program pokračuje dále
        {
            Success("Toto písmeno se v tomto slově nachází !!!");
            secret = Replace(result, key, secret); //nahrazení podtržítek odhalenými písmeny
            win = CheckWin(secret); //kontrola, zda-li je hádané slovo kompletně odhaleno
            if (win) //pokud je hádané slovo kompletně odhaleno, program ukončí cyklus
            {
                break;
            }
        }
        else //pokud není hádané písmeno v hádaném slově, program sníží počet životů o 1
        {
            Warning("Toto písmeno se v tomto slově nenachází, byl vám ubrán jeden život !!!");
            helth--;
        }
    }

    if (win) //pokud je proměnná win true, hráč vyhrál hru
    {
        Success("Gratuluji, vyhrál jste");
        Console.WriteLine($"Hádané slovo: {selectedWord}");
        Console.WriteLine("Chcete hrát znovu? (y/n)");
        var key = Console.ReadKey().Key.ToString().ToLower(); //přečtení klávesy a převedení na malá písmena a převedení na string
        if (key == "y")
        {
            ResetValues(); //resetování hodnot
            continue;
        }
        else
        {
            break;
        }
    }
    else
    {
        Warning("Bohužel jste prohrál");
        Console.WriteLine($"Hádané slovo: {selectedWord}");
        Console.WriteLine("Chcete hrát znovu? (y/n)");
        var key = Console.ReadKey().Key.ToString().ToLower(); //přečtení klávesy a převedení na malá písmena a převedení na string
        if (key == "y")
        {
            Console.WriteLine("\n");
            ResetValues(); //resetování hodnot
            continue;
        }
        else
        {
            break;
        }
    }
}


// CheckWin() - vrací true, pokud je pole stringů, které je předáno jako parametr, kompletně odhaleno (neobsahuje podtržítka)
static bool CheckWin(string[] arr)
{
    if (!arr.Contains("_")) //pokud pole obsahuje podtržítka, vrátí false
    {
        return true;
    }
    else
    {
        return false;
    }
}

// Replace() - vrací pole stringů, které obsahuje stejný počet prvků jako je počet znaků v textu, který je uložen v poli words.
// Každý prvek tohoto pole obsahuje jeden znak z textu. Pokud je znak shodný s parametrem key, je na stejné pozici v poli result nahrazen znakem key.
static string[] Replace(string[] arr, string key, string[] result)
{
    for (int i = 0; i < arr.Length; i++) //procházení pole
    {
        if (arr[i] == key) //pokud je znak shodný s parametrem key, je na stejné pozici v poli result nahrazen znakem key
        {
            result[i] = key; //nahrazení znaku
        }
    }
    return result;
}

// Secret() - vrací pole stringů, které obsahuje stejný počet prvků jako je počet znaků v textu, který je uložen v poli words. Každý prvek tohoto pole obsahuje znak podtržítka.
static string[] Secret(string word)
{ 
    string[] textArr = new string[word.Length]; //vytvoření pole stringů o délce slova
    for (int i = 0; i < word.Length; i++) //procházení pole
    {
        textArr[i] = "_"; //přidání podtržítka do pole
    }
    return textArr;
}

// Split() - vrací pole stringů, které obsahuje stejný počet prvků jako je počet znaků v textu, který je uložen v poli words. Každý prvek tohoto pole obsahuje jeden znak z textu.
static string[] Split(string text)
{
    string[] textArr = new string[text.Length]; //vytvoření pole stringů o délce slova
    for (int i = 0; i < text.Length; i++)
    {
        textArr[i] = text[i].ToString().ToLower(); //přidání znaku do pole a převedení na malá písmena
    }
    return textArr;
}

// SelectWord() - vrací náhodné slovo z pole words
static string SelectWord(string[] words)
{
    Random rnd = new Random();
    int index = rnd.Next(0, words.Length);
    return words[index];
}

// ResetValues() - resetuje hodnoty proměnných
void ResetValues()
{
    win = false;
    helth = defaultHelth;
    guessedLetters.Clear();
}

// Warning() - vypíše text v červené barvě
void Warning(string text)
{
    Console.WriteLine(string.Empty);
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(text);
    Console.ResetColor();
    Console.WriteLine(string.Empty);
}

// Success() - vypíše text v zelené barvě
void Success(string text)
{
    Console.WriteLine(string.Empty);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(text);
    Console.ResetColor();
    Console.WriteLine(string.Empty);
}
