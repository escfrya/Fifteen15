using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace Fifteen.Model
{
    public class FifteenModel
    {
        public const int AreaSize = 4;
        private const int RandomStepCount = 50;

        public Cell[] Cells { get; set; }
        public Cell NullCell { get; set; }
        public DelegateCommand<Cell> ClickCommand { get; set; }
        public FifteenModel()
        {
            ClickCommand = new DelegateCommand<Cell>(Clicked);
            Cells = new Cell[AreaSize * AreaSize - 1];
            NullCell = new Cell(0);
            CreateArea();
            SetPositions();
            SetRandomArea();
        }


        private bool CellCanMove(Cell cell)
        {
            return Math.Abs(cell.Column - NullCell.Column) + Math.Abs(cell.Row - NullCell.Row) == 1;
        }

        private void Clicked(Cell cell)
        {
            Clicked(cell, true);
        }

        private void Clicked(Cell cell, bool userCall)
        {
            if (!CellCanMove(cell))
                return;
            var nullCellRow = NullCell.Row;
            var nullCellColumn = NullCell.Column;
            NullCell.Row = cell.Row;
            NullCell.Column = cell.Column;
            cell.Row = nullCellRow;
            cell.Column = nullCellColumn;
            if (userCall && Finished())
                GameFinished();
        }

        private bool Finished()
        {
            int index = 0;
            for (int i = 0; i < AreaSize; i++)
            {
                for (int j = 0; j < AreaSize; j++)
                {
                    if (index == AreaSize * AreaSize - 1)
                        return true;
                    if (Cells[index].Row != i || Cells[index].Column != j)
                        return false;
                    index++;
                }
            }
            return true;
        }

        private void GameFinished()
        {
            MessageBox.Show("УРА!!!!");
            SetRandomArea();
        }

        private void SetRandomArea()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          for (int i = 0; i < RandomStepCount; i++)
                                          {
                                              Thread.Sleep(20);
                                              SetRandomStep();
                                          }
                                      });

        }

        private void SetRandomStep()
        {
            var random = new Random();
            int row = NullCell.Row;
            int column = NullCell.Column;
            switch (random.Next(0,4))
            {
                case 0:
                    row += 1;
                    break;
                case 1:
                    row -= 1;
                    break;
                case 2:
                    column += 1;
                    break;
                case 3:
                    column -= 1;
                    break;
                default:
                    return;
            }
            if (row < 0) row = 1;
            if (row == AreaSize) row -= 2;
            if (column < 0) column = 1;
            if (column == AreaSize) column -= 2;
            Cell cell = Cells.First(x => x.Column == column && x.Row == row);
            Clicked(cell, false);
        }

        private void SetPositions()
        {
            NullCell.Row = AreaSize - 1; 
            NullCell.Column = AreaSize -1;
            int index = 0;
            for (int i = 0; i < AreaSize; i++)
            {
                for (int j = 0; j < AreaSize; j++)
                {
                    if (index == AreaSize * AreaSize - 1)
                        return;     
                    Cells[index].Row = i;
                    Cells[index].Column = j;
                    index++;
                }
            }
            
        }

        private void CreateArea()
        {
            for (int i = 0; i < 15; i++)
            {
                Cells[i] = new Cell(i + 1);
            }
        }

    }

    public class Cell : INotifyPropertyChanged
    {
        public int Value { get; private set; }

        private int column;
        public int Column
        {
            get { return column; }
            set
            {
                column = value;
                OnPropertyChanged("Column");
            }
        }

        private int row;
        public int Row
        {
            get { return row; }
            set
            { 
                row = value;
                OnPropertyChanged("Row");
            }
        }
        public Cell(int value)
        {
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
