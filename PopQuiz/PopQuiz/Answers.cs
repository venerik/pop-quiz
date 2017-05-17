using System.Collections.Generic;

namespace PopQuiz
{
    public class Test
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string NumText { get; set; }


        public Test(string numText, string question, string answer)
        {
            NumText = numText;
            Question = question;
            Answer = answer;
        }

        public static Dictionary<int, Test> GetTests()
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
            test = new Test("Ninth", "What also?", "...");
            tests.Add(9, test);
            test = new Test("Tenth", "Done now?", "...");
            tests.Add(10, test);

            return tests;
        }


    }
}