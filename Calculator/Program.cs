// Program.cs
using CalculatorLibrary;
using System.Text.RegularExpressions;

namespace CalculatorProgram
{

    class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;
            double? copiedResult = null;
            
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            Calculator calculator = new Calculator();

            while (!endApp)
            {
                Console.WriteLine("Type 'm' to use the calculator, or 'l' to list the latest calculations.");
                string? userInput = "";
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case("m"):
                        string? numInput1 = "";
                        string? numInput2 = "";
                        double result = 0;

                        double cleanNum1 = 0;
                        if (copiedResult.HasValue)
                        {
                            Console.Write($"Use copied result ({copiedResult.Value}) as first number? (y/n): ");
                            string? useCopiedResult = Console.ReadLine();
                            if (useCopiedResult == "y")
                            {
                                cleanNum1 = copiedResult.Value;
                                Console.WriteLine($"Using {cleanNum1} as the first number.");
                            }
                            else
                            {
                                Console.Write("Type a number, and then press Enter: ");
                                numInput1 = Console.ReadLine();
                                while (!double.TryParse(numInput1, out cleanNum1))
                                {
                                    Console.Write("This is not valid input. Please enter an integer value: ");
                                    numInput1 = Console.ReadLine();
                                }
                            }
                        }
                        else
                        {
                            Console.Write("Type a number, and then press Enter: ");
                            numInput1 = Console.ReadLine();
                            while (!double.TryParse(numInput1, out cleanNum1))
                            {
                                Console.Write("This is not valid input. Please enter an integer value: ");
                                numInput1 = Console.ReadLine();
                            }
                        }

                        // Ask the user to type the second number.
                        Console.Write("Type another number, and then press Enter: ");
                        numInput2 = Console.ReadLine();

                        double cleanNum2 = 0;
                        while (!double.TryParse(numInput2, out cleanNum2))
                        {
                            Console.Write("This is not valid input. Please enter an integer value: ");
                            numInput2 = Console.ReadLine();
                        }

                        // Ask the user to choose an operator.
                        Console.WriteLine($"The calculator has been used {calculator.GetUsageCount()} times.\n");
                        Console.WriteLine("Choose an operator from the following list:");
                        Console.WriteLine("\ta - Add");
                        Console.WriteLine("\ts - Subtract");
                        Console.WriteLine("\tm - Multiply");
                        Console.WriteLine("\td - Divide");
                        Console.WriteLine("\tp - Power (x^y)");
                        Console.WriteLine("\tr - Square Root (√x)");
                        Console.WriteLine("\te - 10^x");
                        Console.WriteLine("\ti - Sin(x)");
                        Console.WriteLine("\to - Cos(x)");
                        Console.WriteLine("\tt - Tan(x)");

                        Console.Write("Your option? ");
                        string? op = Console.ReadLine();

                        // Validate input is not null, and matches the pattern
                        if (op == null || !Regex.IsMatch(op, "^[asmdpreiot]$"))
                        {
                            Console.WriteLine("Error: Unrecognized input.");
                        }
                        else
                        {
                            try
                            {
                                result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                                if (double.IsNaN(result))
                                {
                                    Console.WriteLine("This operation will result in a mathematical error.\n");
                                }
                                else
                                {
                                    Console.WriteLine("Your result: {0:0.##}\n", result);

                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                            }
                        }
                        Console.WriteLine("------------------------\n");

                        


                        // Wait for the user to respond before closing.
                        Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                        if (Console.ReadLine() == "n") endApp = true;

                        calculator.LogUsageCount();
                        calculator.LogCalculation($"{cleanNum1} {op} {cleanNum2} = {result}");

                        Console.WriteLine("\n");
                        break;
                    case("l"):
                        for (int id = 0; id < calculator.GetLatestCalculation().Count; id++)
                        {
                            Console.WriteLine($"Calculation {id + 1}: {calculator.GetLatestCalculation()[id]}");
                        }
                        Console.WriteLine("------------------------\n");

                        Console.WriteLine("Press 'd' to delete the Previous Calculation or Press 'c' to copy the results for further calculation");
                        string? actionInputForL = Console.ReadLine();

                        switch (actionInputForL)
                        {
                            case ("d"):
                                Console.WriteLine("Press id number to clear that calculation, or press any other key to continue: ");
                                string? clearInput = Console.ReadLine();
                                for (int id = 0; id < calculator.GetLatestCalculation().Count; id++)
                                {
                                    if (clearInput == (id + 1).ToString())
                                    {
                                        calculator.ClearCalculationById(id);
                                        Console.WriteLine($"Calculation {id + 1} cleared.");
                                        break;
                                    }
                                }
                                break;
                            case ("c"):
                                Console.WriteLine("Press id number to copy the result, or press any other key to continue: ");
                                string? copyInput = Console.ReadLine();
                                if (copyInput != null && int.TryParse(copyInput, out int copyId) && copyId > 0 && copyId <= calculator.GetLatestCalculation().Count)
                                {
                                    string[] parts = calculator.GetLatestCalculation()[copyId - 1].Split('=');
                                    if (parts.Length == 2 && double.TryParse(parts[1].Trim(), out double parsedResult))
                                    {
                                        copiedResult = parsedResult;
                                        Console.WriteLine($"Result {copiedResult.Value} copied for further calculation.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Error: Unable to parse the selected calculation.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid calculation ID.");
                                }
                                break;
                        }

                        break;

                }


                
            }
            calculator.Finish();
            return;
        }
    }
}