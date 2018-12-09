using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Module_Software_Manager.Black_Software_Manager
{

    /// <summary>
    /// The sub module db from sub module of Black Software Manager module for black software
    /// </summary>
    class _m_Black_Software_Manager_DB
    {
        /// <summary>
        /// The database connection publicly
        /// use at when ever we need to connect database
        /// </summary>
        SQLiteConnection dbConnection;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_Black_Software_Manager_DB"/> class.
        /// for security and set crypting database set password for your db
        /// </summary>
        public _m_Black_Software_Manager_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=BlackSoftware.sqlite;Version=3;");
            dbConnection.Open();
        }

        /// <summary>
        /// Gets the name of all software.
        /// </summary>
        /// <returns>return a list of string contain black software display name</returns>
        public List<string> GetAllSoftwareName()
        {
            string sql = "select * from BlackSoftwareList";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<string> AllSoftwareName = new List<string>();
            while (reader.Read())
            {
                AllSoftwareName.Add(ReaderToSoftwareName(reader));
            }
            return AllSoftwareName;

        }

        /// <summary>
        /// Readers the name of to software.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        /// <returns>get BlackSoftwareName column from reader and return it</returns>
        private string ReaderToSoftwareName(SQLiteDataReader Reader)
        {
            return Reader["BlackSoftwareName"].ToString();
        }


        /// <summary>
        /// Adds the black software to database.
        /// </summary>
        /// <param name="BlackSoftwareName">Name of the black software.</param>
        public void AddBlackSoftwareToDB(string BlackSoftwareName)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into BlackSoftwareList (BlackSoftwareName)  values (@BlackSoftwareName)";
            cmd.Parameters.Add("BlackSoftwareName", DbType.String).Value = BlackSoftwareName;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the black software from database.
        /// </summary>
        /// <param name="BlackSoftwareName">Name of the black software.</param>
        public void RemoveBlackSoftwareFromDB(string BlackSoftwareName)
        {
            string sql = "DELETE from BlackSoftwareList where BlackSoftwareName==@BlackSoftwareName";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@BlackSoftwareName", BlackSoftwareName));
            SQLiteDataReader reader = cmd.ExecuteReader();
        }
    }
}
