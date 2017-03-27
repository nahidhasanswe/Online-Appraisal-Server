using System.Web.Mvc;

namespace AppraisalSystem.Areas.JobDescription
{
    public class JobDescriptionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JobDescription";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "JobDescription_default",
            //    "JobDescription/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}