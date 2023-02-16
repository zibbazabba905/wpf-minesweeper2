using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sweeper06
{
    /// <summary>
    /// Interaction logic for Cell.xaml
    /// </summary>
    public partial class Cell : UserControl, INotifyPropertyChanged
    {
        public Cell()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string cellLabel;
        public string CellLabel
        {
            get { return cellLabel; }
            set
            {
                if (cellLabel != value)
                {
                    cellLabel = value;
                    RaisePropertyChanged("CellLabel");
                }
            }
        }

        public int KeyNumber { get; set; }
        public Tuple ArrayPosition { get; set; }
        public List<Cell> Neighbors { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public bool Visited { get; set; }


        public bool LeftMouseButtonHeld { get; set; }
        public bool RightMouseButtonHeld { get; set; }


        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            VisitCell();
        }
        private void CellButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                LeftMouseButtonHeld = true;
            if (e.ChangedButton == MouseButton.Right)
                RightMouseButtonHeld = true;
        }

        private void CellButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LeftMouseButtonHeld && RightMouseButtonHeld)
            {
                DoChording();
            }
            if (e.ChangedButton == MouseButton.Left)
                LeftMouseButtonHeld = false;
            if (e.ChangedButton == MouseButton.Right)
                RightMouseButtonHeld = false;
        }
        private void CellButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ToggleFlag();
        }
        public void SetNeighbors(List<Cell> inputList)
        {
            Neighbors = inputList;
            KeyNumber = BombCount();
        }
        private int BombCount()
        {
            int count = -0;
            foreach (Cell cell in Neighbors)
            {
                if (!cell.IsBomb)
                    continue;
                count++;
            }
            return count;
        }
        private void VisitCell()
        {
            if (Minefield.ActiveGame == false)
                return;

            Visited = true;
            if (IsBomb)
            {
                ColorCell(Brushes.DarkRed);
                RemoveFromRemainingCells();
                return;
            }
            ColorCell(Brushes.DarkGray);
            RemoveFromRemainingCells();

            if (KeyNumber != 0)
            {
                CellLabel = $"{KeyNumber}";
                return;
            }
            VisitNeighbors();
        }
        private void ColorCell(Brush color)
        {
            CellButton.Background = color;
        }
        private void RemoveFromRemainingCells()
        {
            Minefield.RemoveCellDelegate(this);
        }
        private void ToggleFlag()
        {
            if (Visited)
                return;
            IsFlagged = !IsFlagged;
            Minefield.DealWithFlagCell(this);//Might not need to track this
            ColorCell(IsFlagged ? Brushes.Red : Brushes.LightGray);
        }
        private void DoChording()
        {
            int flagcount = 0;
            foreach (Cell cell in Neighbors)
            {
                if (!cell.IsFlagged)
                    continue;
                flagcount++;
            }
            if (!(flagcount == KeyNumber))
                return;
            VisitNeighbors();
        }
        private void VisitNeighbors()
        {
            foreach (Cell cell in Neighbors)
            {
                if (cell.Visited || cell.IsFlagged)
                    continue;
                cell.VisitCell();
            }
        }
    }
}
