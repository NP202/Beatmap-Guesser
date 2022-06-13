using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace Beatmap_Guesser
{
    public partial class LoginForm : Form
    {

        public string username;
        private string password;
        public Player logged_in_player { get; set; } = null;

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
            bool valid_user = false;
            bool valid_pass = false;
            if (username == null || password == null) return false;
            if (username == "Guest")
            {
                MessageBox.Show("Cannot select \"Guest\" as a username! Please choose another.");
            }
            else if (username.Length < 3)
            {
                MessageBox.Show("Usernames must be at least three characters long.");
            }
            else if (password.Length < 5)
            {
                MessageBox.Show("Passwords must be at least five characters long.");
            }
            else
            {
                valid_user = true;
                valid_pass = true;
            }

            if (valid_user && valid_pass)//if the username and passwords are of an acceptable length, verify them
            { 
                string dir_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\osu! Beatmap Guesser\\userdata";
                string file_path = username + ".xml";
                string full_filepath = Path.Combine(dir_path, file_path);

                //valid username
                if (File.Exists(full_filepath))
                {
                    Player searched = Player.retrievePlayer(username);

                    //password should already be hashed before the user is written into an xml file
                    bool match = Crypto.VerifyHashedPassword(searched.password, password);

                    //valid password
                    if (match)
                    {
                        bool message_shown = false;
                        if (!new_user)
                        {
                            MessageBox.Show("Successful Login");
                            this.logged_in_player = searched;
                            this.Hide();
                            message_shown = true;

                        }
                        return true;
                        
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
                    if (!this.Visible)
                    {
                        return false;//stops dialogueResult from generating twice
                    }
                    DialogResult dialogResult = MessageBox.Show("The specified user does not currently exist. Would you like to create a new user?",
                         "New User Confirmation", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        
                        this.Hide();
                        return false;
                    }
                    else 
                    {
                        //create and save new user
                        Player p = new Player(username);
                        p.password = Crypto.HashPassword(password);//password saved as hash
                        p.savePlayer();
                        new_user = true;//flag to not show login verification twice
                        MessageBox.Show("Successfully created new user with username " + username + ".");
                        this.logged_in_player = p;
                        this.Hide();
                        return true;

                    }

                }
            }
            return false;
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}