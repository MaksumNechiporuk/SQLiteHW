using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sqlite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection con = new SQLiteConnection($"Data Source={"Users.sqlite"}");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (txtLogin.Text != "" && txtPassword.Password != "")
            {

                btnAddNew.Content = "Add new";

                con.Open();
                string Login = txtLogin.Text;
                string pass = txtPassword.Password;
                string hashed = BCrypt.HashPassword(pass, BCrypt.GenerateSalt(12));

                string query = $"Insert into tblUsers(Login,Password) values('{Login}','{ hashed}')";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                FillDataGrid();
                Reset();


            }
            else
                MessageBox.Show("Fill in all the fields");
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
