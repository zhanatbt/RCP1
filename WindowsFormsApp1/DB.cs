using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    class DB
    {
        SqlConnection connection = new SqlConnection(@"Data Source=HOME-PC\SQLEXPRESS;Initial Catalog=RKP1;Integrated Security=True");

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public SqlConnection getConnection()
        {
            return connection;
        }
    }
}
