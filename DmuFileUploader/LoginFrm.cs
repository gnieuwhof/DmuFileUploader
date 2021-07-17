namespace DmuFileUploader
{
    using System;
    using System.Drawing;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class LoginFrm : Form
    {
        // These sample application registration values are available for all online instances.
        private const string CLIENT_ID = "51f81489-12ee-4a9e-aaae-a2591f45987d";

        private readonly HttpClient httpClient;


        public LoginFrm(HttpClient httpClient, ConnectionInfo connectionInfo)
        {
            this.httpClient = httpClient ??
                throw new ArgumentNullException(nameof(httpClient));

            InitializeComponent();

            if(connectionInfo != null)
            {
                this.UrlTxt.Text = connectionInfo.Resource.Host;
                this.UsernameTxt.Text = connectionInfo.Username;
                this.PasswordTxt.Text = connectionInfo.Password;
            }
        }


        public ConnectionInfo Info { get; private set; }


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

                Uri resource = NormalizeHostUrl(this.UrlTxt.Text);
                string username = this.UsernameTxt.Text;
                string password = this.PasswordTxt.Text;

                Uri authorizeUrl = await GetAuthorizationUrl(httpClient, resource);

                HttpResponseMessage authResponse = await Authentication.GetAuthResponse(httpClient,
                    authorizeUrl, resource, CLIENT_ID, username, password);

                if (!authResponse.IsSuccessStatusCode)
                {
                    this.StopProgressBar();

                    OAuthError oAuthError = await Authentication.GetError(authResponse);

                    MessageBox.Show(oAuthError.ErrorDescription);
                    return;
                }

                var authHeader = await Authentication
                    .GetAuthenticationHeader(authResponse);

                this.Info = new ConnectionInfo(
                    resource,
                    username,
                    password,
                    authHeader,
                    DateTime.Now
                    );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                this.StopProgressBar();

                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
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

        private void Error(string message)
        {
            MessageBox.Show(message, "DMU File Uploader",
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
