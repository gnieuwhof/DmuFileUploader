namespace DmuFileUploader
{
    using System;
    using System.Net.Http;
    using System.Windows.Forms;

    public partial class MainFrm : Form
    {
        private readonly HttpClient httpClient;

        private ConnectionInfo connectionInfo;

        private string file;


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
            this.Login();
        }

        private DialogResult Login()
        {
            this.WriteLine("Login");

            using (var login = new LoginFrm(this,
                this.httpClient, this.connectionInfo))
            {
                DialogResult result = login.ShowDialog();

                if (result == DialogResult.OK)
                {
                    this.connectionInfo = login.Info;

                    string uri = this.connectionInfo.Resource.AbsoluteUri;
                    string username = this.connectionInfo.Username;

                    this.SetStatus($"Connected to {uri}");

                    this.WriteLine();
                    this.WriteLine("Connected");
                    this.WriteLine($"Host: {uri}");
                    this.WriteLine($"User: {username}");
                    this.WriteLine();
                }
                else
                {
                    this.WriteLine("Login cancelled");
                    this.WriteLine();
                }

                return result;
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WriteLine("Select file");

            var openFileDialog = new OpenFileDialog
            {
                Filter = "Configuration Migration Utility file (*.zip)|*.zip",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.file = openFileDialog.FileName;

                this.WriteLine($"Selected file: {this.file}");
                this.WriteLine();

                if (this.connectionInfo == null)
                {
                    Login();

                    if(this.connectionInfo == null)
                    {
                        return;
                    }
                }

                if(this.connectionInfo != null)
                {
                    this.WriteLine("");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("BUSINESS!!");
                    this.WriteLine("");
                }
            }
            else
            {
                this.WriteLine("Selecting file cancelled");
                this.WriteLine();
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

        public void Write(string text)
        {
            this.TextTxt.AppendText(text);
        }

        public void WriteLine(string line = "")
        {
            string text = Environment.NewLine;

            if (!string.IsNullOrWhiteSpace(line))
            {
                string time = DateTime.Now.ToString("HH:mm:ss");

                text += $"{time} {line}";
            }

            this.Write(text);
        }

        private void SetStatus(string status)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            this.statusLabel.Text = $"{time} Status: {status}";
        }
    }
}
