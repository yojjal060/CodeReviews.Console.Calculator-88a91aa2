// CalculatorLibrary.cs
using Newtonsoft.Json;


namespace CalculatorLibrary
{
    public class Calculator
    {

        JsonWriter writer;
        int Count = 0;

        List<string> LatestCalculation = new List<string>();

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "d":
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;
                case "p":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Power");
                    break;
                case "r":
                    result = Math.Sqrt(num1);
                    writer.WriteValue("Square Root");
                    break;
                case "e":
                    result = Math.Pow(10, num1);
                    writer.WriteValue("10^x");
                    break;
                case "i":
                    result = Math.Sin(num1);
                    writer.WriteValue("Sine");
                    break;
                case "o":
                    result = Math.Cos(num1);
                    writer.WriteValue("Cosine");
                    break;
                case "t":
                    result = Math.Tan(num1);
                    writer.WriteValue("Tangent");
                    break;
                case "l":
                    for (int id = 0; id < GetLatestCalculation().Count; id++)
                    {
                        Console.WriteLine($"Calculation {id+1}: {GetLatestCalculation()[id]}");
                    }

                    writer.WriteValue("List latest calculations");
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }

        // TODO
        // Create a functionality that will count the amount of times the calculator was used.

        public void LogUsageCount()
        {
            Count++;
        }

        public int GetUsageCount()
        {
            return Count;
        }

        public void LogCalculation(string calculation)
        {
            LatestCalculation.Add(calculation);
        }

        public void ClearCalculationById(int id)
        {
            if (id >= 0 && id < LatestCalculation.Count)
            {
                LatestCalculation.RemoveAt(id);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid ID. Please provide a valid index.");
            }
        }

        public List<string> GetLatestCalculation()
        {
            return LatestCalculation;
        }

    }
}