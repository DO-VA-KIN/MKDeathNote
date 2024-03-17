using Prism.Mvvm;


namespace Snake.Core
{
    internal class CellVM : BindableBase
    {
        public ushort Row { get; }
        public ushort Column { get; }

        private CellType _cellType;
        public CellType CellType
        {
            get => _cellType;
            set
            {
                _cellType = value;
                RaisePropertyChanged(nameof(CellType));
            }
        }

        public CellVM(ushort row, ushort column)
        {
            Row = row;
            Column = column;
        }


    }
}
