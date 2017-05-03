using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Core;
using Appraisal.BusinessLogicLayer.Employee;
using Microsoft.AspNet.Identity;

namespace AppraisalSystem.Areas.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/Core/Organogram")]
    public class OrganogramController : ApiController
    {
        [HttpGet]
        [Route("GetMyEmployeesForOrganogram")]
        public IHttpActionResult GetMyEmployeesForOrganogram()
        {
            try
            {

                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetMyEmployeesForOrganogram(User.Identity.GetUserName()));
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }
        [HttpGet]
        [Route("GetEmployeeForTotalOrganogram")]
        [Authorize(Roles = "Super Admin,Department Head")]
        public IHttpActionResult GetEmployeeForTotalOrganogram(string id)
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeForTotalOrganogram(id));
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeNumberForSelfAppraisal")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult GetEmployeeNumberForSelfAppraisal()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForSelfAppraisal());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            } // 
        }

        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        [Route("GetEmployeeNumberForPerformenseAppraisal")]
        public IHttpActionResult GetEmployeeNumberForPerformenseAppraisal()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForPerformenseAppraisal());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }

        [Authorize(Roles = "Super Admin")]
        [HttpGet]
        [Route("GetEmployeeNumberForJobDescription")]
        public IHttpActionResult GetEmployeeNumberForJobDescription()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForJobDescription());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeNumberForMarchantising")]
        public IHttpActionResult GetEmployeeNumberForMarchantising()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForMarchantising());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeNumberForHumanResource")]
        public IHttpActionResult GetEmployeeNumberForHumanResource()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForHumanResource());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }
        [HttpGet]
        [Route("GetEmployeeNumberForCommercial")]
        public IHttpActionResult GetEmployeeNumberForCommercial()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForCommercial());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }
        [HttpGet]
        [Route("GetEmployeeNumberForAccounts")]
        public IHttpActionResult GetEmployeeNumberForAccounts()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForAccounts());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }
        [HttpGet]
        [Route("GetEmployeeNumberForQuality")]
        public IHttpActionResult GetEmployeeNumberForQuality()
        {
            try
            {
                OrganogramData orgData = new OrganogramData();
                return Ok(orgData.GetEmployeeNumberForQuality());
            }
            catch (Exception exName)
            {
                return BadRequest(exName.Message);
            }
        }
    }
}
