using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Logic
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Entailment
    {
        public bool TTEntails(Sentence kb, Sentence query)
        {
            var kbSymbols = kb.GetSymbols();
            var querySymbols = query.GetSymbols();
            if (querySymbols.Any(s => !kbSymbols.Contains(s)))
            {
                throw new Exception("Query uses symbols that are absent in the knowledge base");
            }

            return TTCheckAll(kb, query, kbSymbols, new Dictionary<CellSymbol, bool>());
        }

        private bool TTCheckAll(Sentence kb, Sentence query,
            List<CellSymbol> symbols, IDictionary<CellSymbol, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PLTrue(kb, model))
                    return PLTrue(query, model);

                return true;
            }

            CellSymbol p = symbols[0];
            var rest = new List<CellSymbol>(symbols);
            rest.RemoveAt(0);

            return TTCheckAll(kb, query, rest, Extend(p, true, model)) &&
                   TTCheckAll(kb, query, rest, Extend(p, false, model));
        }

        private static bool PLTrue(Sentence expression, IDictionary<CellSymbol, bool> model)
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