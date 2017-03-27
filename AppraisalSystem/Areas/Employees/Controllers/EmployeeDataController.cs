using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Employee;
using Microsoft.AspNet.Identity;
using RepositoryPattern;

namespace AppraisalSystem.Areas.Employees.Controllers
{
    [RoutePrefix("api/Employees/EmployeesData")]
    public class EmployeeDataController : ApiController
    {
        [HttpGet]
        [Route("GetLoggedInEmployeeWithJobDescription")]
        public IHttpActionResult GetLoggedInEmployeeWithJobDescription()
        {
            var user = User.Identity.GetUserName();
            EmployeeData employeeData = new EmployeeData(new UnitOfWork());
            var jobDesc = employeeData.GetEmployeeWithJobDescriptionById(user);
            return Ok(jobDesc);
        }

        [HttpGet]
        [Route("GetIndividualJobObjective")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetIndividualJobObjective()
        {
            var user = User.Identity.GetUserName();
            EmployeeData employeesData = new EmployeeData(new UnitOfWork());
           return Ok(employeesData.GetIndividualJobObjective(user));
        }

        [HttpGet]
        [Route("GetJobObjectiveById/{id}")]
        public IHttpActionResult GetJobObjectiveById(string id)
        {
            EmployeeData employeesData = new EmployeeData(new UnitOfWork());
            return Ok(employeesData.GetJobObjectiveById(Guid.Parse(id)));
        }

        [HttpGet]
        [Route("GetEmployeeBySupervisorId")]
        public IHttpActionResult GetEmployeeBySupervisorId()
        {
            var employee = User.Identity.GetUserName();
            EmployeeData employeesData = new EmployeeData(new UnitOfWork());
            return Ok(employeesData.GetEmployeeBySupervisorId(employee));
        }

        [HttpGet]
        [Route("GetJobObjectiveByEmployeeId")]
        public IHttpActionResult GetJobObjectiveByEmployeeId(string id)
        {
            try
            {
                EmployeeData employeesData = new EmployeeData(new UnitOfWork());
                return Ok(employeesData.GetIndividualJobObjective(id));
            }
            catch (Exception EX_NAME)
            {
               return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeesByDepartment")]
        public IHttpActionResult GetEmployeesByDepartment()
        {
            try
            {
                string userId = User.Identity.GetUserName();
                EmployeeData employeesData = new EmployeeData(new UnitOfWork());
                return Ok(employeesData.GetEmployeesByDepartment(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetOtherEmployeesList")]
        public IHttpActionResult GetOtherEmployeesList()
        {
            try
            {
                string userId = User.Identity.GetUserName();
                EmployeeData employeesData = new EmployeeData(new UnitOfWork());
                return Ok(employeesData.GetOtherEmployeesList(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
        
        [HttpGet]
        [Route("GetEmployeeJobDescriptionSingleObject")]
        public IHttpActionResult GetEmployeeJobDescriptionSingleObject()
        {
            try
            {
                //string userId = User.Identity.GetUserName();
                string userId = User.Identity.GetUserName();
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetEmployeeJobDescriptionSingleObject(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetIndividualEmployeeObjectiveList")]
        public IHttpActionResult GetIndividualEmployeeObjectiveList()
        {
            try
            {
                string userId = User.Identity.GetUserName();
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetIndividualEmployeeObjectiveList(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetSelfAppraisalForIndividualEmployee")]
        public IHttpActionResult GetSelfAppraisalForIndividualEmployee()
        {
            try
            {
//string userId = User.Identity.GetUserName();
                string userId = User.Identity.GetUserName();
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetSelfAppraisalForIndividualEmployee(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetOthersEmployeeObjectives")]
        public IHttpActionResult GetOthersEmployeeObjectives()
        {
            try
            {
                //string userId = User.Identity.GetUserName();
                string userId = User.Identity.GetUserName();
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetOthersEmployeeObjectives(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetOthersEmployeeObjectiveById/{id}")]
        public IHttpActionResult GetOthersEmployeeObjectiveById(string id)
        {
            try
            {
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetOthersEmployeeObjectiveById(id));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }


        [Authorize]
        [HttpGet]
        [Route("GetMyEmployeeList")]
        public IHttpActionResult GetMyEmployeeList()
        {
            try
            {
                //string userId = User.Identity.GetUserName();
                string userId = User.Identity.GetUserName();
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetMyEmployeeList(userId));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
        
        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public IHttpActionResult GetEmployeeById(string id)
        {
            try
            {
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetEmployeeById(id));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }

        [HttpGet]
        [Route("GetMyEmployeesForOrganogram/{id}")]
        public IHttpActionResult GetMyEmployeesForOrganogram(string id)
        {
            try
            {
                JobObjectiveData employeesData = new JobObjectiveData();
                return Ok(employeesData.GetMyEmployeesForOrganogram(id));
            }
            catch (Exception EX_NAME)
            {
                return BadRequest(EX_NAME.Message);
            }
        }
    }
}
