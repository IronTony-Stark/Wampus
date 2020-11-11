﻿using System.Collections.Generic;
using Logic.Operators;

namespace Logic
{
    public class Utils
    {
        private static readonly int rows = MapGenerator.mapSize;
        private static readonly int cols = MapGenerator.mapSize;
        private static readonly HashSet<(int, int)> reachable = new HashSet<(int, int)>();

        public static ILogicalExpression HasWampus(int row, int col)
        {
            var logicalExpressions = GetSurroundingBy(row, col, Symbol.Stench);
            // var possiblePositionsBy = GetPossiblePositionsBy(row, col, Symbol.Wampus);
            // logicalExpressions.AddRange(possiblePositionsBy);
            return new And(logicalExpressions);
        }

        public static ILogicalExpression HasPit(int row, int col)
        {
            var logicalExpressions = GetSurroundingBy(row, col, Symbol.Wind);
            // var possiblePositionsBy = GetPossiblePositionsBy(row, col, Symbol.Pit);
            // logicalExpressions.AddRange(possiblePositionsBy);
            return new And(logicalExpressions);
        }

        private static List<ILogicalExpression> GetSurroundingBy(int row, int col, Symbol s)
        {
            var symbols = new List<ILogicalExpression>();

            if (row > 0
                && reachable.Contains((row - 1, col))
            )
                symbols.Add(new CellSymbol(row - 1, col, s));

            if (col > 0
                && reachable.Contains((row, col - 1))
            )
                symbols.Add(new CellSymbol(row, col - 1, s));

            if (row < rows - 1
                && reachable.Contains((row + 1, col))
            )
                symbols.Add(new CellSymbol(row + 1, col, s));

            if (col < cols - 1
                && reachable.Contains((row, col + 1))
            )
                symbols.Add(new CellSymbol(row, col + 1, s));

            return symbols;
        }

        private static List<ILogicalExpression> GetPossiblePositionsBy(int row, int col, Symbol s)
        {
            var symbols = new List<ILogicalExpression>();

            if (row > 1
                && reachable.Contains((row - 2, col))
            )
                symbols.Add(new Not(new CellSymbol(row - 2, col, s)));

            if (col > 1
                && reachable.Contains((row, col - 2))
            )
                symbols.Add(new Not(new CellSymbol(row, col - 2, s)));

            if (row < rows - 2
                && reachable.Contains((row + 2, col))
            )
                symbols.Add(new Not(new CellSymbol(row + 2, col, s)));

            if (col < cols - 2
                && reachable.Contains((row, col + 2))
            )
                symbols.Add(new Not(new CellSymbol(row, col + 2, s)));


            if (row > 0 && col > 0
                && reachable.Contains((row - 1, col - 1))
            )
                symbols.Add(new Not(new CellSymbol(row - 1, col - 1, s)));

            if (row > 0 && col < cols - 1
                && reachable.Contains((row - 1, col + 1))
            )
                symbols.Add(new Not(new CellSymbol(row - 1, col + 1, s)));

            if (row < rows - 1 && col < cols - 1
                && reachable.Contains((row + 1, col + 1))
            )
                symbols.Add(new Not(new CellSymbol(row + 1, col + 1, s)));

            if (row < rows - 1 && col > 0
                && reachable.Contains((row + 1, col - 1))
            )
                symbols.Add(new Not(new CellSymbol(row + 1, col - 1, s)));


            return symbols;
        }

        public static void AddReachable(int row, int col)
        {
            reachable.Add((row, col));
        }
    }
}