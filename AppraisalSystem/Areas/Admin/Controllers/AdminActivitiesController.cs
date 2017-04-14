using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Admin;
using Appraisal.BusinessLogicLayer.HBOU;
using AppraisalSln.Models;
using Microsoft.AspNet.Identity;
using RepositoryPattern;


namespace AppraisalSystem.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/AdminActivities")]
    public class AdminActivitiesController : ApiController
    {
        [HttpPost]
        [Route("SaveFiscalYear")]
        public IHttpActionResult SaveFiscalYear([FromBody]FiscalYear fiscalYear)
        {
            try
            {
                if (fiscalYear == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                AdminActivities activities = new AdminActivities(new UnitOfWork());
                activities.CreatedBy = User.Identity.GetUserName();
                activities.SaveFiscalYear(fiscalYear);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("MakeABudget")]
        public IHttpActionResult MakeABudget([FromBody]DirectorActivities directorActivities)
        {
            try
            {
                if (directorActivities == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                AdminActivities activitie = new AdminActivities(new UnitOfWork());
                activitie.CreatedBy = User.Identity.GetUserName();
                activitie.MakeABudget(directorActivities);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("SetObjectDeadline")]
        public IHttpActionResult SetObjectDeadline([FromBody]DepartmentConfig config)
        {
            try
            {
                if (config == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                AdminActivities activity = new AdminActivities(new UnitOfWork());
                activity.CreatedBy = User.Identity.GetUserName();
                activity.SetObjectDeadline(config);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("SetAppraisalDeadline")]
        public IHttpActionResult SetAppraisalDeadline([FromBody]DepartmentConfig config)
        {
            try
            {
                if (config == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                AdminActivities activity = new AdminActivities(new UnitOfWork());
                activity.CreatedBy = User.Identity.GetUserName();
                activity.SetAppraisalDeadline(config);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
        [HttpPost]
        [Route("UpdateAppraisalDeadline")]
        public IHttpActionResult UpdateAppraisalDeadline([FromBody]DepartmentConfig config)
        {
            try
            {
                if (config == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                AdminActivities activity = new AdminActivities(new UnitOfWork());
                activity.CreatedBy = User.Identity.GetUserName();
                activity.UpdateAppraisalDeadline(config);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
        [HttpGet]
        [Route("ApproveJobDescriptionByHOBU/{id}")]
        public IHttpActionResult ApproveJobDescriptionByHOBU(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                HboActivities activity = new HboActivities();
                activity.CreatedBy = User.Identity.GetUserName();
                activity.ApproveJobDescriptionByHOBU(id);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("ApproveJobDescriptionByReportee/{id}")]
        public IHttpActionResult ApproveJobDescriptionByReportee(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                HboActivities activity = new HboActivities();
                activity.CreatedBy = User.Identity.GetUserName();
                activity.ApproveJobDescriptionByReportee(id);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
        [HttpGet]
        [Route("ApproveObjectiveByReportee/{id}")]
        public IHttpActionResult ApproveObjectiveByReportee(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                HboActivities activity = new HboActivities();
                activity.CreatedBy = User.Identity.GetUserName();
                activity.ApproveObjectiveByReportee(id);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }


        [HttpPost]
        [Route("UpdateIncreamentTableData")]
        public IHttpActionResult UpdateIncreamentTableData([FromBody]Increament increament)
        {
            if (increament == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            try
            {
                AdminActivities activity = new AdminActivities(new UnitOfWork());
                activity.CreatedBy = User.Identity.GetUserName();
                activity.UpdateIncreamentTableData(increament);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
    }
}
