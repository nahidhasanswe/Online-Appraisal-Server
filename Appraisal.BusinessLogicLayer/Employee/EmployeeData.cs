

using System;
using System.Linq;
using Appraisal.BusinessLogicLayer.Core;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Employee
{
    public class EmployeeData :IDisposable
    {
        private readonly UnitOfWork _unitOfWork;

        public EmployeeData(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public object GetEmployeeWithJobDescriptionById(string id)
        {
            Validation validation = new Validation(new UnitOfWork());
           
            if (validation.HasSetJobDescription(id))
            {
              var employee =
                GetUnitOfWork()
                    .EmployeeRepository.Get().Where(e => e.EmployeeId == id && e.IsActive == true).ToList()
                    .Select(s => new
                     {
                        s.EmployeeId,
                        s.EmployeeName,
                        s.Email,
                        s.JoiningDate,
                        designation = s.Designation.Name??"",
                        department = s.Section.Department.Name??"",
                        section = s.Section.Name??"",
                        EmployeeCompany = s.groups,
                        ReportToCompany = s.Employee2.groups,
                        s.Location,
                        reportToName = s.Employee2.EmployeeName,
                        reportToId = s.Employee2.EmployeeId,
                        reportToDesignation = s.Employee2.Designation,
                        reportToDepartment=s.Employee2.DesignationId,
                         jobDescription = s.JobDescription.Select(c=>new
                         {
                             c.KeyAccountabilities,
                             c.JobPurposes,
                             c.Id
                         }).FirstOrDefault()
                    })
                    .FirstOrDefault();
                return employee;
            }
            else
            {
             var employee =
               GetUnitOfWork()
                   .EmployeeRepository.Get().Where(e => e.EmployeeId == id && e.IsActive == true).ToList()
                   .Select(s => new
                   {
                       s.EmployeeId,
                       s.EmployeeName,
                       s.Email,
                       s.JoiningDate,
                       designation = s.Designation.Name ?? "",
                       department = s.Section.Department.Name ?? "",
                       section = s.Section.Name ?? "",
                       s.Location,
                       reportToName = s.Employee2.EmployeeName??"",
                       reportToId = s.Employee2.EmployeeId??"",
                       reportToDesignation = s.Employee2.Email??""
                   })
                   .FirstOrDefault();
                   return employee;
            }
         
        }

        public object GetIndividualJobObjective(string id)
        {
            var objective = GetUnitOfWork().ObjectiveMainRepository.Get()
                .Where(a => a.EmployeeId == id && a.IsActive == true)
                .Select(s => new
                {
                    s.Id,
                    s.EmployeeId,
                    s.OverallComment,
                    s.OverallScore,
                    s.CreatedBy,
                    s.CreatedDate,
                    objectiveSub = s.ObjectiveSub.Select(a=>new
                    {
                        a.Id,
                        a.ObjectiveMainId,
                        a.KPI,
                        a.Status,
                        a.Target,
                        a.Comments,
                        a.SelfAppraisal,
                        a.Weight,
                        a.EvidenceFile,
                        a.CreatedBy,
                        a.CreatedDate,
                        a.Note
                    })
                }).ToList();
            return objective;
        }
        
        public object GetJobObjectiveById(Guid objectiveId)
        {
            var objective = GetUnitOfWork().ObjectiveMainRepository.Get()
                .Where(a => a.Id == objectiveId && a.IsActive == true)
                .Select(s => new
                {
                    s.Id,
                    s.EmployeeId,
                    s.OverallComment,
                    s.OverallScore,
                    s.CreatedBy,
                    s.CreatedDate,
                    objectiveSub = s.ObjectiveSub.Select(a => new
                    {
                        a.Id,
                        a.ObjectiveMainId,
                        a.KPI,
                        a.Status,
                        a.Target,
                        a.Comments,
                        a.SelfAppraisal,
                        a.Weight,
                        a.EvidenceFile,
                        a.CreatedBy,
                        a.CreatedDate,
                        a.Note
                    })
                }).ToList();
            return objective;
        }

        public object GetEmployeeBySupervisorId(string employeeId)
        {
            string reportToId = GetUnitOfWork()
                                .EmployeeRepository.Get()
                                .Where(a => a.EmployeeId == employeeId && a.IsActive == true)
                                .Select(s => s.ReportTo)
                                .FirstOrDefault();

            var objective = GetUnitOfWork().EmployeeRepository.Get()
                            .Where(a => a.ReportTo == reportToId && a.IsActive == true)
                            .Select(s => new
                            {
                                s.EmployeeId,
                                s.EmployeeName,
                                section = s.Section.Name,
                                department = s.Section.Department.Name,
                                s.JoiningDate,
                                EmployeeCompany = s.groups,
                                ReportToCompany = s.Employee2.groups
                            }).ToList();
            return objective;
        }

        public object GetEmployeesByDepartment(string userId)
        {

            var deptId = GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.EmployeeId == userId && a.IsActive == true)
                    .Select(s => s.Section.DeparmentId)
                    .FirstOrDefault();

            var employeess =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.Section.DeparmentId == deptId)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        s.EmployeeName,
                        s.Email,
                        s.JoiningDate,
                        designation = s.Designation.Name ?? "",
                        department = s.Section.Department.Name ?? "",
                        section = s.Section.Name ?? "",
                        s.Location,
                        EmployeeCompany = s.groups,
                        ReportToCompany = s.Employee2.groups,
                        reportTo = GetUnitOfWork().EmployeeRepository.Get().Where(a => a.EmployeeId == s.ReportTo).Select(b => new
                        {
                            reportToId = b.EmployeeId,
                            reportToName = b.EmployeeName,
                            reportToEmail = b.Email
                        }).FirstOrDefault()
                    })
                    .ToList();
            return employeess;
        }

        public object GetOtherEmployeesList(string userId)
        {

            var employeess =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.ReportTo == userId && a.IsActive == true)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        s.EmployeeName,
                        s.Email,
                        s.JoiningDate,
                        EmployeeCompany = s.groups,
                        ReportToCompany = s.Employee2.groups,
                        designation = s.Designation.Name ?? "",
                        department = s.Section.Department.Name ?? "",
                        section = s.Section.Name ?? "",
                        s.Location,
                        isReportToConfirm = s.JobDescription.Select(a=>a.IsReportToConfirmed??false).FirstOrDefault(),
                        isHOBUConfirmed = s.JobDescription.Select(a => a.IsHOBUConfirmed ?? false).FirstOrDefault(),
                        ReportToName = s.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee2.Section.Department.Name,
                        JobPurpose = s.JobDescription.Select(a=>a.JobPurposes??"").FirstOrDefault(),
                        KeyAccountabilities = s.JobDescription.Select(a => a.KeyAccountabilities ?? "").FirstOrDefault(),
                    })
                    .ToList();
            return employeess;
        }

        public string GetEmployeeNameByEmployeeId(string id)
        {
            return GetUnitOfWork().EmployeeRepository.Get().Where(x => x.EmployeeId == id && x.IsActive == true).Select(s => s.EmployeeName).FirstOrDefault();
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
