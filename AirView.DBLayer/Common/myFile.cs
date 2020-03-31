using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWI.Libraries.Common
{
  public  class myFile
    {

        public string StreamToString(Stream InputStream) {
            try
            {
                using (var streamReader = new StreamReader(InputStream, Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }
            catch 
            {

                throw;
            }
            
        }

        public bool Write(string Path,string FileName,string Extension, string Text) {
            try
            {
                using (var tw = new StreamWriter(Path + FileName + "." + Extension, true))
                {
                    tw.WriteLine(Text);
                    tw.Close();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            

        }

        public string FileToString(string Path) {

            var fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
              return streamReader.ReadToEnd();
            }
        }

    }

   public class CSVParser
    {
        public string siteCode;

        public static CSVParser FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            CSVParser dailyValues = new CSVParser();
            dailyValues.siteCode = values[4].ToString();
            
            return dailyValues;
        }
    }
}
