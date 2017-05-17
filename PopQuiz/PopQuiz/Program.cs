using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace PopQuiz
{
    class Program
    {
        static Random _random = new Random();

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

            int[] array = { 1, 2, 3, 4, 5, 6, 7, 8 };
            Shuffle(array);

            //int space1 = 25;
            string Team1 = "";
            string Team2 = "";
            string team;
            string captains;
            string players1 = null;
            string text;
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;

            text = "Welcome to the pop quiz!\nWould you like me to continue voicing this quiz?\nPress Y for Yes or any other key to continue without:";
            Console.WriteLine(text);
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoice("Microsoft Zira Desktop");
            //synth.Speak(text);

            cki = Console.ReadKey();
            Console.Clear();

            bool voiceYN = String.Equals(cki.Key.ToString(), "Y", StringComparison.InvariantCultureIgnoreCase);

            text = "Thank you for your selection, would you like me to assign you a team at random or just pick team captains for you?\nPress T for a random Team or C for team Captains:";
            Console.WriteLine(text);
            if (voiceYN == true)
            {
                synth.Speak(text);
            }

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
                        text = "\nThat is not a valid selection, please enter either a T or a C";
                        Console.WriteLine(text);
                        if (voiceYN == true)
                        {
                            synth.Speak(text);
                        }
                        goto incorrect;
                    }
                    Console.Clear();
                    var myPlayer = thePlayers[array[7]];
                    captains = myPlayer.Name;
                    myPlayer = thePlayers[array[3]];

                    team = ("                         " + captains + myPlayer.Name);
                    captains = ($"                         Team 1:                   Team2:\n{team}");

                    text = ($"The selected team captains are:\n{captains}\n\nPress any key to continue when you are ready to proceed");
                    Console.WriteLine(text);
                    if (voiceYN == true)
                    {
                        synth.Speak(text);
                    }
                    Console.ReadKey();
                    Console.Clear();

                }
                else
                {
                    Console.Clear();
                    for (int i = 2; i < 9; i += 2)
                    {

                        var myPlayer1 = thePlayers[array[i - 2]];
                        var myPlayer2 = thePlayers[array[i - 1]];

                        players1 = (players1 + "                         " + myPlayer1.Name + myPlayer2.Name + "\n");

                    }

                    team = players1;
                    players1 = ("                         Team 1:                   Team2:\n" + players1);


                    text = ("You have chosen to have your teams randomly assigned.\nYour teams are shown below:");

                    Console.WriteLine("{0}\n\n{1}", text, players1);
                    if (voiceYN == true)
                    {
                        synth.Speak(text);
                    }

                }
            }

            text = "Now that the teams are set, it is time to decide your team names.\n\nTeam 1, what will your team name be?";
            Console.WriteLine(text);
            if (voiceYN == true)
            {
                synth.Speak(text);
            }

        Start:
            Team1 = Console.ReadLine();
            //This is the first if to give a true result initally when a false is expected but does not break the program
            if (string.IsNullOrEmpty(Team1))
            {
                goto Start;
            }

            text = ("\nWelcome to the game " + Team1 + "!\n\nTeam 2, now it's your turn.\nPlease enter your team name...");
            Console.WriteLine(text);
            if (voiceYN == true)
            {
                synth.Speak(text);
            }
        same:
            Team2 = Console.ReadLine();
            //This is the second
            if (string.IsNullOrEmpty(Team2))
            {
                goto same;
            }
            Console.Title = "Quiz";

            bool result = String.Equals(Team1, Team2, StringComparison.InvariantCultureIgnoreCase);

            if (result == true)
            {
                text = "\n\nYou can't have the same team names...\nStop being lazy and give me another one!";
                Console.WriteLine(text);
                if (voiceYN == true)
                {
                    synth.Speak(text);
                }
                goto same;
            }

            text = ("\n\nWelcome to the game " + Team2 + "!\nTeam names are set... Let's get ready to begin.");
            Console.WriteLine(text);
            if (voiceYN == true)
            {
                synth.Speak(text);
            }

            int tl = Team1.Length;
            int space2 = (25 - tl);
            string space = null;

            for (int i = 0; i <= space2; i++)
            {
                space = (space + " ");
            }

            team = ($"                         {Team1}{space}{Team2}\n{team}\n\n");
            if (voiceYN == false)
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
                        teamIntro = ($"{Team1} time for your final question.");
                    }
                    else
                    {
                        switch (array[i])
                        {
                            case 1:
                                teamIntro = ($"{Team1} you're up!");
                                break;
                            case 2:
                                teamIntro = ($"{Team1} it's your turn.");
                                break;
                            case 3:
                                teamIntro = ($"{Team1} this question's yours.");
                                break;
                            case 4:
                                teamIntro = ($"{Team1} let's see if you can get this one.");
                                break;
                            case 5:
                                teamIntro = ($"{Team1}! I've picked this one especially for you...");
                                break;
                            case 6:
                                teamIntro = ($"{Team1} this is a tough one!");
                                break;
                            case 7:
                                teamIntro = ($"{Team1} whenever you're ready, here's your question:");
                                break;
                            default:
                                teamIntro = ($"{Team1} try this one on for size:");
                                break;
                        }
                    }
                }
                else
                {
                    if (i == 9)
                    {
                        teamIntro = ($"{Team2} time for your final question.");
                    }
                    else
                    {
                        switch (array[i])
                        {
                            case 1:
                                teamIntro = ($"{Team2} you're up!");
                                break;
                            case 2:
                                teamIntro = ($"{Team2} it's your turn.");
                                break;
                            case 3:
                                teamIntro = ($"{Team2} this question's yours.");
                                break;
                            case 4:
                                teamIntro = ($"{Team2} let's see if you can get this one.");
                                break;
                            case 5:
                                teamIntro = ($"{Team2}! I've picked this one especially for you...");
                                break;
                            case 6:
                                teamIntro = ($"{Team2} this is a tough one!");
                                break;
                            case 7:
                                teamIntro = ($"{Team2} whenever you're ready, here's your question:");
                                break;
                            default:
                                teamIntro = ($"{Team2} try this one on for size:");
                                break;
                        }
                    }
                }

                text = ($"{team}{myQuestion.NumText} Question:\n\n{teamIntro}\n\n{myQuestion.Question}");
                Console.WriteLine(text);

                if (voiceYN == true)
                {
                    synth.Speak(text);
                }

            blank:
                string myAnswer = Console.ReadLine();
                //This is the third
                if (string.IsNullOrEmpty(myAnswer))
                {
                    goto blank;
                }

                result = string.Equals(myAnswer, myQuestion.Answer, StringComparison.InvariantCultureIgnoreCase);

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
                    for (int p = 0; p < Profanity.Count; p++)
                    {
                        if (myAnswer.ToLower().Contains(Profanity[p]))
                        {
                            result = true;
                            break;
                        }
                    }

                    if (result == true)
                    {
                        Console.WriteLine("\nProfanity is not accepted! 1 point has been deducted from your team's score");
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
    }
}