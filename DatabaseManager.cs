using System;
using System.Data;
using System.Data.SqlClient; // Note: Use Microsoft.Data.SqlClient if System.Data.SqlClient is missing

namespace CyberBot_POE
{
    public class DatabaseManager
    {
        // Connection string pointing directly to your LocalDB instance and the database we planned
        private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CyberBotDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        /// <summary>
        /// Saves a new cybersecurity task into the database.
        /// </summary>
        public bool InsertTask(string title, string description, string reminder)
        {
            string query = "INSERT INTO SecurityTasks (Title, Description, Reminder, IsCompleted) VALUES (@Title, @Description, @Reminder, 0);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Using parameters prevents SQL Injection security vulnerabilities!
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@Reminder", string.IsNullOrEmpty(reminder) ? (object)DBNull.Value : reminder);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Retrieves all current security tasks from the database.
        /// </summary>
        public DataTable GetAllTasks()
        {
            DataTable table = new DataTable();
            string query = "SELECT Id, Title, Description, Reminder, IsCompleted FROM SecurityTasks;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(table);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Fetch Error: " + ex.Message);
            }

            return table;
        }

        /// <summary>
        /// Updates the status of a specific task item by its ID.
        /// </summary>
        public bool UpdateTaskStatus(int id, bool isCompleted)
        {
            string query = "UPDATE SecurityTasks SET IsCompleted = @IsCompleted WHERE Id = @Id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IsCompleted", isCompleted ? 1 : 0);
                        cmd.Parameters.AddWithValue("@Id", id);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Update Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Completely deletes a specific task item by its ID.
        /// </summary>
        public bool DeleteTask(int id)
        {
            string query = "DELETE FROM SecurityTasks WHERE Id = @Id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database Delete Error: " + ex.Message);
                return false;
            }
        }
    }
}