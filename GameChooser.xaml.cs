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
using System.Windows.Shapes;

namespace Boggle
{
    /// <summary>
    /// Interaction logic for GameChooser.xaml
    /// </summary>
    public partial class GameChooser : Window
    {
        private MainWindow mw;
        DataLayer db = new DataLayer();

        public GameChooser(MainWindow mw)
        {
            this.mw = mw;
            InitializeComponent();
            db.Connect();
            List<Board> board = db.GetAllBoards();
            BoardsGrid.ItemsSource = board;
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            Board b = (Board) BoardsGrid.SelectedItem;
            mw.GenerateNewPuzzle(b.Letters, b.HScore, b.HScoreName);
            this.Close();
        }

      
    }
}
