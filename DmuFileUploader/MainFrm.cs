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

            this.Text = Program.Title;
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

                    this.SetStatus($"Connected to {host}");
                }
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Configuration Migration Utility file (*.zip)|*.zip",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"Selected file: {openFileDialog.FileName}");
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            var startText = new[]
            {
                Program.Title,
                "",
                "This tool can be used to upload a file created with the " +
                "\"Common Data Service Configuration Migration\" tool from the Dynamics 365 SDK.",
                "",
                "NOTE!!!",
                "The primary key of the records will not be changed.",
                "This could decrease performace.",
                ""
            };

            string text = string.Join(Environment.NewLine, startText);

            this.Write(text);

            this.SetStatus("Not connected");
        }

        private void Write(string text)
        {
            this.TextTxt.Text += text;

            this.TextTxt.DeselectAll();
        }

        private void WriteLine(string line)
        {
            this.Write(Environment.NewLine + line);
        }

        private void SetStatus(string status)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            this.statusLabel.Text = $"{time} Status: {status}";
        }
    }
}
