using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Appraisal.BusinessLogicLayer;
using AppraisalSln.Models;
using RepositoryPattern;
using Appraisal.BusinessLogicLayer.Admin;
using Appraisal.BusinessLogicLayer.Core;
using System.Web;
using Newtonsoft.Json;
using AppraisalSystem.Models;
using System.IO;
using Appraisal.BusinessLogicLayer.Employee;
using Microsoft.AspNet.Identity;

namespace AppraisalSystem.Areas.Employees.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employees/JobObjectives")]
    public class JobObjectivesController : ApiController
    {
        private string serverPath = System.Configuration.ConfigurationManager.AppSettings.Get("ServerPath");
        [HttpPost]
        [Route("SaveObjective")]
        public IHttpActionResult SaveObjective([FromBody]ObjectiveSub sub)
        {
            try
            {
                if (sub == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                Validation validation = new Validation(new UnitOfWork());
                JobObjective activities = new JobObjective(new UnitOfWork());
                activities.CreatedBy = User.Identity.GetUserName();

                if (!validation.IsJobDescriptionConfirmed(activities.CreatedBy))
                {
                    return BadRequest("Your job description is not confirmed by your supervisor!");
                }

                activities.SaveObjective(sub);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("SavePerformanceAppraisal")]
        public IHttpActionResult SavePerformanceAppraisal([FromBody]List<PerformanceAppraisalPoco> list)
        {
            try
            {
                if (list == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                Validation validation = new Validation(new UnitOfWork());

                var weight = list.Sum(a => a.Weight);
                JobObjective activities = new JobObjective(new UnitOfWork());
                activities.CreatedBy = User.Identity.GetUserName();
                if (!validation.IsJobObjectiveDeadLineValid(activities.CreatedBy))
                {
                    return BadRequest("You have missed your deadline");
                }
                activities.SavePerformanceAppraisal(list);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("SaveEvidenceFile")]
        public IHttpActionResult SaveEvidenceFile()
        {
            string path = HttpContext.Current.Server.MapPath("~/EvidenceFiles/");

            var objective = HttpContext.Current.Request.Form["ObjectiveModel"];

            var model = JsonConvert.DeserializeObject<ObjectiveModal>(objective);

            var files = HttpContext.Current.Request.Files;

            if (files != null)
            {
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileName = UploadFile(file, path);

                        //Here you put the business logic for update EvidenceFile
                        var objectiveId = model.objectiveId; //Get Objective Modal
                        EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
                        employees.CreatedBy = User.Identity.GetUserName();
                        if (employees.CreatedBy == null) return BadRequest("Authentication error");
                        employees.UploadFileEvidence(new FileUploadPoco { ObjectiveId = model.objectiveId, FilePathe = fileName });

                    }
                }

            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }


        private string UploadFile(HttpPostedFile file, string mapPath)
        {

            string fileName = new FileInfo(file.FileName).Name;

            if (file.ContentLength > 0)
            {
                Guid id = Guid.NewGuid();

                var filePath = Path.GetFileName(id.ToString() + "_" + fileName);

                if (!File.Exists(mapPath + filePath))
                {

                    file.SaveAs(mapPath + filePath);
                    return serverPath + "/EvidenceFiles/" + filePath;
                }
                return null;
            }
            return null;

        }

    }
}
