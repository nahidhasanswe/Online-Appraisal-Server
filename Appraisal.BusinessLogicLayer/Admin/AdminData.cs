using System;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Admin
{
   public class AdminData : IDisposable
    {
       private UnitOfWork _unitOfWork;

       public AdminData()
       {
           _unitOfWork = new UnitOfWork();
       }
        public object GetAllEmployees()
        {
            var main =
                GetUnitOfWork()
                    .EmployeeRepository.Get().Where(a=>a.EmployeeId != "0")
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        employeeName = s.EmployeeName,
                        Email = s.Email,
                        isHOBUConfirmed = s.JobDescription.Select(a => a.IsHOBUConfirmed).FirstOrDefault(),
                        DesignationId = s.DesignationId,
                        Designation = s.Designation.Name,
                        SectionId = s.SectionId,
                        Department = s.Section.Department.Name,
                        DepartmentId = s.Section.DeparmentId,
                        Section = s.Section.Name,
                        EmployeeCompany = s.groups,
                        ReportToCompany = s.Employee2.groups,
                        Location = s.Location,
                        JoiningDate = s.JoiningDate,
                        ReportToName = s.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee2.Section.Name,
                        JobPurpose = s.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.JobPurposes).FirstOrDefault(),
                        KeyAccountabilities = s.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.KeyAccountabilities).FirstOrDefault(),
                        s.JobObjectiveDeadline,
                        s.groups,
                        s.ReportTo,
                        s.SelfAppraisalDeadline,
                        isLocked = GetLock(s.EmployeeId)
                    })
                    .ToList();
            return main;
        }

       private bool GetLock(string username)
       {
           var firstOrDefault = GetUnitOfWork().AspNetUsersRepository.Get().FirstOrDefault(a => a.UserName == username);
           return firstOrDefault != null && (firstOrDefault.isLocked ?? false);
       }

       public object GetAllEmployeesObjectives()
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        EmployeeName = s.ObjectiveMain.Employee.EmployeeName,
                        Email = s.ObjectiveMain.Employee.Email,
                        EmployeeCompany = s.ObjectiveMain.Employee.groups,
                        ReportToCompany = s.ObjectiveMain.Employee.Employee2.groups,
                        Designation = s.ObjectiveMain.Employee.Designation.Name,
                        Department = s.ObjectiveMain.Employee.Section.Department.Name,
                        Section = s.ObjectiveMain.Employee.Section.Name,
                        JoiningDate = s.ObjectiveMain.Employee.JoiningDate,
                        ReportToName = s.ObjectiveMain.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.ObjectiveMain.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.ObjectiveMain.Employee.Employee2.Section.Department.Name,
                        ObjectiveId = s.Id,
                        EvidenceFile = s.EvidenceFile ?? "",
                        s.KPI,
                        s.Target,
                        s.Title,
                        s.Note,
                        s.Weight,
                        IsObjectiveApproved = s.IsObjectiveApproved ?? false,
                        s.PerfomenseAppraisal,
                        s.Comments,
                        s.Score,
                        s.SelfAppraisal
                    })
                    .ToList();
            return main;
        }

        public object GetAllObjectivesWithIncreamentForDirector()
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a=>a.TotalScore != null)
                    .Select(s => new
                    {
                        EmployeeId = s.EmployeeId,
                        EmployeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        JoiningDate = s.Employee.JoiningDate,
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Department.Name,
                        TotalScore = s.TotalScore,
                        Increament = GetIncreament(s.TotalScore??0) + "%"
                    })
                    .ToList();
            return main;
        }

       private int GetIncreament(int totalScore)
       {
           int inc = GetUnitOfWork().IncreamentRepository.Get()
                .Where(a=>a.LowerScore < totalScore && a.UpperScore > totalScore)
                .Select(s=>s.Promotion).FirstOrDefault();
           return inc;
       }
        public object GetAllObjectives()
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        EmployeeId = s.EmployeeId,
                        EmployeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        JoiningDate = s.Employee.JoiningDate,
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Department.Name,
                        objectiveMainId = s.Id,
                        s.OverallComment,
                        s.OverallScore,
                        PDP = s.PersonalDevelopmentPlan,
                        s.IsActive,
                        objectiveSub = s.ObjectiveSub.Where(b => b.IsObjectiveApproved == true)
                        .Select(c => new
                        {
                            ObjecttiveId = c.Id,
                            c.Title,
                            c.Status,
                            c.KPI,
                            c.Target,
                            c.Weight,
                            c.EvidenceFile,
                            IsObjectiveApproved = c.IsObjectiveApproved ?? false,
                            c.PerfomenseAppraisal,
                            c.Comments,
                            c.Score,
                            c.SelfAppraisal
                        })
                        .ToList()
                    })
                    .FirstOrDefault();
            return main;
        }

        public object GetObjectiveByEmployeeId(string id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a=>a.EmployeeId == id)
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        EmployeeId = s.EmployeeId,
                        EmployeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
                        JoiningDate = s.Employee.JoiningDate,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Department.Name,
                        objectiveMainId = s.Id,
                        s.OverallComment,
                        s.OverallScore,
                        PDP = s.PersonalDevelopmentPlan,
                        s.IsActive,
                        s.TotalScore,
                        objectiveSub = s.ObjectiveSub
                        .Select(c => new
                        {
                            ObjecttiveId = c.Id,
                            c.Title,
                            c.Status,
                            c.KPI,
                            c.Target,
                            c.Weight,
                            c.EvidenceFile,
                            IsObjectiveApproved = c.IsObjectiveApproved ?? false,
                            c.PerfomenseAppraisal,
                            c.Comments,
                            c.Score,
                            c.SelfAppraisal
                        })
                        .ToList()
                    })
                    .FirstOrDefault();
            return main;
        }
        public object GetDepartments()
       {
           var deprt = GetUnitOfWork().DepartmentRepository.Get().Select(s => new
           {
               Id = s.Id,
               Department = s.Name
           }).ToList();

           return deprt;
       }

       public object GetSections()
       {
           var sec = GetUnitOfWork().SectionRepository.Get().Select(s => new
           {
               s.Id,
               Section = s.Name,
               department = s.Department.Name,
               departmentId = s.DeparmentId
           }).ToList();

           return sec;
       }

        public object GetDesignation()
        {
            var sec = GetUnitOfWork().DesignationRepository.Get().Select(s => new
            {
                Id = s.Id,
                Designation = s.Name
            }).ToList();

            return sec;
        }


        private UnitOfWork GetUnitOfWork()
       {
           return _unitOfWork;
       }

       public void Dispose()
       {
          GetUnitOfWork().Dispose();
       }

       public object GetIncreamentTableData()
       {
           var data = GetUnitOfWork().IncreamentRepository.Get().ToList();
           return data;
       }
    }
}
