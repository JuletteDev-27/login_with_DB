using System.Windows.Forms;

namespace login_with_DB
{
    public partial class loggedInWindow : Form
    {
        public loggedInWindow(string account_username)
        {
            InitializeComponent();
            label1.Text.Replace("!", " " + account_username.ToUpper());
        }
    }
}
