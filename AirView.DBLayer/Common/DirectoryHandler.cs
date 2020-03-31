using System;
using System.Collections.Generic;
using System.IO;


namespace SWI.Libraries.Common
{
    
    /*----MoB!----*/
    public class DirectoryHandler
    {
        public bool CreateDirectory(string path)
        {
            try
            {
                if (!(Directory.Exists(path)))
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }


        public bool DeletFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception) { return false; }


        }


        public bool FileExist(string path)
        {
            try
            {
               return File.Exists( path);
            }
            catch (Exception) { return false; }
        }

        public List<string> Folders(string Path) {
            //var rec = dh.Folders(Server.MapPath("~/Content/AirViewLogs/")); 
            List<string> data = new List<string>();
            foreach (var d in Directory.GetDirectories(Path))
            {
                var dir = new DirectoryInfo(d);
                var dirName = dir.Name;
                data.Add(dirName);
            }
            return data;
        }


        public List<string> Files(string DirectoryPath)
        {
            //var rec1 = dh.Files(Server.MapPath("~/Content/AirViewLogs/")); 
            List<string> data = new List<string>();
            string[] files = Directory.GetFiles(DirectoryPath);
            foreach (string file in files) { 
                data.Add(Path.GetFileName(file));
            }
           
            return data;
        }
    }
}