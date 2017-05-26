using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Employee
{
    public class EmployeesActivities : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        public string CreatedBy { get; set; }

        public EmployeesActivities(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Save(RepositoryPattern.Employee employee)
        {
            var isExists = GetUnitOfWork().EmployeeRepository.Get().Where(a => a.EmployeeId == employee.EmployeeId).ToList();
            if (isExists.Any())
            {
                var aspUser = GetUnitOfWork().AspNetUsersRepository.Get().Where(a => a.UserName == employee.EmployeeId).FirstOrDefault();
                if (employee.Email != null && String.IsNullOrEmpty(aspUser.Email))
                {
                    aspUser.Email = employee.Email;
                    GetUnitOfWork().AspNetUsersRepository.Update(aspUser);
                }               

                employee.UpdatedBy = CreatedBy;
                employee.IsActive = true;
                employee.UpdatedDate = DateTime.Now;
                GetUnitOfWork().EmployeeRepository.Update(employee);
                
            }
            else
            {
                employee.CreatedBy = CreatedBy;
                employee.IsActive = true;
                employee.CreatedDate = DateTime.Now;
                GetUnitOfWork().EmployeeRepository.Insert(employee);
            }
                
           
            GetUnitOfWork().Save();
        }
        public void InsertEmployee(RepositoryPattern.Employee employee)
        {
           
            
                employee.CreatedBy = CreatedBy;
                employee.IsActive = true;
                employee.CreatedDate = DateTime.Now;
                GetUnitOfWork().EmployeeRepository.Insert(employee);
            GetUnitOfWork().Save();
        }
        public void SaveDesignation(Designation designation)
        {
            if (designation.Id != Guid.Empty)
            {
                GetUnitOfWork().DesignationRepository.Update(designation);
            }
            else
            {
                designation.Id = Guid.NewGuid();
                GetUnitOfWork().DesignationRepository.Insert(designation);
            }
            GetUnitOfWork().Save();
        }

        public void SaveDepartment(Department department)
        {
            if (department.Id != Guid.Empty)
            {
                department.UpdatedBy = CreatedBy;
                department.UpdatedDate = DateTime.Now;
                GetUnitOfWork().DepartmentRepository.Update(department);
            }
            else
            {
                department.CreatedBy = CreatedBy;
                department.CreatedDate = DateTime.Now;
                department.Id = Guid.NewGuid();
                GetUnitOfWork().DepartmentRepository.Insert(department);
            }
            GetUnitOfWork().Save();
        }

        public void SaveSection(Section section)
        {
            if (section.Id != Guid.Empty)
            {
                GetUnitOfWork().SectionRepository.Update(section);
            }
            else
            {
                section.Id = Guid.NewGuid();
                GetUnitOfWork().SectionRepository.Insert(section);
            }
            GetUnitOfWork().Save();
        }

        public void SaveSelfAppraisal(ObjectiveMain main)
        {
            InsertOverScore(main);
            InsertSelfAppraisal(main.ObjectiveSub);
            GetUnitOfWork().Save();
        }

        private void InsertOverScore(ObjectiveMain main)
        {
            ObjectiveMain objectiveMain = GetUnitOfWork().ObjectiveMainRepository.GetById(main.Id);
            objectiveMain.OverallScore = main.OverallScore??0;
            objectiveMain.OverallComment = main.OverallComment??"";
            objectiveMain.PersonalDevelopmentPlan = main.PersonalDevelopmentPlan??"";
            objectiveMain.UpdatedBy = CreatedBy;
            objectiveMain.CreatedDate = DateTime.Now;
            GetUnitOfWork().ObjectiveMainRepository.Update(objectiveMain);
        }

        private void InsertSelfAppraisal(ICollection<ObjectiveSub> objectiveSub)
        {
            foreach (ObjectiveSub sub in objectiveSub)
            {
                ObjectiveSub objSub = new ObjectiveSub();
                objSub.SelfAppraisal = sub.SelfAppraisal;
                objSub.Note = objSub.Note;
                objSub.UpdatedBy = CreatedBy;
                objSub.CreatedDate = DateTime.Now;
                GetUnitOfWork().ObjectiveSubRepository.Update(objSub);
            }
        }

        public void UploadFileEvidence(FileUploadPoco poco)
        {
            var objective = GetUnitOfWork().ObjectiveSubRepository.Get().FirstOrDefault(a => a.Id == poco.ObjectiveId);
            if (objective != null)
            {
                objective.EvidenceFile = poco.FilePathe;
                objective.UpdatedBy = CreatedBy;
                objective.UpdatedDate = DateTime.Now;
                GetUnitOfWork().ObjectiveSubRepository.Update(objective);
                GetUnitOfWork().Save();
            }
            else
            {
                throw new Exception("Sorry, we couldn't find your objective!");
            }
        }

        public UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }

        public void Dispose()
        {
            GetUnitOfWork().Dispose();
        }
    }
}
