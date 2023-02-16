using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public List<GridSizeComponent> BoardSizes { get; set; } = new List<GridSizeComponent>
        {
            new GridSizeComponent("Small", new Tuple(10, 10)),
            new GridSizeComponent("Medium",new Tuple(20, 20)),
            new GridSizeComponent("Large", new Tuple(40, 30)),
        };


        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Minefield.ResetGameDelegate();
        }

        private void SizeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Minefield.ResetGameDelegate();
        }

        private void BombCountSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Minefield.ResetGameDelegate();

        }
    }
}
