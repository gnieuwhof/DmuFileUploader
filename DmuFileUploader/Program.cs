namespace DmuFileUploader
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Windows.Forms;

    static class Program
    {
        public static string Title => "Data Migration Utility | File Uploader";


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Application.Run(new MainFrm(httpClient));
            }
        }
    }
}
