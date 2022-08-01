using System;
using System.Windows.Forms;
namespace Beatmap_Guesser
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {

            Application.Run(ContainerForm.GetInstance());
        }

    }

}

