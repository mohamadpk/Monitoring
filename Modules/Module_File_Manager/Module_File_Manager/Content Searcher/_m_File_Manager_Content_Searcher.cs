using System;
using System.Collections.Generic;


namespace Module_File_Manager.Content_Searcher
{
    /// <summary>
    /// The main from sub module of file manager module for search on content of file
    /// </summary>
    public class _m_File_Manager_Content_Searcher
    {
        public enum Enum_direction { RtoL,LtoR};
        /// <summary>
        /// Contents the searcher.
        /// </summary>
        /// <param name="ListToSearch">The list to search.</param>
        /// <param name="KeyToSearch">The key to search.</param>
        /// <param name="direction">The direction set to RtoL for persian.</param>
        /// <returns>list of math files</returns>
        public static List<string> Content_Searcher(List<string> ListToSearch,string KeyToSearch,Enum_direction direction)
        {
            string ConvertedKey = KeyToSearch;
            string content = "";
            List<string> Finded = new List<string>();
            foreach(string Target_File in ListToSearch)
            {
                if(Target_File.Substring(Target_File.Length-4)==".pdf")
                {
                    if (direction==Enum_direction.RtoL)
                    {
                        ConvertedKey = Reverse(KeyToSearch);
                    }
                    content = Module_File_Manager.Content_Searcher._m_File_Manager_Content_Searcher_pdf_Searcher.PdfToText(Target_File);

                }else if(Target_File.Substring(Target_File.Length - 5).Contains(".doc"))//or docx
                {
                    content = Module_File_Manager.Content_Searcher._m_File_Manager_Content_Searcher_word_Searcher.WordToText(Target_File);
                }
                else if (Target_File.Substring(Target_File.Length - 5).Contains(".xlsx"))
                {
                    content = Module_File_Manager.Content_Searcher._m_File_Manager_Content_Searcher_excel_Searcher.ExcelToText(Target_File);
                }
                else
                {
                    content = Module_File_Manager.Content_Searcher._m_File_Manager_Content_Searcher_textual_Searcher.TextualToText(Target_File);
                }
                if (content.Contains(ConvertedKey))
                    Finded.Add(Target_File);
            }
            return Finded;
        }
        /// <summary>
        /// Reverses the specified inp.
        /// </summary>
        /// <param name="inp">The s.</param>
        /// <returns>return reversed string fromn input</returns>
        private static string Reverse(string inp)
        {
            char[] charArray = inp.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
