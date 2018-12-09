using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic;
using System.Diagnostics;
using Utils;
using System.Threading;
using Module_File_Manager.Response;
using System.Reflection;

namespace Module_File_Manager
{
    /// <summary>
    /// The file manager module main
    /// </summary>
    public class _m_File_Manager
    {
        /// <summary>
        /// The MFMW is object of file watcher publiclly on this class
        /// use at OnStartFileWatcher for init
        /// use at OnStopFileWatcher for un init
        /// use at OnAddFilterToFileWatcher for add a folder address to filtered folder list
        /// use at OnRemoveFilterFromFileWatcher for remove a folder address from filtered folder list
        /// </summary>
        Tree_Watcher._m_File_Manager_Watcher mfmw=null;

        /// <summary>
        /// The MFMT is object of file tree creator publically on this class
        /// use at OnStartCreateDrivesTree for init
        /// use at OnStopCreateDrivesTree for un init
        /// </summary>
        Tree_Creator._m_File_Manager_Tree mfmt = null;

        /// <summary>
        /// Called when [search in file content].
        /// </summary>
        /// <param name="ListToSearch">The list to search.</param>
        /// <param name="KeyToSearch">The key to search.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>OnSearchInFileContentResponse object.if successful return found list  else return error</returns>
        public OnSearchInFileContentResponse OnSearchInFileContent(List<string> ListToSearch, string KeyToSearch, Module_File_Manager.Content_Searcher._m_File_Manager_Content_Searcher.Enum_direction direction)
        {
            OnSearchInFileContentResponse ROnSearchInFileContentResponse = new OnSearchInFileContentResponse();
            ROnSearchInFileContentResponse.KeyToSearch = KeyToSearch;
            try
            {
                List<string> Finded= Module_File_Manager.Content_Searcher._m_File_Manager_Content_Searcher.Content_Searcher(ListToSearch, KeyToSearch, direction);
                ROnSearchInFileContentResponse.Paths = Finded;
            }
            catch (Exception ex)
            {
                ROnSearchInFileContentResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnSearchInFileContentResponse;
        }
        /// <summary>
        /// Called when [delete directory].
        /// </summary>
        /// <param name="Target_Directory">The target directory.</param>
        /// <returns>OnDeleteDirectoryResponse object.if successful return true  else return error</returns>
        public OnDeleteDirectoryResponse OnDeleteDirectory(string Target_Directory)
        {
            OnDeleteDirectoryResponse ROnDeleteDirectoryResponse = new OnDeleteDirectoryResponse();
            ROnDeleteDirectoryResponse.Target_Directory = Target_Directory;
            try {
                Directory.Delete(Target_Directory, true);
                ROnDeleteDirectoryResponse.Sucsess = true;
            }
            catch(Exception ex)
            {
                ROnDeleteDirectoryResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnDeleteDirectoryResponse;
        }
        /// <summary>
        /// Called when [delete file].
        /// </summary>
        /// <param name="Target_File">The target file.</param>
        /// <returns>OnDeleteFileResponse object.if successful return true  else return error</returns>
        public OnDeleteFileResponse OnDeleteFile(string Target_File)
        {
            OnDeleteFileResponse ROnDeleteFileResponse = new OnDeleteFileResponse();
            ROnDeleteFileResponse.Target_File = Target_File;
            try {
                File.Delete(Target_File);
                ROnDeleteFileResponse.Sucsess = true;
            }catch(Exception ex)
            {
                ROnDeleteFileResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnDeleteFileResponse;
        }
        /// <summary>
        /// Called when [rename file].
        /// </summary>
        /// <param name="Target_Source">The target source.</param>
        /// <param name="Target_Destination">The target destination.</param>
        /// <returns>OnRenameFileResponse object.if successful return true  else return error</returns>
        public OnRenameFileResponse OnRenameFile(string Target_Source, string Target_Destination)
        {
            return (OnRenameFileResponse)OnCutFile(Target_Source, Target_Destination);
        }
        /// <summary>
        /// Called when [cut file].
        /// </summary>
        /// <param name="Target_Source">The target source.</param>
        /// <param name="Target_Destination">The target destination.</param>
        /// <returns>OnCutFileResponse object.if successful return true  else return error</returns>
        public OnCutFileResponse OnCutFile(string Target_Source,string Target_Destination)
        {
            OnCutFileResponse ROnCutFileResponse = new OnCutFileResponse();
            ROnCutFileResponse.Target_Source = Target_Source;
            ROnCutFileResponse.Target_Destination = Target_Destination;
            try
            {
                File.Move(Target_Source, Target_Destination);
                ROnCutFileResponse.Sucsess = true;
            }catch(Exception ex)
            {
                ROnCutFileResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnCutFileResponse;
        }

        /// <summary>
        /// Called when [copy file].
        /// </summary>
        /// <param name="Target_Source">The target source.</param>
        /// <param name="Target_Destination">The target destination.</param>
        /// <returns>OnCutFileResponse object.if successful return true  else return error</returns>
        public OnCopyFileResponse OnCopyFile(string Target_Source,string Target_Destination)
        {
            OnCopyFileResponse ROnCopyFileResponse = new OnCopyFileResponse();
            ROnCopyFileResponse.Target_Source = Target_Source;
            ROnCopyFileResponse.Target_Destination = Target_Destination;
            try
            {
                File.Copy(Target_Source, Target_Destination);
                ROnCopyFileResponse.Sucsess = true;
            }
            catch(Exception ex)
            {
                ROnCopyFileResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnCopyFileResponse;
        }
        /// <summary>
        /// Called when [new file].
        /// </summary>
        /// <param name="Target_File">The target file.</param>
        /// <returns>OnNewFileResponse object.if successful return new file name else return error</returns>
        public OnNewFileResponse OnNewFile(string Target_File)
        {
            OnNewFileResponse ROnNewFileResponse = new OnNewFileResponse();
            ROnNewFileResponse.Target_File = Target_File;
            try {
                FileStream Resualt = File.Create(Target_File);
                ROnNewFileResponse.Sucsess = true;
            }catch(Exception ex)
            {
                ROnNewFileResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnNewFileResponse;
        }
        /// <summary>
        /// Called when [new dir].
        /// </summary>
        /// <param name="Target_Dir">The target dir.</param>
        /// <returns>OnNewDirResponse object.if successful return new dir name else return error</returns>
        public OnNewDirResponse OnNewDir(string Target_Dir)
        {
            OnNewDirResponse ROnNewDirResponse = new OnNewDirResponse();
            ROnNewDirResponse.Target_File = Target_Dir;
            try {
                
                DirectoryInfo Resualt = Directory.CreateDirectory(Target_Dir);
                ROnNewDirResponse.Sucsess = true;
            }
            catch(Exception ex)
            {
                ROnNewDirResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnNewDirResponse;
        }
        /// <summary>
        /// Called when [execute].
        /// </summary>
        /// <param name="Target_File">The target file.</param>
        /// <param name="Arguments">The arguments.</param>
        /// <returns>json object.if successful return new process id else return error</returns>
        public OnExecuteResponse OnExecute(string Target_File, string Arguments="")
        {
            OnExecuteResponse ROnExecuteResponse = new OnExecuteResponse();
            ROnExecuteResponse.Target_File = Target_File;
            ROnExecuteResponse.Arguments = Arguments;
            try {
                Process Resualt = Process.Start(Target_File, Arguments);
                ROnExecuteResponse.Pid=Resualt.Id;
            }
            catch(Exception ex)
            {
                ROnExecuteResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnExecuteResponse;
        }
        /// <summary>
        /// Called when [drives].
        /// </summary>
        /// <returns>OnDrivesResponse object.if successful return drive and drive description else return error</returns>
        public OnDrivesResponse OnDrives()
        {

            OnDrivesResponse ROnDrivesResponse = new OnDrivesResponse();
            try {
                System.IO.DriveInfo[] Drives_Info = System.IO.DriveInfo.GetDrives();

                foreach (DriveInfo drive in Drives_Info)
                {
                    OnDrivesResponse.Drive RDrive = new OnDrivesResponse.Drive();
                    RDrive.Name = drive.Name;
                    RDrive.DriveFormat= drive.DriveFormat;
                    RDrive.AvailableFreeSpace = drive.AvailableFreeSpace;
                    RDrive.Drivetype = drive.DriveType;
                    RDrive.IsReady = drive.IsReady;
                    RDrive.TotalFreeSpace = drive.TotalFreeSpace;
                    RDrive.TotalSize = drive.TotalSize;
                    RDrive.VolumeLabel = drive.VolumeLabel;
                    ROnDrivesResponse.Drives.Add(RDrive);
                }
            }catch(Exception ex)
            {
                ROnDrivesResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnDrivesResponse;
        }
        /// <summary>
        /// Gets the shortcut target file.
        /// </summary>
        /// <param name="shortcutFilename">The shortcut filename.</param>
        /// <returns>get target file of shortcut file</returns>
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            try
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutFilename);
                return link.TargetPath;
            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }
        /// <summary>
        /// Called when [dir].
        /// </summary>
        /// <param name="Input_Directory">The input directory.</param>
        /// <returns>OnDirResponse object.all files or folders and that info</returns>
        public OnDirResponse OnDir(string Input_Directory)
        {
            OnDirResponse JOnDir = new OnDirResponse();
            try {
                string[] Files_In_Target_Directory = Directory.GetFiles(Input_Directory, "*");
                foreach (string file in Files_In_Target_Directory)
                {
                    OnDirResponse.FileOrDir fileOrdir = new OnDirResponse.FileOrDir();
                    fileOrdir.Path = file;
                    fileOrdir.File_Property_Info= GetProperty(JOnDir, file);
                    JOnDir.fileOrdir.Add(fileOrdir);
                }

                string[] Directorys_in_target_Directory = Directory.GetDirectories(Input_Directory, "*");
                foreach (string dir in Directorys_in_target_Directory)
                {
                    OnDirResponse.FileOrDir fileOrdir = new OnDirResponse.FileOrDir();
                    fileOrdir.Path = dir;
                    fileOrdir.File_Property_Info = GetProperty(JOnDir, dir);
                    JOnDir.fileOrdir.Add(fileOrdir);
                }
                
                
            }
            catch(Exception ex)
            {
                JOnDir.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return JOnDir;
        }


        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="ResponseMain">The response main.</param>
        /// <param name="File_Or_Directory">The file or directory.</param>
        /// <returns>the property info of fiel or folder</returns>
        private OnDirResponse.FileOrDir.Property GetProperty(OnDirResponse ResponseMain, string File_Or_Directory)
        {
            OnDirResponse.FileOrDir.Property JGetProperty = new OnDirResponse.FileOrDir.Property();
            try {
                
                FileInfo fi = new FileInfo(File_Or_Directory);
                JGetProperty.Attributes = fi.Attributes;

                if (fi.Attributes.HasFlag(FileAttributes.Directory))//is dir
                {
                    JGetProperty.Type = "Dir";
                }
                else //is file
                {

                    JGetProperty.Type = "File";
                    JGetProperty.Length = fi.Length;//why Len is here? because just file have len

                }
                JGetProperty.Accesses = GetAccessControl(ResponseMain,File_Or_Directory);
                JGetProperty.CreationTime = fi.CreationTime;
                JGetProperty.IsReadOnly = fi.IsReadOnly;
                JGetProperty.LastAccessTime = fi.LastAccessTime;
                JGetProperty.LastWriteTime = fi.LastWriteTime;

                
            }
            catch(Exception ex)
            {
                ResponseMain.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return JGetProperty;
        }



        /// <summary>
        /// Gets the access control.
        /// </summary>
        /// <param name="ResponseMain">The response main.</param>
        /// <param name="File_Or_Directory">The file or directory.</param>
        /// <returns>reeturn acess control list of file or folder</returns>
        private List<OnDirResponse.FileOrDir.Property.AccessControl> GetAccessControl(OnDirResponse ResponseMain,string File_Or_Directory)
        {
            List<OnDirResponse.FileOrDir.Property.AccessControl> JGetAccessControl = new List<OnDirResponse.FileOrDir.Property.AccessControl>();
            try {
                System.Security.AccessControl.FileSecurity file_sec = System.IO.File.GetAccessControl(File_Or_Directory);
                System.Security.AccessControl.AuthorizationRuleCollection ruls = file_sec.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
                foreach (System.Security.AccessControl.FileSystemAccessRule rul in ruls)
                {
                    OnDirResponse.FileOrDir.Property.AccessControl JRul = new OnDirResponse.FileOrDir.Property.AccessControl();
                    JRul.IdentityReference = rul.IdentityReference.Value;
                    JRul.FileSystemRights = rul.FileSystemRights.ToString();
                    JRul.AccessControlType = rul.AccessControlType.ToString();
                    JRul.IsInherited = rul.IsInherited ? "Inherited" : "Explicit";
                    JRul.PropagationFlags = rul.PropagationFlags.ToString();
                    JRul.InheritanceFlags = rul.InheritanceFlags.ToString();
                    JRul.Owner = System.IO.File.GetAccessControl(File_Or_Directory).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();//
                    JGetAccessControl.Add(JRul);
                }
                
            }catch(Exception ex)
            {
                ResponseMain.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return JGetAccessControl;
        }

        /// <summary>
        /// Called when [set attributes].
        /// </summary>
        /// <param name="File_Or_Directory">The file or directory.</param>
        /// <param name="Attributes">The attributes.</param>
        /// <returns>OnSetAttributesResponse object.if successful return new file or directory attrib else return error</returns>
        public OnSetAttributesResponse OnSetAttributes(string File_Or_Directory, FileAttributes Attributes)
        {
            OnSetAttributesResponse ROnSetAttributesResponse = new OnSetAttributesResponse();
            ROnSetAttributesResponse.File_Or_Directory = File_Or_Directory;
            ROnSetAttributesResponse.Input_Attributes = Attributes;
            try
            {
                FileInfo fi = new FileInfo(File_Or_Directory);
                fi.Attributes =Attributes;
                ROnSetAttributesResponse.Output_Attributes = fi.Attributes;
            }catch(Exception ex)
            {
                ROnSetAttributesResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnSetAttributesResponse;
        }

        /// <summary>
        /// Called when [search].
        /// </summary>
        /// <param name="Target_Directory">The target directory.</param>
        /// <param name="Search_Pattern">The search pattern.</param>
        /// <param name="Include_Sub_Directory">The include sub directory.</param>
        /// <param name="Filter_Query">The filter query.</param>
        /// <returns>OnSearchResponse object.if successful return a list of file match with Filter_Query and Search_Pattern(if no error and no file find return empty list) else return error</returns>
        public OnSearchResponse OnSearch(string Target_Directory, string Search_Pattern, int Include_Sub_Directory,
            string Filter_Query = "1=1")
        {

            OnSearchResponse ROnSearchResponse = new OnSearchResponse();
            ROnSearchResponse.Target_Directory = Target_Directory;
            ROnSearchResponse.Search_Pattern = Search_Pattern;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Target_Directory);
                IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles(Search_Pattern, (SearchOption)Include_Sub_Directory);
                var query = fileList.Where(Filter_Query);
                foreach (FileInfo file in query)
                {
                    ROnSearchResponse.Paths.Add(file.FullName);
                }
            }
            catch (Exception ex)
            {
                ROnSearchResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnSearchResponse;
        }

        /// <summary>
        /// Called when [start get tree].
        /// </summary>
        /// <returns>OnStartCreateDrivesTreeResponse object.if successful return a "CreateDrivesTreeStarted" Message and record tree to Tree_Creator.sqlite database of windows dir include files else return error</returns>
        public OnStartCreateDrivesTreeResponse OnStartCreateDrivesTree()
        {
            OnStartCreateDrivesTreeResponse ROnStartCreateDrivesTreeResponse = new OnStartCreateDrivesTreeResponse();
            try
            {
                JArray JTrees = new JArray();

                if (mfmt == null)
                {
                    mfmt = new Tree_Creator._m_File_Manager_Tree();
                    Thread Tree_CreatorThread = new Thread(new ThreadStart(mfmt.Record_Drive_Tree));
                    Tree_CreatorThread.Start();
                    ROnStartCreateDrivesTreeResponse.Description= "CreateDrivesTreeStarted";
                }
                else
                {
                    ROnStartCreateDrivesTreeResponse.Description = "CreateDrivesTreeStartedBefore";
                }
            }
            catch(Exception ex)
            {
                ROnStartCreateDrivesTreeResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }

            return ROnStartCreateDrivesTreeResponse;
        }
        /// <summary>
        /// Called when [stop create drives tree].
        /// </summary>
        /// <returns>OnStopCreateDrivesTreeResponse object</returns>
        public OnStopCreateDrivesTreeResponse OnStopCreateDrivesTree()
        {
            OnStopCreateDrivesTreeResponse ROnStopCreateDrivesTreeResponse = new OnStopCreateDrivesTreeResponse();
            try
            {
                if (mfmt != null)
                {
                    mfmt.keepRunning = false;
                    mfmt = null;
                    ROnStopCreateDrivesTreeResponse.Description = "CreateDrivesTreeStoped";
                }
                else
                {
                    ROnStopCreateDrivesTreeResponse.Description= "CreateDrivesTreeStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopCreateDrivesTreeResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopCreateDrivesTreeResponse;
        }


        /// <summary>
        /// Called when [start file watcher].
        /// </summary>
        /// <returns>OnStartFileWatcherResponse object.if successful return a "FileWatcherStarted" Message if File watcher is already running return "FileWatcherStartedBefore" message else return error</returns>
        public OnStartFileWatcherResponse OnStartFileWatcher()
        {
            OnStartFileWatcherResponse ROnStartFileWatcherResponse = new OnStartFileWatcherResponse();
            try
            {
                if (mfmw == null)
                {
                    mfmw = new Tree_Watcher._m_File_Manager_Watcher();
                    Thread FileWatcherThread = new Thread(new ThreadStart(mfmw.StartWatching));
                    FileWatcherThread.Start();
                    ROnStartFileWatcherResponse.Description= "FileWatcherStarted";
                }
                else
                {
                    ROnStartFileWatcherResponse.Description= "FileWatcherStartedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStartFileWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartFileWatcherResponse;
        }
        /// <summary>
        /// Called when [stop file watcher].
        /// </summary>
        /// <returns>OnStopFileWatcherResponse object.if successful return a "FileWatcherStoped" Message if File watcher is already stoped or not created ergo return "FileWatcherStopedBefore" message else return error</returns>
        public OnStopFileWatcherResponse OnStopFileWatcher()
        {
            OnStopFileWatcherResponse ROnStopFileWatcherResponse = new OnStopFileWatcherResponse();
            try
            {
                if (mfmw != null)
                {
                    mfmw.keepRunning = false;
                    mfmw = null;
                    ROnStopFileWatcherResponse.Description = "FileWatcherStoped";
                }
                else
                {
                    ROnStopFileWatcherResponse.Description = "FileWatcherStopedBefore";
                }
            }
            catch (Exception ex)
            {
                ROnStopFileWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopFileWatcherResponse;
        }
        /// <summary>
        /// Called when [add filter to file watcher].
        /// </summary>
        /// <param name="FilterdFolder">The filterd folder.</param>
        /// <returns>OnAddFilterToFileWatcherResponse object.if successful return a "NewFilteredFolderAdded" Message if File watcher is stoped or not created ergo return "FileWatcherIsNotRunning" message else return error</returns>
        public OnAddFilterToFileWatcherResponse OnAddFilterToFileWatcher(string FilterdFolder)
        {
            OnAddFilterToFileWatcherResponse ROnAddFilterToFileWatcherResponse = new OnAddFilterToFileWatcherResponse();
            try
            {
                if(mfmw!=null)
                {
                    mfmw.AddFilterFolder(FilterdFolder);
                    ROnAddFilterToFileWatcherResponse.Description= "NewFilteredFolderAdded";
                }
                else
                {
                    ROnAddFilterToFileWatcherResponse.Description= "FileWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnAddFilterToFileWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnAddFilterToFileWatcherResponse;
        }
        /// <summary>
        /// Called when [remove filter from file watcher].
        /// </summary>
        /// <param name="FilterdFolder">The filterd folder.</param>
        /// <returns>OnRemoveFilterFromFileWatcherResponse object.if successful return a "FilteredFolderRemoved" Message and if the old filter is not find for remove return "FilteredFolderNotFindToRemoved" message if File watcher is stoped or not created ergo return "FileWatcherIsNotRunning" message else return error</returns>
        public OnRemoveFilterFromFileWatcherResponse OnRemoveFilterFromFileWatcher(string FilterdFolder)
        {
            OnRemoveFilterFromFileWatcherResponse ROnRemoveFilterFromFileWatcherResponse = new OnRemoveFilterFromFileWatcherResponse();
            try
            {
                if (mfmw != null)
                {
                   if(mfmw.RemoveFilterFolder(FilterdFolder))
                    {
                        ROnRemoveFilterFromFileWatcherResponse.Description = "FilteredFolderRemoved";
                    }
                    else
                    {
                        ROnRemoveFilterFromFileWatcherResponse.Description= "FilteredFolderNotFindToRemoved";
                    }
                    
                }
                else
                {
                    ROnRemoveFilterFromFileWatcherResponse.Description = "FileWatcherIsNotRunning";
                }
            }
            catch (Exception ex)
            {
                ROnRemoveFilterFromFileWatcherResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnRemoveFilterFromFileWatcherResponse;
        }

    }
}
