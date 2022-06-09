namespace DmuFileUploader
{
    using Newtonsoft.Json;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class LoginFrm : Form
    {
        // These sample application registration values are available for all online instances.
        public const string CLIENT_ID = "51f81489-12ee-4a9e-aaae-a2591f45987d";


        private readonly MainFrm mainFrm;
        private readonly HttpClient httpClient;


        public LoginFrm(MainFrm mainFrm, 
            HttpClient httpClient, ConnectionInfo connectionInfo)
        {
            this.mainFrm = mainFrm ??
                throw new ArgumentNullException(nameof(mainFrm));

            this.httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

            InitializeComponent();

            if(connectionInfo != null)
            {
                this.UrlTxt.Text = connectionInfo.Resource.Host;
                this.UsernameTxt.Text = connectionInfo.Username;
                this.PasswordTxt.Text = connectionInfo.Password;
            }
            else
            {
                this.TryLoadLoginInfo();
            }
        }


        public ConnectionInfo Info { get; private set; }


        private void TryLoadLoginInfo()
        {
            try
            {
                if (File.Exists("login_info.json"))
                {
                    string json = File.ReadAllText("login_info.json");

                    var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(json);

                    this.UrlTxt.Text = loginInfo.Url;
                    this.UsernameTxt.Text = loginInfo.Username;

                    this.ActiveControl = this.PasswordTxt;
                }
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void CancelBtn_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void LoginBtn_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.UrlTxt.Text))
            {
                Error("The URL field cannot be empty");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.UsernameTxt.Text))
            {
                Error("The User Name field cannot be empty");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.PasswordTxt.Text))
            {
                Error("The Password field cannot be empty");
                return;
            }

            try
            {
                this.StartProgressBar();

                this.mainFrm.WriteLine("Retrieving OAuth token...");

                Uri resource = NormalizeHostUrl(this.UrlTxt.Text);
                this.mainFrm.WriteLine($"Absolute URI: {resource.AbsoluteUri}");

                string username = this.UsernameTxt.Text;
                string password = this.PasswordTxt.Text;

                this.mainFrm.WriteLine("Getting authorization URL...");
                Uri authorizeUrl = await GetAuthorizationUrl(httpClient, resource);
                this.mainFrm.WriteLine($"Authorization URL: {authorizeUrl.AbsoluteUri}");

                this.mainFrm.WriteLine("Retrieving token.");
                HttpResponseMessage authResponse = await Authentication.GetAuthResponse(httpClient,
                    authorizeUrl, resource, CLIENT_ID, username, password);

                this.mainFrm.WriteLine($"Status: {authResponse.StatusCode}");
                if (!authResponse.IsSuccessStatusCode)
                {
                    this.StopProgressBar();

                    OAuthError oAuthError = await Authentication.GetError(authResponse);

                    this.mainFrm.WriteLine($"Error: {oAuthError.ErrorDescription}");
                    this.mainFrm.WriteLine();
                    Error(oAuthError.ErrorDescription);
                    return;
                }

                this.mainFrm.WriteLine("Creating Brearer token.");
                var authHeader = await Authentication
                    .GetAuthenticationHeader(authResponse);

                this.Info = new ConnectionInfo(
                    resource,
                    authorizeUrl,
                    username,
                    password,
                    authHeader
                    );

                this.DialogResult = DialogResult.OK;
                this.Close();

                this.mainFrm.WriteLine("OAuth token created.");
            }
            catch (Exception ex)
            {
                this.StopProgressBar();

                Error(ex.InnerException?.Message ?? ex.Message);

                return;
            }

            this.TrySaveLoginInfo();
        }

        private void TrySaveLoginInfo()
        {
            try
            {
                var loginInfo = new LoginInfo
                {
                    Url = this.UrlTxt.Text,
                    Username = this.UsernameTxt.Text,
                    Password = null
                };

                string json = JsonConvert.SerializeObject(loginInfo);

                File.WriteAllText("login_info.json", json);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        private void StartProgressBar()
        {
            this.ProgressBar.Style = ProgressBarStyle.Marquee;

            this.ProgressBar.MarqueeAnimationSpeed = 5;
        }

        private void StopProgressBar()
        {
            this.ProgressBar.Style = ProgressBarStyle.Blocks;
        }

        private static void Error(string message)
        {
            MessageBox.Show(message, Program.Title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static Uri NormalizeHostUrl(string url)
        {
            if (!url.StartsWith("http"))
            {
                url = $"http://{url}";
            }

            Uri uri = new Uri(url, UriKind.Absolute);

            var https = new Uri($"https://{uri.Host}");

            return https;
        }

        private static async Task<Uri> GetAuthorizationUrl(
            HttpClient httpClient, Uri resource)
        {
            HttpResponseMessage response = await httpClient.GetAsync(
                new Uri(resource, "/api/data/v9.2"));

            string wwwAuthenticate = $"{response.Headers.WwwAuthenticate}";

            int start = wwwAuthenticate.IndexOf("https://");
            int end = wwwAuthenticate.IndexOf("oauth2/");
            end += "oauth2/".Length;

            int length = (end == -1)
                ? wwwAuthenticate.Length - start
                : end - start;

            string authorizationUrl = wwwAuthenticate
                .Substring(start, length);

            Uri result = UriCombine(authorizationUrl, "token");

            return result;
        }

        private static Uri UriCombine(string baseUrl, string relative)
        {
            var baseUri = new Uri(baseUrl);

            var result = new Uri(baseUri, relative);

            return result;
        }

        private void StatusStrip_Resize(object sender, EventArgs e)
        {
            ProgressBar.Size = new Size(StatusStrip.Width - 20, StatusStrip.Height);
        }

        private void UrlTxt_KeyDown(object sender, KeyEventArgs e)
        {
            this.HandleKeyDown(sender, e);
        }

        private void UsernameTxt_KeyDown(object sender, KeyEventArgs e)
        {
            this.HandleKeyDown(sender, e);
        }

        private void PasswordTxt_KeyDown(object sender, KeyEventArgs e)
        {
            this.HandleKeyDown(sender, e);
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.LoginBtn_Click(sender, e);

                // https://stackoverflow.com/questions/6290967/stop-the-ding-when-pressing-enter
                e.SuppressKeyPress = true;
            }
        }
    }
}
