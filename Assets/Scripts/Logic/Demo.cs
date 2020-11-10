/*
using System;
using Logic;
using static Logic.Utils;

// ReSharper disable All
namespace Wampus
{
    internal class Program
    {
        private static Brain brain = new Brain();
        private const int rows = 4;
        private const int cols = 4;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("0, 0");
            AddVisited(0, 0);
            AddVisited(1, 0);
            AddVisited(0, 1);
            Tell(0, 0, false, false);
            TellNot(0, 0, Symbol.P);
            TellNot(0, 0, Symbol.W);
            
            Console.WriteLine(brain.Ask(new Not(HasWampus(0, 1)))); // True
            Console.WriteLine(brain.Ask(new Not(HasWampus(1, 0)))); // True
         
            
            Console.WriteLine("\n0, 1");
            AddVisited(1, 1);
            AddVisited(0, 2);
            Tell(0, 1, true, false);
            TellNot(0, 1, Symbol.P);
            TellNot(0, 1, Symbol.W);
            
            Console.WriteLine(brain.Ask(HasPit(1, 1))); // False
            Console.WriteLine(brain.Ask(HasPit(0, 2))); // False
            Console.WriteLine(brain.Ask(new Not(HasPit(1, 1)))); // False
            Console.WriteLine(brain.Ask(new Not(HasPit(0, 2)))); // False
            
            
            Console.WriteLine("\n1, 0");
            AddVisited(2, 0);
            Tell(1, 0, false, true);
            TellNot(1, 0, Symbol.P);
            TellNot(1, 0, Symbol.W);
            
            Console.WriteLine(brain.Ask(new Not(HasPit(1, 1)))); // True
            Console.WriteLine(brain.Ask(new Not(HasWampus(1, 1)))); // True
            
            TellNot(1, 1, Symbol.P);
            TellNot(1, 1, Symbol.W);

            Console.WriteLine(brain.Ask(HasWampus(2, 0))); // True
            Console.WriteLine(brain.Ask(HasPit(0, 2))); // True
        }

        public static void TellNot(int row, int col, Symbol s)
        {
            CellSymbol cellNot = new CellSymbol(row, col, s);
            brain.Tell(new Not(cellNot));
        }

        public static void Tell(int row, int col, bool b, bool s)
        {
            CellSymbol cellB = new CellSymbol(row, col, Symbol.B);
            CellSymbol cellS = new CellSymbol(row, col, Symbol.S);
            
            if (b)
            {
                brain.Tell(cellB);
            }
            else
            {
                brain.Tell(new Not(cellB));
            }

            if (s)
            {
                brain.Tell(cellS);
            }
            else
            {
                brain.Tell(new Not(cellS));   
            }
        }
    }
}
*/