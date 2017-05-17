using System.Collections.Generic;

namespace PopQuiz
{
    public class Test
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string NumText { get; set; }
        public bool IsFinal { get; set; }

        public Test(string numText, string question, string answer)
        {
            NumText = numText;
            Question = question;
            Answer = answer;
        }
    }
}