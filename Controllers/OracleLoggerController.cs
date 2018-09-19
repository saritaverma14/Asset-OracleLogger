using Asset_OracleLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using Newtonsoft.Json.Linq;
using Oracle.DataAccess.Client;
using System.Data.Objects;

namespace Asset_OracleLogger.Controllers
{
    public class OracleLoggerController : Controller
    {
        // GET: OracleLogger
        Entities dbLogger = new Entities();

        SV_LOGGER lModel = new SV_LOGGER();
        public ActionResult LoggerUI(string sortOrder, string currentFilter, string searchString, int? page, int? ItemCount)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TimeReceive = sortOrder == "StartTime" ? "starttime_desc" : "StartTime";
            ViewBag.ResponseTime = sortOrder == "EndTime" ? "endtime_desc" : "EndTime";
            ViewBag.Program = sortOrder == "Program" ? "Program_desc" : "Program";
            ViewBag.Project = sortOrder == "Project" ? "Project_desc" : "Project";
            ViewBag.Release = sortOrder == "Release" ? "Release_desc" : "Release";
            ViewBag.ApiOperation = sortOrder == "APIOperation" ? "APIOperation_desc" : "APIOperation";
            ViewBag.Sprint = sortOrder == "Sprint" ? "Sprint_desc" : "Sprint";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var Logger = from s in dbLogger.SV_LOGGER
                         where s.SERVICETYPE == "REST"
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        Logger = Logger.OrderBy(s => s.ID);
                        break;
                    case "Program":
                        Logger = Logger.OrderBy(s => s.PROGRAM);
                        break;
                    case "Program_desc":
                        Logger = Logger.OrderByDescending(s => s.PROGRAM);
                        break;
                    case "Project":
                        Logger = Logger.OrderBy(s => s.PROJECT);
                        break;
                    case "Project_desc":
                        Logger = Logger.OrderByDescending(s => s.PROJECT);
                        break;
                    case "Release":
                        Logger = Logger.OrderBy(s => s.RELEASE);
                        break;
                    case "Release_desc":
                        Logger = Logger.OrderByDescending(s => s.RELEASE);
                        break;
                    case "Sprint":
                        Logger = Logger.OrderByDescending(s => s.SPRINT);
                        break;
                    case "Sprint_desc":
                        Logger = Logger.OrderBy(s => s.SPRINT);
                        break;
                    case "APIOperation":
                        Logger = Logger.OrderBy(s => s.APIOPERATION);
                        break;
                    case "APIOperation_desc":
                        Logger = Logger.OrderByDescending(s => s.APIOPERATION);
                        break;
                    case "StartTime":
                        Logger = Logger.OrderBy(s => s.TIMERECEIVED);
                        break;
                    case "EndTime":
                        Logger = Logger.OrderBy(s => s.RESPONSETIME);
                        break;
                    case "starttime_desc":
                        Logger = Logger.OrderByDescending(s => s.TIMERECEIVED);
                        break;
                    case "endtime_desc":
                        Logger = Logger.OrderByDescending(s => s.RESPONSETIME);
                        break;
                    default:
                        Logger = Logger.OrderByDescending(s => s.ID);
                        break;
                }

            }
            else
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        Logger = Logger.OrderBy(s => s.ID);
                        break;
                    case "Program":
                        Logger = Logger.OrderBy(s => s.PROGRAM);
                        break;
                    case "Program_desc":
                        Logger = Logger.OrderByDescending(s => s.PROGRAM);
                        break;
                    case "Project":
                        Logger = Logger.OrderBy(s => s.PROJECT);
                        break;
                    case "Project_desc":
                        Logger = Logger.OrderByDescending(s => s.PROJECT);
                        break;
                    case "Release":
                        Logger = Logger.OrderBy(s => s.RELEASE);
                        break;
                    case "Release_desc":
                        Logger = Logger.OrderByDescending(s => s.RELEASE);
                        break;
                    case "Sprint":
                        Logger = Logger.OrderByDescending(s => s.SPRINT);
                        break;
                    case "Sprint_desc":
                        Logger = Logger.OrderBy(s => s.SPRINT);
                        break;
                    case "APIOperation":
                        Logger = Logger.OrderBy(s => s.APIOPERATION);
                        break;
                    case "APIOperation_desc":
                        Logger = Logger.OrderByDescending(s => s.APIOPERATION);
                        break;
                    case "StartTime":
                        Logger = Logger.OrderBy(s => s.TIMERECEIVED);
                        break;
                    case "EndTime":
                        Logger = Logger.OrderBy(s => s.RESPONSETIME);
                        break;
                    case "starttime_desc":
                        Logger = Logger.OrderByDescending(s => s.TIMERECEIVED);
                        break;
                    case "endtime_desc":
                        Logger = Logger.OrderByDescending(s => s.RESPONSETIME);
                        break;
                    default:
                        Logger = Logger.OrderByDescending(s => s.ID);
                        break;
                }
            }
            TimeZoneInfo TimeZone = TimeZoneInfo.Local;
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);

            //DateTime currentDate = System.DateTime.UtcNow;
            foreach (SV_LOGGER cust in dbLogger.SV_LOGGER)
            {
                SV_LOGGER existing = dbLogger.SV_LOGGER.Find(cust.ID);
                existing.RESPONSETIME = cust.RESPONSETIME;
                existing.TIMERECEIVED = cust.TIMERECEIVED;
                DateTime currentDate = DateTime.SpecifyKind(Convert.ToDateTime(existing.RESPONSETIME), DateTimeKind.Utc);

                DateTime TimeReceived = DateTime.SpecifyKind(Convert.ToDateTime(existing.TIMERECEIVED), DateTimeKind.Utc);

                lModel.TIMERECEIVED = ConvertUtcToEasternStandardReceivd(TimeReceived);
                lModel.RESPONSETIME = ConvertUtcToEasternStandard(currentDate);
                if (TimeZone.StandardName == "Eastern Standard Time")
                {
                    ViewBag.Time = "EST";
                }
                else if (TimeZone.StandardName == "India Standard Time")
                {
                    ViewBag.Time = "IST";
                }
                else
                {
                    ViewBag.Time = "UTC";
                }

            }
            int pageSize;
            if ((ItemCount == 0) || (ItemCount == null))
            {
                pageSize = 10;
            }
            else
            {
                pageSize = Convert.ToInt32(ItemCount);
            }
            int pageNumber = (page ?? 1);
            BindProgram();
            BindProject();
            BindApiOperation();
            BindRelease();
            BindSprint();
            return View(Logger.ToPagedList(pageNumber, pageSize));

        }
        private static DateTime ConvertUtcToEasternStandard(DateTime ResponseTime)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(ResponseTime, easternZone);
        }
        private static DateTime ConvertUtcToEasternStandardReceivd(DateTime TimeReceived)
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(TimeReceived, easternZone);
        }
        [HttpPost]
        public ActionResult LoggerUI(SV_LOGGER lModel, string sortOrder, string currentFilter, string searchString, int? page, int? ItemCount, string ArrayIds)
        {

            // sv_logger lModel = new sv_logger();
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TimeReceive = sortOrder == "StartTime" ? "starttime_desc" : "StartTime";
            ViewBag.ResponseTime = sortOrder == "EndTime" ? "endtime_desc" : "EndTime";
            ViewBag.Program = sortOrder == "Program" ? "Program_desc" : "Program";
            ViewBag.Project = sortOrder == "Project" ? "Project_desc" : "Project";
            ViewBag.Release = sortOrder == "Release" ? "Release_desc" : "Release";
            ViewBag.ApiOperation = sortOrder == "APIOperation" ? "APIOperation_desc" : "APIOperation";
            ViewBag.Sprint = sortOrder == "Sprint" ? "Sprint_desc" : "Sprint";
            string Program = lModel.PROGRAM;
            string Project = lModel.PROJECT;
            string Release = lModel.RELEASE;
            string ApiOperation = lModel.APIOPERATION;
            string Sprint = lModel.SPRINT;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var Logger = from s in dbLogger.SV_LOGGER
                         where s.SERVICETYPE == "REST"
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                var SPRINT = dbLogger.SV_LOGGER.Where(a => a.SPRINT == searchString).Select(a => a.SPRINT).FirstOrDefault();
                if (SPRINT != null)
                {
                    Logger = Logger.Where(s => s.SPRINT.Equals(searchString)
                       );
                }
                else
                {
                    string text1 = searchString;
                    int num1;
                    bool res = int.TryParse(text1, out num1);
                    if (res == false)
                    {
                        Logger = Logger.Where(s => s.PROGRAM.Equals(searchString)
                                       || s.PROJECT.Equals(searchString)
                                       || s.RELEASE.Equals(searchString)
                                       || s.APIOPERATION.Equals(searchString)
                                       || s.SPRINT.Equals(searchString)
                                       || s.KEYDATA.Equals(searchString)
                                );

                    }
                    else
                    {
                        Logger = (from a in dbLogger.SV_LOGGER
                                  where a.ID == num1
                                  select a).Distinct();

                        List<SV_LOGGER> EMPDetailList = new List<SV_LOGGER>();
                        if (Logger.ToList().Count > 0)
                        {
                            Logger.ToList().ForEach(rec =>
                            {
                                EMPDetailList.Add(new SV_LOGGER
                                {
                                    ID = Convert.ToInt32(rec.ID),
                                    PROGRAM = Convert.ToString(rec.PROGRAM),
                                    PROJECT = rec.PROJECT,
                                    RELEASE = rec.RELEASE,
                                    SPRINT = rec.SPRINT,
                                    APIOPERATION = rec.APIOPERATION,
                                    KEYDATA = rec.KEYDATA,
                                    REQUEST = rec.REQUEST,
                                    RESPONSE = rec.RESPONSE,
                                    TIMERECEIVED = rec.TIMERECEIVED,
                                    RESPONSETIME = rec.RESPONSETIME
                                });
                            });
                        }
                    }

                }
            }
            else if ((Program != null) && (Project != null) && (Release != null) && (ApiOperation != null) && (Sprint != null))
            {
                if ((Sprint == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                       && s.PROJECT.Equals(lModel.PROJECT)
                                       && s.RELEASE.Equals(lModel.RELEASE)
                                       && s.APIOPERATION.Equals(lModel.APIOPERATION)

                                );
                }

                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                        && s.PROJECT.Equals(lModel.PROJECT)
                                        && s.RELEASE.Equals(lModel.RELEASE)
                                         && s.APIOPERATION.Equals(lModel.APIOPERATION)
                                        && s.SPRINT.Equals(lModel.SPRINT)
                                 );
                }
            }

            else if ((Program != null) && (Project != null) && (Release != null) && (ApiOperation != null))
            {
                if ((ApiOperation == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                       && s.PROJECT.Equals(lModel.PROJECT)
                                       && s.RELEASE.Equals(lModel.RELEASE)

                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                        && s.PROJECT.Equals(lModel.PROJECT)
                                        && s.RELEASE.Equals(lModel.RELEASE)
                                        && s.APIOPERATION.Equals(lModel.APIOPERATION)
                                 );
                }
            }
            else if ((Program != null) && (Project != null) && (Release != null))
            {
                if ((Release == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                       && s.PROJECT.Equals(lModel.PROJECT)

                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                        && s.PROJECT.Equals(lModel.PROJECT)
                        && s.RELEASE.Equals(lModel.RELEASE)

                        );
                }
            }
            else if ((Program != null) && (Project != null) && (Sprint != null))
            {
                if ((Sprint == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                       && s.PROJECT.Equals(lModel.PROJECT)
                                       && s.RELEASE.Equals(lModel.RELEASE)
                                       && s.APIOPERATION.Equals(lModel.APIOPERATION)

                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                           && s.PROJECT.Equals(lModel.PROJECT)
                                           && s.SPRINT.Equals(lModel.SPRINT)
                                    );
                }

            }
            else if ((Program != null) && (Project != null))
            {
                if ((Project == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)

                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                           && s.PROJECT.Equals(lModel.PROJECT)

                                    );
                }

            }
            else if (ApiOperation != null)
            {
                Logger = Logger.Where(s => s.APIOPERATION.Equals(lModel.APIOPERATION)

                             );
            }

            else if (Program != null)
            {
                Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)

                             );
            }
            else if (Project != null)
            {
                Logger = Logger.Where(s => s.PROJECT.Equals(lModel.PROJECT)

                             );
            }
            else if ((Program != null) && (Project != null))
            {
                Logger = Logger.Where(s => s.PROGRAM.Equals(lModel.PROGRAM)
                                    && s.PROJECT.Equals(lModel.PROJECT)

                             );
            }
            else if (Sprint != null)
            {
                Logger = Logger.Where(s => s.SPRINT.Equals(lModel.SPRINT)

                             );
            }

            else if (Release != null)
            {
                Logger = Logger.Where(s => s.RELEASE.Equals(lModel.RELEASE)

                             );
            }

            switch (sortOrder)
            {
                case "name_desc":
                    Logger = Logger.OrderBy(s => s.ID);
                    break;
                case "Program":
                    Logger = Logger.OrderBy(s => s.PROGRAM);
                    break;
                case "Program_desc":
                    Logger = Logger.OrderByDescending(s => s.PROGRAM);
                    break;
                case "Project":
                    Logger = Logger.OrderBy(s => s.PROJECT);
                    break;
                case "Project_desc":
                    Logger = Logger.OrderByDescending(s => s.PROJECT);
                    break;
                case "Release":
                    Logger = Logger.OrderBy(s => s.RELEASE);
                    break;
                case "Release_desc":
                    Logger = Logger.OrderByDescending(s => s.RELEASE);
                    break;
                case "Sprint":
                    Logger = Logger.OrderByDescending(s => s.SPRINT);
                    break;
                case "Sprint_desc":
                    Logger = Logger.OrderBy(s => s.SPRINT);
                    break;
                case "APIOperation":
                    Logger = Logger.OrderBy(s => s.APIOPERATION);
                    break;
                case "APIOperation_desc":
                    Logger = Logger.OrderByDescending(s => s.APIOPERATION);
                    break;
                case "StartTime":
                    Logger = Logger.OrderBy(s => s.TIMERECEIVED);
                    break;
                case "EndTime":
                    Logger = Logger.OrderBy(s => s.RESPONSETIME);
                    break;
                case "starttime_desc":
                    Logger = Logger.OrderByDescending(s => s.TIMERECEIVED);
                    break;
                case "endtime_desc":
                    Logger = Logger.OrderByDescending(s => s.RESPONSETIME);
                    break;
                default:
                    Logger = Logger.OrderByDescending(s => s.ID);
                    break;
            }
            TimeZoneInfo TimeZone = TimeZoneInfo.Local;
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);

            //DateTime currentDate = System.DateTime.UtcNow;
            foreach (SV_LOGGER cust in dbLogger.SV_LOGGER)
            {
                SV_LOGGER existing = dbLogger.SV_LOGGER.Find(cust.ID);
                existing.RESPONSETIME = cust.RESPONSETIME;
                existing.TIMERECEIVED = cust.TIMERECEIVED;
                DateTime currentDate = DateTime.SpecifyKind(Convert.ToDateTime(existing.RESPONSETIME), DateTimeKind.Utc);
                DateTime TimeReceived = DateTime.SpecifyKind(Convert.ToDateTime(existing.TIMERECEIVED), DateTimeKind.Utc);
                lModel.TIMERECEIVED = ConvertUtcToEasternStandardReceivd(TimeReceived);
                lModel.RESPONSETIME = ConvertUtcToEasternStandard(currentDate);
                if (TimeZone.StandardName == "Eastern Standard Time")
                {
                    ViewBag.Time = "EST";
                }
                else if (TimeZone.StandardName == "India Standard Time")
                {
                    ViewBag.Time = "IST";
                }
                else
                {
                    ViewBag.Time = "UTC";
                }

            }
            int pageSize;
            if ((ItemCount == 0) || (ItemCount == null))
            {
                pageSize = 10;
            }
            else
            {
                pageSize = Convert.ToInt32(ItemCount);
            }

            int pageNumber = (page ?? 1);
            BindProgram();
            BindProject();
            BindApiOperation();
            BindRelease();
            BindSprint();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ViewDetail", Logger.ToPagedList(pageNumber, pageSize));
            }
            return View(Logger.ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult _CopyLoggerDetails(string ArrayIds)
        {
            if (ArrayIds != null)
            {
               
                TempData["CheckedRecords"] = ArrayIds;
                BindRelease();
                return PartialView("_CopyLoggerDetails");
            }
            return PartialView("_CopyLoggerDetails");
        }
        [HttpPost]
        public ActionResult _CopyLoggerDetails(SV_LOGGER sModel, string Login, string cancel)
        {
            BindRelease();
            if (!string.IsNullOrEmpty(Login))
            {
                if (sModel.RELEASE != null)
                {
                    string ArrayIds = TempData["CheckedRecords"].ToString();
                    decimal[] nums = ArrayIds.TrimStart(',').Split(',').Select(decimal.Parse).ToArray();
                    SV_SERVICEDETAILS objdetailModel = new SV_SERVICEDETAILS();

                    var logId = (from t in dbLogger.SV_LOGGER
                                 select t).ToList().Where(t => nums.Contains(t.ID));
                    if (logId != null)
                    {
                        foreach (SV_LOGGER logger in logId)
                        {
                            dbLogger.ADD_SERVICEDETAILS(logger.PROGRAM, logger.PROJECT, sModel.RELEASE, logger.APIOPERATION, logger.SPRINT, objdetailModel.ARGUMENT1, objdetailModel.ARGUMENTVALUE1, objdetailModel.ARGUMENT2, objdetailModel.ARGUMENTVALUE2, objdetailModel.ARGUMENT3, objdetailModel.ARGUMENTVALUE3, objdetailModel.ARGUMENT4, objdetailModel.ARGUMENTVALUE4, objdetailModel.ARGUMENT5, objdetailModel.ARGUMENTVALUE5, objdetailModel.REQUESTHEADER1, objdetailModel.REQUESTHEADERVALUE1, objdetailModel.REQUESTHEADER2, objdetailModel.REQUESTHEADERVALUE2, objdetailModel.REQUESTHEADER3, objdetailModel.REQUESTHEADERVALUE3, objdetailModel.RESPONSEHEADER1, objdetailModel.RESPONSEHEADERVALUE1, objdetailModel.RESPONSEHEADER2, objdetailModel.RESPONSEHEADERVALUE2, objdetailModel.LIVESERVER, objdetailModel.LIVEPORT, logger.METHOD, logger.URI, objdetailModel.LASTMODIFIEDBY, objdetailModel.USERID, objdetailModel.NUMOFMODIFICATIONS, logger.RESPONSE, logger.REQUEST, "REST");
                            dbLogger.SaveChanges();
                            ModelState.Clear();
                        }
                        Response.Write("<script> window.close();window.opener.location.reload();</script>");
                        TempData["msg"] = "<script>alert('Records Has Been Copied In SV-TDM Sucessfullly..!');</script>";
                    }
                    else
                    {
                        TempData["msg"] = "Id is not available in TDM UI..!";
                        return PartialView("_CopyLoggerDetails");
                    }
                }
                else
                {
                    TempData["ErrorResponse"] = "Please Enter Release...";
                    return PartialView("_CopyLoggerDetails");
                }
            }
            else if (!string.IsNullOrEmpty(cancel))
            {
                ViewBag.ShouldClose = true;
                // Response.Redirect(HttpContext.RewritePath.Request.Url.ToString(), true);
                Response.Write("<script>window.opener.location.reload();</script>");

            }
            return View();
            
        }
        private void BindApiOperation()
        {
            using (Entities cshparpEntity = new Entities())
            {

                var fromDatabaseEF = new SelectList(cshparpEntity.SV_LOGGER.Select(m => m.APIOPERATION).Distinct().ToList(), "APIOPERATION");
                ViewData["DBMyApi"] = fromDatabaseEF;
            }

        }

        private void BindRelease()
        {
            using (Entities cshparpEntity = new Entities())
            {
                var fromDatabaseEF = new SelectList(cshparpEntity.SV_LOGGER.Select(m => m.RELEASE).Distinct().ToList(), "RELEASE");
                ViewData["DBMyRelease"] = fromDatabaseEF;
            }

        }

        private void BindSprint()
        {
            using (Entities cshparpEntity = new Entities())
            {
                var result = new SelectList(cshparpEntity.SV_LOGGER.Select(m => m.SPRINT).Distinct().ToList(), "SPRINT");
                ViewData["DBMySprint"] = result;
            }

        }

        private void BindProject()
        {
            using (Entities cshparpEntity = new Entities())
            {
                var fromDatabaseEF = new SelectList(cshparpEntity.SV_LOGGER.Select(m => m.PROJECT).Distinct().ToList(), "PROJECT");
                ViewData["DBMyProject"] = fromDatabaseEF;
            }

        }

        private void BindProgram()
        {
            using (Entities cshparpEntity = new Entities())
            {

                var result = new SelectList(cshparpEntity.SV_LOGGER.Select(m => m.PROGRAM).Distinct().ToList(), "PROGRAM");
                ViewData["DBMyProgram"] = result;
            }
        }
        [HttpGet]
        public JsonResult Project(string Program)
        {
            using (Entities cshparpEntity = new Entities())
            {

                var ProjectList = new SelectList(cshparpEntity.SV_LOGGER.Where(c => c.PROGRAM == Program).Select(m => m.PROJECT).Distinct().ToList(), "PROJECT");
                ViewData["DBMyProject"] = ProjectList;
            }
            return Json(ViewData["DBMyProject"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Release(string Project)
        {

            using (Entities cshparpEntity = new Entities())
            {
                var ReleaseList = new SelectList(cshparpEntity.SV_LOGGER.Where(c => c.PROJECT == Project).Select(m => m.RELEASE).Distinct().ToList(), "RELEASE");

                ViewData["DBMyRelease"] = ReleaseList;

            }
            return Json(ViewData["DBMyRelease"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ApiOperation(string Release)
        {

            using (Entities cshparpEntity = new Entities())
            {
                var ApiList = new SelectList(cshparpEntity.SV_LOGGER.Where(c => c.RELEASE == Release).Select(m => m.APIOPERATION).Distinct().ToList(), "APIOPERATION");


                ViewData["DBMyApi"] = ApiList;

            }
            return Json(ViewData["DBMyApi"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Sprint(string ApiOperation)
        {

            using (Entities cshparpEntity = new Entities())
            {
                var SprintList = new SelectList(cshparpEntity.SV_LOGGER.Where(c => c.APIOPERATION == ApiOperation).Select(m => m.SPRINT).Distinct().ToList(), "SPRINT");


                ViewData["DBMySprint"] = SprintList;

            }


            return Json(ViewData["DBMySprint"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetData(string Sprint)
        {
            using (Entities cshparpEntity = new Entities())
            {
                var SprintList = new SelectList(cshparpEntity.SV_LOGGER.Select(m => m.SPRINT).Distinct().ToList(), "SPRINT");


                ViewData["DBMySprint"] = SprintList;

            }
            return Json(ViewData["DBMySprint"], JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetList()
        //{
        //    using (Entities context = new Entities())
        //    {
        //        ObjectResult<SV_LOGGER> emps = context.LOGGER_GETLIST();
        //        List<SV_LOGGER> emplist = new List<SV_LOGGER>();
        //        foreach (var emp in emps)
        //        {
        //            emplist.Add(emp);
        //        }
        //        return View(emplist);
        //    }
        //}

    }
}