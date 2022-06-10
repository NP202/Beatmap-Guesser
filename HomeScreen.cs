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
        public Player player;
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
            else
            {
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                bool login_response = loginForm.verifyLogin();

                if (login_response)//successful login
                {
                    Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
                    gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString(), player);
                    //this.Hide();
                    gameDisplay.start();
                    //this.Close();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            player = new Player("Guest");
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("No difficulty selected! Please select a difficulty and try again.");
            }
            else
            {
                Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
                gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString(), player);
                //this.Hide();
                gameDisplay.start();
                //this.Close();

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
               + "\n\nTo start, please select a difficulty, and choose whether you would like to Play As Guest or Login."
               + "\n\n If you'd like to see these instructions again, click the Help button!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            showHelp();
        }
    }
}
