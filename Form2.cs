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
    public partial class FilepathForm : Form
    {

        public FilepathForm()
        {
            InitializeComponent();
        }

        public string getFilePath()
        {
            return folderBrowserDialog1.SelectedPath;
        }

        public DialogResult getDialogResult()
        {
            return folderBrowserDialog1.ShowDialog();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
