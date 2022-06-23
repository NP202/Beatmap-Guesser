using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beatmap_Guesser
{
    public partial class HomeScreen : Form
    {

        public GameDisplay gameDisplay;
       // public Player player;
        public static bool first_game_flag = true;
        public HomeScreen()
        {
            InitializeComponent();
            Shown += Form3_Show1;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }
        private void Form3_Show1(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
  
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("No difficulty selected! Please select a difficulty and try again.");
            }
            else // difficulty selected, login flow
            {

                if (GameDisplay.player != null && GameDisplay.player.Name != "Guest")//player has already logged in before and is currently logged-in
                {
                    //no need to set player again - stay logged in to previous user
                    Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
                    gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString());

                    ContainerForm cf = ContainerForm.GetInstance();

                    cf.AddFormAndShow(gameDisplay);

                    this.Close();

                }
                else 
                { 
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                bool login_response = loginForm.verifyLogin();

                    if (login_response)//successful login
                    {

                        GameDisplay.player = loginForm.logged_in_player;
                        Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
                        gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString());
                        ContainerForm cf = ContainerForm.GetInstance();

                        cf.AddFormAndShow(gameDisplay);


                        this.Close();

                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("No difficulty selected! Please select a difficulty and try again.");
            }
            else
            {
                GameDisplay.player = new Player("Guest");
                Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
                gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString());

                ContainerForm cf = ContainerForm.GetInstance();

                cf.AddFormAndShow(gameDisplay);

                this.Close();

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void showHelp()
        {
            MessageBox.Show("Hi! Welcome to the osu! Beatmap Guesser, a game where you guess the beatmap based on the provided background!"
               + "\n\nThere are two ways to begin play - Login and Play As Guest. Login requires a username and password combination, " +
               "and allows you to save and view statistics for all games you've played previously."
               + "\n\nTo start, please select a difficulty, how many songs you would like per-round, and choose whether you would like to Play As Guest or Login."
               + "\n\nIf you would like to see these instructions again, click the Help button!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showHelp();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            SongHandler.MAX_SONGS = (int)songCounter.Value;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (GameDisplay.player != null && GameDisplay.player.Name != "Guest")
            {
                int correct_lifetime = GameDisplay.player.CorrectlyGuessed;
                int total_lifetime = GameDisplay.player.TotalGuessed;
                double guess_percent;

                    if (total_lifetime != 0)
                {
                    guess_percent = (double)correct_lifetime / total_lifetime * 100;
                }
                else
                {
                    guess_percent = 0;
                }

                MessageBox.Show("Statistics for " + GameDisplay.player.Name + ".\n" + "===================================\n\n" + "Total Songs Guessed Correctly: " + correct_lifetime + "\n\n" +
                    "Total Songs Shown: " + total_lifetime + "\n\n" + "Overall Guess Percent: " + guess_percent + "%");
            }
            else
            {
                MessageBox.Show("You are not logged in!");
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {

            if (GameDisplay.player != null && GameDisplay.player.Name != "Guest")
            {
                GameDisplay.player = null;
                MessageBox.Show("Successfully logged out.");
            }
            else
            {
                MessageBox.Show("You are already logged out!");
            }

        }
    }
}
