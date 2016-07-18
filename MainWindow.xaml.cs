using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Boggle 
{

    class Puzzle
    {
        string _tile00;
        string _tile01;
        string _tile02;
        string _tile03;
        string _tile10;
        string _tile11;
        string _tile12;
        string _tile13;
        string _tile20;
        string _tile21;
        string _tile22;
        string _tile23;
        string _tile30;
        string _tile31;
        string _tile32;
        string _tile33;
        string _alltiles;

        public string tile00
        {
            get { return _tile00; }
            set { _tile00 = value; }
        }
        public string tile01
        {
            get { return _tile01; }
            set { _tile01 = value; }
        }
        public string tile02
        {
            get { return _tile02; }
            set { _tile02 = value; }
        }
        public string tile03
        {
            get { return _tile03; }
            set { _tile03 = value; }
        }
        public string tile10
        {
            get { return _tile10; }
            set { _tile10 = value; }
        }
        public string tile11
        {
            get { return _tile11; }
            set { _tile11 = value; }
        }
        public string tile12
        {
            get { return _tile12; }
            set { _tile12 = value; }
        }
        public string tile13
        {
            get { return _tile13; }
            set { _tile13 = value; }
        }
        public string tile20
        {
            get { return _tile20; }
            set { _tile20 = value; }
        }
        public string tile21
        {
            get { return _tile21; }
            set { _tile21 = value; }
        }
        public string tile22
        {
            get { return _tile22; }
            set { _tile22 = value; }
        }
        public string tile23
        {
            get { return _tile23; }
            set { _tile23 = value; }
        }
        public string tile30
        {
            get { return _tile30; }
            set { _tile30 = value; }
        }
        public string tile31
        {
            get { return _tile31; }
            set { _tile31 = value; }
        }
        public string tile32
        {
            get { return _tile32; }
            set { _tile32 = value; }
        }
        public string tile33
        {
            get { return _tile33; }
            set { _tile33 = value; }
        }
        public string alltiles
        {
            get { return _alltiles; }
            set { _alltiles = value; }
        }

        public Puzzle(string letters)
        {
            _tile00 = letters[0].ToString();
            _tile01 = letters[1].ToString();
            _tile02 = letters[2].ToString();
            _tile03 = letters[3].ToString();
            _tile10 = letters[4].ToString();
            _tile11 = letters[5].ToString();
            _tile12 = letters[6].ToString();
            _tile13 = letters[7].ToString();
            _tile20 = letters[8].ToString();
            _tile21 = letters[9].ToString();
            _tile22 = letters[10].ToString();
            _tile23 = letters[11].ToString();
            _tile30 = letters[12].ToString();
            _tile31 = letters[13].ToString();
            _tile32 = letters[14].ToString();
            _tile33 = letters[15].ToString();
            _alltiles = letters;
        }


    }

    class Die
    {
        string _chars;
        public string chars
        {
            get { return _chars; }
            set { _chars = value; }
        }
    }
    class Word
    {
        string _theword;
        int _wordscore;

        public string TheWord
        {
            get { return _theword; }
            set { _theword = value; }
        }
        public int WordScore
        {
            get { return _wordscore; }
            set { _wordscore = value; }
        }
        public Word(string w, int s)
        {
            this._theword = w;
            this._wordscore = s;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataLayer db = new DataLayer();
        DispatcherTimer _timer;
        TimeSpan _time;
        bool GameOn;
        static Random _random = new Random();
        static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        string[] DieRollsList = new string[16];
        string[] diechars = new string[] { "AAEEGN", "ELRTTY", "AOOTTW", "ABBJOO", "EHRTVW", "CIMOTU", "DISTTY", "EIOSST", "DELRVY", "ACHOPS", "HIMNQU", "EEINSU", "EEGHNW", "AFFKPS", "HLNNRZ", "DEILRX" };
        List<string> BoggleDictionary = new List<string>();
        List<Word> ListOfWordsFoundInCurrentPuzzle = new List<Word>();
        List<int[]> ListOfWordsFoundAsYouType = new List<int[]>();
        Puzzle CurrentPuzzle;
        string[,] Puzzle2DArray = new string[4, 4];
        int? PreviousTileClicked;
        static bool ClickOn = false;

        private void LoadDictionary()
        {
            string[] DictionaryWords = System.IO.File.ReadAllLines(@"enable1.txt");
            foreach (string dictionaryword in DictionaryWords)
            {
                BoggleDictionary.Add(dictionaryword);
            }
        }

        private void GenerateNewPuzzle()
        {
            ResetAllToWhite();
            TypedWord.Text = "";
            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                Die d = new Die();
                d.chars = diechars[i];
                int ind = r.Next(0, 6);
                DieRollsList[i] = d.chars[ind].ToString();
            }
            Shuffle(DieRollsList);
            int DieNO = 0;
            foreach (Label lb in DieGrid.Children)
            {
                if (DieRollsList[DieNO] == "Q")
                {
                    lb.Content = "Qu";
                }
                else
                {
                    lb.Content = DieRollsList[DieNO];
                }
                DieNO++;
            }
            string PuzzleString = String.Join("", DieRollsList);
            CurrentPuzzle = new Puzzle(PuzzleString);
            WordsFoundListView.Items.Clear();
            ListOfWordsFoundInCurrentPuzzle.Clear();
            CurrentScoreLabel.Content = 0;
            HighScoreLabel.Content = 0;
            HighScoreName.Content = "";
        }
        public void GenerateNewPuzzle(string letters, int hscore, string hname)
        {
            ResetAllToWhite();
            TypedWord.Text = "";
            for (int i = 0; i < 16; i++)
            {
                DieRollsList[i] = letters[i].ToString();
            }
            int DieNO = 0;
            foreach (Label lb in DieGrid.Children)
            {
                if (DieRollsList[DieNO] == "Q")
                {
                    lb.Content = "Qu";
                }
                else
                {
                    lb.Content = DieRollsList[DieNO];
                }
                DieNO++;
            }
            HighScoreLabel.Content = hscore;
            HighScoreName.Content = hname;
            string PuzzleString = String.Join("", DieRollsList);
            CurrentPuzzle = new Puzzle(PuzzleString);
            WordsFoundListView.Items.Clear();
            ListOfWordsFoundInCurrentPuzzle.Clear();
            CurrentScoreLabel.Content = 0;
            StartTimer();
            TypedWord.Focus();
        }

        private void StartTimer()
        {
            TimeLabel.Content = "2:00";
            _time = TimeSpan.FromSeconds(120);
            TimeLabel.Foreground = Brushes.Green;
            if (GameOn == true)
            {
                _timer.Stop();
            }
            else
            {
                GameOn = true;
            }
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    GameOn = false;
                    SetAllToRed();
                    SaveGame();
                }
                else
                {
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    TimeLabel.Content = _time.ToString(@"m\:ss");
                    TimeSpan _timecheckone = TimeSpan.FromSeconds(30);
                    TimeSpan _timechecktwo = TimeSpan.FromSeconds(10);
                    if (_time == _timecheckone)
                    {
                        TimeLabel.Foreground = Brushes.Orange;
                    }
                    if (_time == _timechecktwo)
                    {
                        TimeLabel.Foreground = Brushes.Red;
                    }
                }
            }, Application.Current.Dispatcher);
            _timer.Start();
        }

        private bool ValidateWord(string wq, string woq)
        {
            if (BoggleDictionary.Contains(wq))
            {
                bool validinboard = HighLightWord(woq);
                return validinboard;
            }
            else
            {
                BadWordsAreRed();
                return false;
            }
        }

        private void BadWordsAreRed()
        {
            foreach (Label lb in DieGrid.Children)
            {
                if (lb.Background == Brushes.LimeGreen)
                {
                    lb.Background = Brushes.Red;
                }
            }
        }

        private void ResetAllToWhite()
        {
            foreach (Label lb in DieGrid.Children)
            {
                lb.Background = Brushes.White;
            }
        }

        private void SetAllToRed()
        {
            foreach (Label lb in DieGrid.Children)
            {
                lb.Background = Brushes.Red;
            }
        }

        private bool HighLightWord(string w)
        {
            bool result = false;
            bool WereDoneHere = false;
            string WordToHighLight = w;
            ListOfWordsFoundAsYouType.Clear();
            if (GameOn == true)
            {
                ResetAllToWhite();
                if (WordToHighLight.Length > 0)
                {
                    int i = 0;
                    foreach (char c in CurrentPuzzle.alltiles)
                    {
                        int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                        if (c == WordToHighLight[0])
                        {
                            WordFound[i] = 0;
                            ListOfWordsFoundAsYouType.Add(WordFound);
                        }
                        i++;
                    }
                    for (int j = 1; j < WordToHighLight.Length; j++)
                    {
                        result = false;
                        List<int[]> NewWordsCollector = new List<int[]>();
                        // for each letter user types
                        foreach (int[] WordInProgress in ListOfWordsFoundAsYouType)
                        {
                            // take each word being remembered
                            if (WordInProgress.Contains(j - 1))
                            {
                                // if it contains the preveous char
                                for (int k = 0; k < 16; k++) // k represents char in the puzzle
                                {
                                    if (WordInProgress[k] == j - 1)
                                    {
                                        // go over each char in the puzzle AND....
                                        if (k <= 11) //CHECK RIGHT
                                        {
                                            int checkdiff = k + 4;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                        if (k % 4 < 3) //CHECK DOWN
                                        {
                                            int checkdiff = k + 1;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                        if (k >= 4) //CHECK LEFT
                                        {
                                            int checkdiff = k - 4;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                        if (k % 4 > 0) //CHECK UP
                                        {
                                            int checkdiff = k - 1;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }

                                        if ((k >= 4) && (k % 4 > 0)) //CHECK LEFT UP
                                        {
                                            int checkdiff = k - 5;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                        if ((k >= 4) && (k % 4 < 3)) //CHECK LEFT DOWN
                                        {
                                            int checkdiff = k - 3;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                        if ((k <= 11) && (k % 4 > 0)) //CHECK RIGHT UP
                                        {
                                            int checkdiff = k + 3;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                        if ((k <= 11) && (k % 4 < 3)) //CHECK RIGHT DOWN
                                        {
                                            int checkdiff = k + 5;
                                            if ((WordToHighLight[j] == CurrentPuzzle.alltiles[checkdiff]) && WordInProgress[checkdiff] == 99)
                                            {
                                                int[] WordFound = { 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, 99, };
                                                Array.Copy(WordInProgress, WordFound, 16);
                                                WordFound[checkdiff] = j;
                                                result = true;
                                                NewWordsCollector.Add(WordFound);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (int[] newword in NewWordsCollector)
                        {
                            ListOfWordsFoundAsYouType.Add(newword);
                        }
                    }
                }
                foreach (int[] WordInProgress in ListOfWordsFoundAsYouType)
                {
                    if ((WordInProgress.Contains(WordToHighLight.Length - 1)) && (WereDoneHere == false))
                    {
                        for (int k = 0; k < 16; k++)
                        {
                            if (WordInProgress[k] != 99)
                            {
                                switch (k)
                                {
                                    case 0:
                                        die00.Background = Brushes.LimeGreen;
                                        break;
                                    case 1:
                                        die01.Background = Brushes.LimeGreen;
                                        break;
                                    case 2:
                                        die02.Background = Brushes.LimeGreen;
                                        break;
                                    case 3:
                                        die03.Background = Brushes.LimeGreen;
                                        break;
                                    case 4:
                                        die10.Background = Brushes.LimeGreen;
                                        break;
                                    case 5:
                                        die11.Background = Brushes.LimeGreen;
                                        break;
                                    case 6:
                                        die12.Background = Brushes.LimeGreen;
                                        break;
                                    case 7:
                                        die13.Background = Brushes.LimeGreen;
                                        break;
                                    case 8:
                                        die20.Background = Brushes.LimeGreen;
                                        break;
                                    case 9:
                                        die21.Background = Brushes.LimeGreen;
                                        break;
                                    case 10:
                                        die22.Background = Brushes.LimeGreen;
                                        break;
                                    case 11:
                                        die23.Background = Brushes.LimeGreen;
                                        break;
                                    case 12:
                                        die30.Background = Brushes.LimeGreen;
                                        break;
                                    case 13:
                                        die31.Background = Brushes.LimeGreen;
                                        break;
                                    case 14:
                                        die32.Background = Brushes.LimeGreen;
                                        break;
                                    case 15:
                                        die33.Background = Brushes.LimeGreen;
                                        break;
                                }
                                WereDoneHere = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void TileClick(object sender, RoutedEventArgs e)
        {
            if ((GameOn == true) && (ClickOn == true))
            {
                if (TypedWord.Text.Length == 0)
                {
                    ResetAllToWhite();
                    PreviousTileClicked = null;
                }
                Label lb = (Label)sender;
                if (lb.Background == Brushes.White)
                {
                    int tilepos = int.Parse(lb.Name.Substring(3));
                    if ((TypedWord.Text.Length == 0) ||
                        (tilepos == PreviousTileClicked + 1) ||
                        (tilepos == PreviousTileClicked + 10) ||
                        (tilepos == PreviousTileClicked + 11) ||
                        (tilepos == PreviousTileClicked + 9) ||
                        (tilepos == PreviousTileClicked - 1) ||
                        (tilepos == PreviousTileClicked - 10) ||
                        (tilepos == PreviousTileClicked - 11) ||
                        (tilepos == PreviousTileClicked - 9))
                    {
                        TypedWord.Text = TypedWord.Text + lb.Content.ToString();
                        lb.Background = Brushes.LimeGreen;
                        PreviousTileClicked = tilepos;
                    }
                }
                else if (lb.Background == Brushes.LimeGreen)
                {
                    ResetAllToWhite();
                    PreviousTileClicked = null;
                    TypedWord.Text = "";
                }
            }
        }

        private void AddWordToScore(string w)
        {
            w = TypedWord.Text.ToUpper();
            int score = 0;
            int totalscore = 0;
            if (w.Length < 5)
            {
                score = 1;
            }
            else if (w.Length < 6)
            {
                score = 2;
            }
            else if (w.Length < 7)
            {
                score = 3;
            }
            else if (w.Length < 8)
            {
                score = 5;
            }
            else
            {
                score = 11;
            }
            Word word = new Word(w, score);
            List<string> TempCheck = new List<string>();
            foreach (Word wt in ListOfWordsFoundInCurrentPuzzle)
            {
                TempCheck.Add(wt.TheWord);
            }
            if (!TempCheck.Contains(w))
            {
                ListOfWordsFoundInCurrentPuzzle.Add(word);
                WordsFoundListView.Items.Add(word);
                foreach (Word ws in ListOfWordsFoundInCurrentPuzzle)
                {
                    totalscore = totalscore + ws.WordScore;
                }
                CurrentScoreLabel.Content = totalscore;
            }
            WordsFoundListView.ScrollIntoView(word);
        }

        public MainWindow()
        {
            InitializeComponent();
         
            LoadDictionary();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPuzzle();
            StartTimer();
            TypedWord.Focus();
        }

        private void OKBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ClickOn == true)
            {
                if (BoggleDictionary.Contains(TypedWord.Text.ToUpper()))
                {
                    AddWordToScore(TypedWord.Text.ToUpper());
                }
                else
                {
                    BadWordsAreRed();
                }
                TypedWord.Text = "";
            }
            else if (ClickOn == false)
            {
                string WordWithoutQU = TypedWord.Text.ToUpper().Replace("QU", "Q");
                HighLightWord(WordWithoutQU);
                if ((GameOn == true) && (TypedWord.Text.Length > 2))
                {
                    string WordInput = TypedWord.Text.ToUpper();
                    bool valid = ValidateWord(WordInput, WordWithoutQU);
                    if (valid == true)
                    {
                        AddWordToScore(WordInput);
                    }
                    TypedWord.Text = "";
                }
            }
        }

        private void TypedWord_KeyUp(object sender, KeyEventArgs e)
        {
            if (ClickOn == false)
            {
                string WordWithoutQU = TypedWord.Text.ToUpper().Replace("QU", "Q");
                HighLightWord(WordWithoutQU);
                if ((e.Key == Key.Enter) && (GameOn == true) && (TypedWord.Text.Length > 2))
                {
                    string WordInput = TypedWord.Text.ToUpper();
                    bool valid = ValidateWord(WordInput, WordWithoutQU);
                    if (valid == true)
                    {
                        AddWordToScore(WordInput);
                    }
                    TypedWord.Text = "";
                }
            }
        }

    
        private void SaveGame()
        {
            bool GameExists = false;
            int CurrentBoardID = 0;
            DataLayer db = new DataLayer();
            SqlConnection Connection = db.Connect();
            string PlayerName = "Anonymous";
            using (SqlCommand cmd = new SqlCommand("SELECT Letters, ID FROM Boards", Connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) == CurrentPuzzle.alltiles)
                        {
                            GameExists = true;
                            CurrentBoardID = reader.GetInt32(1);
                        }
                    }
                }
            }
            if (GameExists == false)
            {
                db.AddBoard(CurrentPuzzle.alltiles);
                using (SqlCommand cmd = new SqlCommand("SELECT Letters, ID FROM Boards", Connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0) == CurrentPuzzle.alltiles)
                            {
                                GameExists = true;
                                CurrentBoardID = reader.GetInt32(1);
                            }
                        }
                    }
                }
            }
            if (PlayerNameInput.Text != "Enter Your Name")
            {
                PlayerName = PlayerNameInput.Text;
            }
            db.AddScore(PlayerName, int.Parse(CurrentScoreLabel.Content.ToString()), CurrentBoardID);
          }

        private void ToggleClick_Click(object sender, RoutedEventArgs e)
        {
            ClickOn = !ClickOn;
            if (ClickOn == true)
            {
                foreach (Label lb in DieGrid.Children)
                {
                    lb.Cursor = Cursors.Hand;
                    OKBTN.Visibility = Visibility.Visible;
                    TypedWord.Width = 177;
                    TypedWord.Margin = new Thickness(284, 35, 0, 0);
                }
            }
            else
            {
                foreach (Label lb in DieGrid.Children)
                {
                    lb.Cursor = Cursors.Arrow;
                    OKBTN.Visibility = Visibility.Collapsed;
                    TypedWord.Width = 217;
                    TypedWord.Margin = new Thickness(244, 35, 0, 0);
                }
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPuzzle();
            StartTimer();
            TypedWord.Focus();
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            db.Connect();
            GameChooser gc = new GameChooser(this);
            gc.Owner = this;
            gc.ShowDialog();
        }

        private void ExitClick_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
