using Microsoft.Office.Interop.Word;

namespace Module_File_Manager.Content_Searcher
{
    /// <summary>
    /// The sub module word_Searcher from sub module of file manager module for search on content of files
    /// </summary>
    class _m_File_Manager_Content_Searcher_word_Searcher
    {
        /// <summary>
        /// Words to text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return extracted all string from word doc and docx file</returns>
        public static string WordToText(string path)
        {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Document doc = app.Documents.Open(path);

            string allWords = doc.Content.Text;
            doc.Close();
            app.Quit();
            return allWords;
        }

    }
}
