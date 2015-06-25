using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEATS.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult Index()
        {
            return View();
        }

        // GET: FileUpload
        public ActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file_Uploader)
        {
            if (file_Uploader != null)
            {
                string fileName = string.Empty;
                string destinationPath = string.Empty;

                List<FileUploadModel> uploadFileModel = new List<FileUploadModel>();

                fileName = Path.GetFileName(file_Uploader.FileName);
                destinationPath = Path.Combine(Server.MapPath("~/MyFiles/"), fileName);
                file_Uploader.SaveAs(destinationPath);
                if (Session["fileUploader"] != null)
                {
                    var isFileNameRepete = ((List<FileUploadModel>)Session["fileUploader"]).Find(x => x.FileName == fileName);
                    if (isFileNameRepete == null)
                    {
                        uploadFileModel.Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                        ((List<FileUploadModel>)Session["fileUploader"]).Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                        ViewBag.Message = "File Uploaded Successfully";
                    }
                    else
                    {
                        ViewBag.Message = "File is already exists";
                    }
                }
                else
                {
                    uploadFileModel.Add(new FileUploadModel { FileName = fileName, FilePath = destinationPath });
                    Session["fileUploader"] = uploadFileModel;
                    ViewBag.Message = "File Uploaded Successfully";
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult RemoveUploadFile(string fileName)
        {
            int sessionFileCount = 0;

            try
            {
                if (Session["fileUploader"] != null)
                {
                    ((List<FileUploadModel>)Session["fileUploader"]).RemoveAll(x => x.FileName == fileName);
                    sessionFileCount = ((List<FileUploadModel>)Session["fileUploader"]).Count;
                    if (fileName != null || fileName != string.Empty)
                    {
                        FileInfo file = new FileInfo(Server.MapPath("~/MyFiles/" + fileName));
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(sessionFileCount, JsonRequestBehavior.AllowGet);
        }


        public FileResult OpenFile(string fileName)
        {
            try
            {
                return File(new FileStream(Server.MapPath("~/MyFiles/" + fileName), FileMode.Open), "application/octetstream", fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
    }
}