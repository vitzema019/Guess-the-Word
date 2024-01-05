# Guess the Word Game

## Author
Vít Zeman

## Date
2024-01-5 (yyyy-mm-dd)

## Version
1.0.0

## Description
Guess the Word is a game where players guess words related to information technology. The game is written in C# and developed in Visual Studio Code.

## Changelog
- **2024-01-5**: Initial release of the program

## Usage
Feel free to modify the following variables if needed:
```csharp
const int defaultHealth = 12;  // default health value (used when resetting the game)
const int textSpeed = 25;  // text display speed (used when displaying text)
const int borderTextSpeed = 5;  // border display speed (used when displaying text)
string[] words = new string[]  // array of strings containing words to be guessed
{
    // ... (list of words related to information technology)
};
```
## Instructions
1. Press **ENTER** to start the game.
2. You have a limited number of lives (initially set to `defaultHealth`).
3. Guess the word based on the displayed underscores representing each letter.
4. Input a letter when prompted. Incorrect guesses will decrease your health.
5. Successfully guessing a letter will reveal it in the word.
6. Win the game by guessing the entire word or lose by running out of lives.

**Have fun playing Guess the Word!**



