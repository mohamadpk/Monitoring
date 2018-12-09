using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Utils
{
    class _s_Utils_ErrosHandling_DB
    {
        /// <summary>
        /// The database connection publicly
        /// use at when ever we need to connect database
        /// </summary>
        SQLiteConnection dbConnection;
        /// <summary>
        /// Initializes a new instance of the <see cref="_s_Utils_ErrosHandling_DB"/> class.
        /// for security and set crypting database set password for your db
        /// </summary>
        public _s_Utils_ErrosHandling_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=Errors.sqlite;Version=3;");
            dbConnection.Open();
        }

        /// <summary>
        /// Gets all errors.
        /// </summary>
        /// <returns>return a list of ErrorDescriptor class</returns>
        public List<_s_Utils_ErrosHandling.ErrorDescriptor> GetAllErrors()
        {
            string sql = "select * from ErrorList";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = cmd.ExecuteReader();
            List<_s_Utils_ErrosHandling.ErrorDescriptor> AllErrors = new List<_s_Utils_ErrosHandling.ErrorDescriptor>();
            while (reader.Read())
            {
                AllErrors.Add(ReaderToErrorDescriptor(reader));
            }
            return AllErrors;

        }



        /// <summary>
        /// Readers to error descriptor.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        /// <returns>get ErrorDescriptor from reader and return it</returns>
        private _s_Utils_ErrosHandling.ErrorDescriptor ReaderToErrorDescriptor(SQLiteDataReader Reader)
        {
            _s_Utils_ErrosHandling.ErrorDescriptor suee = new _s_Utils_ErrosHandling.ErrorDescriptor();
            suee.MethodName= Reader["MethodName"].ToString();
            suee.ExeptionMessage = Reader["ExeptionMessage"].ToString();
            suee.DateOfError =DateTime.Parse(Reader["DateOfError"].ToString());
            return suee;
        }

        /// <summary>
        /// Adds the error to database.
        /// </summary>
        /// <param name="ErrorDescriptor">The error descriptor.</param>
        public void AddErrorToDB(_s_Utils_ErrosHandling.ErrorDescriptor ErrorDescriptor)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into ErrorList (MethodName,ExeptionMessage,DateOfError)  values (@MethodName,@ExeptionMessage,@DateOfError)";
            cmd.Parameters.Add("MethodName", DbType.String).Value = ErrorDescriptor.MethodName;
            cmd.Parameters.Add("ExeptionMessage", DbType.String).Value = ErrorDescriptor.ExeptionMessage;
            cmd.Parameters.Add("DateOfError", DbType.DateTime).Value = ErrorDescriptor.DateOfError;
            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Removes all erros before date from database.
        /// </summary>
        /// <param name="DateOfError">The date of error.</param>
        public void RemoveAllErrosBeforeDateFromDB(DateTime DateOfError)
        {
            string sql = "DELETE from ErrorList where DateOfError<@DateOfError";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@DateOfError", DateOfError));
            SQLiteDataReader reader = cmd.ExecuteReader();
        }
    }
}
