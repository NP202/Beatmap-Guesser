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
                //show username and password form
                //login
                Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
                gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString(), player);
                this.Hide();
                gameDisplay.start();
                this.Close();
               
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
                this.Hide();
                gameDisplay.start();
                this.Close();

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
