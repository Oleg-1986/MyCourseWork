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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace CourseWork_Storage
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    ///
    public class Order
    {
        private string Client;
        private string NoWayBill;
        private string Condition;
        
        
        public string condition
        {
            get
            {
                return Condition;
            }
            set
            {
                string c = value;
                if (c.Length == 0)
                {
                    throw new ArgumentException("Поле должно быть заполнено");
                }
                else
                {
                    Condition = value;
                }
            }
        }
        public string nowaybill
        {
            get
            {
                return NoWayBill;
            }
            set
            {
                string w = value;
                if (w.Length >= 10)
                    throw new ArgumentException("Номер накладной не может иметь больше 10 символов");

                else
                {
                    NoWayBill = value;
                }
            }
        }
        private string OrderDate;
        public string client
        {
            get
            {
                return Client;
            }
            set
            {


                Regex r = new Regex(@"[0-9]");
                string t = value;
                if (r.IsMatch(t))
                {
                    throw new ArgumentException("Фамилия не должна содержать цифры");
                }
                else
                {
                    Client = value;
                }
            }
        }

        public string orderdate
        {
            get
            {
                return OrderDate;
            }
            set
            {
                string d = value;
                if (d.Length == 0)
                {
                    throw new ArgumentException("Поле должно быть заполнено");
                }
                else
                {
                    OrderDate = value;
                }
            }
        }
    }
    public class Statement
    {
        private double Quantity;
        public double quantity
        {
            get
            {
                return Quantity;
            }
            set
            {
                if (value <= 0)
                {


                    throw new ArgumentException("Значение должно быть положительным!");
                }
                else
                {
                    Quantity = value;
                }
            }

        }


    }


    
    public partial class ManagerWindow : Window
    {
        Order ord;
        Statement stm;
        public ManagerWindow()
        {
            InitializeComponent();
            ord = new Order();
            stm = new Statement();
            tbClient.DataContext = ord;
            tbWayBill.DataContext = ord;
            tbDateOrder.DataContext = ord;
            cbCondition.DataContext = ord;
            cbClient.DataContext = ord;
            tbQuantity.DataContext = stm;
            
            string[] st = { "Оформление", "Отгружен" };          
            foreach(string a in st)
            {
                cbCondition.Items.Add(a);
            }
            FillcbClient();
            FillcbOrderID();
            FillcbItemID();
            FillcbTovID();
            FillcbZakID();
            FillcbDelZakID();
            FillcbChangeZakID();
        }
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-H6UBJSJ; Initial Catalog=MyStorage; Integrated Security=True;");


        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();


                SqlCommand sqlcmd = new SqlCommand("CreateOrder", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Client", SqlDbType.VarChar).Value = tbClient.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@NoWayBill", SqlDbType.VarChar).Value = tbWayBill.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@OrderDate", SqlDbType.Date).Value = tbDateOrder.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@Condition", SqlDbType.Date).Value = cbCondition.Text.Trim();
                sqlcmd.ExecuteNonQuery();
                sqlCon.Close();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void CheckOrder_Click(object sender, RoutedEventArgs e)
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
                DataGrid2.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlCon.Close();

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            finally
            {

            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();


                SqlCommand sqlcmd = new SqlCommand("DeleteOrder", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;              
                sqlcmd.Parameters.AddWithValue("@OrderID", SqlDbType.Int).Value = cbDelZakID.Text.Trim();
                sqlcmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void CheckStatement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand cmd = new SqlCommand("DataStatement", sqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Statement");
                dataAdp.Fill(dt);
                DataGrid3.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlCon.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void CreateStatement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();


                SqlCommand sqlcmd = new SqlCommand("CreateStatement", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@OrderID", SqlDbType.Int).Value = cbOrderID.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@ItemID", SqlDbType.Int).Value = cbItemID.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@Quantity", SqlDbType.Int).Value = tbQuantity.Text.Trim();
                sqlcmd.ExecuteNonQuery();
                sqlCon.Close();
                cbTovID.Items.Add(cbItemID.Text);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void btSelectStorage_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-H6UBJSJ; Initial Catalog=MyStorage; Integrated Security=True;");
           
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("DataStorage", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter dataAdp = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable("Storage");
                dataAdp.Fill(dt);
                DataGrid4.ItemsSource = dt.DefaultView;
                dataAdp.Update(dt);
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteItemID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();


                SqlCommand sqlcmd = new SqlCommand("DElItemID", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@OrderID", SqlDbType.Int).Value = cbZakID.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@ItemID", SqlDbType.Int).Value = cbTovID.Text.Trim();
                sqlcmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.Close();
        }

        private void ChangeStatement_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();


                SqlCommand sqlcmd = new SqlCommand("ChangeStatement", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;              
                sqlcmd.Parameters.AddWithValue("@OrderID", SqlDbType.Int).Value = cbChangeZakID.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@DispatchDate", SqlDbType.Int).Value = tbDispatchDate.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@Condition", SqlDbType.Int).Value = cbCondition.Text.Trim();
                sqlcmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

       private void FillcbClient()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("SelectClient", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader drd = sqlcmd.ExecuteReader();
                while(drd.Read())
                {
                    cbClient.Items.Add(drd["Client"].ToString());

                    
                }
               
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FillcbOrderID()
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
                    cbOrderID.Items.Add(drd["OrderID"].ToString());
                    

                }

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillcbItemID()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("SelectItemID", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader drd = sqlcmd.ExecuteReader();
                while (drd.Read())
                {
                    cbItemID.Items.Add(drd["ItemID"].ToString());

                }

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillcbTovID()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("SelectItemForDel", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;              
                SqlDataReader drd = sqlcmd.ExecuteReader();
                while (drd.Read())
                {
                    cbTovID.Items.Add(drd["ItemID"].ToString());
                }

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillcbZakID()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("SelectOrderForDel", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader drd = sqlcmd.ExecuteReader();
                while (drd.Read())
                {
                    cbZakID.Items.Add(drd["OrderID"].ToString());
                    

                }

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillcbDelZakID()
        {
            cbDelZakID.Items.Clear();
            try
            {
                
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("SelectZakIDForDel", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.UpdateCommand = sqlcmd;
                SqlDataReader drd = sqlcmd.ExecuteReader();
                while (drd.Read())
                {
                    cbDelZakID.Items.Add(drd["OrderID"].ToString());


                }
               
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillcbChangeZakID()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();
                SqlCommand sqlcmd = new SqlCommand("cbChangeZakID", sqlCon);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader drd = sqlcmd.ExecuteReader();
                while (drd.Read())
                {
                    cbChangeZakID.Items.Add(drd["OrderID"].ToString());


                }

                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CbCondition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbCondition.Text = cbCondition.SelectedItem.ToString();
        }

        private void CbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbClient.Text = cbClient.SelectedItem.ToString();

        }

        private void CbOrderID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbOrderID.Text = cbOrderID.SelectedItem.ToString();
        }

        private void CbItemID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbItemID.Text = cbItemID.SelectedItem.ToString();
        }

        private void CbTovID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbTovID.Text = cbTovID.SelectedItem.ToString();
        }

        private void CbZakID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbZakID.Text = cbZakID.SelectedItem.ToString();
        }

        private void CbChangeZakID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbChangeZakID.Text = cbChangeZakID.SelectedItem.ToString();
        }

        private void cbDelZakID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            cbDelZakID.Text = cbDelZakID.SelectedItem.ToString();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {

            cbClient.Items.Add(tbClient.Text);
           
           
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("SELECT OrderID FROM Orders ", sqlCon);
           
            sqlCmd.CommandType = CommandType.Text;
            SqlDataReader drd = sqlCmd.ExecuteReader();
            while (drd.Read())
            {
                cbOrderID.Items.Add(drd["OrderID"].ToString());
                cbDelZakID.Items.Add(drd["OrderID"].ToString());
                cbZakID.Items.Add(drd["OrderID"].ToString());
                cbChangeZakID.Items.Add(drd["OrderID"].ToString());
            }                     

            sqlCon.Close();
        }
    }
}
