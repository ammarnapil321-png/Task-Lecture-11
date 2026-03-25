using Task_Lecture_11;

namespace Task_Lecture_11
{
    class Question
    {
        public string Header { get; set; }
        public int Mark { get; set; }
        public string Level { get; set; }

        public virtual bool CheckAnswer(string answer)
        {
            return false;
        }
        public virtual void Display()
        {
            Console.WriteLine($"{Header}");
        }
    }
    class TFQ : Question
    {
        public bool CorrectAnswer;
        public override void Display()
        {
            base.Display();
            Console.WriteLine("1.True");
            Console.WriteLine("2.False");
        }
        public override bool CheckAnswer(string answer)
        {
            bool userAnswer = answer == "1";
            return userAnswer == CorrectAnswer;
        }
    }
    class OneQ : Question
    {
        public string[] Options;
        public int CorrectAnswer;

        public override void Display()
        {
            base.Display();
            for (int i = 0; i < Options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Options[i]}");
            }
        }

        public override bool CheckAnswer(string answer)
        {
            return int.Parse(answer) == CorrectAnswer;
        }
    }
    class MCQ : Question
    {
        public string[] Options;
        public List<int> CorrectAnswers;

        public override void Display()
        {
            base.Display();
            for (int i = 0; i < Options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Options[i]}");
            }
            Console.WriteLine("Enter answers like: 1,2");
        }

        public override bool CheckAnswer(string answer)
        {
            string[] parts = answer.Split(',');

            int[] userAnswers = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                userAnswers[i] = int.Parse(parts[i].Trim());
            }
            int[] correctAnswers = new int[CorrectAnswers.Count];

            for (int i = 0; i < CorrectAnswers.Count; i++)
            {
                correctAnswers[i] = CorrectAnswers[i];
            }
            Array.Sort(userAnswers);
            Array.Sort(correctAnswers);
            if (userAnswers.Length != correctAnswers.Length)
                return false;

            for (int i = 0; i < userAnswers.Length; i++)
            {
                if (userAnswers[i] != correctAnswers[i])
                    return false;
            }

            return true;
        }
    }
    class Program
    {
        static List<Question> questions = new List<Question>();

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("1. Doctor Mode");
                Console.WriteLine("2. Student Mode");
                Console.WriteLine("3. Exit");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 1)
                    DoctorMode();
                else if (choice == 2)
                    StudentMode();
                else
                    break;
            }
            static void DoctorMode()
            {
                Console.Write("Enter number of questions: ");
                int count = int.Parse(Console.ReadLine());

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Question {i + 1}");

                    Console.WriteLine("1. True/False");
                    Console.WriteLine("2. Choose One");
                    Console.WriteLine("3. MCQ");

                    int type = int.Parse(Console.ReadLine());

                    Console.Write("Header: ");
                    string header = Console.ReadLine();

                    Console.Write("Mark: ");
                    int mark = int.Parse(Console.ReadLine());

                    Console.Write("Level (Easy/Medium/Hard): ");
                    string level = Console.ReadLine();

                    if (type == 1)
                    {
                        TFQ q = new TFQ();
                        q.Header = header;
                        q.Mark = mark;
                        q.Level = level;

                        Console.Write("Correct Answer (true/false): ");
                        q.CorrectAnswer = bool.Parse(Console.ReadLine());

                        questions.Add(q);
                    }
                    else if (type == 2)
                    {
                        OneQ q = new OneQ();
                        q.Header = header;
                        q.Mark = mark;
                        q.Level = level;

                        q.Options = new string[4];
                        for (int j = 0; j < 4; j++)
                        {
                            Console.Write($"Option {j + 1}: ");
                            q.Options[j] = Console.ReadLine();
                        }

                        Console.Write("Correct option (1-4): ");
                        q.CorrectAnswer = int.Parse(Console.ReadLine());

                        questions.Add(q);
                    }
                    else if (type == 3)
                    {
                        MCQ q = new MCQ();
                        q.Header = header;
                        q.Mark = mark;
                        q.Level = level;

                        q.Options = new string[4];

                        for (int j = 0; j < 4; j++)
                        {
                            Console.Write($"Option {j + 1}: ");
                            q.Options[j] = Console.ReadLine();
                        }

                        Console.Write("Correct answers (e.g. 1,2): ");
                        string input = Console.ReadLine();

                        string[] parts = input.Split(',');

                        List<int> correctAnswers = new List<int>();

                        for (int I = 0; I < parts.Length; I++)
                        {
                            int value = int.Parse(parts[I].Trim());
                            correctAnswers.Add(value);
                        }

                        q.CorrectAnswers = correctAnswers;

                        questions.Add(q);
                    }
                }
            }
            static void StudentMode()
            {
                if (questions.Count == 0)
                {
                    Console.WriteLine("No questions available!");
                    return;
                }
                Console.WriteLine("1. Practical");
                Console.WriteLine("2. Final");
                int type = int.Parse(Console.ReadLine());

                Console.Write("Choose Level (Easy/Medium/Hard): ");
                string level = Console.ReadLine();

                List<Question> filtered = new List<Question>();
                for (int i = 0; i < questions.Count; i++)
                {
                    if (questions[i].Level.ToLower() == level.ToLower())
                    {
                        filtered.Add(questions[i]);
                    }
                }
                if (filtered.Count == 0)
                {
                    Console.WriteLine("No questions for this level");
                    return;
                }

                int numberOfQuestions = (type == 1) ? filtered.Count / 2 : filtered.Count;
                if (numberOfQuestions == 0) numberOfQuestions = 1;

                List<Question> Quet = new List<Question>();
                for (int i = 0; i < numberOfQuestions && i < filtered.Count; i++)
                {
                    Quet.Add(filtered[i]);
                }
                int total = 0;
                int score = 0;
                for (int i = 0; i < Quet.Count; i++)
                {
                    Console.WriteLine("----------------");

                    Quet[i].Display();
                    Console.Write("Your Answer: ");
                    string a = Console.ReadLine();
                    if (Quet[i].CheckAnswer(a))
                    {
                        score += Quet[i].Mark;
                    }

                    total += Quet[i].Mark;
                }

                Console.WriteLine($"Your Grade: {score} / {total}");
            }
        }
    }
}