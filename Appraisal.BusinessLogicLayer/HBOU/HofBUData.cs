using System;
using System.Linq;
using System.Linq.Expressions;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.HBOU
{
    public class HofBUData : IDisposable
    {
        private UnitOfWork _unitOfWork;

        public HofBUData()
        {
            _unitOfWork = new UnitOfWork();
        }

        public UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }
        public object GetEmployeesForHOBU(string userId)
        {
            try
            {
                var deptId = GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.EmployeeId == userId)
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
                            EmployeeCompany = s.groups,
                            ReportToCompany = s.Employee2.groups,
                            designation = s.Designation.Name ?? "",
                            department = s.Section.Department.Name ?? "",
                            section = s.Section.Name ?? "",
                            s.Location,
                            isReportToConfirm = s.JobDescription.Select(a => a.IsReportToConfirmed ?? false).FirstOrDefault(),
                            isHOBUConfirmed = s.JobDescription.Select(a => a.IsHOBUConfirmed ?? false).FirstOrDefault(),
                            ReportToName = s.Employee2.EmployeeName,
                            ReportToDesignation = s.Employee2.Designation.Name,
                            ReportToDepartment = s.Employee2.Section.Department.Name,
                            JobPurpose = s.JobDescription.Select(a => a.JobPurposes ?? "").FirstOrDefault(),
                            KeyAccountabilities = s.JobDescription.Select(a => a.KeyAccountabilities ?? "").FirstOrDefault(),
                        })
                        .ToList();
                return employeess;
            }
            catch (Exception e)
            {
                return "Error from BLL, Inner Exception:" + e.InnerException + "\n Message: "+e.Message;
            }
        }

        public object GetEmployeesObjectiveForHOBU(string userId)
        {

            var deptId = GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.EmployeeId == userId)
                    .Select(s => s.Section.DeparmentId)
                    .FirstOrDefault();

            var list =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.ObjectiveMain.Employee.Section.Department.Id == deptId)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        EmployeeName = s.ObjectiveMain.Employee.EmployeeName,
                        Email = s.ObjectiveMain.Employee.Email,
                        isObjectApprove = s.IsObjectiveApproved??false,
                        Designation = s.ObjectiveMain.Employee.Designation.Name,
                        Department = s.ObjectiveMain.Employee.Section.Department.Name,
                        Section = s.ObjectiveMain.Employee.Section.Name,
                        JoiningDate = s.ObjectiveMain.Employee.JoiningDate,
                        ReportToName = s.ObjectiveMain.Employee.Employee2.EmployeeName,
                        EmployeeCompany = s.ObjectiveMain.Employee.groups,
                        ReportToCompany = s.ObjectiveMain.Employee.Employee2.groups,
                        ReportToDesignation = s.ObjectiveMain.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.ObjectiveMain.Employee.Section.Department.Name,
                        ObjectiveId = s.Id,
                        Title = s.Title,
                        KPI = s.KPI,
                        Target = s.Target,
                        Weight = s.Weight,
                        Note = s.Note,
                        SelfAppraisal = s.Comments,
                        EvidenceFile = s.EvidenceFile,
                        PerformanceAppraisal = s.PerfomenseAppraisal,
                        Comments = s.Comments
                    })
                    .ToList();
            return list;
        }

        public object GetEmployeesObjectiveForHOBUforPerformanceAppraisal(string userId)
        {
            var list =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.ObjectiveMain.Employee.ReportTo == userId && a.IsObjectiveApproved == true)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        ObjectiveId = s.Id,
                        Title = s.Title,
                        KPI = s.KPI,
                        Target = s.Target,
                        Weight = s.Weight,
                        Note = s.Note,
                        SelfAppraisal = s.SelfAppraisal,
                        EvidenceFile = s.EvidenceFile,
                        PerformanceAppraisal = s.PerfomenseAppraisal,
                        Comments = s.Comments,
                        Score = s.Score,
                        EmployeeCompany = s.ObjectiveMain.Employee.groups,
                        ReportToCompany = s.ObjectiveMain.Employee.Employee2.groups,
                        isSubmitSelfAppraisal = s.ObjectiveMain.OverallScore != null
                    })
                    .ToList();
            return list;
        }

        public object GetDeadline()
        {
            var deadline = GetUnitOfWork().DepartmentConfigRepository.Get().Select(s => new
            {
                s.Id,
                s.DepartmentId,
                s.Department.Name,
                s.JobObjectiveDeadline,
                s.SelfAppraisalDeadline
            }).ToList();
            return deadline;
        }

        public object GetIncrementData()
        {
            var data = GetUnitOfWork().IncreamentRepository.Get().Select(s => new
            {
                s.Id,
                s.LowerScore,
                s.UpperScore,
                s.Promotion
            }).ToList();
            return data;
        }

        public object GetEmployeesObjectiveForHOBUWithReportTo(string userId)
        {
            var list =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.Employee.ReportTo == userId)
                    .Select(s => new
                    {
                        EmployeeId = s.EmployeeId,
                        EmployeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        JoiningDate = s.Employee.JoiningDate,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Section.Department.Name,
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
                        ObjectiveId = s.Id,
                        OverallScore = s.OverallScore,
                        TotalScore = s.TotalScore,
                        OverallComments = s.OverallComment,
                        PDP = s.PersonalDevelopmentPlan,
                        isDone = s.TotalScore != null
                    })
                    .ToList();
            return list;
        }

        public object GetEmployeeByidForHOBU(string employeeId)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.EmployeeId == employeeId)
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        employeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        isHOBUConfirmed = s.Employee.JobDescription.Select(a => a.IsHOBUConfirmed).FirstOrDefault(),
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        isReportToConfirm = s.Employee.JobDescription.Select(a => a.IsReportToConfirmed).FirstOrDefault(),
                        Section = s.Employee.Section.Name,
                        Location = s.Employee.Location,
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
                        JoiningDate = s.Employee.JoiningDate,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Name,
                        JobPurpose = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.JobPurposes).FirstOrDefault(),
                        KeyAccountabilities = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.KeyAccountabilities).FirstOrDefault()
                    })
                    .ToList();
            return main;
        }

        public object GetIndividualEmployeeObjectiveById(string objectiveId)
        {
            var list =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.Id == objectiveId)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        EmployeeName = s.ObjectiveMain.Employee.EmployeeName,
                        Email = s.ObjectiveMain.Employee.Email,
                        isObjectApprove = s.IsObjectiveApproved,
                        EmployeeCompany = s.ObjectiveMain.Employee.groups,
                        ReportToCompany = s.ObjectiveMain.Employee.Employee2.groups,
                        Designation = s.ObjectiveMain.Employee.Designation.Name,
                        Department = s.ObjectiveMain.Employee.Section.Department.Name,
                        Section = s.ObjectiveMain.Employee.Section.Name,
                        JoiningDate = s.ObjectiveMain.Employee.JoiningDate,
                        ReportToName = s.ObjectiveMain.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.ObjectiveMain.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.ObjectiveMain.Employee.Section.Department.Name,
                        ObjectiveId = s.Id,
                        Title = s.Title,
                        KPI = s.KPI,
                        Target = s.Target,
                        Weight = s.Weight,
                        Note = s.Note,
                        SelfAppraisal = s.Comments,
                        EvidenceFile = s.EvidenceFile,
                        PerformanceAppraisal = s.PerfomenseAppraisal,
                        Comments = s.Comments
                    })
                    .ToList();
            return list;
        }

        public void Dispose()
        {
            GetUnitOfWork().Dispose();
        }
    }
}
