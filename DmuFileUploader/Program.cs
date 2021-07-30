namespace DmuFileUploader
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Windows.Forms;

    static class Program
    {
        public static string Title => "Data Migration Utility | File Uploader";

        public static string ApiPath => "api/data/v9.2/";


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
