using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;

namespace PopQuiz
{
    public class GameController
    {
        private Random _random = new Random();
        private Game _game;
        private bool _voiceYN;
        private SpeechSynthesizer _synth;
        private Queue<string> _teamIntros;

        public GameController()
        {
            _game = new Game(new Team(), new Team());
        }

        public void Start()
        {
            SetupGame();
            PlayGame();
        }

        private SpeechSynthesizer Synthesizer
        {
            get
            {
                if (_synth == null)
                {
                    _synth = new SpeechSynthesizer();
                    _synth.SelectVoice("Microsoft Zira Desktop");
                }
                return _synth;
            }
        }

        private void SetupGame()
        {
            AdjustConsoleSettings();
            AskForVoiceSupport();
            var teamType = AskForTypeOfTeams();
            CreateTeams(teamType);

            Broadcast("Now that the teams are set, it is time to decide your team names.\n\nTeam 1, what will your team name be?");

            AskForTeamName(_game.Team1);
            Broadcast("\nWelcome to the game " + _game.Team1.Name + "!\n\nTeam 2, now it's your turn.\nPlease enter your team name...");

            AskForTeamName(_game.Team2, _game.Team1.Name);
            Broadcast("\n\nWelcome to the game " + _game.Team2.Name + "!\nTeam names are set... Let's get ready to begin.");

            ShowCountDown();
        }

        private void PlayGame()
        {
            foreach (var test in _game.Tests)
            {
                Console.Clear();

                var teamIntro = test.IsFinal ? GetTeamIntroFinalQuestion(_game.CurrentTeam.Name) :  GetTeamIntro(_game.CurrentTeam.Name);

                Broadcast(View.PrintTeams(_game.Team1, _game.Team2));
                Broadcast($"{test.NumText} Question:\n\n{teamIntro}\n\n{test.Question}");

                var answer = AskForAnswer();
                if (string.Equals(answer, test.Answer, StringComparison.InvariantCultureIgnoreCase))
                {
                    _game.CurrentTeam.IncreaseScore(1);
                }

                if (AnswerContainsProfanity(answer))
                {
                    Broadcast("\nProfanity is not accepted! 1 point has been deducted from your team's score");
                    _game.CurrentTeam.DecreaseScore(1);
                    System.Threading.Thread.Sleep(2000);
                }

                _game.NextTeam();
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
            if (teamType == TeamType.Captains)
            {
                Console.Clear();
                _game.Team1.Captain = _game.Players[0];
                _game.Team2.Captain = _game.Players[1];

                var captains = View.PrintTeams(_game.Team1, _game.Team2);

                Broadcast($"The selected team captains are:\n{captains}\n\nPress any key to continue when you are ready to proceed");

                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.Clear();
                var team = _game.Team1;
                foreach(var player in _game.Players)
                {
                    team.AddPlayer(player);
                    team = team == _game.Team1 ? _game.Team2 : _game.Team1;
                }
                Broadcast("You have chosen to have your teams randomly assigned.\nYour teams are shown below:\n\n" + View.PrintTeams(_game.Team1, _game.Team2));
            }
        }

        private string AskForAnswer()
        {
            string answer;

            do
            {
                answer = Console.ReadLine();
            } while (string.IsNullOrEmpty(answer));

            return answer;
        }

        private TeamType AskForTypeOfTeams()
        {
            Broadcast("Would you like me to assign you a team at random or just pick team captains for you?\nPress T for a random Team or C for team Captains:");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey();
                var validKeys = new ConsoleKey[] { ConsoleKey.T, ConsoleKey.C };
                if (validKeys.Contains(cki.Key))
                {
                    break;
                }
                Broadcast("\nThat is not a valid selection, please enter either a T or a C");
            } while (true);
 
            return cki.Key == ConsoleKey.T ? TeamType.Players : TeamType.Captains;
        }

        private void AskForTeamName(Team team)
        {
            team.Name = AskForAnswer();
        }

        private void AskForTeamName(Team team, string otherTeamsName)
        {
            do
            {
                team.Name = AskForAnswer();
                if (!String.Equals(team.Name, otherTeamsName, StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }
                Broadcast("\n\nYou can't have the same team names...\nStop being lazy and give me another one!");
            } while (true);
        }

        private void AskForVoiceSupport()
        {
            Console.WriteLine("Welcome to the pop quiz!\nWould you like me to continue voicing this quiz?\nPress Y for Yes or any other key to continue without:");

            ConsoleKeyInfo cki = Console.ReadKey();
            _voiceYN = cki.Key == ConsoleKey.Y;

            Console.Clear();
            Broadcast("Thank you for your selection.");
        }

        private static void AdjustConsoleSettings()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.TreatControlCAsInput = false;
            Console.Title = "Quiz";
        }

        private string GetTeamIntroFinalQuestion(string teamName)
        {
            return $"{teamName} time for you final question.";
        }

        private string GetTeamIntro(string teamName)
        {
            var nextTeamIntro = TeamIntros.Dequeue();
            TeamIntros.Enqueue(nextTeamIntro);
            return string.Format(nextTeamIntro, teamName);
        }

        private static bool AnswerContainsProfanity(string answer)
        {
            var profaneWords = new List<string> { "fuck", "shit", "bollocks", "wank", "cunt", "tosser", "bastard", "fanny", "faggot", "arse" };

            foreach(var word in profaneWords)
            {
                if (answer.ToLower().Contains(word))
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
                Synthesizer.Speak(message);
            }
        }

        private Queue<string> TeamIntros
        {
            get
            {
                if (_teamIntros != null)
                {
                    return _teamIntros;
                }

                InitializeTeamIntros();

                return _teamIntros;
            }
        }

        private void InitializeTeamIntros()
        {
            var intros = new List<string>
                {
                    "{0} you're up!",
                    "{0} it's your turn.",
                    "{0} this question's yours.",
                    "{0} let's see if you can get this one.",
                    "{0}! I've picked this one especially for you...",
                    "{0} this is a tough one!",
                    "{0} whenever you're ready, here's your question:",
                    "{0} try this one on for size:"
                };

            _teamIntros = new Queue<string>();
            foreach (var intro in intros.OrderBy(x => _random.Next()))
            {
                _teamIntros.Enqueue(intro);
            }
        }
    }
}