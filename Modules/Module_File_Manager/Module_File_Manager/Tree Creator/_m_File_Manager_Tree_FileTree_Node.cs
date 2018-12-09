using System;

namespace Module_File_Manager.Tree_Creator
{
    /// <summary>
    /// The ft node schema
    /// </summary>
    class _m_File_Manager_Tree_FileTree_Node
    {
        /// <summary>
        /// The identifier of db row file or dir or drive
        /// </summary>
        int _Id;
        /// <summary>
        /// The identifier parent of db row means parent dir of file or dir
        /// the parent of drive is zero
        /// </summary>
        int _Id_Parent;
        /// <summary>
        /// The short name of file or dir or drive
        /// </summary>
        string _Name;
        /// <summary>
        /// enum show type of row dir or file or drive
        /// </summary>
        public enum enum_Type { Drive = 0, Directory = 1, File = 2 };
        enum_Type _Type;
        /// <summary>
        /// The last change of row in db
        /// </summary>
        DateTime _LastChange;
        /// <summary>
        /// status of get and processing and insert childs from dir or drive
        /// </summary>
        public enum enum_Progress { Not_Progress = 0, On_Progress = 1, End_Progress = 2 }
        enum_Progress _Progress;
        /// <summary>
        /// The deleted flag status of file or dir 
        /// when the dir is deleted all child dirs and files flag set to deleted
        /// </summary>
        bool _Deleted;
        /// <summary>
        /// The renamed to  status of file or dir
        /// when file or dir renamed the renamed column set to new id of new inserted row
        /// when the dir is renamed all child dirs and files parent id set to id of new inserted row
        /// </summary>
        int _RenamedTo;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public int Id_Parent
        {
            get { return _Id_Parent; }
            set { _Id_Parent = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public enum_Type Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public DateTime LastChange
        {
            get { return _LastChange; }
            set { _LastChange = value; }
        }

        public enum_Progress Progress
        {
            get { return _Progress; }
            set { _Progress = value; }
        }

        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }
        public int RenamedTo
        {
            get { return _RenamedTo; }
            set { _RenamedTo = value; }
        }
        
    }
}
