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
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseWork_Storage
{
    /// <summary>
    /// Interaction logic for StorerWindow.xaml
    /// </summary>
    public partial class StorerWindow : Window
    {
        public StorerWindow()
        {
            InitializeComponent();
            FillcbOrderID2();
        }

        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-H6UBJSJ; Initial Catalog=MyStorage; Integrated Security=True;");
        private void Button_ProductList(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("DataStorage", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter dataAdp = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable("Storage");
                dataAdp.Fill(dt);
                DataGrid.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);              
               sqlCon.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_CatalogList(object sender, RoutedEventArgs e)
        {

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("DataStorage", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter dataAdp = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable("Storage");
                dataAdp.Fill(dt);
                DataGrid.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
           
                this.Close();
            
        }

        private void OrderView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand cmd = new SqlCommand("DataOrders", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Orders");
                dataAdp.Fill(dt);
                DataGrid5.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlCon.Close();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void StatementView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();


                SqlCommand sqlcmd = new SqlCommand("Statement_Storage", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@OrderID", SqlDbType.VarChar).Value = tbStatementID.Text.Trim();
                SqlDataAdapter dataAdp = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable("Storage");
                dataAdp.Fill(dt);
                DataGrid6.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlCon.Close();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void FillcbOrderID2()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("SelectOrderID", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader drd = sqlcmd.ExecuteReader();
                while (drd.Read())
                {
                    cbStatementID.Items.Add(drd["OrderID"].ToString());


                }

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CbStatementID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbStatementID.Text = cbStatementID.SelectedItem.ToString();
        }

       
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand cmd = new SqlCommand("Raspredelenie", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dataAdp.Fill(dt);
                DataGrid10.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                SqlCommand cmd2 = new SqlCommand("Raspredelenie2", sqlCon);
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdp2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                dataAdp2.Fill(dt2);
                DataGrid11.ItemsSource = dt2.DefaultView;
                dataAdp.Update(dt2);

                sqlCon.Close();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

      
    }
}
