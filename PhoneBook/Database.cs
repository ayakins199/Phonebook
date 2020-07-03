using System.Data.SQLite;
using System.IO;

namespace PhoneBook
{
    class Database
    {
        public SQLiteConnection myConn; 

        public Database()
        {
            myConn = new SQLiteConnection("Data Source=PhoneBookDB.db"); 
            if (!File.Exists("./PhoneBookDB.db"))
            {
                SQLiteConnection.CreateFile("PhoneBookDB.db");
                OpenConnection();
                string sql_query = "create table tbl_phonebook (userid  INTEGER PRIMARY KEY AUTOINCREMENT, FirstName Text, Surname Text, PhoneNumber int)";
                SQLiteCommand cmd = new SQLiteCommand(sql_query, myConn);
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void OpenConnection()
        {
            if(myConn.State != System.Data.ConnectionState.Open)
            {
                myConn.Open();
            }
        }

        public void CloseConnection()
        {
            if (myConn.State != System.Data.ConnectionState.Closed)
            {
                myConn.Clone();
            }
        }
    }
}
