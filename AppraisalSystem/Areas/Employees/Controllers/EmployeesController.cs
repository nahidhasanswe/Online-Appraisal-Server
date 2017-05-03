using System;
using System.Web.Http;
using System.Web.UI.WebControls;
using Appraisal.BusinessLogicLayer;
using Appraisal.BusinessLogicLayer.Admin;
using Appraisal.BusinessLogicLayer.Core;
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
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult Save(Employee employee)
        {
            try
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
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("SaveSeflAppraisal")]
        public IHttpActionResult SaveSeflAppraisal(ObjectiveMain main)
        {
            try
            {
                if (main == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }

                Validation validation = new Validation(new UnitOfWork());
                JobObjective employees = new JobObjective(new UnitOfWork());
                employees.CreatedBy = User.Identity.GetUserName();
                if (main.Id == Guid.Empty) return BadRequest("Objective main id can't be null");

                if (validation.IsSelfAppraisalDeadLineNull(employees.CreatedBy))
                {
                    return BadRequest("Your self appraisal deadline set  yet by your supervisor!");
                }

                if (!validation.IsSelfAppraisalDeadLineValid(employees.CreatedBy))
                {
                    return BadRequest("You have missed your deadline");
                }

                employees.InsertSeflAppraisalToMain(main);
                return Ok("You are successfully submitted self appraisal to " + employees.GetReportTo(employees.CreatedBy));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("SaveDesignation")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult SaveDesignation(Designation designation)
        {
            try
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
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }


        [HttpPost]
        [Route("SaveDepartment")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult SaveDepartment(Department department)
        {
            try
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
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("SaveSection")]
        [Authorize(Roles = "Super Admin")]
        public IHttpActionResult SaveSection(Section section)
        {
            try
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
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("UploadFileEvidence")]
        public IHttpActionResult UploadFileEvidence(FileUploadPoco fileUpload)
        {
            try
            {
                if (fileUpload == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                if (fileUpload.ObjectiveId == null || fileUpload.FilePathe == null)
                {
                    return BadRequest(fileUpload.FilePathe == null ? "We didn't get your file path" : "Objective Id can't be null or empty!");
                }

                EmployeesActivities employees = new EmployeesActivities(new UnitOfWork());
                employees.CreatedBy = User.Identity.GetUserName();
                if (employees.CreatedBy == null) return BadRequest("Authentication error");
                employees.UploadFileEvidence(fileUpload);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
