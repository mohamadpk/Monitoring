using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;

namespace Module_Process_Manager.Process_Watcher
{
    /// <summary>
    /// The sub module db from sub module of process watcher module for black process
    /// </summary>
    class _m_Process_Manager_Watcher_DB
    {
        /// <summary>
        /// The database connection publicly
        /// use at when ever we need to connect database
        /// </summary>
        SQLiteConnection dbConnection;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Process_Manager_Watcher_DB"/> class.
        /// for security and set crypting database set password for your db
        /// </summary>
        public _m_Process_Manager_Watcher_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=BlackProcess.sqlite;Version=3;");
            dbConnection.Open();
        }

        /// <summary>
        /// Gets the name of all process.
        /// </summary>
        /// <returns>return a list of string contain black process name</returns>
        public List<string> GetAllProcessName()
        {
            string sql = "select * from BlackProcessList";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<string> AllProcessName = new List<string>();
            while (reader.Read())
            {
                AllProcessName.Add(ReaderToProcessName(reader));
            }
                return AllProcessName;

        }

        /// <summary>
        /// Readers the name of to process.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        /// <returns>get BlackProcessName column from reader and return it</returns>
        private string ReaderToProcessName(SQLiteDataReader Reader)
        {
            return Reader["BlackProcessName"].ToString();
        }


        /// <summary>
        /// Adds the black process to database.
        /// </summary>
        /// <param name="BlackProcessName">Name of the black process.</param>
        public void AddBlackProcessToDB(string BlackProcessName)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into BlackProcessList (BlackProcessName)  values (@BlackProcessName)";
            cmd.Parameters.Add("BlackProcessName", DbType.String).Value = BlackProcessName;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Removes the black process from database.
        /// </summary>
        /// <param name="BlackProcessName">Name of the black process.</param>
        public void RemoveBlackProcessFromDB(string BlackProcessName)
        {
            string sql = "DELETE from BlackProcessList where BlackProcessName==@BlackProcessName";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@BlackProcessName", BlackProcessName));
            SQLiteDataReader reader = cmd.ExecuteReader();
        }
    }
}
