using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Employee;
using Appraisal.BusinessLogicLayer.HBOU;
using Microsoft.AspNet.Identity;

namespace AppraisalSystem.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/HeadOfBussinessUnit")]
    public class HeadOfBussinessUnitController : ApiController
    {
        [HttpGet]
        [Route("GetEmployeesForHOBU")]
        public IHttpActionResult GetEmployeesForHOBU()
        {
            //string userId = User.Identity.GetUserName();
            try
            {
                string userId = User.Identity.GetUserName();
                HofBUData data =  new HofBUData();
                return Ok(data.GetEmployeesForHOBU(userId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("GetIndividualEmployeeObjectiveList")]
        public IHttpActionResult GetEmployeesObjectiveForHOBU()
        {
            //string userId = User.Identity.GetUserName();
            string userId = User.Identity.GetUserName();
            HofBUData data = new HofBUData();
            return Ok(data.GetEmployeesObjectiveForHOBU(userId));
        }
        [HttpGet]
        [Route("GetIndividualEmployeeObjectiveById/{id}")]
        public IHttpActionResult GetIndividualEmployeeObjectiveById(string id)
        {
            HofBUData data = new HofBUData();
            return Ok(data.GetIndividualEmployeeObjectiveById(id));
        }

        [HttpGet]
        [Route("GetEmployeesObjectiveForHOBUforPerformanceAppraisal")]
        public IHttpActionResult GetEmployeesObjectiveForHOBUforPerformanceAppraisal()
        {
            //string userId = User.Identity.GetUserName();
            string userId = User.Identity.GetUserName();
            HofBUData data = new HofBUData();
            return Ok(data.GetEmployeesObjectiveForHOBUforPerformanceAppraisal(userId));
        }

        [HttpGet]
        [Route("GetEmployeesObjectiveForHOBUWithReportTo")]
        public IHttpActionResult GetEmployeesObjectiveForHOBUWithReportTo()
        {
            //string userId = User.Identity.GetUserName();
            string userId = User.Identity.GetUserName();
            HofBUData data = new HofBUData();
            return Ok(data.GetEmployeesObjectiveForHOBUWithReportTo(userId));
        }

        [HttpGet]
        [Route("GetEmployeeByidForHOBU/{id}")]
        public IHttpActionResult GetEmployeeByidForHOBU(string id)
        {
            HofBUData data = new HofBUData();
            return Ok(data.GetEmployeeByidForHOBU(id));
        }

        [HttpGet]
        [Route("GetDeadline")]
        public IHttpActionResult GetDeadline()
        {
            HofBUData data = new HofBUData();
            return Ok(data.GetDeadline());
        }
    }
}
