namespace DmuFileUploader
{
    using System;
    using System.Net.Http;
    using System.Windows.Forms;

    public partial class MainFrm : Form
    {
        private readonly HttpClient httpClient;

        private ConnectionInfo connectionInfo;


        public MainFrm(HttpClient httpClient)
        {
            this.httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var login = new LoginFrm(this.httpClient, this.connectionInfo))
            {
                DialogResult result = login.ShowDialog();

                if (result == DialogResult.OK)
                {
                    this.connectionInfo = login.Info;

                    string host = this.connectionInfo.Resource.AbsoluteUri;
                    string time = this.connectionInfo.Time.ToString("HH:mm:ss");

                    this.statusLabel.Text = $"{time}: Connected to {host}";
                }
            }
        }
    }
}
