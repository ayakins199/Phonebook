using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;

namespace PhoneBook
{
    public partial class UserControl1: UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = new Database();
                db.OpenConnection();

                if (txtFirstName.Text == "" || txtSurname.Text == "" || txtPhoneNumber.Text == "")
                {
                    MessageBox.Show("name or record entry cannot be empty");
                }
                else
                {
                    string check = string.Format("SELECT * FROM tbl_phonebook WHERE FirstName == '{0}' and Surname == '{1}'", txtFirstName.Text.ToUpper(), txtSurname.Text);
                    SQLiteCommand sqlcmd1 = new SQLiteCommand(check, db.myConn);
                    SQLiteDataReader output = sqlcmd1.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("FirstName");
                    dt.Columns.Add("Surname");
                    dt.Columns.Add("PhoneNumber");
                    if (output.HasRows) // dont add record if it already exist
                    {
                        MessageBox.Show("Record already exist");
                        while(output.Read())
                        {
                            DataRow row = dt.NewRow();
                            row["FirstName"] = output["FirstName"];
                            row["Surname"] = output["Surname"];
                            row["PhoneNumber"] = output["PhoneNumber"];
                            dt.Rows.Add(row);
                        }
                        dataGridView1.DataSource = dt;
                        txtFirstName.Text = ""; txtSurname.Text = ""; txtFirstName.Focus();
                        txtPhoneNumber.Text = "";
                    }
                    else
                    {
                        //insert into database
                        string query = "INSERT INTO tbl_phonebook ('FirstName', 'Surname', 'PhoneNumber') VALUES (@FirstName, @Surname, @PhoneNumber)";
                        SQLiteCommand sqlcmd2 = new SQLiteCommand(query, db.myConn);

                        string[] PnoneNumbers = txtPhoneNumber.Text.Split(new[] { Environment.NewLine },
                        StringSplitOptions.None);
                        int output2 = 0;
                        foreach (string number in PnoneNumbers)
                        {
                            sqlcmd2.Parameters.AddWithValue("@FirstName", txtFirstName.Text.ToUpper());
                            sqlcmd2.Parameters.AddWithValue("@Surname", txtSurname.Text.ToUpper());
                            sqlcmd2.Parameters.AddWithValue("@PhoneNumber", number);
                            output2 += sqlcmd2.ExecuteNonQuery();
                        }

                        db.CloseConnection();

                        MessageBox.Show(Convert.ToString(output2) + " record Added successfully");
                        txtFirstName.Text = ""; txtSurname.Text = ""; txtFirstName.Focus();
                        txtPhoneNumber.Text = "";

                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unexpected Error. Please try again!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQuery.Text == "")
                {
                    MessageBox.Show("Invalid Entry");
                    txtQuery.Text = ""; txtQuery.Focus();
                }
                else
                {
                    Database db = new Database();
                    db.OpenConnection();
                    string term = txtQuery.Text.ToUpper();
                    string query = string.Format("SELECT FirstName, Surname, PhoneNumber FROM tbl_phonebook WHERE FirstName == '{0}' or Surname == '{1}' or PhoneNumber == '{2}'", term, term, term);
                    SQLiteCommand sqlcmd = new SQLiteCommand(query, db.myConn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlcmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "tbl_phonebook");
                    dataGridView1.DataSource = ds.Tables["tbl_phonebook"].DefaultView;
                    txtQuery.Text = ""; txtQuery.Focus();
                    db.CloseConnection();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid Entry, please try again");
                txtQuery.Text = ""; txtQuery.Focus();
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = new Database();
                db.OpenConnection();
                string query = string.Format("SELECT FirstName, Surname, PhoneNumber FROM tbl_phonebook");
                SQLiteCommand sqlcmd = new SQLiteCommand(query, db.myConn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(sqlcmd);

                DataSet ds = new DataSet();
                da.Fill(ds, "tbl_phonebook");
                dataGridView1.DataSource = ds.Tables["tbl_phonebook"].DefaultView;
                da.Dispose();
                db.CloseConnection();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unexpected error. Please try again!");
            }

        }
    }
}
