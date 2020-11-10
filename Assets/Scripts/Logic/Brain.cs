﻿﻿namespace Logic
{
    public class Brain
    {
        private readonly KnowledgeBase kb = new KnowledgeBase();

        public void Tell(ILogicalExpression expr)
        {
            kb.Add(expr);
        }

        public bool Ask(ILogicalExpression query)
        {
            return Entailment.TTEntails(kb, query);
        }
    }
}