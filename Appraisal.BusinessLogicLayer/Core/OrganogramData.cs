using System;
using System.Collections;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Core
{
   public class OrganogramData : IDisposable
    {
       private UnitOfWork _unitOfWork;

       public OrganogramData()
       {
           _unitOfWork = new UnitOfWork();
       }
        public object GetMyEmployeesForOrganogram(string id)
        {
            SpRepository spRepository = new SpRepository();
            var data = spRepository.GetEmployeeByReportToId(id);
            return data;
        }

       public object GetEmployeeForTotalOrganogram(string departmentId)
       {
           if (departmentId != null)
           {
               var id = Guid.Parse(departmentId);
               var data = GetUnitOfWork().EmployeeRepository.Get()
               .Where(a => a.Section.DeparmentId == id)
               .Select(s => new
               {
                   s.EmployeeId,
                   EmployeeName = s.EmployeeName,
                   Designation = s.Designation.Name,
                   ReportTo = s.ReportTo ?? "",
                   ReportToName = s.Employee2?.EmployeeName,
                   groupName = s.groups
               }).ToList();
               return data;
           }
           else
           {
                var data = GetUnitOfWork().EmployeeRepository.Get().ToList()
                .Select(s => new
                {
                    s.EmployeeId,
                    EmployeeName = s.EmployeeName,
                    Designation = s.Designation.Name,
                    ReportTo = s.ReportTo??"",
                    ReportToName = s.Employee2?.EmployeeName,
                    groupName = s.groups
                }).ToList();
                return data;
            }
       }

       public object GetEmployeeNumberForMarchantising()
       {
           Guid id = Guid.Parse("0c1de283-f08c-4604-aa61-2ffea15e85fd");
           var submit =
               GetUnitOfWork()
                   .ObjectiveMainRepository
                   .Get().Count(a => a.OverallScore != null && a.Employee.Section.Department.Id == id);
            var unSubmit =
               GetUnitOfWork()
                   .ObjectiveMainRepository
                   .Get().Count(a => a.OverallScore == null && a.Employee.Section.Department.Id == id);
           var data = new
           {
              Submited = submit,
              Unsubmited = unSubmit
           };
           return data;
       }

       private UnitOfWork GetUnitOfWork()
       {
           return _unitOfWork;
       }
       public void Dispose()
       {
           GetUnitOfWork().Dispose();
       }

       public object GetEmployeeNumberForHumanResource()
       {
            Guid id = Guid.Parse("2f5f1a76-c5de-46e7-8026-74985b725f33");
            var submit =
                GetUnitOfWork()
                    .ObjectiveMainRepository
                    .Get().Count(a => a.OverallScore != null && a.Employee.Section.Department.Id == id);
            var unSubmit =
               GetUnitOfWork()
                   .ObjectiveMainRepository
                   .Get().Count(a => a.OverallScore == null && a.Employee.Section.Department.Id == id);
            var data = new
            {
                Submited = submit,
                Unsubmited = unSubmit
            };
            return data;
        }

       public object GetEmployeeNumberForCommercial()
       {
            Guid id = Guid.Parse("06f4cc51-7287-4dc2-b60c-7e47c5caa82e");
            var submit =
                GetUnitOfWork()
                    .ObjectiveMainRepository
                    .Get().Count(a => a.OverallScore != null && a.Employee.Section.Department.Id == id);
            var unSubmit =
               GetUnitOfWork()
                   .ObjectiveMainRepository
                   .Get().Count(a => a.OverallScore == null && a.Employee.Section.Department.Id == id);
            var data = new
            {
                Submited = submit,
                Unsubmited = unSubmit
            };
            return data;
        }

       public object GetEmployeeNumberForAccounts()
       {
            Guid id = Guid.Parse("b97e8713-ef32-4cdc-9144-b13be2834616");
            var submit =
                GetUnitOfWork()
                    .ObjectiveMainRepository
                    .Get().Count(a => a.OverallScore != null && a.Employee.Section.Department.Id == id);
            var unSubmit =
               GetUnitOfWork()
                   .ObjectiveMainRepository
                   .Get().Count(a => a.OverallScore == null && a.Employee.Section.Department.Id == id);
            var data = new
            {
                Submited = submit,
                Unsubmited = unSubmit
            };
            return data;
        }

       public object GetEmployeeNumberForQuality()
       {
            Guid id = Guid.Parse("b0feeb88-96bd-48e2-bcf8-41202fa3adcf");
            var submit =
                GetUnitOfWork()
                    .ObjectiveMainRepository
                    .Get().Count(a => a.OverallScore != null && a.Employee.Section.Department.Id == id);
            var unSubmit =
               GetUnitOfWork()
                   .ObjectiveMainRepository
                   .Get().Count(a => a.OverallScore == null && a.Employee.Section.Department.Id == id);
            var data = new
            {
                Submited = submit,
                Unsubmited = unSubmit
            };
            return data;
        }

       public object GetEmployeeNumberForSelfAppraisal()
       {
           var data = GetUnitOfWork().ObjectiveMainRepository.Get().GroupBy(a => a.Employee.Section.Department.Name).Select(s => new
           {
               department = s.Key,
               submited = s.Count(a=>a.OverallScore != null),
               unsubmited = s.Count(a=>a.OverallScore == null)
           }).ToList();
            return data;
        }

       public object GetEmployeeNumberForPerformenseAppraisal()
       {
            var data = GetUnitOfWork().ObjectiveMainRepository.Get().GroupBy(a => a.Employee.Section.Department.Name).Select(s => new
            {
                department = s.Key,
                submited = s.Count(a => a.TotalScore != null),
                unsubmited = s.Count(a => a.TotalScore == null)
            }).ToList();
            return data;
        }

       public object GetEmployeeNumberForJobDescription()
       {
          SpRepository repository = new SpRepository();
           var  data = repository.GetJobObjectiveDataForChart();
           return data;
       }
    }
}
