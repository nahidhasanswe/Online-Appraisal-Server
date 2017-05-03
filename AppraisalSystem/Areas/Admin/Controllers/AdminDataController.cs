using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Admin;
using Appraisal.BusinessLogicLayer.HBOU;

namespace AppraisalSystem.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/AdminData")]
    public class AdminDataController : ApiController
    {
        [HttpGet]
        [Route("GetAllEmployees")]
        [Authorize(Roles ="Super Admin")]
        public IHttpActionResult GetAllEmployees()
        {
            AdminData data = new AdminData();
            var emp = data.GetAllEmployees();
            return Ok(emp);
        }

        [HttpGet]
        [Route("GetObjectiveByEmployeeId/{id}")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult GetObjectiveByEmployeeId(string id)
        {
            AdminData data = new AdminData();
            var obj = data.GetObjectiveByEmployeeId(id);
            if(obj != null)
            return Ok(obj);

            return NotFound();
        }

        [HttpGet]
        [Route("GetAllEmployeesObjectives")]
        public IHttpActionResult GetAllEmployeesObjectives()
        {
            AdminData data = new AdminData();
            return Ok(data.GetAllEmployeesObjectives());
        }

        [HttpGet]
        [Route("GetAllObjectives")]
        public IHttpActionResult GetAllObjectives()
        {
            AdminData data = new AdminData();
            return Ok(data.GetAllObjectives());
        }

        [HttpGet]
        [Route("GetDepartments")]
        public IHttpActionResult GetDepartments()
        {
            AdminData data = new AdminData();
            return Ok(data.GetDepartments());
        }

        [HttpGet]
        [Route("GetSections")]
        public IHttpActionResult GetSections()
        {
            AdminData data = new AdminData();
            return Ok(data.GetSections());
        }

        [HttpGet]
        [Route("GetDesignation")]
        public IHttpActionResult GetDesignation()
        {
            AdminData data = new AdminData();
            return Ok(data.GetDesignation());
        }

        [HttpGet]
        [Route("GetAllObjectivesWithIncreamentForDirector")]
        public IHttpActionResult GetAllObjectivesWithIncreamentForDirector()
        {
            AdminData data = new AdminData();
            return Ok(data.GetAllObjectivesWithIncreamentForDirector());
        }

        [HttpGet]
        [Route("GetIncreamentTableData")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult GetIncreamentTableData()
        {
            AdminData data = new AdminData();
            return Ok(data.GetIncreamentTableData());
        }


        

    }
}
