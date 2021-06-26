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
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        pirate currentgame;

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (player < 1)
            {
                MessageBox.Show("Choose number of player!");
            }
            else
            {
                start.Visibility = Visibility.Hidden;
                _2players.Visibility = Visibility.Hidden;
                _3players.Visibility = Visibility.Hidden;
                _4players.Visibility = Visibility.Hidden;
                face1.Visibility = Visibility.Visible;
                face2.Visibility = Visibility.Visible;
                face3.Visibility = Visibility.Visible;
                face4.Visibility = Visibility.Visible;
                reset.Visibility = Visibility.Visible;
                timer.Visibility = Visibility.Visible;
                Faces.Visibility = Visibility.Visible;
                face1_Click(sender, e);
                Button[] buttons = new Button[24];
                for (int i = 0; i < 24; i++)
                {
                    if (i < 6)
                    {
                        buttons[i] = page1.Children[i] as Button;
                    }
                    else if (i > 5 && i < 12)
                    {
                        buttons[i] = page2.Children[i - 6] as Button;
                    }
                    else if (i > 11 && i < 18)
                    {
                        buttons[i] = page3.Children[i - 12] as Button;
                    }
                    else if (i > 17)
                    {
                        buttons[i] = page4.Children[i - 18] as Button;
                    }
                }
                currentgame = new pirate(buttons, drawing, player,player_s_turn,timer);
            }
        }

        public int player;
        private void _2players_Checked(object sender, RoutedEventArgs e)
        {
            player = 2;
        }

        private void _3players_Checked(object sender, RoutedEventArgs e)
        {
            player = 3;
        }

        private void _4player_Checked(object sender, RoutedEventArgs e)
        {
            player = 4;
        }

        private void face1_Click(object sender, RoutedEventArgs e)
        {
            page1.Visibility = Visibility.Visible;
            page2.Visibility = Visibility.Hidden;
            page3.Visibility = Visibility.Hidden;
            page4.Visibility = Visibility.Hidden;
            Faces.Text = "Face 1 ";
            face1.Background = new SolidColorBrush(Colors.Red);
            face2.Background = new SolidColorBrush(Colors.DodgerBlue);
            face3.Background = new SolidColorBrush(Colors.DodgerBlue);
            face4.Background = new SolidColorBrush(Colors.DodgerBlue);

        }

        private void face2_Click(object sender, RoutedEventArgs e)
        {
            page2.Visibility = Visibility.Visible;
            page1.Visibility = Visibility.Hidden;
            page3.Visibility = Visibility.Hidden;
            page4.Visibility = Visibility.Hidden;
            Faces.Text = "Face 2 ";
            face2.Background = new SolidColorBrush(Colors.Red);
            face1.Background = new SolidColorBrush(Colors.DodgerBlue);
            face3.Background = new SolidColorBrush(Colors.DodgerBlue);
            face4.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void face3_Click(object sender, RoutedEventArgs e)
        {
            page3.Visibility = Visibility.Visible;
            page1.Visibility = Visibility.Hidden;
            page2.Visibility = Visibility.Hidden;
            page4.Visibility = Visibility.Hidden;
            Faces.Text = "Face 3 ";
            face3.Background = new SolidColorBrush(Colors.Red);
            face2.Background = new SolidColorBrush(Colors.DodgerBlue);
            face1.Background = new SolidColorBrush(Colors.DodgerBlue);
            face4.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void face4_Click(object sender, RoutedEventArgs e)
        {
            page4.Visibility = Visibility.Visible;
            page1.Visibility = Visibility.Hidden;
            page2.Visibility = Visibility.Hidden;
            page3.Visibility = Visibility.Hidden;
            Faces.Text = "Face 4 ";
            face4.Background = new SolidColorBrush(Colors.Red);
            face2.Background = new SolidColorBrush(Colors.DodgerBlue);
            face3.Background = new SolidColorBrush(Colors.DodgerBlue);
            face1.Background = new SolidColorBrush(Colors.DodgerBlue);
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            start.Visibility = Visibility.Visible;
            _2players.Visibility = Visibility.Visible;
            _3players.Visibility = Visibility.Visible;
            _4players.Visibility = Visibility.Visible;
            face1.Visibility = Visibility.Hidden;
            face2.Visibility = Visibility.Hidden;
            face3.Visibility = Visibility.Hidden;
            face4.Visibility = Visibility.Hidden;
            Faces.Visibility = Visibility.Hidden;
            reset.Visibility = Visibility.Hidden;
            page1.Visibility = Visibility.Hidden;
            page2.Visibility = Visibility.Hidden;
            page3.Visibility = Visibility.Hidden;
            page4.Visibility = Visibility.Hidden;
            currentgame.reset();
        }

    }

    public class pirate{
        private int boom;
        public Random rnd = new Random();
        private TextBlock tb ;
        private TextBlock lose;
        private TextBlock player_turn;
        private int num = 1;
        public int player;
        private int loser;
        private TextBlock timer;
        private Canvas ann;
        public List<Button> buttonList = new List<Button>();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private int counter;

        public pirate(Button[] b, Canvas c ,int player , TextBlock player_turn , TextBlock timer){
            ann = c;
            tb = ann.Children[0] as TextBlock;
            lose = ann.Children[1] as TextBlock;
            this.player = player;
            this.player_turn = player_turn;
            this.timer = timer;
            boom = rnd.Next(0, 24);
            buttonList = b.ToList<Button>();
            for (int i = 0; i < 24; i++)
                buttonList[i].Click += new RoutedEventHandler(OnMove);

            //MessageBox.Show(boom.ToString());
            player_turn.Text = "Player 1's turn";
            counter = 0;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        public void reset()
        {
            num = 1;
            boom = rnd.Next(0, 24);
            for (int i = 0; i < 24; i++)
            {
                buttonList[i].Visibility = Visibility.Visible;
                buttonList[i].Click -= new RoutedEventHandler(OnMove);
            }
            ann.Visibility = Visibility.Hidden;
            player_turn.Text = "";
            timer.Visibility = Visibility.Hidden;
        }
        public void OnMove(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Visibility = Visibility.Hidden;
            num++;
            int index = buttonList.IndexOf(button);
            turns();
            if (index == boom)
            {
                ann.Visibility = Visibility.Visible;
                lose.Visibility = Visibility.Visible;
                //Thread.Sleep(100);
                lose.Text = "Player " + loser + " lose";
                dispatcherTimer.Tick -= dispatcherTimer_Tick;
                dispatcherTimer.Stop();
            }
            else
            {
                dispatcherTimer.Tick -= dispatcherTimer_Tick;
                dispatcherTimer.Stop();
                counter = 0;
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                dispatcherTimer.Start();
            }
        }

        public void turns()
        {
            if (player == 2)
            {
                if (num % player == 0)
                {
                    loser = 1;
                    player_turn.Text = "Player 2's turn";
                }
                else
                {
                    loser = 2;
                    player_turn.Text = "Player 1's turn";
                }
            }
            else if (player == 3)
            {
                if (num % player == 0)
                {
                    loser = 2;
                    player_turn.Text = "Player 3's turn";
                }
                else if (num % player == 1)
                {
                    loser = 3;
                    player_turn.Text = "Player 1's turn";
                }
                else
                {
                    loser = 1;
                    player_turn.Text = "Player 2's turn";
                }  
            }
            else if (player == 4)
            {
                if (num % player == 0)
                {
                    loser = 3;
                    player_turn.Text = "Player 4's turn";
                }
                else if (num % player == 1)
                {
                    loser = 4;
                        player_turn.Text = "Player 1's turn";
                }
                else if (num % player == 2)
                {
                    loser = 1;
                    player_turn.Text = "Player 2's turn";
                }
                else
                {
                    loser = 2;
                    player_turn.Text = "Player 3's turn";
                } 
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            counter++;
            if(10 - counter <4)
                timer.Foreground = new SolidColorBrush(Colors.Red);
            else
                timer.Foreground = new SolidColorBrush(Colors.Black);
            if (10 - counter < 0)
            {
                dispatcherTimer.Tick -= dispatcherTimer_Tick;
                dispatcherTimer.Stop();
                ann.Visibility = Visibility.Visible;
                lose.Visibility = Visibility.Visible;
                lose.Text = "Player " + num % player + " lose";
            }
            else
                timer.Text = "Time left : " + (10 - counter).ToString() + "s ";// + boom;
        }
    }
}
