using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Data.OleDb;


namespace UserAutorisation
{
    public partial class Form1 : Form
    {
        private object btnLogin;
        private object txtUser;
        private object txtPass;

        public Form1()
        {
            InitializeComponent();
            btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            GetDataFromDB getUser;
            try
            {
                getUser = new GetDataFromDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            string name = txtUser.Text;
            string pass = txtPass.Text;

            if (getUser.UserExists(name, pass))
            {
                MessageBox.Show("Autorisation completed!");
            }
            else
            {
                MessageBox.Show("Sorry, autorisation fails!");
            }
        }
    }
    class GetDataFromDB
    {
        private string connectionString = string.Empty;

        public GetDataFromDB()
        {
            string dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserDB.mdb");
            if (!File.Exists(dbFilePath))
                throw new FileNotFoundException("File of Database not found!");
            connectionString = string.Format(
                "{0}{1}", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=", dbFilePath);
        }
        public bool UserExists(string userName, string userPass)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string cmdText = string.Format(
                    "SELECT userName FROM Users WHERE userName='{0}' AND userPassword='{1}'"
                     , userName, userPass);
                using (OleDbCommand cmd = new OleDbCommand(cmdText, connection))
                {
                    using (OleDbDataReader dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.HasRows)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}