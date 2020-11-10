﻿using System;
using System.Collections.Generic;
using Logic.Operators;
using static Logic.Symbol;

namespace Logic
{
    public class CellSymbol : ILogicalExpression
    {
        private readonly int row;
        private readonly int col;
        private readonly Symbol symbol;

        public CellSymbol(int row, int col, Symbol symbol)
        {
            this.row = row;
            this.col = col;
            this.symbol = symbol;
        }

        public bool Eval(IDictionary<CellSymbol, bool> model)
        {
            return model[this];
        }

        public List<CellSymbol> GetSymbols()
        {
            return new List<CellSymbol> {this};
        }

        public override bool Equals(object obj)
        {
            CellSymbol cellSymbol = obj as CellSymbol;

            if (cellSymbol == null)
            {
                return false;
            }

            return Equals(cellSymbol);
        }

        private bool Equals(CellSymbol other)
        {
            return row == other.row &&
                   col == other.col &&
                   symbol == other.symbol;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = row;
                hashCode = (hashCode * 397) ^ col;
                hashCode = (hashCode * 397) ^ (int) symbol;
                return hashCode;
            }
        }
    }
}