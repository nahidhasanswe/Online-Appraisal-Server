using System;
using System.Linq;
using Appraisal.BusinessLogicLayer.Core;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Employee
{
    public class JobDescriptionByEmployee
    {
        public string CreatedBy { get; set; }

        public void Save(JobDescription description)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (description.Id != Guid.Empty)
            {
                JobDescription descriptions = unitOfWork.JobDescriptionRepository.GetById(description.Id);
                if (descriptions == null)
                {
                    throw new Exception("We can't find your job description ");
                }
                descriptions.KeyAccountabilities = description.KeyAccountabilities;
                descriptions.JobPurposes = description.JobPurposes;
                descriptions.UpdatedBy = CreatedBy;
                descriptions.UpdatedDate = DateTime.Now;
                unitOfWork.JobDescriptionRepository.Update(descriptions);
            }
            else
            {
                Validation validation = new Validation(new UnitOfWork());

                if (validation.HasSetJobDescription(CreatedBy))
                {
                    throw new Exception("You already set a job description.");
                }
                description.CreatedBy = CreatedBy;
                description.EmployeeId = CreatedBy;
                description.IsReportToConfirmed = false;
                description.IsHOBUConfirmed = false;
                description.CreatedDate = DateTime.Now;
                description.Id = Guid.NewGuid();
                unitOfWork.JobDescriptionRepository.Insert(description);
            }
            unitOfWork.Save();
            string email = unitOfWork.EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.Employee2?.Email;
            string sender = unitOfWork.EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.EmployeeName;

            EmailNotifier notifier = new EmailNotifier();
            if (email != null)
                notifier.Send("MyEmployee?id=" + CreatedBy, "Dear sir,\n I have submited my job description on " + DateTime.Now.Date + ".", email, sender);
        }
    }
}
