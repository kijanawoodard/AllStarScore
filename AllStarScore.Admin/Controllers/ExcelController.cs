using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllStarScore.Admin.Controllers
{
    public class ExcelController : Controller
    {
		[HttpPost, ValidateInput(false)]
        public ActionResult Index(DownloadAsExcelCommand command)
        {
	        Response.Buffer = true;
	        Response.AddHeader("content-disposition", "attachment; filename=" + command.FileName);
	        Response.ContentType = "application/vnd.ms-excel";

			return Content(command.Data);
        }

    }

	public class DownloadAsExcelCommand
	{
		public string FileName { get; set; }
		public string Data { get; set; }
	}
}
