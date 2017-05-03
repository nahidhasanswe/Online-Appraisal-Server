using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Core;
using Appraisal.BusinessLogicLayer.Employee;
using Microsoft.AspNet.Identity;
using RepositoryPattern;
using Appraisal.BusinessLogicLayer;

namespace AppraisalSystem.Areas.JobDescription.Controllers
{
    [RoutePrefix("api/JobDescription/JobDescription")]
    [Authorize]
    public class JobDescriptionController : ApiController
    {

        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Save([FromBody] RepositoryPattern.JobDescription description)
        {
            try
            {
                if (description == null)
                {
                    return BadRequest("Description can't be null or empty");
                }

                JobDescriptionByEmployee employee = new JobDescriptionByEmployee();
                Validation validation = new Validation(new UnitOfWork());
                employee.CreatedBy = User.Identity.GetUserName();

                if (validation.HasSetJobDescription(employee.CreatedBy))
                {
                    return BadRequest("You already set a job description.");
                }

               
                employee.Save(description);
                return Ok("Congrats! Save successfully!");
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.InnerException?.ToString());
            }
        }

        [HttpGet]
        [Route("HasSetJobDescription")]
        public IHttpActionResult HasSetJobDescription()
        {
            try
            {
                Validation validation = new Validation(new UnitOfWork());
                string userId = User.Identity.GetUserName();

                return Ok(validation.HasSetJobDescription(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("IsValidToUpdateJobDescription")]
        public IHttpActionResult IsValidToUpdateJobDescription(string id)
        {
            try
            {
                Validation validation = new Validation(new UnitOfWork());

                return Ok(validation.IsValidToUpdateJobDescription(Guid.Parse(id)));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpPost]
        [Route("UpdateJobDescripsion")]
        public IHttpActionResult UpdateJobDescripsion([FromBody] RepositoryPattern.JobDescription description)
        {
            try
            {
                if (description == null)
                {
                    return BadRequest("Description can't be null or empty");
                }

                JobDescriptionByEmployee employee = new JobDescriptionByEmployee();
                Validation validation = new Validation(new UnitOfWork());
                employee.CreatedBy = User.Identity.GetUserName();

                if (validation.IsValidToUpdateJobDescription(description.Id))
                {
                    return BadRequest("Job description is already confirmed by your supervisor.");
                }
                if (description.Id == Guid.Empty)
                {
                    return BadRequest("Job description id can't be empty");
                }
                if (description.KeyAccountabilities == null || description.JobPurposes == null)
                {
                    return BadRequest("Job description KeyAccountabilities or JobPurposes can't be empty");
                }
                employee.Save(description);
                return Ok("Congrats! Updated successfully!");
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.ToString());
            }
        }
    }
}
