using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Logic
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Entailment
    {
        public bool TTEntails(LogicalExpression kb, LogicalExpression query)
        {
            var kbSymbols = kb.GetSymbols();
            var querySymbols = query.GetSymbols();
            if (querySymbols.Any(s => !kbSymbols.Contains(s)))
            {
                throw new Exception("Query uses symbols that are absent in the knowledge base");
            }
        
            return TTCheckAll(kb, query, kbSymbols, new Dictionary<string, bool>());
        }

        private bool TTCheckAll(LogicalExpression kb, LogicalExpression query, 
            IReadOnlyList<string> symbols, IDictionary<string, bool> model)
        {
            if (symbols.Count == 0)
            {
                if (PLTrue(kb, model))
                    return PLTrue(query, model);

                return true;
            }

            string p = symbols[0];
            var rest = new List<string>(symbols);
            rest.RemoveAt(0);
        
            return TTCheckAll(kb, query, rest, Extend(p, true, model)) && 
                   TTCheckAll(kb, query, rest, Extend(p, false, model));
        }

        private static bool PLTrue(LogicalExpression expression, IDictionary<string, bool> model)
        {
            return expression.Eval(model);
        }

        private static Dictionary<string, bool> Extend(string symbol, bool value, IDictionary<string, bool> model)
        {
            return new Dictionary<string, bool>(model) {[symbol] = value};
        }
    }
}