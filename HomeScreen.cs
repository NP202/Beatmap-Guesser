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
            Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
            gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString());

            if (gameDisplay.difficulty != "Easy" || gameDisplay.difficulty != "Normal" || gameDisplay.difficulty != "Hard")
            {
                MessageBox.Show("No difficulty selected! Please select a difficulty and try again.");
            }
            else
            {
                gameDisplay.Show();
                Console.WriteLine("Selected Difficulty: " + gameDisplay.difficulty);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Selected Difficulty: " + comboBox1.SelectedItem.ToString());
            gameDisplay = new GameDisplay(comboBox1.SelectedItem.ToString());

            if (gameDisplay.difficulty != "Easy" || gameDisplay.difficulty != "Normal" || gameDisplay.difficulty != "Hard")
            {
                MessageBox.Show("No difficulty selected! Please select a difficulty and try again.");
            }
            else
            {
                gameDisplay.Show();
                Console.WriteLine("Selected Difficulty: " + gameDisplay.difficulty);
            }

        }

    }
}
