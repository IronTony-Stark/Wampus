﻿﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public static class Entailment
    {
        public static bool TTEntails(ILogicalExpression kb, ILogicalExpression query)
        {
            var kbSymbols = kb.GetSymbols();
            var querySymbols = query.GetSymbols();
            kbSymbols.AddRange(querySymbols);

            return TTCheckAll(kb, query, kbSymbols, new Dictionary<CellSymbol, bool>());
        }

        private static bool TTCheckAll(ILogicalExpression kb, ILogicalExpression query,
            IReadOnlyList<CellSymbol> symbols, IDictionary<CellSymbol, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PLTrue(kb, model))
                {
                    bool res = PLTrue(query, model);

                    return res;
                }

                return true;
            }

            CellSymbol p = symbols[0];
            var rest = new List<CellSymbol>(symbols);
            rest.RemoveAt(0);

            return TTCheckAll(kb, query, rest, Extend(p, true, model)) &&
                   TTCheckAll(kb, query, rest, Extend(p, false, model));
        }

        private static bool PLTrue(ILogicalExpression expression, IDictionary<CellSymbol, bool> model)
        {
            return expression.Eval(model);
        }

        private static Dictionary<CellSymbol, bool> Extend(CellSymbol symbol, bool value,
            IDictionary<CellSymbol, bool> model)
        {
            return new Dictionary<CellSymbol, bool>(model) {[symbol] = value};
        }
    }
}