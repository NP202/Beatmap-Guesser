using System;
using System.Windows.Forms;

namespace Beatmap_Guesser
{
    public partial class FilepathForm : Form
    {
        /** Wrapper form for the folderBrowserDialog window.
         */

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
            MessageBox.Show("If you can't find your Songs folder, it's likely in D:\\osu\\Songs or a similar directory!");
        }
    }
}
