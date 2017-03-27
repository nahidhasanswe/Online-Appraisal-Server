using System;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Employee
{
    public class JobObjectiveData : IDisposable
    {
        private UnitOfWork _unitOfWork;

        public JobObjectiveData()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Object GetIndividualEmployeeObjectiveList(string employeeId)
        {
            var list =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.ObjectiveMain.EmployeeId == employeeId)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        EmployeeName = s.ObjectiveMain.Employee.EmployeeName,
                        Email = s.ObjectiveMain.Employee.Email,
                        isObjectApprove = s.IsObjectiveApproved,
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

       
        public Object GetSelfAppraisalForIndividualEmployee(string employeeId)
        {
            var list =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.EmployeeId == employeeId)
                    .Select(s => new
                    {
                        s.Id,
                        s.EmployeeId,
                        s.OverallScore,
                        s.OverallComment,
                        s.CreatedBy,
                        s.CreatedDate,
                        objectiveSub = s.ObjectiveSub.Select(a => new
                        {
                            a.Id,
                            a.ObjectiveMainId,
                            a.IsObjectiveApproved,
                            a.KPI,
                            a.Note,
                            a.Title,
                            a.Target,
                            a.Weight,
                            a.Comments,
                            a.SelfAppraisal,
                            a.CreatedDate,
                            a.CreatedBy
                        }).ToList()
                    }).FirstOrDefault();
            return list;
        }

        public object GetObjectiveMainByDepartmentId(Guid id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.Employee.Section.DeparmentId == id)
                    .Select(s => new
                    {
                        objectiveMainId = s.Id,
                        employeeName = s.Employee.EmployeeName ?? "",
                        employeeId = s.EmployeeId,
                        section = s.Employee.Section.Name ?? ""
                    })
                    .ToList();
            return main;
        }

        public object GetObjectiveMainByEmployeeId(string id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.EmployeeId == id)
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        objectiveMainId = s.Id,
                        s.EmployeeId,
                        employeeName = s.Employee.EmployeeName,
                        objectiveSub = s.ObjectiveSub
                        .Select(c => new
                        {
                            c.Id,
                            c.Title,
                            c.Status,
                            c.KPI,
                            c.Target,
                            c.Weight,
                            c.EvidenceFile,
                            IsObjectiveApproved = c.IsObjectiveApproved ?? false
                        })
                        .ToList()

                    })
                    .FirstOrDefault();
            return main;
        }

        public object GetEmployeeJobDescriptionSingleObject(string id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.EmployeeId == id)
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        employeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        isHOBUConfirmed = s.Employee.JobDescription.Select(a => a.IsHOBUConfirmed).FirstOrDefault(),
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        Location = s.Employee.Location,
                        JoiningDate = s.Employee.JoiningDate,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Name,
                        JobdescriptionId = s.Employee.JobDescription.Select(a=>a.Id).FirstOrDefault(),
                        JobPurpose = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.JobPurposes).FirstOrDefault(),
                        KeyAccountabilities = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.KeyAccountabilities).FirstOrDefault(),

                    })
                    .FirstOrDefault();
            return main;
        }

        public object GetMyEmployeeList(string id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.Employee.ReportTo == id)
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        employeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        isHOBUConfirmed = s.Employee.JobDescription.Select(a => a.IsHOBUConfirmed).FirstOrDefault(),
                        Designation = s.Employee.Designation.Name,
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        Location = s.Employee.Location,
                        JoiningDate = s.Employee.JoiningDate,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Name,
                        JobPurpose = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.JobPurposes).FirstOrDefault(),
                        KeyAccountabilities = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.KeyAccountabilities).FirstOrDefault(),

                    })
                    .ToList();
            return main;
        }

        public object GetEmployeeById(string id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.EmployeeId == id )
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        employeeName = s.Employee.EmployeeName,
                        Email = s.Employee.Email,
                        isHOBUConfirmed = s.Employee.JobDescription.Select(a => a.IsHOBUConfirmed).FirstOrDefault(),
                        Designation = s.Employee.Designation.Name,
                        isReportToConfirm = s.Employee.JobDescription.Select(a=>a.IsReportToConfirmed).FirstOrDefault(),
                        Department = s.Employee.Section.Department.Name,
                        Section = s.Employee.Section.Name,
                        Location = s.Employee.Location,
                        JoiningDate = s.Employee.JoiningDate,
                        ReportToName = s.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.Employee.Employee2.Section.Name,
                        JobPurpose = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.JobPurposes).FirstOrDefault(),
                        KeyAccountabilities = s.Employee.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.KeyAccountabilities).FirstOrDefault(),

                    })
                    .ToList();
            return main;
        }

        public object GetObjectiveSubById(string id)
        {
            var main =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.Id == id)
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        s.Id,
                        s.EvidenceFile,
                        s.KPI,
                        s.Target,
                        s.Title,
                        s.Note,
                        s.Weight,
                        s.IsObjectiveApproved
                    })
                    .FirstOrDefault();
            return main;
        }

        public object GetOthersEmployeeObjectives(string id)
        {
          
            var main =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.ObjectiveMain.Employee.ReportTo == id).ToList()
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        EmployeeName = s.ObjectiveMain.Employee.EmployeeName,
                        Email = s.ObjectiveMain.Employee.Email,
                        Designation = s.ObjectiveMain.Employee.Designation.Name,
                        Department = s.ObjectiveMain.Employee.Section.Department.Name,
                        Section = s.ObjectiveMain.Employee.Section.Name,
                        JoiningDate = s.ObjectiveMain.Employee.JoiningDate,
                        ReportToName = s.ObjectiveMain.Employee.Employee2.EmployeeName,
                        ReportToDesignation = s.ObjectiveMain.Employee.Employee2.Designation.Name,
                        ReportToDepartment = s.ObjectiveMain.Employee.Employee2.Section.Department.Name,
                        ObjectiveId = s.Id,
                        EvidenceFile = s.EvidenceFile??"",
                        s.KPI,
                        s.Target,
                        s.Title,
                        s.Note,
                        s.Weight,
                        IsObjectiveApproved = s.IsObjectiveApproved??false,
                        s.PerfomenseAppraisal,
                        s.Comments,
                        s.Score,
                        s.SelfAppraisal
                    })
                    .ToList();
            return main;
        }

        public object GetOthersEmployeeObjectiveById(string id)
        {

            var main =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.Id == id).ToList()
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        EmployeeId = s.ObjectiveMain.EmployeeId,
                        EmployeeName = s.ObjectiveMain.Employee.EmployeeName,
                        Email = s.ObjectiveMain.Employee.Email,
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

        public object GetMyEmployeesForOrganogram(string id)
        {
            var main =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.ReportTo == id).ToList()
                    .OrderByDescending(o => o.CreatedDate)
                    .Select(s => new
                    {
                        EmployeeId = s.EmployeeId,
                        EmployeeName = s.EmployeeName,
                        Designation = s.Designation.Name,
                        Department = s.Section.Department.Name,
                        JoiningDate = s.JoiningDate,
                        ReportToName = s.Employee2.EmployeeName,

                    })
                    .ToList();
            return main;
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
