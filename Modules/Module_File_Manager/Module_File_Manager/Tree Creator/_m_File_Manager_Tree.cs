using System;
using System.IO;

namespace Module_File_Manager.Tree_Creator
{
    /// <summary>
    /// The main from sub module of file manager module for create tree of directores and files
    /// </summary>
    class _m_File_Manager_Tree
    {

        /// <summary>
        /// The ft database
        /// </summary>
        _m_File_Manager_Tree_FileTree_DB ftDB;
        /// <summary>
        /// The keep running publicly on this class
        /// when a _m_File_Manager_Tree object created and Record_Drive_Tree called on a new thread
        /// the new thread work StartWatching end after create watcher this bool variable 
        /// keep running a while on end of Record_Drive_Tree function
        /// for kill the thread set the keep running to false is enough
        /// </summary>
        public bool keepRunning = true;
        /// <summary>
        /// Records the drive tree.
        /// </summary>
        public void Record_Drive_Tree()
        {
            ftDB = new _m_File_Manager_Tree_FileTree_DB();
            ///<description>
            ///when row count in db is bigger than 0 start to record sub directoriesa and files
            ///from drives 
            ///</description>
            if (ftDB.GetRowCount() > 0)
            {
                while (keepRunning)
                {
                    _m_File_Manager_Tree_FileTree_Node ft = ftDB.GetNext();
                    if (ft != null)
                    {
                        Record_Directories(ft);
                        Record_Files(ft);
                        ///<description>
                        ///when every ft is get from db Progress flag is set to Not_Progress
                        ///on processing time the Progress is set to On_Progress
                        ///when sub directories and files from ft is inserted to db
                        ///the Progress flag set to End_Progress
                        ///</description>
                        ftDB.UpdateToEndProgress(ft);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            ///<description>
            ///when program frist run the db is null and row count is 0
            ///we frist get list of drive and insert on db then call Record_Drive_Tree again
            ///</description>
            else
            {
                DriveInfo[] Drives_Info = DriveInfo.GetDrives();
                foreach (DriveInfo drive in Drives_Info)
                {
                    if (drive.IsReady)
                    {
                        _m_File_Manager_Tree_FileTree_Node ft = new _m_File_Manager_Tree_FileTree_Node();
                        ft.Id_Parent = 0;
                        ft.Name = drive.Name;
                        ft.Type = _m_File_Manager_Tree_FileTree_Node.enum_Type.Drive;
                        ft.LastChange = DateTime.Now;
                        ft.Progress = _m_File_Manager_Tree_FileTree_Node.enum_Progress.Not_Progress;
                        ftDB.WriteNew(ft);
                    }
                }
                Record_Drive_Tree();
            }
        }


        /// <summary>
        /// Records the directories.
        /// </summary>
        /// <param name="ftParent">The ft parent.</param>
        private void Record_Directories(_m_File_Manager_Tree_FileTree_Node ftParent)
        {
            try
            {

                string[] Directorys_in_target_Directory = Directory.GetDirectories(ftParent.Name, "*");
                foreach (string dir in Directorys_in_target_Directory)
                {
                    DirectoryInfo dirinfo = new DirectoryInfo(dir);
                    _m_File_Manager_Tree_FileTree_Node ft = new _m_File_Manager_Tree_FileTree_Node();
                    ft.Id_Parent = ftParent.Id;
                    ft.Name = dirinfo.Name;
                    ft.Type = _m_File_Manager_Tree_FileTree_Node.enum_Type.Directory;
                    ft.LastChange = DateTime.Now;
                    ft.Progress = _m_File_Manager_Tree_FileTree_Node.enum_Progress.Not_Progress;
                    ftDB.WriteNew(ft);
                }
            }
            catch (Exception ex)
            {
                return;
            }

        }

        /// <summary>
        /// Records the files.
        /// </summary>
        /// <param name="ftParent">The ft parent.</param>
        private void Record_Files(_m_File_Manager_Tree_FileTree_Node ftParent)
        {
            try
            {
                string[] Files_In_Target_Directory = Directory.GetFiles(ftParent.Name, "*");
                foreach (string file in Files_In_Target_Directory)
                {
                    FileInfo fileinfo = new FileInfo(file);
                    _m_File_Manager_Tree_FileTree_Node ft = new _m_File_Manager_Tree_FileTree_Node();
                    ft.Id_Parent = ftParent.Id;
                    ft.Name = fileinfo.Name;
                    ft.Type = _m_File_Manager_Tree_FileTree_Node.enum_Type.File;
                    ft.LastChange = DateTime.Now;
                    ft.Progress = _m_File_Manager_Tree_FileTree_Node.enum_Progress.End_Progress;
                    ftDB.WriteNew(ft);

                }
            }
            catch (Exception ex)
            {
                return;
            }
        }


    }
}
