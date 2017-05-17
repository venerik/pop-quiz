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
    }
}
