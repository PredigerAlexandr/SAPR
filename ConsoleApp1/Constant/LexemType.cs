using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Constant
{
    public enum LexemType : int
    {
        Error = -1,
        TypeVar = 0, //int,long
        Variable = 1, //i,k
        Delimetr = 2,  //{,(
        Identifier = 3, // public, protect, for, while
        Constant = 4,
        Operation = 5, //*-+     
    }
}
