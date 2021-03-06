﻿using System;
using System.IO;
using System.Collections.Generic;

namespace hangman
{
    class Program
    {
        static void Main(string[] args)
        {
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

            int indexOfLetterToTry = 0; // to iterate over letters
            int currentTry = 1;
            while (currentTry <= maxTryNumber)
            {
                Console.WriteLine($"Try n°{currentTry}");

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
                    letterToTry = chooseLetterToTry(words[indexOfClosestWord], ref index, alreadyTriedLetters);
                }
                else letterToTry = chooseLetterToTry(letters, ref indexOfLetterToTry, alreadyTriedLetters);

                Console.WriteLine($"Chosen letter is {letterToTry}");

                if (letterToTry != (char)0)
                {
                    TryThisLetter(letterToTry, wordToFind, foundLetters);
                    alreadyTriedLetters.Add(letterToTry);
                    currentTry++;
                }

                if (Equals(wordToFind, foundLetters))
                    break;
            }

            if (currentTry >= maxTryNumber)
                Console.WriteLine($"CPU has lost");
            else
                Console.WriteLine($"CPU has founded your word in {currentTry-1} turns !");

            Console.Write("Founded letters are ");
            Console.WriteLine(foundLetters);
        }

        static char chooseLetterToTry(string word, ref int startIndex, List<char> alreadyTriedLetters)
        {
            char letterToTry = (char)0;
            
            do letterToTry = word[startIndex++];
            while (startIndex < word.Length && alreadyTriedLetters.Contains(letterToTry));

            return letterToTry;
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
