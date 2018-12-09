using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
namespace Module_File_Manager.Tree_Creator
{
    /// <summary>
    /// The sub module db from sub module of file manager module for create tree of directores and files
    /// all work of ft with db is coded on this class
    /// </summary>
    class _m_File_Manager_Tree_FileTree_DB
    {
        /// <summary>
        /// The database connection publicly
        /// use at when ever we need to connect database
        /// </summary>
        SQLiteConnection dbConnection;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_File_Manager_Tree_FileTree_DB"/> class.
        /// for security and set crypting database set password for your db
        /// </summary>
        public _m_File_Manager_Tree_FileTree_DB()
        {
            dbConnection = new SQLiteConnection("Data Source=tree.sqlite;Version=3;");
            dbConnection.Open();
        }

        /// <summary>
        /// Gets the next child of directory unprocessed from db.
        /// </summary>
        /// <returns>return a ft node</returns>
        public _m_File_Manager_Tree_FileTree_Node GetNext()
        {
            string sql = "select * from FileTree where Progress==@Progress limit 1";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@Progress", _m_File_Manager_Tree_FileTree_Node.enum_Progress.On_Progress));
            SQLiteDataReader reader = cmd.ExecuteReader();
            if(reader.Read())
            {
                _m_File_Manager_Tree_FileTree_Node ft=ReaderToFileTree(reader);
                DeleteOnProgressSubs(ft.Id);
                return ft;
            }
            else
            {
                cmd.Parameters.Add(new SQLiteParameter("@Progress", _m_File_Manager_Tree_FileTree_Node.enum_Progress.Not_Progress));
                reader.Close();
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    _m_File_Manager_Tree_FileTree_Node ft = ReaderToFileTree(reader);
                    UpdateToOnProgress(ft);
                    //get full addr
                    ft.Name=GetPath(ft);
                    return ft;
                }
            }

            return null;
        }

        /// <summary>
        /// Deletes the on progress subs.
        /// </summary>
        /// <param name="Id_Parent">The identifier parent.</param>
        public void DeleteOnProgressSubs(int Id_Parent)
        {
            string sql = "DELETE from FileTree where Id_Parent==@Id_Parent";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@Id_Parent",Id_Parent));
            SQLiteDataReader reader = cmd.ExecuteReader();   
        }
        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <param name="ft">The ft.</param>
        /// <returns>return the full path from input ft</returns>
        public string GetPath(_m_File_Manager_Tree_FileTree_Node ft)
        {
            string Path = ft.Name;
            _m_File_Manager_Tree_FileTree_Node parentft;
            while (true)
            {
                if (ft.Id_Parent == 0)
                    break;
                parentft = GetParent(ft);
                
                if (parentft == null)
                    break;
                if(parentft.Type==_m_File_Manager_Tree_FileTree_Node.enum_Type.Directory)
                {
                    Path = parentft.Name + "\\" + Path;
                }
                else
                {
                    Path = parentft.Name + Path;
                }
                
                ft = parentft;
            }
            

            return Path;
        }


        /// <summary>
        /// Gets the parent of ft node.
        /// </summary>
        /// <param name="ft">The ft.</param>
        /// <returns>return a ft node</returns>
        public _m_File_Manager_Tree_FileTree_Node GetParent(_m_File_Manager_Tree_FileTree_Node ft)
        {
            string sql = "select * from FileTree where Id==@Id_Parent limit 1";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@Id_Parent", ft.Id_Parent));
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                _m_File_Manager_Tree_FileTree_Node parentft = ReaderToFileTree(reader);
                return parentft;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets the childs of fd if fd is directory.
        /// </summary>
        /// <param name="ft">The ft.</param>
        /// <returns>return list of ft node</returns>
        public List<_m_File_Manager_Tree_FileTree_Node> GetChilds(_m_File_Manager_Tree_FileTree_Node ft)
        {
            List <_m_File_Manager_Tree_FileTree_Node> Childs= new List<_m_File_Manager_Tree_FileTree_Node>();
            string sql = "select * from FileTree where Id_Parent==@Id";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SQLiteParameter("@Id", ft.Id));
            SQLiteDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    Childs.Add(ReaderToFileTree(reader));
                }

            if (Childs.Count > 0)
            {
                return Childs;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Writes the new ft on db.
        /// </summary>
        /// <param name="ft">The ft.</param>
        public void WriteNew(_m_File_Manager_Tree_FileTree_Node ft)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into FileTree (Id_Parent, Name,Type,LastChange,Progress)  values (@Id_Parent,@Name,@Type,@LastChange,@Progress)";
            cmd.Parameters.Add("Id_Parent", DbType.Int32).Value = ft.Id_Parent;
            cmd.Parameters.Add("Name", DbType.String).Value = ft.Name;
            cmd.Parameters.Add("Type", DbType.Int32).Value = ft.Type;
            cmd.Parameters.Add("LastChange",DbType.DateTime).Value= ft.LastChange;
            cmd.Parameters.Add("Progress",DbType.Int32).Value= ft.Progress;
            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Readers to file tree object convertor.
        /// </summary>
        /// <param name="Reader">The reader.</param>
        /// <returns></returns>
        public _m_File_Manager_Tree_FileTree_Node ReaderToFileTree(SQLiteDataReader Reader)
        {
            _m_File_Manager_Tree_FileTree_Node ft = new _m_File_Manager_Tree_FileTree_Node();
            ft.Id =int.Parse(Reader["Id"].ToString());
            ft.Id_Parent = int.Parse(Reader["Id_Parent"].ToString());
            ft.Name = Reader["Name"].ToString();
            ft.Type= (_m_File_Manager_Tree_FileTree_Node.enum_Type)int.Parse(Reader["Type"].ToString());
            ft.LastChange = DateTime.Parse(Reader["LastChange"].ToString());
            ft.Progress = (_m_File_Manager_Tree_FileTree_Node.enum_Progress)int.Parse(Reader["Progress"].ToString());
            return ft;
        }
        /// <summary>
        /// Gets the row count of db.
        /// </summary>
        /// <returns>integer count of all row on db</returns>
        public int GetRowCount()
        {
            string sql = "SELECT Count(*) FROM FileTree";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            int count = int.Parse(cmd.ExecuteScalar().ToString());
            return count;
        }

        /// <summary>
        /// Updates to on progress to ft.
        /// </summary>
        /// <param name="ft">The ft.</param>
        public void UpdateToOnProgress(_m_File_Manager_Tree_FileTree_Node ft)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update FileTree set Progress=:Progress  where id=:id";
            cmd.Parameters.Add("Progress", DbType.Int32).Value = _m_File_Manager_Tree_FileTree_Node.enum_Progress.On_Progress; ;
            cmd.Parameters.Add("Id", DbType.Int32).Value = ft.Id;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates to end progress to ft.
        /// </summary>
        /// <param name="ft">The ft.</param>
        public void UpdateToEndProgress(_m_File_Manager_Tree_FileTree_Node ft)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update FileTree set Progress=:Progress  where id=:id";
            cmd.Parameters.Add("Progress", DbType.Int32).Value = _m_File_Manager_Tree_FileTree_Node.enum_Progress.End_Progress; ;
            cmd.Parameters.Add("Id", DbType.Int32).Value = ft.Id;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the file tree node.
        /// </summary>
        /// <param name="ft">The ft.</param>
        public void UpdateFileTreeNode(_m_File_Manager_Tree_FileTree_Node ft)
        {
            SQLiteCommand cmd = new SQLiteCommand(dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update FileTree set Id_Parent=:Id_Parent, Name = :Name ,Type=:Type,LastChange=:LastChange ,Progress=:Progress ,Deleted=:Deleted,RenamedTo=:RenamedTo  where id=:id";
            cmd.Parameters.Add("Id_Parent", DbType.Int32).Value = ft.Id_Parent;
            cmd.Parameters.Add("Name", DbType.String).Value = ft.Name;
            cmd.Parameters.Add("Type", DbType.Int32).Value = ft.Type;
            cmd.Parameters.Add("LastChange", DbType.DateTime).Value = ft.LastChange;
            cmd.Parameters.Add("Progress", DbType.Int32).Value = ft.Progress;
            cmd.Parameters.Add("Deleted", DbType.Boolean).Value = ft.Deleted;
            cmd.Parameters.Add("RenamedTo", DbType.Int32).Value = ft.RenamedTo;
            cmd.Parameters.Add("Id", DbType.Int32).Value = ft.Id;
            cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// Gets the parent from path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return file tree node from parent ft</returns>
        public _m_File_Manager_Tree_FileTree_Node GetParentFromPath(string path)
        {
            string[] SplitedPath = SplitPath(path);
            SplitedPath[0] += "\\";
            int Id_Parent = 0;
            _m_File_Manager_Tree_FileTree_Node ft=null;
            int Counter = 0;
            foreach (string Piece in SplitedPath)
            {
                if(Counter==SplitedPath.Length-1)
                {
                    return ft;
                }
                Counter++;

                 ft = GetRowByParentIdAndName(Id_Parent, Piece);
                if(ft!=null)
                {
                    Id_Parent = ft.Id;
                }else
                {
                    return null;
                }
            }

            return ft;
        }
        /// <summary>
        /// Gets from path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return file tree node from ft address</returns>
        public _m_File_Manager_Tree_FileTree_Node GetFromPath(string path)
        {
            string[] SplitedPath = SplitPath(path);
            SplitedPath[0] += "\\";
            int Id_Parent = 0;
            _m_File_Manager_Tree_FileTree_Node ft = null;
            foreach (string Piece in SplitedPath)
            {
                ft = GetRowByParentIdAndName(Id_Parent, Piece);
                if (ft != null)
                {
                    Id_Parent = ft.Id;
                }
                else
                {
                    return null;
                }
            }

            return ft;
        }

        /// <summary>
        /// Gets the ft node of the row by parent identifier and name.
        /// </summary>
        /// <param name="Id_Parent">The identifier parent.</param>
        /// <param name="Name">The name.</param>
        /// <returns>return file tree node from ft parent id and name</returns>
        private _m_File_Manager_Tree_FileTree_Node GetRowByParentIdAndName(int Id_Parent,string Name)
        {
            string sql = "select * from FileTree where Id_Parent==:Id_Parent and Name==:Name limit 1";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("Id_Parent", DbType.Int32).Value = Id_Parent;
            cmd.Parameters.Add("Name", DbType.String).Value = Name;
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                _m_File_Manager_Tree_FileTree_Node parentft = ReaderToFileTree(reader);
                return parentft;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Splits the path by "\\".
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return string array from splitted input</returns>
        private string[] SplitPath(string path)
        {
            return path.Split(new string[] { "\\" }, StringSplitOptions.None);
        }
    }
}
