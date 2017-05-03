using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer;
using Appraisal.BusinessLogicLayer.Core;
using AppraisalSln.Models;
using Microsoft.AspNet.Identity;
using RepositoryPattern;

namespace AppraisalSystem.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/DirectorActivities")]
    public class DirectorActivitiesController : ApiController
    {
        [HttpPost]
        [Route("SaveSection")]
        public IHttpActionResult SaveSection([FromBody]Section section)
        {
            if (section == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            if (section.DeparmentId == Guid.Empty)
            {
                return BadRequest("Department Id can't be null or empty!");
            }

            DirectorActivity activitie = new DirectorActivity(new UnitOfWork());
            activitie.CreatedBy = User.Identity.GetUserName();
            activitie.SaveSection(section);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("SaveDepartment")]
        public IHttpActionResult SaveDepartment([FromBody]Department department)
        {
            if (department == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            
            DirectorActivity activitie = new DirectorActivity(new UnitOfWork());
            activitie.CreatedBy = User.Identity.GetUserName();
            activitie.SaveDepartment(department);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("ChangeObjectiveDeadLine")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult ChangeObjectiveDeadLine([FromBody]ChangingDeadlinePoco poco)
        {
            if (poco == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }

            DirectorActivity activitie = new DirectorActivity(new UnitOfWork());
            activitie.CreatedBy = User.Identity.GetUserName();
            activitie.ChangeObjectiveDeadLine(poco);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("ChangeJobDescriptionDeadLine")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult ChangeJobDescriptionDeadLine([FromBody]ChangingDeadlinePoco poco)
        {
            if (poco == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }

            DirectorActivity activitie = new DirectorActivity(new UnitOfWork());
            activitie.CreatedBy = User.Identity.GetUserName();
            activitie.ChangeJobDescriptionDeadLine(poco);
            return Ok(ActionMessage.SaveMessage);
        }


    }
}
