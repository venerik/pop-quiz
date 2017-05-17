﻿using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace PopQuiz
{
    class Program
    {
        static Random _random = new Random();
        static bool _voiceYN;
        static SpeechSynthesizer _synth;

        static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        private static void FlushKeyboard()
        {
            while (Console.In.Read() != -1) ;
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.TreatControlCAsInput = false;
            Console.Title = "Quiz";

            int[] shuffledIntegers = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Shuffle(shuffledIntegers);

            string teamName1 = "";
            string teamName2 = "";
            // Unclear what these are intended to do (for formatting it seems)
            string team;
            string captains;
            string players1 = null;
            string message;
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;

            message = "Welcome to the pop quiz!\nWould you like me to continue voicing this quiz?\nPress Y for Yes or any other key to continue without:";
            Console.WriteLine(message);

            _synth = new SpeechSynthesizer();
            _synth.SelectVoice("Microsoft Zira Desktop");
            //synth.Speak(text);

            cki = Console.ReadKey();
            Console.Clear();

            _voiceYN = String.Equals(cki.Key.ToString(), "Y", StringComparison.InvariantCultureIgnoreCase);

            message = "Thank you for your selection, would you like me to assign you a team at random or just pick team captains for you?\nPress T for a random Team or C for team Captains:";
            Broadcast(message);

        incorrect:
            cki = Console.ReadKey();

            bool tResult = String.Equals(cki.Key.ToString(), "T", StringComparison.InvariantCultureIgnoreCase);
            bool cResult = String.Equals(cki.Key.ToString(), "C", StringComparison.InvariantCultureIgnoreCase);

            var thePlayers = Team.GetPlayer();
            {

                if (tResult == false)
                {
                    if (cResult == false)
                    {
                        message = "\nThat is not a valid selection, please enter either a T or a C";
                        Broadcast(message);
                        goto incorrect;
                    }
                    Console.Clear();
                    var myPlayer = thePlayers[shuffledIntegers[7]];
                    captains = myPlayer.Name;
                    myPlayer = thePlayers[shuffledIntegers[3]];

                    team = ("                         " + captains + myPlayer.Name);
                    captains = ($"                         Team 1:                   Team2:\n{team}");

                    message = ($"The selected team captains are:\n{captains}\n\nPress any key to continue when you are ready to proceed");
                    Broadcast(message);

                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    for (int i = 2; i < 9; i += 2)
                    {

                        var myPlayer1 = thePlayers[shuffledIntegers[i - 2]];
                        var myPlayer2 = thePlayers[shuffledIntegers[i - 1]];

                        players1 = (players1 + "                         " + myPlayer1.Name + myPlayer2.Name + "\n");

                    }

                    team = players1;
                    players1 = ("                         Team 1:                   Team2:\n" + players1);


                    message = ("You have chosen to have your teams randomly assigned.\nYour teams are shown below:\n\n" + players1);
                    Broadcast(message);
                }
            }

            message = "Now that the teams are set, it is time to decide your team names.\n\nTeam 1, what will your team name be?";
            Broadcast(message);

        Start:
            teamName1 = Console.ReadLine();
            //This is the first if to give a true result initally when a false is expected but does not break the program
            if (string.IsNullOrEmpty(teamName1))
            {
                goto Start;
            }

            message = ("\nWelcome to the game " + teamName1 + "!\n\nTeam 2, now it's your turn.\nPlease enter your team name...");
            Broadcast(message);
        same:
            teamName2 = Console.ReadLine();
            //This is the second
            if (string.IsNullOrEmpty(teamName2))
            {
                goto same;
            }
            Console.Title = "Quiz";

            bool result = String.Equals(teamName1, teamName2, StringComparison.InvariantCultureIgnoreCase);

            if (result == true)
            {
                message = "\n\nYou can't have the same team names...\nStop being lazy and give me another one!";
                Broadcast(message);
                goto same;
            }

            message = ("\n\nWelcome to the game " + teamName2 + "!\nTeam names are set... Let's get ready to begin.");
            Broadcast(message);

            int tl = teamName1.Length;
            int space2 = (25 - tl);
            string space = null;

            for (int i = 0; i <= space2; i++)
            {
                space = (space + " ");
            }

            team = ($"                         {teamName1}{space}{teamName2}\n{team}\n\n");
            if (_voiceYN == false)
            {
                Console.WriteLine("\n3");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("2");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("1");
                System.Threading.Thread.Sleep(1000);
            }

            Console.Clear();

            string teamIntro;
            var theAnswer = Answers.GetAnswer();

            bool IsOdd(int value)
            {
                return value % 2 != 0;
            }

            int t1Score = 0;
            int t2Score = 0;

            List<string> Profanity = new List<string>();
            Profanity.Add("fuck");
            Profanity.Add("shit");
            Profanity.Add("bollocks");
            Profanity.Add("wank");
            Profanity.Add("cunt");
            Profanity.Add("tosser");
            Profanity.Add("bastard");
            Profanity.Add("fanny");
            Profanity.Add("faggot");
            Profanity.Add("arse");

            for (int i = 0; i < 10; i++)
            {
                Console.Clear();
                var myQuestion = theAnswer[i + 1];

                if (IsOdd(i + 1))
                {
                    if (i == 8)
                    {
                        teamIntro = ($"{teamName1} time for your final question.");
                    }
                    else
                    {
                        switch (shuffledIntegers[i])
                        {
                            case 1:
                                teamIntro = ($"{teamName1} you're up!");
                                break;
                            case 2:
                                teamIntro = ($"{teamName1} it's your turn.");
                                break;
                            case 3:
                                teamIntro = ($"{teamName1} this question's yours.");
                                break;
                            case 4:
                                teamIntro = ($"{teamName1} let's see if you can get this one.");
                                break;
                            case 5:
                                teamIntro = ($"{teamName1}! I've picked this one especially for you...");
                                break;
                            case 6:
                                teamIntro = ($"{teamName1} this is a tough one!");
                                break;
                            case 7:
                                teamIntro = ($"{teamName1} whenever you're ready, here's your question:");
                                break;
                            default:
                                teamIntro = ($"{teamName1} try this one on for size:");
                                break;
                        }
                    }
                }
                else
                {
                    if (i == 9)
                    {
                        teamIntro = ($"{teamName2} time for your final question.");
                    }
                    else
                    {
                        switch (shuffledIntegers[i])
                        {
                            case 1:
                                teamIntro = ($"{teamName2} you're up!");
                                break;
                            case 2:
                                teamIntro = ($"{teamName2} it's your turn.");
                                break;
                            case 3:
                                teamIntro = ($"{teamName2} this question's yours.");
                                break;
                            case 4:
                                teamIntro = ($"{teamName2} let's see if you can get this one.");
                                break;
                            case 5:
                                teamIntro = ($"{teamName2}! I've picked this one especially for you...");
                                break;
                            case 6:
                                teamIntro = ($"{teamName2} this is a tough one!");
                                break;
                            case 7:
                                teamIntro = ($"{teamName2} whenever you're ready, here's your question:");
                                break;
                            default:
                                teamIntro = ($"{teamName2} try this one on for size:");
                                break;
                        }
                    }
                }

                message = ($"{team}{myQuestion.NumText} Question:\n\n{teamIntro}\n\n{myQuestion.Question}");
                Broadcast(message);

            blank:
                string answer = Console.ReadLine();
                //This is the third
                if (string.IsNullOrEmpty(answer))
                {
                    goto blank;
                }

                result = string.Equals(answer, myQuestion.Answer, StringComparison.InvariantCultureIgnoreCase);

                if (result == true)
                {
                    if (IsOdd(i + 1))
                    {
                        t1Score = t1Score + 1;
                    }
                    else
                    {
                        t2Score = t2Score + 1;
                    }
                }
                else
                {
                    if (AnswerContainsProfanity(answer))
                    {
                        Broadcast("\nProfanity is not accepted! 1 point has been deducted from your team's score");
                        if (IsOdd(i))
                        {
                            t1Score = t1Score - 1;
                        }
                        else
                        {
                            t2Score = t2Score - 1;
                        }

                        System.Threading.Thread.Sleep(2000);
                    }
                }
            }
        }

        private static bool AnswerContainsProfanity(string answer)
        {
            List<string> Profanity = new List<string>();
            Profanity.Add("fuck");
            Profanity.Add("shit");
            Profanity.Add("bollocks");
            Profanity.Add("wank");
            Profanity.Add("cunt");
            Profanity.Add("tosser");
            Profanity.Add("bastard");
            Profanity.Add("fanny");
            Profanity.Add("faggot");
            Profanity.Add("arse");

            for (int p = 0; p < Profanity.Count; p++)
            {
                if (answer.ToLower().Contains(Profanity[p]))
                {
                    return true;
                }
            }

            return false;
        }

        private static void Broadcast(string message)
        {
            Console.WriteLine(message);
            if (_voiceYN == true)
            {
                _synth.Speak(message);
            }
        }
    }
}