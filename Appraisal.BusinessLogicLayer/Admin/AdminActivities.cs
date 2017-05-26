using System;
using System.Collections.Generic;
using System.Linq;
using AppraisalSystem.Models;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Admin
{
    public class AdminActivities : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;
        public string CreatedBy { get; set; }

        public AdminActivities(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void SaveFiscalYear(FiscalYear fiscalYear)
        {
            if (fiscalYear.Id != null)
            {
                fiscalYear.UpdatedBy = CreatedBy;
                fiscalYear.UpdatedDate = DateTime.Now;
                GetUnitOfWork().FiscalYearRepository.Update(fiscalYear);
            }
            else
            {
                fiscalYear.CreatedBy = CreatedBy;
                fiscalYear.CreatedDate = DateTime.Now;
                fiscalYear.Id = GetFiscalYearId(fiscalYear);
                GetUnitOfWork().FiscalYearRepository.Insert(fiscalYear);
            }
            GetUnitOfWork().Save();
        }

        public void MakeABudget(DirectorActivities activities)
        {
            if (activities.Id != Guid.Empty)
            {
                activities.UpdatedBy = CreatedBy;
                activities.UpdatedDate = DateTime.Now;
                GetUnitOfWork().DirectorActivitiesRepository.Update(activities);
            }
            else
            {
                activities.CreatedBy = CreatedBy;
                activities.CreatedDate = DateTime.Now;
                activities.Id = Guid.NewGuid();
                GetUnitOfWork().DirectorActivitiesRepository.Insert(activities);
            }
            GetUnitOfWork().Save();
        }

        private string GetFiscalYearId(FiscalYear fiscalYear)
        {

            MakeDisableLastFiscalYear();

            string sMonth = fiscalYear.FiscalYearStart.Value.ToString("MMM").ToUpper();
            string eMonth = fiscalYear.FiscalYearEnd.Value.ToString("MMM").ToUpper();
            string sYear = fiscalYear.FiscalYearStart.Value.ToString("yyyy");
            string eYear = fiscalYear.FiscalYearEnd.Value.ToString("yyyy");
            return sMonth + sYear + "-" + eMonth + eYear;
        }

        private void MakeDisableLastFiscalYear()
        {
            bool act = DeativatePreviousObjective();
            if (act)
            {
                var fisc = GetUnitOfWork().FiscalYearRepository.Get().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
                if (fisc != null)
                {
                    fisc.IsActive = false;
                    GetUnitOfWork().FiscalYearRepository.Update(fisc);
                }
            }
            else
            {
                throw new Exception("Employee's previous objective deactivation problem!");
            }
        }

        private bool DeativatePreviousObjective()
        {
            var objs = GetUnitOfWork().ObjectiveMainRepository.Get().ToList();
            foreach (var emp in objs)
            {
                try
                {
                    emp.IsActive = false;
                    emp.UpdatedBy = CreatedBy;
                    emp.UpdatedDate = DateTime.Now;
                    GetUnitOfWork().ObjectiveMainRepository.Update(emp);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public void SetSeflfAppraisalDeadline(DepartmentConfig config)
        {
            var res = GetUnitOfWork().DepartmentConfigRepository.Get().Any(a => a.DepartmentId == config.DepartmentId);
            if (res) throw new Exception("This Department already set it's deadline.");
            if (config.Id != Guid.Empty)
            {
                config.UpdatedBy = CreatedBy;
                config.UpdatedDate = DateTime.Now;
                GetUnitOfWork().DepartmentConfigRepository.Update(config);
            }
            else
            {
                config.CreatedBy = CreatedBy;
                config.CreatedDate = DateTime.Now;
                config.Id = Guid.NewGuid();
                GetUnitOfWork().DepartmentConfigRepository.Insert(config);
            }
            UpdateEmployeeForAppraisal(config);
            GetUnitOfWork().Save();
        }

        private void UpdateEmployeeForObjective(DepartmentConfig config)
        {
            var employees =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.Section.DeparmentId == config.DepartmentId)
                    .ToList();
            foreach (var employee in employees)
            {
                employee.JobObjectiveDeadline = config.JobObjectiveDeadline;
                employee.UpdatedBy = CreatedBy;
                employee.UpdatedDate = DateTime.Now;
                GetUnitOfWork().EmployeeRepository.Update(employee);
            }
        }

        public void SetAppraisalDeadline(DepartmentConfig configs)
        {
            DepartmentConfig config = GetUnitOfWork().DepartmentConfigRepository.Get()
                .Where(d => d.DepartmentId == configs.DepartmentId)
                .OrderByDescending(a => a.CreatedDate)
                .FirstOrDefault();

            if (config != null)
            {
                config.UpdatedBy = CreatedBy;
                config.UpdatedDate = DateTime.Now;
                config.JobObjectiveDeadline = configs.JobObjectiveDeadline;
                GetUnitOfWork().DepartmentConfigRepository.Update(config);
                UpdateEmployeeForObjective(config);
            }
            else
            {
                throw new Exception("Job objective deadline is not set yet");
            }
            GetUnitOfWork().Save();
        }
        public void UpdateAppraisalDeadline(DepartmentConfig configs)
        {
            DepartmentConfig config = GetUnitOfWork().DepartmentConfigRepository.Get()
                .Where(d => d.DepartmentId == configs.DepartmentId)
                .OrderByDescending(a => a.CreatedDate)
                .FirstOrDefault();

            if (config != null)
            {
                config.UpdatedBy = CreatedBy;
                config.UpdatedDate = DateTime.Now;
                config.JobObjectiveDeadline = configs.JobObjectiveDeadline ?? config.JobObjectiveDeadline;
                config.SelfAppraisalDeadline = configs.SelfAppraisalDeadline ?? config.SelfAppraisalDeadline;
                GetUnitOfWork().DepartmentConfigRepository.Update(config);
                UpdateEmployeeForAppraisal(config);
            }
            else
            {
                throw new Exception("Job objective is not set yet");
            }
            GetUnitOfWork().Save();
        }
        private void UpdateEmployeeForAppraisal(DepartmentConfig config)
        {
            var employees =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(a => a.Section.DeparmentId == config.DepartmentId)
                    .ToList();
            foreach (var employee in employees)
            {
                employee.SelfAppraisalDeadline = config.SelfAppraisalDeadline;
                employee.JobObjectiveDeadline = config.JobObjectiveDeadline;
                employee.UpdatedBy = CreatedBy;
                employee.UpdatedDate = DateTime.Now;
                GetUnitOfWork().EmployeeRepository.Update(employee);
            }
        }

        public void UpdateIncreamentTableData(Increament increament)
        {
            increament.UpdatedBy = CreatedBy;
            increament.UpdatedDate = DateTime.Now;
            GetUnitOfWork().IncreamentRepository.Update(increament);
            GetUnitOfWork().Save();
        }

        public UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }

        public void Dispose()
        {
            GetUnitOfWork().Dispose();
        }

        public void DeleteEmployee(string id)
        {
            var emp = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == id);
            if (emp != null)
            {
                if (emp.IsActive == false)
                {
                    throw new Exception("The employee already deleted!");
                }
                emp.IsActive = false;
                GetUnitOfWork().EmployeeRepository.Update(emp);
                GetUnitOfWork().Save();
            }
        }

        public void ActiveEmployee(string id)
        {
            var emp = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == id);
            if (emp != null)
            {
                if (emp.IsActive == true)
                {
                    throw new Exception("The employee already active!");
                }
                emp.IsActive = true;
                GetUnitOfWork().EmployeeRepository.Update(emp);
                GetUnitOfWork().Save();
            }
        }
    }
}
