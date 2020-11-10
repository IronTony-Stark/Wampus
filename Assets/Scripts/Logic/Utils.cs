﻿using System.Collections.Generic;
using Logic.Operators;

namespace Logic
{
    public class Utils
    {
        private static readonly int rows = 4;
        private static readonly int cols = 4;
        private static List<(int, int)> visisted = new List<(int, int)>();

        public static ILogicalExpression HasWampus(int row, int col)
        {
            var logicalExpressions = GetSurroundingBy(row, col, Symbol.S);
            var possiblePositionsBy = GetPossiblePositionsBy(row, col, Symbol.W);
            logicalExpressions.AddRange(possiblePositionsBy);
            return new And(logicalExpressions);
        }

        public static ILogicalExpression HasPit(int row, int col)
        {
            var logicalExpressions = GetSurroundingBy(row, col, Symbol.B);
            var possiblePositionsBy = GetPossiblePositionsBy(row, col, Symbol.P);
            logicalExpressions.AddRange(possiblePositionsBy);
            return new And(logicalExpressions);
        }

        private static List<ILogicalExpression> GetSurroundingBy(int row, int col, Symbol s)
        {
            var symbols = new List<ILogicalExpression>();

            if (row > 0
                // && visisted.Contains((row - 1, col))
            )
                symbols.Add(new CellSymbol(row - 1, col, s));

            if (col > 0
                // && visisted.Contains((row, col - 1))
            )
                symbols.Add(new CellSymbol(row, col - 1, s));

            if (row < rows - 1
                // && visisted.Contains((row + 1, col))
            )
                symbols.Add(new CellSymbol(row + 1, col, s));

            if (col < cols - 1
                // && visisted.Contains((row, col + 1))
            )
                symbols.Add(new CellSymbol(row, col + 1, s));

            return symbols;
        }

        private static List<ILogicalExpression> GetPossiblePositionsBy(int row, int col, Symbol s)
        {
            var symbols = new List<ILogicalExpression>();

            if (row > 1
                // && visisted.Contains((row - 2, col))
            )
                symbols.Add(new Not(new CellSymbol(row - 2, col, s)));

            if (col > 1
                // && visisted.Contains((row, col - 2))
            )
                symbols.Add(new Not(new CellSymbol(row, col - 2, s)));

            if (row < rows - 2
                // && visisted.Contains((row + 2, col))
            )
                symbols.Add(new Not(new CellSymbol(row + 2, col, s)));

            if (col < cols - 2
                // && visisted.Contains((row, col + 2))
            )
                symbols.Add(new Not(new CellSymbol(row, col + 2, s)));


            if (row > 0 && col > 0
                // && visisted.Contains((row - 1, col - 1))
            )
                symbols.Add(new Not(new CellSymbol(row - 1, col - 1, s)));

            if (row > 0 && col < cols - 1
                // && visisted.Contains((row - 1, col + 1))
            )
                symbols.Add(new Not(new CellSymbol(row - 1, col + 1, s)));

            if (row < rows - 1 && col < cols - 1
                // && visisted.Contains((row + 1, col + 1))
            )
                symbols.Add(new Not(new CellSymbol(row + 1, col + 1, s)));

            if (row < rows - 1 && col > 0
                // && visisted.Contains((row + 1, col - 1))
            )
                symbols.Add(new Not(new CellSymbol(row + 1, col - 1, s)));


            return symbols;
        }

        public static void AddVisited(int row, int col)
        {
            visisted.Add((row, col));
        }
    }
}