using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Module_File_Manager.Tree_Watcher
{
    /// <summary>
    /// The sub module db from sub module of file manager module for filtered folders
    /// </summary>
    class _m_File_Manager_Watcher_DB
    {
        /// <summary>
        /// The database connection publicly
        /// use at when ever we need to connect database
        /// </summary>
        SQLiteConnection dbConnection;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_File_Manager_Watcher_DB"/> class.
        /// for security and set crypting database set password for your db
        /// </summary>
        public _m_File_Manager_Watcher_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=FilterFolder.sqlite;Version=3;");
            dbConnection.Open();
        }

        /// <summary>
        /// Gets all filtered folder address.
        /// </summary>
        /// <returns>return a list of string contain filtered folder address</returns>
        public List<string> GetAllFilteredFolderAddress()
        {
            string sql = "select * from FilteredFolderList";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<string> AllProcessName = new List<string>();
            while (reader.Read())
            {
                AllProcessName.Add(ReaderToFilteredFolderAddress(reader));
            }
            return AllProcessName;

        }

        /// <summary>
        /// Readers to filtered folder address.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        /// <returns>get FilterdFolderAddress column from reader and return it</returns>
        private string ReaderToFilteredFolderAddress(SQLiteDataReader Reader)
        {
            return Reader["FilterdFolderAddress"].ToString();
        }


        /// <summary>
        /// Adds the black process to database.
        /// </summary>
        /// <param name="FilterdFolderAddress">The filterd folder address.</param>
        public void AddBlackProcessToDB(string FilterdFolderAddress)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into FilteredFolderList (FilterdFolderAddress)  values (@FilterdFolderAddress)";
            cmd.Parameters.Add("BlackProcessName", DbType.String).Value = FilterdFolderAddress;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Removes the black process from database.
        /// </summary>
        /// <param name="FilterdFolderAddress">The filterd folder address.</param>
        public void RemoveBlackProcessFromDB(string FilterdFolderAddress)
        {
            string sql = "DELETE from FilteredFolderList where FilterdFolderAddress==@FilterdFolderAddress";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@FilterdFolderAddress", FilterdFolderAddress));
            SQLiteDataReader reader = cmd.ExecuteReader();
        }
    }
}
