using System;
using System.Collections.Generic;
using System.IO;


namespace Module_File_Manager.Tree_Watcher
{
    /// <summary>
    /// The directory watcher sub module from file manager module
    /// please using this class with thread because of endless loop
    /// </summary>
    class _m_File_Manager_Watcher
    {

        /// <summary>
        /// Adds the filter folder.
        /// </summary>
        /// <param name="FilterdFolderAddress">The filterd folder address.</param>
        public void AddFilterFolder(string FilterdFolderAddress)
        {
            mfmwd.AddBlackProcessToDB(FilterdFolderAddress);
            FilterFolderList.Add(FilterdFolderAddress);
        }
        /// <summary>
        /// Removes the filter folder.
        /// </summary>
        /// <param name="FilterdFolderAddress">The filterd folder address.</param>
        /// <returns></returns>
        public bool RemoveFilterFolder(string FilterdFolderAddress)
        {
            mfmwd.RemoveBlackProcessFromDB(FilterdFolderAddress);
            return FilterFolderList.Remove(FilterdFolderAddress);
        }

        /// <summary>
        /// The keep running publicly on this class
        /// when a _m_File_Manager_Watcher object created and StartWatching called on a new thread
        /// the new thread work StartWatching end after create watcher this bool variable 
        /// keep running a while on end of StartWatching function
        /// for kill the thread set the keep running to false is enough
        /// </summary>
        public bool keepRunning = true;
        /// <summary>
        /// The filter folder list contain folder no watching set for them
        /// </summary>
        private List<string> FilterFolderList;
        /// <summary>
        /// The MSMWD is object of file watcher database publicly on this class
        /// use at _m_File_Manager_Watcher for init
        /// use at AddFilterFolder for add a folder address to filtered folder list
        /// use at RemoveFilterFolder for remove a folder address from filtered folder list
        /// </summary>
        _m_File_Manager_Watcher_DB mfmwd = null;
        /// <summary>
        /// The MFMTFDB is object of file tree creator database publicly on this class
        /// use at _m_File_Manager_Watcher for init
        /// use at Watcher_Renamed for write file or folder renamed event to db
        /// use at SetNewftToParentAllSubFT for write change parent of file or folder 
        /// use at Watcher_Deleted for write file or folder delete event to db
        /// use at SetDeletedFlagToAllSubFT for write delete flag on sub file and folder to db
        /// use at Watcher_Changed for write file or folder change event to db
        /// use at WriteChange for write all change of file or folder to db
        /// </summary>
        Tree_Creator._m_File_Manager_Tree_FileTree_DB mfmtfdb=null;
        /// <summary>
        /// Initializes a new instance of the <see cref="_m_File_Manager_Watcher"/> class.
        /// </summary>
        public _m_File_Manager_Watcher()
        {
            mfmtfdb = new Tree_Creator._m_File_Manager_Tree_FileTree_DB();
            mfmwd = new _m_File_Manager_Watcher_DB();
            FilterFolderList = mfmwd.GetAllFilteredFolderAddress();
        }

        /// <summary>
        /// Starts the watching.
        /// </summary>
        public void StartWatching()
        {
            System.IO.DriveInfo[] Drives_Info = System.IO.DriveInfo.GetDrives();
            foreach (DriveInfo drive in Drives_Info)
            {
                ///<description>
                ///check is ready of drive because when the virtual empty drive or cd drive
                ///is empty excption is happend
                ///</description>
                if (drive.IsReady)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    ///<description>
                    ///for know about 6553600 magic number go to
                    ///https://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.internalbuffersize(v=vs.110).aspx
                    ///</description>
                    watcher.InternalBufferSize = 6553600;
                    watcher.Path = drive.Name;
                    watcher.IncludeSubdirectories = true;
                    ///<description>
                    ///get all change
                    ///</description>
                    watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime
                        | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                        | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                    watcher.Changed += Watcher_Changed;
                    watcher.Created += Watcher_Created;
                    watcher.Deleted += Watcher_Deleted;
                    watcher.Error += Watcher_Error;
                    watcher.Renamed += Watcher_Renamed;
                    watcher.EnableRaisingEvents = true;
                }
            }

            while (keepRunning)
            {
                ///<description>
                ///when application is form base this work for anti freez message queue 
                ///on command base program this line is no mean
                ///</description>
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <summary>
        /// Handles the Renamed event of the Watcher control.
        /// if the renamed is folder all child dir and files get new parent 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenamedEventArgs"/> instance containing the event data.</param>
        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (IsFiltered(e.FullPath))
                return;
            
            Tree_Creator._m_File_Manager_Tree_FileTree_Node oldft = mfmtfdb.GetFromPath(e.OldFullPath);
            if (oldft != null)
            {

                WriteChange(e);
                Tree_Creator._m_File_Manager_Tree_FileTree_Node newft = mfmtfdb.GetFromPath(e.FullPath);
                oldft.LastChange = newft.LastChange;
                oldft.Deleted = true;
                oldft.RenamedTo = newft.Id;

                mfmtfdb.UpdateFileTreeNode(oldft);
                SetNewftToParentAllSubFT(newft, oldft);
            }
            else
            {
                WriteChange(e);
            }
        }
        /// <summary>
        /// Sets the newft to parent all sub ft.
        /// </summary>
        /// <param name="newft">The newft.</param>
        /// <param name="oldft">The oldft.</param>
        private void SetNewftToParentAllSubFT(Tree_Creator._m_File_Manager_Tree_FileTree_Node newft, Tree_Creator._m_File_Manager_Tree_FileTree_Node oldft)
        {
            while (true)
            {
                List<Tree_Creator._m_File_Manager_Tree_FileTree_Node> Childs = mfmtfdb.GetChilds(oldft);
                if (Childs != null)
                    foreach (Tree_Creator._m_File_Manager_Tree_FileTree_Node Child in Childs)
                    {
                        Child.LastChange = DateTime.Now;
                        Child.Id_Parent = newft.Id;
                        mfmtfdb.UpdateFileTreeNode(Child);
                    }
                break;
            }

        }
        /// <summary>
        /// Handles the Error event of the Watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ErrorEventArgs"/> instance containing the event data.</param>
        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the Deleted event of the Watcher control.
        /// if the renamed is folder all child dir and files get deleted flag
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (IsFiltered(e.FullPath))
                return;
            Tree_Creator._m_File_Manager_Tree_FileTree_Node ft = mfmtfdb.GetFromPath(e.FullPath);
            if (ft != null)
            {
                ft.LastChange = DateTime.Now;
                ft.Deleted = true;
                mfmtfdb.UpdateFileTreeNode(ft);
                SetDeletedFlagToAllSubFT(ft);
            }
            else
            {

                ///<description>
                ///if the deleted file or folder is not find ergo when the file is created
                ///the whatcher is not up (for many reason like program is down or any)
                ///now we go back to top and top to find last parent is logged into db
                ///after find that start to record all childs to end after that 
                ///set deleted flag to current file or dir
                ///</description>
                WriteChange(e);
                Watcher_Deleted(sender, e);
            }
        }

        /// <summary>
        /// Sets the deleted flag to all sub ft.
        /// </summary>
        /// <param name="ft">The ft.</param>
        private void SetDeletedFlagToAllSubFT(Tree_Creator._m_File_Manager_Tree_FileTree_Node ft)
        {
            while(true)
            {
                List<Tree_Creator._m_File_Manager_Tree_FileTree_Node> Childs=  mfmtfdb.GetChilds(ft);
                if(Childs!=null)
                foreach(Tree_Creator._m_File_Manager_Tree_FileTree_Node Child in Childs)
                {
                    Child.LastChange = DateTime.Now;
                    Child.Deleted = true;
                    mfmtfdb.UpdateFileTreeNode(Child);
                    SetDeletedFlagToAllSubFT(Child);
                }
                break;
            }
        }
        /// <summary>
        /// Handles the Created event of the Watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            //if (IsFiltered(e.FullPath))
            //    return;
            WriteChange(e);
        }

        /// <summary>
        /// Handles the Changed event of the Watcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (IsFiltered(e.FullPath))
                return;
            Tree_Creator._m_File_Manager_Tree_FileTree_Node ft = mfmtfdb.GetFromPath(e.FullPath);
            if (ft != null)
            {
                ft.LastChange = DateTime.Now;
                mfmtfdb.UpdateFileTreeNode(ft);
            }
            else
            {
                WriteChange(e);
            }
        }


        /// <summary>
        /// Writes the change.
        /// </summary>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void WriteChange(FileSystemEventArgs e)
        {
            if (IsFiltered(e.FullPath))
                return;
            Tree_Creator._m_File_Manager_Tree_FileTree_Node Parent_ft = mfmtfdb.GetParentFromPath(e.FullPath);
            if (Parent_ft != null)
            {
                Tree_Creator._m_File_Manager_Tree_FileTree_Node new_child_ft = new Tree_Creator._m_File_Manager_Tree_FileTree_Node();
                new_child_ft.Id_Parent = Parent_ft.Id;
                new_child_ft.Name = GetNameFromFullPath(e.FullPath);
                FileInfo fi = new FileInfo(e.FullPath);
                ///<description>
                ///detect file or dir
                ///when file has attribiute -1 hasflag not work and detect file like dir
                ///i think when file is open by another process attribiute is -1
                ///</description>
                //Additional information: Access to the path 'C:\Windows\Prefetch\CONSENT.EXE-65F6206D.pf' is denied.
                try
                {
                    if (fi.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        new_child_ft.Type = Tree_Creator._m_File_Manager_Tree_FileTree_Node.enum_Type.Directory;
                    }
                    else
                    {
                        new_child_ft.Type = Tree_Creator._m_File_Manager_Tree_FileTree_Node.enum_Type.File;
                    }
                }
                catch (Exception ex)
                {
                    new_child_ft.Type = Tree_Creator._m_File_Manager_Tree_FileTree_Node.enum_Type.File;
                }
                new_child_ft.LastChange = DateTime.Now;
                ///<description>
                ///set End_Progress to all new file and dir because before change it . on tree creator
                ///log all dir and folder
                ///the new dir has no child
                ///and when create new child record again
                ///</description>
                new_child_ft.Progress = Tree_Creator._m_File_Manager_Tree_FileTree_Node.enum_Progress.End_Progress;
                mfmtfdb.WriteNew(new_child_ft);
            }
            else
            {

                ///<description>
                ///if the deleted file or folder is not find ergo when the file is created
                ///the whatcher is not up (for many reason like program is down or any)
                ///now we go back to top and top to find last parent is logged into db
                ///after find that start to record all childs to end after that 
                ///</description>


                FileInfo fi = new FileInfo(e.FullPath);
                while (fi.Directory != null)
                {
                    fi = new FileInfo(fi.Directory.FullName);
                    Parent_ft = mfmtfdb.GetParentFromPath(fi.FullName);
                    if (Parent_ft != null)
                    {
                        string directory = fi.FullName.Substring(0, fi.FullName.LastIndexOf("\\"));
                        string name = fi.FullName.Substring(fi.FullName.LastIndexOf("\\")+1);
                        FileSystemEventArgs ee = new FileSystemEventArgs(new WatcherChangeTypes(), directory, name);
                        WriteChange(ee);
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// Determines whether the specified path is filtered.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is filtered; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFiltered(string path)
        {
            foreach (string Filter in FilterFolderList)
            {
                if (path.Contains(Filter))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the name from full path.
        /// </summary>
        /// <param name="FullPath">The full path.</param>
        /// <returns></returns>
        private string GetNameFromFullPath(string FullPath)
        {
            string[] SplitedPath= FullPath.Split(new string[] { "\\" }, StringSplitOptions.None);
            return SplitedPath[SplitedPath.Length-1];
        }
    }
}
