using System.IO;
using System.Web;
using System.Web.Mvc;
using SWI.Libraries.Common;
using SWI.AirView.Common.ResizeUploadImg;

namespace SWI.Security.Controllers
{
    /*----MoB!----*/
    public class CommonController : Controller
    {
        // GET: Common
        public string UploadImg(string UploadPath, string file_name, int height, int width)
        {

            DirectoryHandler dir = new DirectoryHandler();
            dir.CreateDirectory(Server.MapPath("~" + UploadPath));

            foreach (string item in Request.Files)
            {
                int counter = 1;
                string tempUploadPath = "~" + UploadPath;
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;

                if (file.ContentLength > 0)
                {

                    ImageUpload imageUpload = new ImageUpload { Width = width, Height = height };
                    ImageResult imageResult = new ImageResult();
                    string fileName = Path.GetFileName(file.FileName);
                    if (file_name == null)
                    {

                        string prepend = "Sec_";
                        string finalFileName = prepend + ((counter).ToString()) + "_" + fileName;
                        imageResult = imageUpload.UploadFile(file, finalFileName, tempUploadPath);
                    }

                    if (!string.IsNullOrEmpty(file_name))
                    {
                        var Extension = Path.GetExtension(file.FileName);
                        imageResult = imageUpload.UploadFile(file, file_name + Extension, tempUploadPath);


                    }



                    if (imageResult.Success)
                    {
                        return UploadPath + "/" + imageResult.ImageName;
                    }
                    else
                    {
                        return imageResult.ErrorMessage;
                    }
                }
            }

            return null;
        }
    }
}