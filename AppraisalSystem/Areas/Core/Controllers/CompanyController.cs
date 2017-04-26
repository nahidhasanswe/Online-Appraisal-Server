using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Core;
using AppraisalSln.Models;
using AppraisalSystem.Models;
using RepositoryPattern;

namespace AppraisalSystem.Areas.Core.Controllers
{
    [RoutePrefix("api/Core/Company")]
    public class CompanyController : ApiController
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Save([FromBody]Company company)
        {
            try
            {
                CompanyDll dll = new CompanyDll();
                dll.Save(company);
                return Ok(ActionMessage.SaveMessage);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            try
            {
                CompanyDll dll = new CompanyDll();
                return Ok(dll.Get());
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        [Route("GetById/{id}")]
        public IHttpActionResult GetById(string id)
        {
            try
            {
                CompanyDll dll = new CompanyDll(); 
                return Ok(dll.GetById(id));
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [Authorize(Roles = Roles.SuperAdmin)]
        [HttpPost]
        [Route("Delete/{id}")]
        public IHttpActionResult Save(string id)
        {
            try
            {
                CompanyDll dll = new CompanyDll();
                dll.Delete(id);
                return Ok(ActionMessage.DeleteMessage);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return BadRequest("You can't delete this company. Please contact with system developer!");
            }
        }
    }
}
