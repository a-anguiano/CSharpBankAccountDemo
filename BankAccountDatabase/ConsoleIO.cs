using System;

namespace BankAccountDatabase.UI
{
    class ConsoleIO
    {
        public char GetChar(string prompt)
        {
            Console.Write(prompt + ": ");
            char c = Console.ReadKey().KeyChar.ToString().ToUpper()[0];
            Display("");
            return c;
        }
        public string GetString(string prompt)
        {
            Console.Write(prompt + ": ");
            return Console.ReadLine().Trim();
        }

        public decimal GetDecimal(string prompt)
        {
            decimal result = -1;
            bool valid = false;
            while (!valid)
            {
                Console.Write($"{prompt}: ");
                if (!decimal.TryParse(Console.ReadLine(), out result))
                {
                    Error("Please input a proper integer\n\n");
                }
                else
                {
                    valid = true;
                }
            }
            return result;
        }
        public int GetInt(string prompt)
        {
            int result = -1;
            bool valid = false;
            while (!valid)
            {
                Console.Write($"{prompt}: ");
                if (!int.TryParse(Console.ReadLine(), out result))
                {
                    Error("Please input a proper integer\n\n");
                }
                else
                {
                    valid = true;
                }
            }
            return result;
        }
        public void Display(string message)
        {
            Console.WriteLine(message);
        }
        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Display(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Display(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Title(string title)
        {
            Display(new string('=', title.Length));
            Display(title);
            Display(new string('=', title.Length));
            Display("");
        }
    }
}

