﻿using System.Collections.Generic;

namespace Logic
{
    public class Not : ILogicalExpression
    {
        private readonly ILogicalExpression logic;
        
        public Not(ILogicalExpression logic)
        {
            this.logic = logic;
        }

        public bool Eval(IDictionary<CellSymbol, bool> model)
        {
            return !logic.Eval(model);
        }

        public List<CellSymbol> GetSymbols()
        {
            return logic.GetSymbols();
        }
    }
}