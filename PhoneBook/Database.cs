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
                System.Console.WriteLine("Database created successfully");
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
