using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Admin;
using Appraisal.BusinessLogicLayer.HBOU;
using AppraisalSln.Models;
using AppraisalSystem.Models;
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
        [Authorize(Roles = Roles.SuperAdmin)]
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
        [Authorize(Roles = "Super Admin")]
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
        [Route("DeleteEmployee/{id}")]
       // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public IHttpActionResult DeleteEmployee(string id)
        {
            try
            {
                AdminActivities activitie = new AdminActivities(new UnitOfWork());
                activitie.CreatedBy = User.Identity.GetUserName();
                activitie.DeleteEmployee(id);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("ActiveEmployee/{id}")]
        // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public IHttpActionResult ActiveEmployee(string id)
        {
            try
            {
                AdminActivities activitie = new AdminActivities(new UnitOfWork());
                activitie.CreatedBy = User.Identity.GetUserName();
                activitie.ActiveEmployee(id);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }


        [HttpPost]
        [Route("SetObjectDeadline")]
        [Authorize(Roles = "Super Admin")]
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
                activity.SetAppraisalDeadline(config);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("SetAppraisalDeadline")]
        [Authorize(Roles = "Super Admin")]
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
                activity.SetSeflfAppraisalDeadline(config);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
        [HttpPost]
        [Route("UpdateAppraisalDeadline")]
        [Authorize(Roles = "Super Admin")]
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
        [Route("AllowUpdateJobDescriptionByHOBU/{id}")]
        public IHttpActionResult AllowUpdateJobDescriptionByHOBU(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                HboActivities activity = new HboActivities();
                activity.CreatedBy = User.Identity.GetUserName();
                activity.AllowUpdateJobDescriptionByHOBU(id);
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
                return BadRequest(EX_NAME.ToString());
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
        [Authorize(Roles = "Super Admin")]
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
