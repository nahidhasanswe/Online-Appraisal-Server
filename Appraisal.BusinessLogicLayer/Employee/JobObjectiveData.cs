using System;
using System.Linq;
using Appraisal.BusinessLogicLayer.Core;
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
                        EmployeeCompany = s.ObjectiveMain.Employee.groups,
                        ReportToCompany = s.ObjectiveMain.Employee.Employee2.groups,
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
                        objectiveSub = s.ObjectiveSub.Where(a=>a.IsObjectiveApproved == true).Select(a => new
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
                            a.CreatedBy,
                            a.EvidenceFile
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
                        section = s.Employee.Section.Name ?? "",
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
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
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
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
            Validation validation = new Validation(new UnitOfWork());

            if (validation.HasSetJobDescription(id))
            {
                var employee =
                  GetUnitOfWork()
                      .EmployeeRepository.Get().Where(e => e.EmployeeId == id).ToList()
                      .Select(s => new
                      {
                          s.EmployeeId,
                          employeeName = s.EmployeeName,
                          Email = s.Email,
                          isHOBUConfirmed = s.JobDescription.Select(a => a.IsHOBUConfirmed).FirstOrDefault(),
                          Designation = s.Designation.Name,
                          Department = s.Section.Department.Name,
                          Section = s.Section.Name,
                          EmployeeCompany = s.groups,
                          ReportToCompany = s.Employee2.groups,
                          Location = s.Location,
                          JoiningDate = s.JoiningDate,
                          ReportToName = s.Employee2.EmployeeName,
                          ReportToDesignation = s.Employee2.Designation.Name,
                          ReportToDepartment = s.Employee2.Section.Name,
                          JobdescriptionId = s.JobDescription.Select(a => a.Id).FirstOrDefault(),
                          JobPurpose = s.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.JobPurposes).FirstOrDefault(),
                          KeyAccountabilities = s.JobDescription.OrderByDescending(a => a.CreatedBy).Select(b => b.KeyAccountabilities).FirstOrDefault(),

                      })
                      .FirstOrDefault();
                return employee;
            }
            else
            {
                var employee =
                  GetUnitOfWork()
                      .EmployeeRepository.Get().Where(e => e.EmployeeId == id).ToList()
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
                          reportToName = s.Employee2.EmployeeName ?? "",
                          reportToId = s.Employee2.EmployeeId ?? "",
                          reportToDesignation = s.Employee2.Email ?? ""
                      })
                      .FirstOrDefault();
                return employee;
            }
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
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
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
                        EmployeeCompany = s.Employee.groups,
                        ReportToCompany = s.Employee.Employee2.groups,
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
                        EmployeeCompany = s.ObjectiveMain.Employee.groups,
                        ReportToCompany = s.ObjectiveMain.Employee.Employee2.groups,
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

       
        private UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }

        public void Dispose()
        {
            GetUnitOfWork().Dispose();
        }

        public object GetEmployeeWhoHaveSubmitAppraisal()
        {
            var empList = GetUnitOfWork().ObjectiveMainRepository.Get().Select(s => new
            {
                s.EmployeeId,
                s.Employee.EmployeeName,
                EmployeeCompany = s.Employee.groups,
                ReportToCompany = s.Employee.Employee2.groups,
                section = s.Employee.Section.Name,
                sectionID = s.Employee.SectionId,
                department = s.Employee.Section.Department.Name,
                departmentId = s.Employee.Section.DeparmentId,
                s.Employee.groups,
                isSubmitSelfAppraisal = s.OverallScore != null
            }).ToList();
            return empList;
        }
    }
}
