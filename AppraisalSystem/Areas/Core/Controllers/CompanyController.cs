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
    [Authorize]
    [RoutePrefix("api/Core/Company")]
    public class CompanyController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        [Authorize(Roles = "Super Admin")]
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


        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "Super Admin")]
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
         
        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize(Roles = "Super Admin")]
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
        
        [HttpPost]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Super Admin")]
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
