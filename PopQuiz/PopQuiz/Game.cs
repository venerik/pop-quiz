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
                return new List<Test>() {
                    new Test("First", "Who?", "..."),
                    new Test("Second", "What?", "..."),
                    new Test("Third", "Where?", "..."),
                    new Test("Fourth", "When?", "..."),
                    new Test("Fifth", "How?", "..."),
                    new Test("Sixth", "Why?", "..."),
                    new Test("Seventh", "Which?", "..."),
                    new Test("Eighth", "With whom?", "..."),
                    new Test("Ninth", "What also?", "...") { IsFinal = true },
                    new Test("Tenth", "Done now?", "...") { IsFinal = true },
                };
            }
        }

        public IReadOnlyList<Player> Players
        {
            get
            {
                var players = new List<Player>() {
                    new Player("Name 1"),
                    new Player("Name 2"),
                    new Player("Name 3"),
                    new Player("Name 4"),
                    new Player("Name 5"),
                    new Player("Name 6"),
                    new Player("Name 7"),
                    new Player("Name 8")
                };
                return players.OrderBy(x => _random.Next()).ToList();
            }
        }
    }
}
