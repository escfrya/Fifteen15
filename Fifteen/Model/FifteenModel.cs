using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace Fifteen.Model
{
    public class FifteenModel
    {
        public const int AreaSize = 4;
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
            if (Cells[0].Row == 0 && Cells[0].Column == 0 &&
            Cells[1].Row == 0 && Cells[1].Column == 1 &&
            Cells[2].Row == 0 && Cells[2].Column == 2 &&
            Cells[3].Row == 0 && Cells[3].Column == 3 &&
            Cells[4].Row == 1 && Cells[4].Column == 0 &&
            Cells[5].Row == 1 && Cells[5].Column == 1 &&
            Cells[6].Row == 1 && Cells[6].Column == 2 &&
            Cells[7].Row == 1 && Cells[7].Column == 3 &&
            Cells[8].Row == 2 && Cells[8].Column == 0 &&
            Cells[9].Row == 2 && Cells[9].Column == 1 &&
            Cells[10].Row == 2 && Cells[10].Column == 2 &&
            Cells[11].Row == 2 && Cells[11].Column == 3 &&
            Cells[12].Row == 3 && Cells[12].Column == 0 &&
            Cells[13].Row == 3 && Cells[13].Column == 1 &&
            Cells[14].Row == 3 && Cells[14].Column == 2)
                return true;
            return false;
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
                                          for (int i = 0; i < 200; i++)
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
            Cells[0].Row = 0; Cells[0].Column = 0;
            Cells[1].Row = 0; Cells[1].Column = 1;
            Cells[2].Row = 0; Cells[2].Column = 2;
            Cells[3].Row = 0; Cells[3].Column = 3;
            Cells[4].Row = 1; Cells[4].Column = 0;
            Cells[5].Row = 1; Cells[5].Column = 1;
            Cells[6].Row = 1; Cells[6].Column = 2;
            Cells[7].Row = 1; Cells[7].Column = 3;
            Cells[8].Row = 2; Cells[8].Column = 0;
            Cells[9].Row = 2; Cells[9].Column = 1;
            Cells[10].Row = 2; Cells[10].Column = 2;
            Cells[11].Row = 2; Cells[11].Column = 3;
            Cells[12].Row = 3; Cells[12].Column = 0;
            Cells[13].Row = 3; Cells[13].Column = 1;
            Cells[14].Row = 3; Cells[14].Column = 2;
            NullCell.Row = 3; NullCell.Column = 3;
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
