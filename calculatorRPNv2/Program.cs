using System;

namespace calculatorwithRPN
{
    class Program
    {
        // input expression
        static void Main(string[] args)
        {
            Console.Write("Write your expression: ");
            string inputExpression = Console.ReadLine();
            List<object> parsedExpression = Parse(inputExpression);

            List<object> rpn = ToRPN(parsedExpression);
            Console.WriteLine($"\nReverse Polish notation:{string.Join(" ", rpn)}");
            float result = Calculate(rpn);

            Console.WriteLine($"\nResult:{result}");
            Console.ReadLine();
        }
        // get number
        static float GetNumber(object operation, float firstOperator, float secondOperator)
        {
            switch (operation)
            {
                case '+': 
                    return firstOperator + secondOperator;
                case '-': 
                    return firstOperator - secondOperator;
                case '*': 
                    return firstOperator * secondOperator;
                case '/': 
                    return firstOperator / secondOperator;
                default: 
                    return 0;
            }
        }
        // operation priority 
        static int GetPriority(object operation) 
        {
            switch (operation)
            {
                case '+': 
                    return 1;
                case '-': 
                    return 1;
                case '*':
                    return 2;
                case '/': 
                    return 2;
                default: 
                    return 0;
            }
        }
        // parsing inputed expression
        static List<object> Parse(string expression)
        {
            List<object> result = new List<object>();

            string number = "";

            foreach (char token in expression)
            {
                if (token != ' ')
                {
                    if (!char.IsDigit(token))
                    {
                        if (number != "") result.Add(number);
                        result.Add(token);
                        number = "";
                    }
                    else
                    {
                        number += token;
                    }
                }
            }

            if (number != "")
            {
                result.Add(number);
            }

            return result;
        }
        // to Reverse Polish notation(RPN)
        static List<object> ToRPN(List<object> expression)
        {
            Stack<object> operations = new Stack<object>();

            List<object> result = new List<object>();

            foreach (object token in expression)
            {
                if (token is string)
                {
                    result.Add(token);
                }
                else if (token.Equals('+') || token.Equals('-') || token.Equals('*') || token.Equals('/'))
                {
                    while (operations.Count > 0 && GetPriority(operations.Peek()) >= GetPriority(token))
                    {
                        result.Add(operations.Pop());
                    }
                    operations.Push(token);
                }
                else if (token.Equals('('))
                {
                    operations.Push(token);
                }
                else if (token.Equals(')'))
                {
                    while (operations.Count > 0 && !operations.Peek().Equals('('))
                    {
                        result.Add(operations.Pop());
                    }
                    operations.Pop();
                }
            }

            while (operations.Count > 0)
            {
                result.Add(operations.Pop());
            }

            return result;
        }
        // expression calculator 
        static float Calculate(List<object> RPN)
        {
            for (int i = 0; i < RPN.Count; i++)
            {
                if (RPN[i] is char)
                {
                    float fisrtNumber = Convert.ToSingle(RPN[i - 2]);
                    float secondNumber = Convert.ToSingle(RPN[i - 1]);
                    float result = GetNumber(RPN[i], fisrtNumber, secondNumber);

                    RPN.RemoveRange(i - 2, 3);
                    RPN.Insert(i - 2, result);
                    i -= 2;
                }
            }
            return (float)RPN[0];
        }
    }
}
