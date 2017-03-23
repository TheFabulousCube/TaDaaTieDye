using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySite.Helpers;
using MySite.Models;

namespace MySite.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /About/

        public ActionResult Tour()
        {
            return View();
        }
        //
        // GET: /About/

        public ActionResult OBD_Graph()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/Downloads"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("test*.dat");
            List<string> items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return View(items); 
        }
        //
        // GET: /About/

        public ActionResult Resume()
        {
            return View();
        }
        //
        // GET: /About/

        public ActionResult MinecraftMods()
        {
            return View();
        } 
        
        //
        // GET: /About/

        public ActionResult AndroidApps()
        {
            return View();
        }

        //
        // GET: /About/

        public ActionResult Simon()
        {
            return View();
        }

        public FileResult Download(string file)
        {   // use Helper: StripSpecialCharacters()
            string virtualFilePath = "~/Downloads/";
            file = Clean.FileName(file);
            virtualFilePath += file;

            // 10/8/2016 Added this section to track how many times a file is downloaded
            TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();


                // first time this file has been downloaded
                Download tempDownload = new Download();
                tempDownload.FileNameID = file;
                tempDownload.Downloads = 1;
                tempDownload.Date = DateTime.Now.ToString();
                db.Downloads.Add(tempDownload);

           
            // data ready, save to db
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
            string mimeType = MimeMapping.GetMimeMapping(file);
            return File(virtualFilePath, mimeType, file);
        }

    }
}
