using System;
using System.IO;
using System.Collections.Generic;

namespace pendu
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            // All possible letters
            const string letters = "AEIOUYRSTLNECDFGHJKMPQVWXZ";
            const int maxTryNumber = 20;

            // Read dictionnary and init word list
            string[] words;
            using (StreamReader stream = new StreamReader("./dicoFR.txt", System.Text.Encoding.UTF8))
                words = stream.ReadToEnd().Split('\n', StringSplitOptions.TrimEntries);

            // Choose a word
            Console.Write("Word to find out: ");
            string wordToFind = Console.ReadLine();

            // Find out the word
            char[] foundLetters = new char[wordToFind.Length];
            foundLetters.Initialize();

            List<char> alreadyTriedLetters = new List<char>();
            List<int> wrongWordIndex = new List<int>();

            int indexOfLetterToTry = 0;
            int currentTry = 0;
            while (currentTry <= maxTryNumber)
            {
                int indexOfClosestWord = -1;
                int maxNbOfSameLetters = 0;

                for (int i=0; i<words.Length; i++)
                {
                    int nbOfSameLetters = 0;
                    string currentWord = words[i];

                    if (!IsWordValid(currentWord, i, wordToFind, foundLetters, alreadyTriedLetters, wrongWordIndex))
                        continue;

                    for (int j=0; j<foundLetters.Length; j++)
                    {
                        // ne need to check if j >= currentWord.Length because we already know that currentWord.Length == wordToFind.Length
                        if (foundLetters[j] == currentWord[j])
                            nbOfSameLetters++;
                    }

                    if (nbOfSameLetters > maxNbOfSameLetters)
                    {
                        maxNbOfSameLetters = nbOfSameLetters;
                        indexOfClosestWord = i;
                    }
                }

                char letterToTry = (char)0;
                if (maxNbOfSameLetters != 0)
                { 
                    Console.WriteLine($"Selected word is {words[indexOfClosestWord]}");

                    int index = 0;
                    do
                        letterToTry = words[indexOfClosestWord][index++];
                    while (alreadyTriedLetters.Contains(letterToTry));

                    Console.WriteLine($"Found letter to try {letterToTry}");
                }
                else
                {
                    do
                        letterToTry = letters[indexOfLetterToTry++];
                    while (alreadyTriedLetters.Contains(letterToTry));                

                    Console.WriteLine($"Random letter is {letterToTry}");
                }

                if (letterToTry != (char)0)
                {
                    TryThisLetter(letterToTry, wordToFind, foundLetters);
                    alreadyTriedLetters.Add(letterToTry);
                    currentTry++;
                }
                
                Console.WriteLine($"It was try n°{currentTry}");

                if (Equals(wordToFind, foundLetters))
                    break;
            }

            if (currentTry > maxTryNumber)
                Console.WriteLine($"CPU has lost");
            else
                Console.WriteLine($"CPU has founded your word in {currentTry} turns !\nIt's {wordToFind}");

            Console.Write("Founded letters : ");
            Console.WriteLine(foundLetters);
        }

        static bool IsWordValid(string word, int index, string wordToFind, char[] foundLetters, List<char> alreadyTriedLetters, List<int> wrongWordIndex)
        {
            if (word.Length != wordToFind.Length)
                return false;
            
            if (wrongWordIndex.Contains(index))
                return false;

            for (int i=0; i<word.Length; i++)
            {
                if (alreadyTriedLetters.Contains(word[i]))
                {
                    if (word[i] != foundLetters[i])
                    {
                        wrongWordIndex.Add(index);
                        return false;
                    }
                }
            }

            return true;
        }

        static bool Equals(string word, char[] letters)
        {
            for (int i=0; i<word.Length; i++)
            {
                if (word[i] != letters[i])
                    return false;
            }

            return true;
        }

        static void TryThisLetter(char letter, string wordToFind, char[] foundLetters)
        {
            for (int i=0; i<wordToFind.Length; i++)
            {
                if (letter == wordToFind[i])
                    foundLetters[i] = letter;
            }
        }
    }
}
