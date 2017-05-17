using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace PopQuiz
{
    public class GameController
    {
        Random _random = new Random();
        bool _voiceYN;
        SpeechSynthesizer _synth;
        Team _team1 = new Team();
        Team _team2 = new Team();
        int[] _shuffledIntegers;

        public GameController()
        {
            _shuffledIntegers = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            Shuffle(_shuffledIntegers);
        }

        void Shuffle<T>(T[] array)
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

        public void Start()
        {
            AdjustConsoleSettings();
            AskForVoiceSupport();
            var teamType = AskForTypeOfTeams();
            CreateTeams(teamType);

            Broadcast("Now that the teams are set, it is time to decide your team names.\n\nTeam 1, what will your team name be?");

            AskForTeamName(_team1);
            Broadcast("\nWelcome to the game " + _team1.Name + "!\n\nTeam 2, now it's your turn.\nPlease enter your team name...");

            AskForTeamName(_team2, _team1.Name);
            Broadcast("\n\nWelcome to the game " + _team2.Name + "!\nTeam names are set... Let's get ready to begin.");

            ShowCountDown();

            var game = new Game(_team1, _team2);

            foreach(var test in game.Tests)
            {
                Console.Clear();

                var teamIntro = GetTeamIntro(_shuffledIntegers, game.CurrentTeam.Name, test.Key);

                Broadcast(View.PrintTeams(_team1, _team2));
                Broadcast($"{test.Value.NumText} Question:\n\n{teamIntro}\n\n{test.Value.Question}");

                var answer = AskForAnswer();
                if (string.Equals(answer, test.Value.Answer, StringComparison.InvariantCultureIgnoreCase))
                {
                    game.CurrentTeam.IncreaseScore(1);
                }

                if (AnswerContainsProfanity(answer))
                {
                    Broadcast("\nProfanity is not accepted! 1 point has been deducted from your team's score");
                    game.CurrentTeam.DecreaseScore(1);
                    System.Threading.Thread.Sleep(2000);
                }

                game.NextTeam();
            }
        }

        private void ShowCountDown()
        {
            if (_voiceYN == false)
            {
                Console.WriteLine("\n3");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("2");
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("1");
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void CreateTeams(TeamType teamType)
        {
            var players = Player.GetPlayers();
            if (teamType == TeamType.Captains)
            {
                Console.Clear();
                _team1.Captain = players[_shuffledIntegers[7]];
                _team2.Captain = players[_shuffledIntegers[3]];

                var captains = View.PrintTeams(_team1, _team2);

                Broadcast($"The selected team captains are:\n{captains}\n\nPress any key to continue when you are ready to proceed");

                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.Clear();
                for (int i = 2; i < 9; i += 2)
                {

                    var playerTeam1 = players[_shuffledIntegers[i - 2]];
                    var playerTeam2 = players[_shuffledIntegers[i - 1]];
                    _team1.AddPlayer(playerTeam1);
                    _team2.AddPlayer(playerTeam2);
                }
                Broadcast("You have chosen to have your teams randomly assigned.\nYour teams are shown below:\n\n" + View.PrintTeams(_team1, _team2));
            }
        }

        private string AskForAnswer()
        {
        blank:
            string answer = Console.ReadLine();
            //This is the third
            if (string.IsNullOrEmpty(answer))
            {
                goto blank;
            }
            return answer;
        }

        private TeamType AskForTypeOfTeams()
        {
            Broadcast("Would you like me to assign you a team at random or just pick team captains for you?\nPress T for a random Team or C for team Captains:");
        incorrect:
            var cki = Console.ReadKey();

            bool createTeams = String.Equals(cki.Key.ToString(), "T", StringComparison.InvariantCultureIgnoreCase);
            bool createTeamCaptains = String.Equals(cki.Key.ToString(), "C", StringComparison.InvariantCultureIgnoreCase);

            if (!createTeams && !createTeamCaptains)
            {
                Broadcast("\nThat is not a valid selection, please enter either a T or a C");
                goto incorrect;
            }

            return createTeams ? TeamType.Players : TeamType.Captains;
        }

        private void AskForTeamName(Team team)
        {

        Start:
            team.Name = Console.ReadLine();
            //This is the first if to give a true result initally when a false is expected but does not break the program
            if (string.IsNullOrEmpty(team.Name))
            {
                goto Start;
            }


        }

        private void AskForTeamName(Team team, string otherTeamsName)
        {
        same:
            team.Name = Console.ReadLine();
            //This is the second
            if (string.IsNullOrEmpty(team.Name))
            {
                goto same;
            }

            bool result = String.Equals(team.Name, otherTeamsName, StringComparison.InvariantCultureIgnoreCase);

            if (result == true)
            {
                Broadcast("\n\nYou can't have the same team names...\nStop being lazy and give me another one!");
                goto same;
            }
        }

        private void AskForVoiceSupport()
        {
            var message = "Welcome to the pop quiz!\nWould you like me to continue voicing this quiz?\nPress Y for Yes or any other key to continue without:";
            Console.WriteLine(message);

            _synth = new SpeechSynthesizer();
            _synth.SelectVoice("Microsoft Zira Desktop");

            ConsoleKeyInfo cki = Console.ReadKey();
            Console.Clear();

            _voiceYN = String.Equals(cki.Key.ToString(), "Y", StringComparison.InvariantCultureIgnoreCase);

            message = "Thank you for your selection.";
            Broadcast(message);
        }

        private static void AdjustConsoleSettings()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.TreatControlCAsInput = false;
            Console.Title = "Quiz";
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

        private void Broadcast(string message)
        {
            Console.WriteLine(message);
            if (_voiceYN == true)
            {
                _synth.Speak(message);
            }
        }
    }
}