using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions; //importování knihovny Regex

//------------------------------------------------------------------------------------------------------------------------------------------------------
// Game: Guess the Word
// Autor: Vít Zeman
// Datum: 2024-01-5 (rrrr-mm-dd)
// Verze: 1.0.0
// Popis: Hra, ve které hráč hádá slova z oblasti informačních technologií
// Programovací jazyk: C#
// Programovací prostředí: Visual Studio Code

// Changelog:
// (rrrr-mm-dd) - (popis změny)
// 2024-01-5 - vytvoření první verze programu programu

//------------------------------------------------------------------------------------------------------------------------------------------------------

// Podel potřeby lze tyto proměnné změnit
//------------------------------------------------------------------------------------------------------------------------------------------------------
const int defaultHealth = 12; //defaultní hodnota životů (používa se při resetování hry)
const int textSpeed = 25; //rychlost výpisu textu (používa se při výpisu textu)
const int borderTextSpeed = 5; //rychlost zobrazení ohraničení (používa se při výpisu textu)
string[] words = new string[] //pole stringů, které obsahuje všechna slova, která mohou být hádána
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
//------------------------------------------------------------------------------------------------------------------------------------------------------


int health = defaultHealth; //počet životů
var validCharacters = new Regex("^[a-z]$"); //nastavení validních znaků, které mohou být zadány
bool win = false; //proměnná, která určuje, zda-li hráč vyhrál hru
List<string> guessedLetters = new List<string>(); //list stringů, který obsahuje všechny již hádané znaky

ConsoleSlowWriteLine("-----------------------------------------------------", borderTextSpeed);
ConsoleSlowWriteLine("Vítejte v hře Guess the Word.");
ConsoleSlowWriteLine("Cílem této hry je odhadnout slovo na základě jeho délky, které je znázorněno podtržítky.");
ConsoleSlowWriteLine($"Máte k dispozici {health} životů, přičemž každý nesprávný pokus vás stojí jeden život.");
ConsoleSlowWriteLine("Všechna slova, která budete hádat, jsou z oblasti informačních technologií a jsou v anglickém jazyce.");
ConsoleSlowWriteLine("Přeji vám hodně štěstí!");
ConsoleSlowWriteLine("-----------------------------------------------------", borderTextSpeed);
ConsoleSlowWriteLine("Pro zahájení stiskněte klávesu ENTER.");
var keyInf = Console.ReadKey(); //přečtení klávesy
if (keyInf.Key == ConsoleKey.Escape) //pokud je stisknuta klávesa escape, hra se ukončí (přidáno z důvodu zláštního chování programu při stisknutí klávesy escape (loop))
{
    Environment.Exit(0); //ukončení programu
}
Console.Clear();
bool colorChange = false;

while (true) //cyklus, který se opakuje dokud hráč neukončí hru
{
    colorChange = false;
    string selectedWord = SelectWord(words); //slovo, které je aktuálně hádáno
    string[] secret = Secret(selectedWord); //array stringů naplněný podtržítky, které budou reprezentovat neodhalené znaky slova (postupně se budou nahrazovat odhalenými znaky)
    string[] result = Split(selectedWord); //array stringů naplněný znak kompletně odhaleného slova
    bool firstRun = true; //proměnná, která určuje, zda-li je první spuštění hry
    while (health != 0 || health > 0) //cyklus, který se opakuje dokud má hráč životy
    {
        if (firstRun)
        {
            //pouze pro vzhled
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(string.Empty); //vypsání prázdného řádku
            }
            firstRun = false;
        }
        Console.WriteLine("-----------------------------------------------------");
        Console.Write($"Počet životů: {health}\n");
        Console.Write("Aktuální slovo k odhadnutí: ");
        foreach (string item in secret) //vypsání aktuálního stavu hádaného slova
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine("\n-----------------------------------------------------");
        Console.WriteLine(string.Empty);
        Console.Write("Zadejte písmeno: ");
        var keyInfo = Console.ReadKey(); //přečtení klávesy
        if (keyInfo.Key == ConsoleKey.Escape) //pokud je stisknuta klávesa escape, hra se ukončí (přidáno z důvodu zláštního chování programu při stisknutí klávesy escape (loop))
        {
            Environment.Exit(0); //ukončení programu
        }
        var key = keyInfo.Key.ToString().ToLower(); //převedení klávesy na string a převedení na malá písmena
        Console.WriteLine(string.Empty);
        Console.Clear();
        if (!validCharacters.IsMatch(key)) //pokud je klávesa nevalidní, program se vrátí na začátek cyklu
        {
            Warning("Prosím, zadávejte pouze písmena abecedy.");
            continue; //pokračování na začátek cyklu
        }
        else if (guessedLetters.Contains(key)) //pokud je klávesa již hádaná, program se vrátí na začátek cyklu
        {
            Warning($"Již jste zkoušeli písmeno |{key}|.");
            continue; //pokračování na začátek cyklu
        }
        else
        {
            guessedLetters.Add(key); //přidání hádaného písmena do listu
        }
        if (result.Contains(key)) //pokud je hádané písmeno v hádaném slově, program pokračuje dále
        {
            Success($"Písmeno |{key}| se v tomto slově nachází. Gratuluji");
            secret = Replace(result, key, secret); //nahrazení podtržítek odhalenými písmeny
            win = CheckWin(secret); //kontrola, zda-li je hádané slovo kompletně odhaleno
            if (win) //pokud je hádané slovo kompletně odhaleno, program ukončí cyklus
            {
                break;
            }
        }
        else //pokud není hádané písmeno v hádaném slově, program sníží počet životů o 1
        {
            Warning($"Písmeno |{key}| se v tomto slově nenachází. Bohužel, byl vám odečten jeden život.");
            health--;
        }
    }

    if (win) //pokud je proměnná win true, hráč vyhrál hru
    {
        if (EndScreen("Gratuluji k vítězství ve hře! Úspěšně jste uhádl/a slovo.", selectedWord, win))
        {
            Console.Clear();
            continue;
        }
        else
        {
            break;
        }
    }
    else
    {
        //pokud je proměnná win false, hráč prohrál hru
        if (EndScreen("Je mi líto, že jste prohrál/a. Nezoufejte, klíčová je zábava a příležitost se něco nového naučit. :)", selectedWord, win))
        {
            Console.Clear();
            continue;
        }
        else
        {
            break;
        }
    }
}

//Funkce a procedury
//------------------------------------------------------------------------------------------------------------------------------------------------------

// EndScreen() - vypíše zprávu, která je předána jako parametr, a nabídne hráči možnost hrát znovu
bool EndScreen(string message, string selectedWord, bool win)
{
    string key = "";
    do //cyklus, který se opakuje dokud hráč nezadá validní klávesu
    {
        Console.Clear(); //vyčištění konzole
        if (win) //pokud je proměnná win true, hráč vyhrál hru
        {
            Success(message); //vypsání zprávy
        }
        else
        {
            Warning(message); //vypsání zprávy
        }
        Console.WriteLine($"Hádané slovo: {selectedWord}");
        ConsoleSlowWriteLine("Chcete hrát znovu? (y/n)", 0);
        var keyInfo = Console.ReadKey(); //přečtení klávesy
        if (keyInfo.Key == ConsoleKey.Escape) //pokud je stisknuta klávesa escape, hra se ukončí (přidáno z důvodu zláštního chování programu při stisknutí klávesy escape (loop))
        {
            Environment.Exit(0); //ukončení programu
        }
        key = keyInfo.Key.ToString().ToLower(); //přečtení klávesy a převedení na malá písmena a převedení na string
    } while (key != "y" && key != "n");


    if (key == "y")
    {
        Console.WriteLine("\n");
        ResetValues(); //resetování hodnot
        //continue;
        return true;
    }
    else
    {
        //break;
        return false;
    }
}

/*void ConsoleSlowWrite(string text, int delay = textSpeed) //funkce, která vypíše text po znacích s určitým zpožděním
{
    foreach (char c in text)
    {
        Console.Write(c);
        System.Threading.Thread.Sleep(delay);
    }
}*/

void ConsoleSlowWriteLine(string text, int delay = textSpeed) //funkce, která vypíše text po znacích s určitým zpožděním
{
    text += " \n";
    foreach (char c in text)
    {
        Console.Write(c);
        System.Threading.Thread.Sleep(delay);
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
    Random rnd = new Random(); //vytvoření instance třídy Random
    int index = rnd.Next(0, words.Length); //vytvoření náhodného čísla
    return words[index]; //vrácení náhodného slova
}

// ResetValues() - resetuje hodnoty proměnných
void ResetValues()
{
    win = false; //resetování proměnné win
    health = defaultHealth; //resetování počtu životů
    guessedLetters.Clear(); //vyčištění listu
}

// Warning() - vypíše text v červené barvě
void Warning(string text)
{
    Console.WriteLine(string.Empty);
    if (colorChange == false) //pokud je colorChange false, vypíše text v červené barvě, pokud je colorChange true, vypíše text v tmavě červené barvě
    {
        Console.ForegroundColor = ConsoleColor.Red;
        colorChange = true; //změní hodnotu proměnné colorChange na true
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        colorChange = false; //změní hodnotu proměnné colorChange na false
    }
    Console.WriteLine(text);
    Console.ResetColor();
    Console.WriteLine(string.Empty);
}

// Success() - vypíše text v zelené barvě
void Success(string text)
{
    Console.WriteLine(string.Empty);
    if (colorChange == false)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        colorChange = true; //změní hodnotu proměnné colorChange na true
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        colorChange = false; //změní hodnotu proměnné colorChange na false
    }
    Console.WriteLine(text);
    Console.ResetColor();
    Console.WriteLine(string.Empty);
}
//------------------------------------------------------------------------------------------------------------------------------------------------------
