using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Home_assistant
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var privacyForm = new PrivacyPolicyForm();
            Application.Run(privacyForm);

            if (!privacyForm.IsAccepted)
                return;

            Application.Run(new Form1());
        }

    }
}
