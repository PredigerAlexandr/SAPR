using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.Constant
{
    public enum State:int
    {
        Initial = 1,
		ReadingNum = 2,
		ReadDelOrOper = 3,
		ReadingStr = 4,
		Error = 5,
		Final = 6,
    }
}
