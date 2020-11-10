﻿using System.Collections.Generic;

namespace Logic
{
    public class KnowledgeBase : ILogicalExpression
    {
        private readonly List<ILogicalExpression> logics = new List<ILogicalExpression>();

        public void Add(ILogicalExpression logic)
        {
            logics.Add(logic);
        }

        public bool Eval(IDictionary<CellSymbol, bool> model)
        {
            if (logics.Count == 0) return false;

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