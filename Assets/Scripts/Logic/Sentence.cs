using System;
using System.Collections.Generic;
using static Logic.BoolLogic;

namespace Logic
{
    public class Sentence
    {
        private List<CellSymbol> symbols = new List<CellSymbol>();
        private LogicalExpression root;

        public void Add(LogicalExpression expression)
        {
            if (root == null)
            {
                root = expression;
            }
            else
            {
                var left = new List<LogicalExpression> {root};
                var right = new List<LogicalExpression> {expression};
                LogicalExpression and = new LogicalExpression(left, AND, right);
                root = and;
            }
        }

        public bool Eval(IDictionary<CellSymbol, bool> model)
        {
            throw new System.NotImplementedException();
        }

        public List<CellSymbol> GetSymbols()
        {
            return symbols;
        }

        public class LogicalExpression
        {
            private CellSymbol symbol;
            private List<LogicalExpression> left;
            private BoolLogic logic;
            private List<LogicalExpression> right;

            public LogicalExpression(CellSymbol symbol)
            {
                this.symbol = symbol;
            }

            public LogicalExpression(List<LogicalExpression> left, BoolLogic logic, List<LogicalExpression> right)
            {
                if (logic == NOT)
                {
                    throw new Exception("NOT is unary");
                }

                this.left = left;
                this.logic = logic;
                this.right = right;
            }

            public LogicalExpression(BoolLogic logic, List<LogicalExpression> expr)
            {
                this.logic = logic;
                right = expr;
            }
        }
    }
}