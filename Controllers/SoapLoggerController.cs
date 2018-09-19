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
    public class SoapLoggerController : Controller
    {
        // GET: SoapLogger
        Entities dbLogger = new Entities();

        SV_LOGGER lModel = new SV_LOGGER();
        [HttpGet]
        public JsonResult Project(string Program)
        {
            var ProjectList = (from i in dbLogger.SV_LOGGER
                               //  where i.PROGRAM==Program && i.SERVICETYPE=="SOAP"
                               where i.PROGRAM == Program
                               select new { Text = i.PROJECT }).Distinct().ToList();
            ViewData["dbSoapMyProject"] = ProjectList;
            return Json(ViewData["dbSoapMyProject"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Release(string Project, SV_SERVICEDETAILS sModel)
        {
            var ReleaseList = (from i in dbLogger.SV_LOGGER
                               where i.PROJECT == Project
                               select new { Text = i.RELEASE }).Distinct().ToList();
            ViewData["dbSoapMyRelease"] = ReleaseList;
            return Json(ViewData["dbSoapMyRelease"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Operation(string Release)
        {
            var ApiList = (from i in dbLogger.SV_LOGGER
                           where i.RELEASE == Release
                           select new { Text = i.OPERATION }).Distinct().ToList();
            ViewData["dbSoapMyApi"] = ApiList;
            return Json(ViewData["dbSoapMyApi"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Sprint(string Operation)
        {
            var SprintList = (from i in dbLogger.SV_LOGGER
                              where i.OPERATION == Operation
                              select new { Text = i.SPRINT }).Distinct().ToList();
            ViewData["dbSoapMySprint"] = SprintList;
            return Json(ViewData["dbSoapMySprint"], JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetData(string Sprint)
        {
            var SprintList = (from i in dbLogger.SV_LOGGER
                              // where i.APIOperation == ApiOperation
                              select new { Text = i.SPRINT }).Distinct().ToList();
            ViewData["dbSoapMySprint"] = SprintList;
            return Json(ViewData["dbSoapMySprint"], JsonRequestBehavior.AllowGet);
        }
        public ActionResult SoapIndex(string Program, string sortOrder, string currentFilter, string searchString, int? page, int? ItemCount)
        {
            try
            {

                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                ViewBag.Program = sortOrder == "Program" ? "Program_desc" : "Program";
                ViewBag.Project = sortOrder == "Project" ? "Project_desc" : "Project";
                ViewBag.Release = sortOrder == "Release" ? "Release_desc" : "Release";
                ViewBag.Operation = sortOrder == "Operation" ? "Operation_desc" : "Operation";
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
                             where s.SERVICETYPE == "SOAP"
                             select s;


                if (Program != null)
                {
                    Logger = (from s in dbLogger.SV_LOGGER
                              select s);
                }
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
                        case "Operation":
                            Logger = Logger.OrderBy(s => s.OPERATION);
                            break;
                        case "Operation_desc":
                            Logger = Logger.OrderByDescending(s => s.OPERATION);
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
                            Logger = Logger.OrderByDescending(s => s.PROJECT);
                            break;
                        case "Sprint":
                            Logger = Logger.OrderByDescending(s => s.SPRINT);
                            break;
                        case "Sprint_desc":
                            Logger = Logger.OrderBy(s => s.SPRINT);
                            break;
                        case "Operation":
                            Logger = Logger.OrderBy(s => s.OPERATION);
                            break;
                        case "Operation_desc":
                            Logger = Logger.OrderByDescending(s => s.OPERATION);
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
                int pageSize = 10;
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
                BindOperation();
                BindRelease();
                BindSprint();

                return View(Logger.ToPagedList(pageNumber, pageSize));

            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult SoapIndex(SV_LOGGER sModel, string sortOrder, string currentFilter, string searchString, int? page, int? ItemCount, string ArrayIds, string name)
        {
            string selectCount = Request.Form["name"];
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            string Program = sModel.PROGRAM;
            string Project = sModel.PROJECT;
            string Release = sModel.RELEASE;
            string Operation = sModel.OPERATION;
            string Sprint = sModel.SPRINT;
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
                         where s.SERVICETYPE == "SOAP"
                         select s;
            sModel.sDetails = Logger.ToList();
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
                                       || s.OPERATION.Equals(searchString)
                                       || s.SPRINT.Equals(searchString)
                                );
                    }
                    else
                    {
                        Logger = (from a in dbLogger.SV_LOGGER
                                  where a.ID == num1
                                  select a).Distinct();
                        if (Logger.ToList().Count > 0)
                        {
                            List<SV_SERVICEDETAILS> DetailList = new List<SV_SERVICEDETAILS>();
                            Logger.ToList().ForEach(rec =>
                            {
                                DetailList.Add(new SV_SERVICEDETAILS
                                {
                                    ID = Convert.ToInt32(rec.ID),
                                    PROGRAM = Convert.ToString(rec.PROGRAM),
                                    PROJECT = rec.PROJECT,
                                    RELEASE = rec.RELEASE,
                                    OPERATION = rec.OPERATION,
                                    SPRINT = rec.SPRINT,
                                    REQUEST = rec.REQUEST,
                                    RESPONSE = rec.RESPONSE,
                                    LASTMODIFIEDDATE = rec.TIMERECEIVED
                                });
                            });
                        }
                    }
                }
            }
            else if ((Program != null) && (Project != null) && (Release != null) && (Operation != null) && (Sprint != null))
            {
                if ((Sprint == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                       && s.PROJECT.Equals(sModel.PROJECT)
                                       && s.RELEASE.Equals(sModel.RELEASE)
                                       && s.OPERATION.Equals(sModel.OPERATION)
                                );
                }

                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                        && s.PROJECT.Equals(sModel.PROJECT)
                                        && s.RELEASE.Equals(sModel.RELEASE)
                                         && s.OPERATION.Equals(sModel.OPERATION)
                                        && s.SPRINT.Equals(sModel.SPRINT)
                                 );
                }
            }

            else if ((Program != null) && (Project != null) && (Release != null) && (Operation != null))
            {
                if ((Operation == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                       && s.PROJECT.Equals(sModel.PROJECT)
                                       && s.RELEASE.Equals(sModel.RELEASE)

                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                        && s.PROJECT.Equals(sModel.PROJECT)
                                        && s.RELEASE.Equals(sModel.RELEASE)
                                        && s.OPERATION.Equals(sModel.OPERATION)
                                 );
                }
            }
            else if ((Program != null) && (Project != null) && (Release != null))
            {
                if ((Release == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                       && s.PROJECT.Equals(sModel.PROJECT)

                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                        && s.PROJECT.Equals(sModel.PROJECT)
                        && s.RELEASE.Equals(sModel.RELEASE)

                        );
                }
            }
            else if ((Program != null) && (Project != null) && (Sprint != null))
            {
                if ((Sprint == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                       && s.PROJECT.Equals(sModel.PROJECT)
                                       && s.RELEASE.Equals(sModel.RELEASE)
                                       && s.OPERATION.Equals(sModel.APIOPERATION)
                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                           && s.PROJECT.Equals(sModel.PROJECT)
                                           && s.SPRINT.Equals(sModel.SPRINT)
                                    );
                }
            }
            else if ((Program != null) && (Project != null))
            {
                if ((Project == "0"))
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                );
                }
                else
                {
                    Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                           && s.PROJECT.Equals(sModel.PROJECT)

                                    );
                }
            }
            else if (Operation != null)
            {
                Logger = Logger.Where(s => s.OPERATION.Equals(sModel.OPERATION)

                             );
            }

            else if (Program != null)
            {
                Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)

                             );
            }
            else if (Project != null)
            {
                Logger = Logger.Where(s => s.PROJECT.Equals(sModel.PROJECT)

                             );
            }
            else if ((Program != null) && (Project != null))
            {
                Logger = Logger.Where(s => s.PROGRAM.Equals(sModel.PROGRAM)
                                    && s.PROJECT.Equals(sModel.PROJECT)

                             );
            }
            else if (Sprint != null)
            {
                Logger = Logger.Where(s => s.SPRINT.Equals(sModel.SPRINT)

                             );
            }

            else if (Release != null)
            {
                Logger = Logger.Where(s => s.RELEASE.Equals(sModel.RELEASE)

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
                    Logger = Logger.OrderByDescending(s => s.PROJECT);
                    break;
                case "Sprint":
                    Logger = Logger.OrderByDescending(s => s.SPRINT);
                    break;
                case "Sprint_desc":
                    Logger = Logger.OrderBy(s => s.SPRINT);
                    break;
                case "Operation":
                    Logger = Logger.OrderBy(s => s.OPERATION);
                    break;
                case "Operation_desc":
                    Logger = Logger.OrderByDescending(s => s.OPERATION);
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
            BindOperation();
            BindRelease();
            BindSprint();

            if (Request.IsAjaxRequest())
            {

                return PartialView("_ViewSoapDetail", Logger.ToPagedList(pageNumber, pageSize));
            }
            return View(Logger.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult _CopyDetailSoap(string ArrayIds)
        {
            if (ArrayIds != null)
            {
                TempData["CheckedRecords"] = ArrayIds;
                BindRelease();
                return PartialView("_CopyDetailSoap");
            }
            return PartialView("_CopyDetailSoap");
        }
        [HttpPost]
        public ActionResult _CopyDetailSoap(SV_LOGGER sModel, string Login, string cancel)
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

                            dbLogger.ADD_SOAPDETAILS(logger.PROGRAM, logger.PROJECT, sModel.RELEASE, logger.OPERATION, logger.SPRINT, objdetailModel.ARGUMENT1, objdetailModel.ARGUMENTVALUE1, objdetailModel.ARGUMENT2, objdetailModel.ARGUMENTVALUE2, objdetailModel.ARGUMENT3, objdetailModel.ARGUMENTVALUE3, objdetailModel.ARGUMENT4, objdetailModel.ARGUMENTVALUE4, objdetailModel.ARGUMENT5, objdetailModel.ARGUMENTVALUE5, null, null, 0, logger.RESPONSE, logger.REQUEST, "SOAP");
                            dbLogger.SaveChanges();
                            ModelState.Clear();
                        }
                        Response.Write("<script> window.close();window.opener.location.reload();</script>");
                        TempData["msg"] = "<script>alert('Records Has Been Copied In SV-TDM Sucessfullly..!');</script>";
                    }
                    else
                    {
                        TempData["msg"] = "Id is not available in TDM UI..!";
                        return PartialView("_CopyDetailSoap");
                    }
                }
                else
                {
                    TempData["ErrorResponse"] = "Please Enter Release...";
                    return PartialView("_CopyDetailSoap");
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
        private void BindOperation()
        {
            var fromDatabaseEF = new SelectList(dbLogger.SV_LOGGER.Select(m => m.OPERATION).Distinct().ToList(), "OPERATION");
            //var fromDatabaseEF = new SelectList((from x in dbSoap.SV_SOAPSERVICE
            //                                     where x.SERVICETYPE == "REST"
            //                                     select x.APIOPERATION).Distinct().ToList(), "APIOPERATION");
            ViewData["dbSoapMyApi"] = fromDatabaseEF;

        }

        private void BindRelease()
        {

            var fromDatabaseEF = new SelectList(dbLogger.SV_LOGGER.Select(m => m.RELEASE).Distinct().ToList(), "RELEASE");
            ViewData["dbSoapMyRelease"] = fromDatabaseEF;

        }

        private void BindSprint()
        {

            var result = new SelectList(dbLogger.SV_LOGGER.Select(m => m.SPRINT).Distinct().ToList(), "SPRINT");
            ViewData["dbSoapMySprint"] = result;

        }

        private void BindProject()
        {

            var fromDatabaseEF = new SelectList(dbLogger.SV_LOGGER.Select(m => m.PROJECT).Distinct().ToList(), "PROJECT");
            ViewData["dbSoapMyProject"] = fromDatabaseEF;

        }

        private void BindProgram()
        {

            var result = new SelectList(dbLogger.SV_LOGGER.Select(m => m.PROGRAM).Distinct().ToList(), "PROGRAM");
            ViewData["dbSoapMyProgram"] = result;

        }
    }
}