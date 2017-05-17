using System.Collections.Generic;

namespace PopQuiz
{
    public class Answers
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string NumText { get; set; }


        public Answers(string numText, string question, string answer)
        {
            NumText = numText;
            Question = question;
            Answer = answer;
        }

        public static Dictionary<int, Answers> GetAnswer()
        {
            var question = new Dictionary<int, Answers>();
            var theAnswer = new Answers("First", "Who?", "...");
            question.Add(1, theAnswer);
            theAnswer = new Answers("Second", "What?", "...");
            question.Add(2, theAnswer);
            theAnswer = new Answers("Third", "Where?", "...");
            question.Add(3, theAnswer);
            theAnswer = new Answers("Fourth", "When?", "...");
            question.Add(4, theAnswer);
            theAnswer = new Answers("Fifth", "How?", "...");
            question.Add(5, theAnswer);
            theAnswer = new Answers("Sixth", "Why?", "...");
            question.Add(6, theAnswer);
            theAnswer = new Answers("Seventh", "Which?", "...");
            question.Add(7, theAnswer);
            theAnswer = new Answers("Eighth", "With whom?", "...");
            question.Add(8, theAnswer);
            theAnswer = new Answers("Ninth", "What also?", "...");
            question.Add(9, theAnswer);
            theAnswer = new Answers("Tenth", "Done now?", "...");
            question.Add(10, theAnswer);



            return question;
        }


    }
}