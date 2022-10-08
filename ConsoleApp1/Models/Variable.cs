using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Models
{
    internal class Variable
    {
        public int Id { get; set; }
        public string DataType { get; set; }
        public string Name { get; set; }

        public Variable(int id, string dataType, string name)
        {
            Id = id;
            DataType = dataType;
            Name = name;
        }

        public string ToString()
        {
            return $"<{Id}> Variable of type <{DataType}> with name <{Name}>";
        }
    }


}
