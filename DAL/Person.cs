
using Microsoft.Data.SqlClient;
using MVCProject.Models;

namespace MVCProject.DAL
{
    public class Person
    {
        private IConfiguration config;

        public Person(IConfiguration config)
        {
            this.config = config;
        }

        internal bool DeleteUser(string? id)
        {
            //Connect to DB
            String? connStr = config.GetConnectionString("MyConnString");

            // need to install Microsoft.Data.SqlClient
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Create Query
            // should be parametized query
            string query = "DELETE FROM [dbo].[Person] WHERE [PersonID] = @id;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            //Query
            int count = cmd.ExecuteNonQuery();

            //End connection
            conn.Close();

            if (count == 1) { return true; }
            else { return false; }
        }

        internal UserInfo GetUser(string? id)
        {
            //Connect to DB
            String? connStr = config.GetConnectionString("MyConnString");

            // need to install Microsoft.Data.SqlClient
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Create Query
            // should be parametized query
            string query = "SELECT [FName],[LName],[email],[phone],[address],[UserName],[Password] FROM [dbo].[Person] WHERE [PersonID] = @id;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);

            //Query
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            UserInfo user = new UserInfo();
            user.FName = reader["FName"].ToString();
            user.LName = reader["LName"].ToString();
            user.Email = reader["email"].ToString();
            user.Phone = reader["phone"].ToString();
            user.Address = reader["address"].ToString();
            user.Username = reader["UserName"].ToString();
            user.Password = reader["Password"].ToString();

            //End connection
            conn.Close();

            return user;
        }

        internal string InsertPerson(UserInfo a)
        {
            //Connect to DB
            String? connStr = config.GetConnectionString("MyConnString");

            // need to install Microsoft.Data.SqlClient
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Create Query
            // should be parametized query
            string query = "INSERT INTO [Person] ([FName], [LName], [email], [phone], [address], [UserName], [Password])" +
                "VALUES(@FName, @LName, @email, @phone, @address, @UserName, @Password) SELECT SCOPE_IDENTITY() AS PersonID;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FName", a.FName);
            cmd.Parameters.AddWithValue("@LName", a.LName);
            cmd.Parameters.AddWithValue("@email", a.Email);
            cmd.Parameters.AddWithValue("@phone", a.Phone);
            cmd.Parameters.AddWithValue("@address", a.Address);
            cmd.Parameters.AddWithValue("@UserName", a.Username);
            cmd.Parameters.AddWithValue("@Password", a.Password);

            //Query
            //cmd.ExecuteNonQuery();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string? id = reader["PersonID"].ToString();

            //End connection
            conn.Close();

            return id;
        }

        internal bool UpdateUser(UserInfo userInfo, string? id)
        {
            //Connect to DB
            String? connStr = config.GetConnectionString("MyConnString");

            // need to install Microsoft.Data.SqlClient
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Create Query
            // should be parametized query
            string query = "UPDATE [dbo].[Person] SET " + 
                "[FName] = @FName, [LName] = @LName," +
                "[email] = @email, [phone] = @phone," +
                "[address] = @address, [UserName] = @UserName,"+
                "[Password] = @Password WHERE [PersonID] = @id;";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FName", userInfo.FName);
            cmd.Parameters.AddWithValue("@LName", userInfo.LName);
            cmd.Parameters.AddWithValue("@email", userInfo.Email);
            cmd.Parameters.AddWithValue("@phone", userInfo.Phone);
            cmd.Parameters.AddWithValue("@address", userInfo.Address);
            cmd.Parameters.AddWithValue("@UserName", userInfo.Username);
            cmd.Parameters.AddWithValue("@Password", userInfo.Password);
            cmd.Parameters.AddWithValue("@id", id);

            //Query
            int count = cmd.ExecuteNonQuery();

            //End connection
            conn.Close();

            if (count == 1) { return true; }
            else { return false; }
        }
    }
}