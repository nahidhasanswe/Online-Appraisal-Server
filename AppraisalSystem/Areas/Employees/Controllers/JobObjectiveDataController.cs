﻿using System;
using System.Web.Http;
using Appraisal.BusinessLogicLayer.Core;
using Appraisal.BusinessLogicLayer.Employee;
using Microsoft.AspNet.Identity;
using RepositoryPattern;

namespace AppraisalSystem.Areas.Employees.Controllers
{
    [Authorize]
    [RoutePrefix("api/Employees/JobObjectiveData")]
    public class JobObjectiveDataController : ApiController
    {
        [HttpGet]
        [Route("GetObjectiveMainByEmployeeId")]
        public IHttpActionResult GetObjectiveMainByEmployeeId()
        {
            string userId = User.Identity.GetUserName();
            JobObjectiveData data = new JobObjectiveData();
            var objective = data.GetObjectiveMainByEmployeeId(userId);
            return Ok(objective);
        }
        [HttpGet]
        [Route("IsOSubApproved")]
        public IHttpActionResult IsOSubApproved(string id)
        {
             
            Validation data = new Validation(new UnitOfWork());
            var objective = data.IsOSubApproved(id);
            return Ok(objective);
        }
    }
}
