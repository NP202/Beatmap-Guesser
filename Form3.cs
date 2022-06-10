using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beatmap_Guesser
{
    public partial class LoginForm : Form
    {

        private string username;
        private string password;    

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            username = UsernameBox.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            password = PasswordBox.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            verifyLogin();
        }
        bool new_user = false;
   
        public bool verifyLogin()
        {
            
            string dir_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\osu! Beatmap Guesser";
            string file_path = username + ".xml";
            string full_filepath = Path.Combine(dir_path, file_path);

            //valid username
            if (File.Exists(full_filepath))
            {
                Player searched = Player.retrievePlayer(username);

                //valid password
                if (password == searched.password)
                {
                    if (!new_user)
                    {
                        MessageBox.Show("Successful Login");
                    }
                    return true;
                    this.Hide();
                }

                //invalid password
                else
                {
                    MessageBox.Show("Incorrect Password");
                    return false;
                }

            }
            else //no valid file, prompt user to create new account
            {
                   DialogResult dialogResult = MessageBox.Show("The specified user does not currently exist. Would you like to create a new user?",
                        "New User Confirmation", MessageBoxButtons.YesNo);//figure out a way to stop this from generating twice
                
                if (dialogResult == DialogResult.Yes)
                {
                    //create and save new user
                    Player p = new Player(username);
                    p.password = password;
                    p.savePlayer();
                    new_user = true;//flag to not show login verification twice
                    MessageBox.Show("Successfully created new user with username " + username + ".");
                    this.Hide();
                    return true;
                }
                else
                {
                    
                    this.Hide();
                    return false;
                }
            
            }

            return false;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}