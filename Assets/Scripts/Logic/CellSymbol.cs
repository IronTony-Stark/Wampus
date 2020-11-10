namespace Logic
{
    public class CellSymbol
    {
        public readonly int index;
        public readonly Symbol symbol;

        public CellSymbol(int index, Symbol symbol)
        {
            this.index = index;
            this.symbol = symbol;
        }
    }
}