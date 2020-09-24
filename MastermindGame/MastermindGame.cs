using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MastermindGame
{
    public class MastermindGame : IGame
    {
        private const int CodeLength = 4;
        private const int StartNum = 1;
        private const int EndNum = 6;
        private const char RightChar = '+';
        private const char WrongChar = '-';
        private const char Empty = ' ';

        public void Start()
        {
            Console.WriteLine("Mastermind'a hoş geldiniz!");
            Thread.Sleep(1000);
        }

        public void Play()
        {
            while (true)
            {
                var intGuesses = NumGuesses();
                var forecastTime = UserTiming();
                var realCode = RandomizeCode();

                var winState = false;

                Console.Clear();

                for (var i = 1; i <= intGuesses; i++)
                {
                    Console.WriteLine(i + ". Tahminiz - Kalan tahmin sayınız: " + (intGuesses - i) + " - Süreniz: " + forecastTime + " Saniye");
                    Console.Write("\nTahmininiz: ");

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var guessCode = UserCode();
                    stopwatch.Stop();

                    var ts = stopwatch.Elapsed;

                    // Zaman kontrol ediliyor.
                    if (IsTimeExpired(ts, forecastTime)) break;

                    WriteResult(GetCompareString(realCode, guessCode));
                    winState = guessCode.SequenceEqual(realCode);

                    if (!winState) continue;

                    Console.WriteLine("--------------------\n");
                    Console.WriteLine("\nKazandınız! " + i + ". Adımda bildiniz.");
                    break;
                }

                var combineCode = new StringBuilder();
                if (!winState)
                {
                    Console.WriteLine("\nKaybettiniz. :(\n");
                    foreach (var renumber in realCode)
                    {
                        combineCode.Append(renumber);
                    }
                    Console.WriteLine("Gizli sayi: " + combineCode);
                }

                if (IsItEnd())
                    break;

                Console.Clear();
            }
        }

        public void End()
        {
            Console.WriteLine("\nMastermind kapaniyor.\n");
            Thread.Sleep(2000);
        }

        private static bool IsTimeExpired(TimeSpan ts, int forecastTime)
        {
            if (ts.Seconds < forecastTime || ts.Milliseconds <= 0) return false;

            Console.WriteLine("\nGeçen süre: {0:00} Saniye {1:00} Salise", ts.Seconds, ts.Milliseconds / 10);
            return true;

        }

        private static List<int> RandomizeCode()
        {
            var allNums = new List<int>();
            for (var i = StartNum; i <= EndNum; i++)
            {
                allNums.Add(i);
            }
            var random = new Random();
            var shuffledList = allNums.OrderBy(x => random.Next());
            return shuffledList.Take(CodeLength).ToList();
        }

        private static List<int> UserCode()
        {
            var inputNums = new List<int>();
            var foundValidInput = false;
            while (!foundValidInput)
            {
                inputNums.Clear();
                var userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput))
                    foreach (var digit in userInput)
                    {
                        if (!char.IsDigit(digit)) continue;

                        var theNum = int.Parse(digit.ToString());
                        if (theNum >= StartNum && theNum <= EndNum)
                        {
                            inputNums.Add(theNum);
                        }
                    }

                foundValidInput = inputNums.Count == CodeLength;
                if (foundValidInput) continue;

                Console.WriteLine($"Uyarı :Lütfen {CodeLength} basamaklı bir sayı ve {StartNum} ile {EndNum} aralığında bir rakam giriniz.");
                Console.WriteLine();
                Console.Write("Tahmininiz: ");
            }
            return inputNums;
        }

        private static int UserTiming()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Her adımın kaç saniye olacağını giriniz...");
            var stepSecond = 0;
            try
            {
                var entryValues = Console.ReadLine();
                if (!string.IsNullOrEmpty(entryValues))
                    stepSecond = int.Parse(entryValues);
            }
            catch
            {
                Console.WriteLine("\nSüre sayısı için bir tam sayı giriniz.\n");
                Thread.Sleep(2000);
                stepSecond = UserTiming();
            }
            return stepSecond;
        }

        private static string GetCompareString(List<int> realCode, List<int> guessCode)
        {
            var compareString = new StringBuilder();
            for (var i = 0; i < realCode.Count; i++)
            {
                if (realCode[i] == guessCode[i])
                {
                    compareString.Append(RightChar);
                }
                else if (realCode.Contains(guessCode[i]))
                {
                    compareString.Append(WrongChar);
                }
                else
                {
                    compareString.Append(Empty);
                }
            }
            return compareString.ToString();
        }

        private static bool IsItEnd()
        {
            Console.WriteLine("\nTekrar oynamak ister misin? (Y/N)\n");
            while (true)
            {
                var strPlayAgain = Console.ReadLine();
                switch (strPlayAgain)
                {
                    case "N":
                    case "n":
                    case "No":
                    case "no":
                        return true;
                    case "Y":
                    case "y":
                    case "Yes":
                    case "yes":
                        return false;
                    default:
                        Console.WriteLine("\nLütfen bir Y veya bir N girin.\n");
                        break;
                }
            }
        }

        private static void WriteResult(string result)
        {
            Console.WriteLine();
            Console.WriteLine("Gizli Sayı");

            for (var i = 0; i < CodeLength; i++)
            {
                Console.Write("x");
            }
            Console.WriteLine();
            Console.WriteLine(result);
        }

        private static int NumGuesses()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Kaç tane tahminde bulunmak istersiniz ?\n");
            var intGuesses = 0;
            try
            {
                var entryValues = Console.ReadLine();
                if (!string.IsNullOrEmpty(entryValues))
                    intGuesses = int.Parse(entryValues);
            }
            catch
            {
                Console.WriteLine("\nTahmin sayısı için bir tam sayı giriniz.\n");
                Thread.Sleep(2000);
                intGuesses = NumGuesses();
            }
            return intGuesses;
        }
    }
}