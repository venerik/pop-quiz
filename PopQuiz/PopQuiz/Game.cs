using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopQuiz
{
    public class Game
    {
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

        public Dictionary<int, Test> Tests
        {
            get
            {
                var tests = new Dictionary<int, Test>();
                var test = new Test("First", "Who?", "...");
                tests.Add(1, test);
                test = new Test("Second", "What?", "...");
                tests.Add(2, test);
                test = new Test("Third", "Where?", "...");
                tests.Add(3, test);
                test = new Test("Fourth", "When?", "...");
                tests.Add(4, test);
                test = new Test("Fifth", "How?", "...");
                tests.Add(5, test);
                test = new Test("Sixth", "Why?", "...");
                tests.Add(6, test);
                test = new Test("Seventh", "Which?", "...");
                tests.Add(7, test);
                test = new Test("Eighth", "With whom?", "...");
                tests.Add(8, test);
                test = new Test("Ninth", "What also?", "...") { IsFinal = true };
                tests.Add(9, test);
                test = new Test("Tenth", "Done now?", "...") { IsFinal = true };
                tests.Add(10, test);

                return tests;
            }
        }
    }
}
