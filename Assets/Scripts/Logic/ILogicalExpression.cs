﻿using System.Collections.Generic;

namespace Logic
{
    public interface ILogicalExpression
    {
        bool Eval(IDictionary<CellSymbol, bool> model);

        List<CellSymbol> GetSymbols();
    }
}