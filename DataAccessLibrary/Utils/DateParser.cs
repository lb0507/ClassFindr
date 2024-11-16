using ClassFindrDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassFindrDataAccessLibrary.Utils
{
    public class DateParser
    {
        private static readonly string TIME_FORMAT = @"t";

        public static string GetScheduledTime(ClassModel inputtedClass)
        {
            string output = $"({inputtedClass.Time.ToString(TIME_FORMAT)})";   // Initialize the output with the time

            // Parse out the inputted days using delimeters
            string parsedDays = "";
            foreach (char c in inputtedClass.Days ?? "")
            {
                parsedDays += c;
                switch (parsedDays)
                {
                    case "|":
                        parsedDays = "";
                        break;

                    case "Su":
                        output += " Sunday";
                        parsedDays = "";
                        break;

                    case "M":
                        output += " Monday";
                        parsedDays = "";
                        break;

                    case "Tu":
                        output += " Tuesday";
                        parsedDays = "";
                        break;

                    case "W":
                        output += " Wednesday";
                        parsedDays = "";
                        break;

                    case "Th":
                        output += " Thursday";
                        parsedDays = "";
                        break;

                    case "F":
                        output += " Friday";
                        parsedDays = "";
                        break;

                    case "Sa":
                        output += " Saturday";
                        parsedDays = "";
                        break;
                }
            }

            return output;
        }
    }
}
