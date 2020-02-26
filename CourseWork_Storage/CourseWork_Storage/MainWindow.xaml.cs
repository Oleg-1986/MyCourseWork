using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseWork_Storage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btSubmit_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-H6UBJSJ;Initial Catalog=MyStorage; Integrated Security=True;");
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE UserName=@UserName AND UserPassword=@UserPassword";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@UserName", txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@UserPassword", txtPassword.Password);

                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if(count==1)
                {
                    ManagerWindow dashbord = new ManagerWindow();
                    StorerWindow dashbord2 = new StorerWindow();
                    AdminWindow dashbord3 = new AdminWindow();
                    switch(txtUsername.Text)
                    {
                        case "Manager":
                            dashbord.Show();
                            break;
                        case "Storer":
                            dashbord2.Show();
                            break;
                        case "Admin":
                            dashbord3.Show();
                            break;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Имя пользователя и пороль не коректны");
                }
                sqlCon.Close();
            }
            catch(Exception)
            {
            }
        }
    }
}
