using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Minefield.xaml
    /// </summary>
    public partial class Minefield : UserControl
    {
        public Minefield()
        {
            InitializeComponent();
            this.DataContext = this;
            ResetGameDelegate += BuildMinefield;
            RemoveCellDelegate += RemoveCell;
            DealWithFlagCell += FlagCell;
            ResetGameDelegate();
        }

        public delegate void MyDelegate();
        public static MyDelegate ResetGameDelegate;
        public delegate void MyDelegate2(Cell cell);
        public static MyDelegate2 RemoveCellDelegate;
        public static MyDelegate2 DealWithFlagCell;

        public static readonly DependencyProperty DimensionsDependentProperty =
            DependencyProperty.Register("FieldDimensions",
            typeof(Tuple),
            typeof(Minefield));
            //new PropertyMetadata(OnFieldDimensionsChanged));
        public Tuple FieldDimensions
        {
            get => (Tuple)GetValue(DimensionsDependentProperty);
            set => SetValue(DimensionsDependentProperty, value);
        }

        public static readonly DependencyProperty BombCountDependentProperty =
            DependencyProperty.Register("BombCount",
            typeof(int),
            typeof(Minefield));
            //new PropertyMetadata(OnFieldDimensionsChanged));
        public int BombCount
        {
            get => (int)GetValue(BombCountDependentProperty);
            set => SetValue(BombCountDependentProperty, value);
        }

        public List<Cell> RemainingCells { get; set; } = new List<Cell>();
        public List<Cell> MinedCells { get; set; } = new List<Cell>();
        public List<Cell> FlaggedCells { get; set; } = new List<Cell>();

        public static bool ActiveGame = true;


        public ObservableCollection<Cell> CellCollection { get; set; } = new ObservableCollection<Cell>();
        
        public void BuildMinefield()
        {
            ActiveGame = true;
            CellCollection.Clear();
            RemainingCells.Clear();
            MinedCells.Clear();
            FlaggedCells.Clear();
            
            Cell[,] cellArray = new Cell[FieldDimensions.x, FieldDimensions.y];

            //populate array
            for (int y = 0; y < FieldDimensions.y; y++)
            {
                for (int x = 0; x < FieldDimensions.x; x++)
                {
                    Cell nextCell = new Cell();
                    nextCell.ArrayPosition = new(x, y);
                    cellArray[x, y] = nextCell;
                    CellCollection.Add(nextCell);
                    RemainingCells.Add(nextCell);
                }
            }
            //set mines
            Random RNG = new Random();
            int actualMineCount = (int)MathF.Round(cellArray.Length * BombCount/100);
            for (int i = 0; i < actualMineCount; i++)
            {
                Cell setCell = RemainingCells[RNG.Next(RemainingCells.Count)];
                setCell.IsBomb = true;
                MinedCells.Add(setCell);
                RemainingCells.Remove(setCell);
            }

            //set neighbors
            foreach (Cell cell in cellArray)
            {
                List<Cell> neighborsList = new List<Cell>();

                //position -1 to position +1
                Tuple minMaxX = new(cell.ArrayPosition.x - 1, cell.ArrayPosition.x + 2);
                Tuple minMaxY = new(cell.ArrayPosition.y - 1, cell.ArrayPosition.y + 2);

                for (int y = minMaxY.x; y < minMaxY.y; y++)
                {
                    if (y < 0 || y > cellArray.GetLength(1) - 1)
                        continue;
                    for (int x = minMaxX.x; x < minMaxX.y; x++)
                    {
                        if (x < 0 || x > cellArray.GetLength(0) - 1)
                            continue;
                        if (!(cellArray[x, y] == cell))
                        {
                            neighborsList.Add(cellArray[x, y]);
                        }
                    }
                }
                //add neighbor list to cell
                cell.SetNeighbors(neighborsList);
            }
        }
        private void RemoveCell(Cell cell)
        {
            if (cell.IsBomb)
            {
                EndGame("Boom!");
                return;
            }
            RemainingCells.Remove(cell);
            if (RemainingCells.Count > 0)
                return;
            EndGame("WIN");
        }
        private void FlagCell(Cell cell)
        {
            //do I really need to track this?
        }

        private void EndGame(string type)
        {
            ActiveGame = false;
            MessageBox.Show(type);
        }

    }
}
