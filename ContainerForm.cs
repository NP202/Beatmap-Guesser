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
    public sealed partial class ContainerForm : Form
    {

        private static ContainerForm FormInstance = null;
        private ContainerForm()
        {
            InitializeComponent();
            HomeScreen hs = new HomeScreen();
            hs.TopLevel = false;
            //create home form and pass it into panel control
            this.FormsPanel.Controls.Add(hs);
            this.Hide();
            hs.Show();
        }


        public static ContainerForm GetInstance()//gurantee singleton status
        {
            lock (typeof(ContainerForm))
            {
                if (FormInstance == null)
                {
                    FormInstance = new ContainerForm();
                }

                return (FormInstance);
            }
        }
        private void FormsPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        public void AddFormAndShow(Form form)
        {

            form.TopLevel = false;
            this.FormsPanel.Controls.Add(form);
            this.Hide();
            form.Show();
            

        }
        public void AddFormAndShow(GameDisplay form)
        {
            this.Hide();
            form.start();
            
        }
    }
}
