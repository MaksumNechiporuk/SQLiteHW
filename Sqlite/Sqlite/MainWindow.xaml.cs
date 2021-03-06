﻿using System;
using System.Collections.Generic;
using System.Data;
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
            try
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand("UPDATE tblUsers Set Login=@Login Where Id=@Id", con);
                cmd.Parameters.AddWithValue("Id", lblId.Content);
                cmd.Parameters.AddWithValue("Login", txtLogin.Text);
                btnAddNew.IsEnabled = true;
                btnSignIn.IsEnabled = true;
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Update Successfully", "Updated", MessageBoxButton.OK);
                FillDataGrid();
                txtPassword.IsEnabled = true;
                btnUpdate.IsEnabled = false;
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillDataGrid()
        {
            con.Open();

            string q = "SELECT * From tblUsers";
            DataSet dataSet = new DataSet();
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(q, con);
            dataAdapter.Fill(dataSet);
            dgViewDB.ItemsSource = dataSet.Tables[0].DefaultView;
            con.Close();

        }
        private void Reset()
        {
            txtLogin.Clear();
            txtPassword.Clear();
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
            try
            {
                DataRowView d = dgViewDB.SelectedItem as DataRowView;

                SQLiteCommand cmd;
                con.Open();

                string query = $"Delete FROM tblUsers where Login='{d["Login"].ToString()}'";
                cmd = new SQLiteCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
                FillDataGrid();
            }
            catch
            {
            }
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            txtLogin.IsEnabled = true;
            txtPassword.IsEnabled = false;
            DataRowView d = dgViewDB.SelectedItem as DataRowView;
            lblId.Content = d["Id"].ToString();
            txtLogin.Text = d["Login"].ToString();
            btnAddNew.IsEnabled = false;
            btnSignIn.IsEnabled = false;
        }
    }
}
