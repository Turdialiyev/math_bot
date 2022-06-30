using System.Text.RegularExpressions;

public class Postfix
{
   public static string CountExp(string text) 
    {
        string exp = GetPostfix(text);
        Console.WriteLine($"{exp}");
        // Console.WriteLine($"postfix evaluatega o'tti");
        
        var result = PostfixEvaluate(exp);
        //  WriteLine($"{result}"); 
        
        return result;
    }

    public static string PostfixEvaluate(string input)
    {
        Stack<double> stack = new Stack<double>();

        for (int i = 0; i < input.Length; i++)
        {
            char characterOfInput = input[i];

            if (char.IsDigit(characterOfInput))
            {
                double number = 0;

                while (char.IsDigit(characterOfInput))
                {
                    number = number * 10 + (int)(characterOfInput - '0');
                    i++;
                    characterOfInput = input[i];
                }
                i--;
                stack.Push(number);
            }
            else if (characterOfInput == ' ')
            {
                continue;
            }
            else
            {
                double val1 = stack.Pop();
                double val2 = stack.Pop();

                switch (characterOfInput)
                {
                    case '+':
                        stack.Push(val2 + val1);
                        break;
                    case '-':
                        stack.Push(val2 - val1);
                        break;
                    case '*':
                        stack.Push(val2 * val1);
                        break;
                    case '/':
                        stack.Push(val2 / val1);
                        break;
                    case '^':
                        stack.Push((int)Math.Pow(val2, val1));
                        break;
                }
            }
        }
        return stack.Pop().ToString();
    }


    public static string GetPostfix(string input)
    {
        string patternForOperators = @"";
        string[] operatorsArray = Regex.Split(input, patternForOperators);

        string word = "";

        for (int i = 0; i < operatorsArray.Length; i++)
        {
            word += operatorsArray[i];
        }
        word += " ";

        string infixView = "";
        for (int i = 0; i < word.Length; i++)
        {
            infixView += word[i];
            if (IsNumber(word[i]))
            {
                if (IsOperator(word[i + 1]))
                {
                    infixView += " ";
                }
            }
            else if (IsOperator(word[i]))
            {
                infixView += " ";
            }
        }

        /////until here we have the infix view of the expression

        var list = infixView.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        Stack<string> stack = new Stack<string>();
        stack.Push("#");
        Queue<string> queue = new Queue<string>();


        for (int i = 0; i < list.Count; i++)
        {
            if (!IsOperator(list[i]))
            {
                queue.Enqueue(list[i]);
            }
            else if (list[i] == "(")
            {
                stack.Push(list[i]);
            }
            else if (list[i] == "^")
            {
                stack.Push(list[i]);
            }
            else if (list[i] == ")")
            {
                while (stack.Peek() != "#" && stack.Peek() != "(")
                {
                    queue.Enqueue(stack.Pop());

                }
                stack.Pop();
            }
            else
            {
                if (preced(list[i]) > preced(stack.Peek()))
                {
                    stack.Push(list[i]);
                }
                else
                {
                    while (stack.Peek() != "#" && preced(list[i]) <= preced(stack.Peek()))
                    {
                        queue.Enqueue(stack.Pop());
                    }
                    stack.Push(list[i]);
                }
            }
        }


        while (stack.Peek() != "#")
        {
            queue.Enqueue(stack.Pop());
        }

        string exp = "";
        foreach (var item in queue)
        {
            exp += item + " ";
        }

        return exp;
    }


    public static int preced(string ch)
    {
        if (ch == "+" || ch == "-")
        {
            return 1;              //Precedence of + or - is 1
        }
        else if (ch == "*" || ch == "/")
        {
            return 2;            //Precedence of * or / is 2
        }
        else if (ch == "^")
        {
            return 3;            //Precedence of ^ is 3
        }
        else
        {
            return 0;
        }
    }

    public static bool IsNumber(char input) => input switch
    {
        '0' => true,
        '1' => true,
        '2' => true,
        '3' => true,
        '4' => true,
        '5' => true,
        '6' => true,
        '7' => true,
        '8' => true,
        '9' => true,
        _ => false
    };

    public static bool IsOperator(char input) => input switch
    {
        '+' => true,
        '-' => true,
        '*' => true,
        '/' => true,
        '(' => true,
        ')' => true,
        '^' => true,
        '%' => true,
        _ => false
    };
    public static bool IsOperator(string input) => input switch
    {
        "+" => true,
        "-" => true,
        "*" => true,
        "/" => true,
        "(" => true,
        ")" => true,
        "^" => true,
        "%" => true,
        _ => false
    };

}
