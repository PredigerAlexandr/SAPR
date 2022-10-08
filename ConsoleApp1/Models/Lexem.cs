using ConsoleApp1.Constant;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    public class Lexem
    {
        public LexemType LexemType { get; set; }
        public int LexId { get; set; }
        public string Value { get; set; }

        public Lexem(LexemType lexemType, int lexId, string val)
        {
            LexemType = lexemType;
            LexId = lexId;
            Value = val;
        }

        public override string ToString()
        {
            return $"lexem type: {LexemType};\t lexem id: {LexId};\t value: {Value}";
        }
    }
}
