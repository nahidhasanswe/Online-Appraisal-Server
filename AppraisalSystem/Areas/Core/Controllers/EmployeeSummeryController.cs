using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Core;
using AppraisalSln.Models;
using Microsoft.AspNet.Identity;
using RepositoryPattern;

namespace AppraisalSystem.Areas.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/Core/EmployeeSummery")]
    public class EmployeeSummeryController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = "Super Admin,Department Head")]
        public IHttpActionResult Save(EmployeeSummery summery)
        {
            try
            {
                if (summery == null)
                {
                    return BadRequest(ActionMessage.NullOrEmptyMessage);
                }
                EmployeeShortList list = new EmployeeShortList();
                list.CreatedBy = User.Identity.GetUserName();
                list.SaveSummery(summery);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("GetEmployeeSummery")]
        [Authorize(Roles = "Super Admin,Department Head")]
        public IHttpActionResult GetEmployeeSummery()
        {
            try
            {
                EmployeeShortList list = new EmployeeShortList();
               var result = list.GetEmployeeSummery();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("Delete")]
        [Authorize(Roles = "Super Admin,Department Head")]
        public IHttpActionResult Delete(EmployeeSummery summery)
        {
            try
            {
                EmployeeShortList list = new EmployeeShortList();
                list.Delete(summery);
                return Ok("Delete successful");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
