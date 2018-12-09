using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;


namespace Module_File_Manager.Content_Searcher
{
    /// <summary>
    /// The sub module pdf_Searcher from sub module of file manager module for search on content of files
    /// </summary>
    class _m_File_Manager_Content_Searcher_pdf_Searcher
    {
        /// <summary>
        /// PDFs to text.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>return extracted all string from pdf file</returns>
        public static string PdfToText(string path)
        {
            PdfReader reader = new PdfReader(path);

            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {

                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;

            ////text = "";
            //string line;
            //string []words = text.Split('\n');
            //for (int j = 0, len = words.Length; j < len; j++)
            //{
            //    string s= Encoding.UTF7.GetString(f);
            //    byte[] ff = Encoding.UTF7.GetBytes(words[j]);
            //    line = Encoding.UTF7.GetString(ff);

            //}


        }
    }
}
