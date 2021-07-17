namespace DmuFileUploader
{
    using System.Windows.Forms;

    public partial class MessageFrm : Form
    {
        public MessageFrm(string message, string details)
        {
            InitializeComponent();

            this.MessageLbl.Text = message;

            this.DetailtTxt.Text = details;
        }
    }
}
