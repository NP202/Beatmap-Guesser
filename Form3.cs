﻿using System;
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
        
        private void verifyLogin()
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}