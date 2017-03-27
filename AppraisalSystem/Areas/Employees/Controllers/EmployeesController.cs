using System;
using System.Web.Http;
using System.Web.UI.WebControls;
using Appraisal.BusinessLogicLayer;
using Appraisal.BusinessLogicLayer.Admin;
using Appraisal.BusinessLogicLayer.Employee;
using AppraisalSln.Models;
using Microsoft.AspNet.Identity;
using RepositoryPattern;

namespace AppraisalSystem.Areas.Employees.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employees/Employees")]
    public class EmployeesController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Save(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            if (string.IsNullOrEmpty(employee.EmployeeId) || string.IsNullOrEmpty(employee.EmployeeName) ||
                employee.SectionId == Guid.Empty || string.IsNullOrEmpty(employee.ReportTo))
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            if (employee.SectionId == null || employee.DesignationId == null)
            {
                return BadRequest("Section or Department can't be empty!");
            }

            EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
            employees.CreatedBy = User.Identity.GetUserName();
            employees.Save(employee);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("SaveSeflAppraisal")]
        public IHttpActionResult SaveSeflAppraisal(ObjectiveMain main)
        {
            if (main == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            if (main.Id == Guid.Empty) return BadRequest("Objective main id can't be null");
            
            JobObjective employees = new JobObjective(new UnitOfWork());
            employees.CreatedBy = User.Identity.GetUserName();
            employees.InsertSeflAppraisalToMain(main);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("SaveDesignation")]
        public IHttpActionResult SaveDesignation(Designation designation)
        {
            if (designation == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            
            EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
            employees.CreatedBy = User.Identity.GetUserName();
            employees.SaveDesignation(designation);
            return Ok(ActionMessage.SaveMessage);
        }


        [HttpPost]
        [Route("SaveDepartment")]
        public IHttpActionResult SaveDepartment(Department department)
        {
            if (department == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
            employees.CreatedBy = User.Identity.GetUserName();
            employees.SaveDepartment(department);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("SaveSection")]
        public IHttpActionResult SaveSection(Section section)
        {
            if (section == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            if (section.DeparmentId == Guid.Empty || section.DeparmentId == null)
            {
                return BadRequest("Department can't be null");
            }

            EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
            employees.CreatedBy = User.Identity.GetUserName();
            employees.SaveSection(section);
            return Ok(ActionMessage.SaveMessage);
        }

        [HttpPost]
        [Route("UploadFileEvidence")]
        public IHttpActionResult UploadFileEvidence(FileUploadPoco fileUpload)
        {
            if (fileUpload == null)
            {
                return BadRequest(ActionMessage.NullOrEmptyMessage);
            }
            if (fileUpload.ObjectiveId == null || fileUpload.FilePathe == null)
            {
                return BadRequest(fileUpload.FilePathe == null ? "We didn't get your file path": "Objective Id can't be null or empty!");
            }

            EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
            employees.CreatedBy = User.Identity.GetUserName();
            if (employees.CreatedBy == null) return BadRequest("Authentication error");
            employees.UploadFileEvidence(fileUpload);
            return Ok(ActionMessage.SaveMessage);
        }
    }
}
