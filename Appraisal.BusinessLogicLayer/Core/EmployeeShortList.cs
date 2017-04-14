using System;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Core
{
    public class EmployeeShortList : IDisposable
    {
        private UnitOfWork _unitOfWork;
        public string CreatedBy { get; set; }

        public EmployeeShortList()
        {
            _unitOfWork = new UnitOfWork();
        }
       
        public void SaveSummery(EmployeeSummery summery)
        {
            if (summery.Id != Guid.Empty)
            {
                EmployeeSummery sum = GetUnitOfWork().EmployeeSummeryRepository.Get()
                    .FirstOrDefault(a => a.Id == summery.Id);
                if (sum != null)
                {
                    sum.DepartmentName = summery.DepartmentName ?? sum.DepartmentName;
                    sum.HeadOfDepartment = summery.HeadOfDepartment ?? sum.HeadOfDepartment;
                    sum.NumberOfEmployees = summery.NumberOfEmployees ?? sum.NumberOfEmployees;
                    sum.CreatedBy = CreatedBy;
                    sum.CreatedDate = DateTime.Now;
                    GetUnitOfWork().EmployeeSummeryRepository.Update(sum);
                }
            }
            else
            {
                summery.CreatedDate = DateTime.Now;
                summery.CreatedBy = CreatedBy;
                summery.Id = Guid.NewGuid();
                GetUnitOfWork().EmployeeSummeryRepository.Insert(summery);
            }
            GetUnitOfWork().Save();
        }

        public object GetEmployeeSummery()
        {
            var result = GetUnitOfWork().EmployeeSummeryRepository.Get().Select(s => new
            {
                s.DepartmentName,
                s.Id,
                s.HeadOfDepartment,
                s.NumberOfEmployees
            }).ToList();
            return result;
        }

        public void Delete(EmployeeSummery summery)
        {
            GetUnitOfWork().EmployeeSummeryRepository.Delete(summery);
            GetUnitOfWork().Save();
        }

        private UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }
        public void Dispose()
        {
            GetUnitOfWork().Dispose();
        }
    }
}
