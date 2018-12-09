namespace Module_File_Manager.Content_Searcher
{
    /// <summary>
    /// The sub module textual_Searcher from sub module of file manager module for search on content of files
    /// </summary>
    class _m_File_Manager_Content_Searcher_textual_Searcher
    {
        /// <summary>
        /// Textuals to text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return full content string from file</returns>
        public static string TextualToText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
        
    }
}
