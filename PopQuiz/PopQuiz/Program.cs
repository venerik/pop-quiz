using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace PopQuiz
{
    class Program
    {
        static Random _random = new Random();
        static bool _voiceYN;
        static SpeechSynthesizer _synth;
        static Team _team1 = new Team();
        static Team _team2 = new Team();


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

            // Unclear what these are intended to do (for formatting it seems)
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

            bool createTeams = String.Equals(cki.Key.ToString(), "T", StringComparison.InvariantCultureIgnoreCase);
            bool createTeamCaptains = String.Equals(cki.Key.ToString(), "C", StringComparison.InvariantCultureIgnoreCase);

            if(!createTeams && ! createTeamCaptains)
            {
                message = "\nThat is not a valid selection, please enter either a T or a C";
                Broadcast(message);
                goto incorrect;
            }

            var players = Player.GetPlayers();
            if (createTeamCaptains)
            {
                Console.Clear();
                _team1.Captain = players[shuffledIntegers[7]];
                _team2.Captain = players[shuffledIntegers[3]];

                var captains = View.PrintTeams(_team1, _team2);

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

                    var playerTeam1 = players[shuffledIntegers[i - 2]];
                    var playerTeam2 = players[shuffledIntegers[i - 1]];
                    _team1.AddPlayer(playerTeam1);
                    _team2.AddPlayer(playerTeam2);
                }
                message = ("You have chosen to have your teams randomly assigned.\nYour teams are shown below:\n\n" + View.PrintTeams(_team1, _team2));
                Broadcast(message);
            }

            message = "Now that the teams are set, it is time to decide your team names.\n\nTeam 1, what will your team name be?";
            Broadcast(message);

        Start:
            _team1.Name = Console.ReadLine();
            //This is the first if to give a true result initally when a false is expected but does not break the program
            if (string.IsNullOrEmpty(_team1.Name))
            {
                goto Start;
            }

            message = ("\nWelcome to the game " + _team1.Name + "!\n\nTeam 2, now it's your turn.\nPlease enter your team name...");
            Broadcast(message);
        same:
            _team2.Name = Console.ReadLine();
            //This is the second
            if (string.IsNullOrEmpty(_team2.Name))
            {
                goto same;
            }
            Console.Title = "Quiz";

            bool result = String.Equals(_team1.Name, _team2.Name, StringComparison.InvariantCultureIgnoreCase);

            if (result == true)
            {
                message = "\n\nYou can't have the same team names...\nStop being lazy and give me another one!";
                Broadcast(message);
                goto same;
            }

            message = ("\n\nWelcome to the game " + _team2.Name + "!\nTeam names are set... Let's get ready to begin.");
            Broadcast(message);

            int tl = _team1.Name.Length;
            int space2 = (25 - tl);
            string space = null;

            for (int i = 0; i <= space2; i++)
            {
                space = (space + " ");
            }

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
            var tests = Test.GetTests();

            bool IsOdd(int value)
            {
                return value % 2 != 0;
            }

            int t1Score = 0;
            int t2Score = 0;

            for (int i = 0; i < 10; i++)
            {
                Console.Clear();
                var myQuestion = tests[i + 1];

                if (IsOdd(i + 1))
                {
                    teamIntro = GetTeamIntro(shuffledIntegers, _team1.Name, i);
                }
                else
                {
                    teamIntro = GetTeamIntro(shuffledIntegers, _team2.Name, i);
                }

                message = View.PrintTeams(_team1, _team2);
                message += $"{myQuestion.NumText} Question:\n\n{teamIntro}\n\n{myQuestion.Question}";
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

        private static string GetTeamIntro(int[] shuffledIntegers, string teamName, int i)
        {
            string teamIntro;
            if (i >= 8)
            {
                teamIntro = ($"{teamName} time for your final question.");
            }
            else
            {
                switch (shuffledIntegers[i])
                {
                    case 1:
                        teamIntro = ($"{teamName} you're up!");
                        break;
                    case 2:
                        teamIntro = ($"{teamName} it's your turn.");
                        break;
                    case 3:
                        teamIntro = ($"{teamName} this question's yours.");
                        break;
                    case 4:
                        teamIntro = ($"{teamName} let's see if you can get this one.");
                        break;
                    case 5:
                        teamIntro = ($"{teamName}! I've picked this one especially for you...");
                        break;
                    case 6:
                        teamIntro = ($"{teamName} this is a tough one!");
                        break;
                    case 7:
                        teamIntro = ($"{teamName} whenever you're ready, here's your question:");
                        break;
                    default:
                        teamIntro = ($"{teamName} try this one on for size:");
                        break;
                }
            }
            return teamIntro;
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