using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Constant
{
    public class Constants
    {
        public static Dictionary<string, (int, string)> Types = new Dictionary<string, (int, string)>() {
            { "int", (0, "32-bit integer") },
            { "uint", (1, "32-bit unsigned integer") },
            { "long", (2, "64-bit integer") },
            { "ulong", (3, "64-bit unsigned integer") },
            { "string", (4, "string of chars")},
            { "char", (5, "char element")},
            { "bool", (6, "logic element")},
            {"double", (7, "64-bit double") },
            {"float", (8, "32-bit float") }
        };
        public static Dictionary<string, (int, string)> Operations = new Dictionary<string, (int, string)>
        {
            {"=", (0, "assign_operation")},
            {"+", (1, "sum_operation")},
            {"-", (2, "subtraction_operation")},
            {"*", (3, "multiply_operation")},
            {"/", (4, "divide_operation")},
            {"+=", (5, "add_assign_operation")},
            {"/=", (6, "divide_assign_operation")},
            {"*=", (7, "multiply_assign_operation")},
            {"-=", (8, "subtraction_assign_operation")},
            {"==", (9, "equal_operation")},
            {">", (10, "more_operation")},
            {"<", (11, "less_operation")},
            {"++", (12, "increment_operation")},
            {"--", (13, "decrement_operation")},
            {"%", (14, "modulDiv_operation")},
            {"<=", (15, "moreOrEven")},
            {">=", (16, "lessOrEven")}
        };

        public static string[] Keywords = { "internal","public", "private", "protect" ,"return", "if", "else", "break", "continue" };

        public static string[] KeySymbols = { ".", ";", ",", "(", ")", "[", "]", "{", "}" };
    }
}
