using System;
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
        }
    }
}
