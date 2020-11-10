﻿﻿using System;
using System.Collections.Generic;

namespace Logic.Operators
{
    public class And : ILogicalExpression
    {
        private readonly List<ILogicalExpression> logics;

        public And(List<ILogicalExpression> logics)
        {
            // if (logics.Count < 2)
            // {
                // throw new Exception("And must have at least two operands");
            // }
            
            this.logics = logics;
        }

        public bool Eval(IDictionary<CellSymbol, bool> model)
        {
            bool result = logics[0].Eval(model);

            for (int i = 1; i < logics.Count; i++)
            {
                result = result && logics[i].Eval(model);
            }

            return result;
        }
        
        public List<CellSymbol> GetSymbols()
        {
            var cellSymbols = new List<CellSymbol>();

            foreach (ILogicalExpression logic in logics)
            {
                cellSymbols.AddRange(logic.GetSymbols());
            }

            return cellSymbols;
        }
    }
}