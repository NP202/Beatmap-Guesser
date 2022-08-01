using System.Windows.Forms;

namespace Beatmap_Guesser
{
    public sealed partial class ContainerForm : Form
    {
       /** Wrapper Form to contain every other form in the project. This is used to standardize the layout and proportions of each form.
        * Class is given Singleton status so as to not use more than one Container form per instance of the application. 
        */

        private static ContainerForm FormInstance = null;
        private ContainerForm()
        {
            this.DoubleBuffered = true;
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
