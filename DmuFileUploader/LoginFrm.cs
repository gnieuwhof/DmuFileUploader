namespace DmuFileUploader
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class LoginFrm : Form
    {
        // These sample application registration values are available for all online instances.
        private const string CLIENT_ID = "51f81489-12ee-4a9e-aaae-a2591f45987d";

        public LoginFrm()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private async void LoginBtn_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.UrlTxt.Text))
            {

            }

            if (!Uri.TryCreate(this.UrlTxt.Text, UriKind.Absolute, out Uri resource))
            {

            }

            string userName = this.UserNameTxt.Text;
            string password = this.PasswordTxt.Text;

            await GetToken(resource, userName, password);
        }

        private static async Task GetToken(Uri resource, string userName, string password)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(new Uri(resource, "/api/data/v9.2"));

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                // ???????????????
            }


            string wwwAuthenticate = $"{response.Headers.WwwAuthenticate}";

            int start = wwwAuthenticate.IndexOf("https://");
            int end = wwwAuthenticate.IndexOf(',');

            int length = (end == -1)
                ? wwwAuthenticate.Length - start
                : end - start;

            string authorizeUrl = wwwAuthenticate.Substring(start, length).Replace("authorize", "token");

            var header = await Authentication.GetAuthenticationHeader(httpClient,
                authorizeUrl, $"{resource}", CLIENT_ID, userName, password);
        }
    }
}
