using System;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Core
{
    public class DirectorActivity : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        public string CreatedBy { set; get; }

        public DirectorActivity(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SaveSection(Section section)
        {
            if (section.Id != Guid.Empty)
            {
                GetUnitOfWork().SectionRepository.Update(section);
            }
            else
            {
                GetUnitOfWork().SectionRepository.Insert(section);
            }
            GetUnitOfWork().Save();
        }
        public void SaveDepartment(Department department)
        {
            if (department.Id != Guid.Empty)
            {
                GetUnitOfWork().DepartmentRepository.Update(department);
            }
            else
            {
                GetUnitOfWork().DepartmentRepository.Insert(department);
            }
            GetUnitOfWork().Save();
        }
        public void ChangeObjectiveDeadLine(ChangingDeadlinePoco deadlinePoco)
        {
            var emp = GetUnitOfWork()
                        .EmployeeRepository
                        .Get()
                        .FirstOrDefault(a => a.EmployeeId == deadlinePoco.EmployeeId);
            if (emp != null)
            {
                 emp.JobObjectiveDeadline = deadlinePoco.NewDeadLine;
                emp.UpdatedDate = DateTime.Now;
                emp.UpdatedBy = CreatedBy;
            }
            else
            {
                throw new Exception("Employee can' find!");
            }
              
            GetUnitOfWork().EmployeeRepository.Update(emp);
            GetUnitOfWork().Save();
        }

        public void ChangeJobDescriptionDeadLine(ChangingDeadlinePoco deadlinePoco)
        {
            var emp = GetUnitOfWork()
                        .EmployeeRepository
                        .Get()
                        .FirstOrDefault(a => a.EmployeeId == deadlinePoco.EmployeeId);
            if (emp != null)
            {
                emp.JobObjectiveDeadline = deadlinePoco.NewDeadLine;
                emp.UpdatedDate = DateTime.Now;
                emp.UpdatedBy = CreatedBy;
            }
            else
            {
                throw new Exception("Employee can' find!");
            }

            GetUnitOfWork().EmployeeRepository.Update(emp);
            GetUnitOfWork().Save();
        }
        private UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
