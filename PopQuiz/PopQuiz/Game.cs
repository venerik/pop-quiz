using System;
using System.Collections.Generic;
using System.Linq;

namespace PopQuiz
{
    public class Game
    {
        private Random _random = new Random();

        public Game(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
            CurrentTeam = team1;
        }

        public Team Team1 { get; private set; }
        public Team Team2 { get; private set; }

        public Team CurrentTeam { get; private set; }

        public void NextTeam()
        {
            CurrentTeam = CurrentTeam == Team1 ? Team2 : Team1;
        }

        public IEnumerable<Test> Tests
        {
            get
            {
                var tests = new List<Test>();
                var test = new Test("First", "Who?", "...");
                tests.Add(test);
                test = new Test("Second", "What?", "...");
                tests.Add(test);
                test = new Test("Third", "Where?", "...");
                tests.Add(test);
                test = new Test("Fourth", "When?", "...");
                tests.Add(test);
                test = new Test("Fifth", "How?", "...");
                tests.Add(test);
                test = new Test("Sixth", "Why?", "...");
                tests.Add(test);
                test = new Test("Seventh", "Which?", "...");
                tests.Add(test);
                test = new Test("Eighth", "With whom?", "...");
                tests.Add(test);
                test = new Test("Ninth", "What also?", "...") { IsFinal = true };
                tests.Add(test);
                test = new Test("Tenth", "Done now?", "...") { IsFinal = true };
                tests.Add(test);

                return tests;
            }
        }

        public IReadOnlyList<Player> Players
        {
            get
            {
                    var players = new List<Player>();
                    var player = new Player("Name 1");
                    players.Add(player);
                    player = new Player("Name 2");
                    players.Add(player);
                    player = new Player("Name 3");
                    players.Add(player);
                    player = new Player("Name 4");
                    players.Add(player);
                    player = new Player("Name 5");
                    players.Add(player);
                    player = new Player("Name 6");
                    players.Add(player);
                    player = new Player("Name 7");
                    players.Add(player);
                    player = new Player("Name 8");
                    players.Add(player);

                    return players.OrderBy(x => _random.Next()).ToList();
            }
        }
    }
}
