using System;
using System.Linq;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Core
{
    public class Validation : IDisposable
    {
        private readonly UnitOfWork _unitOfWork;

        public Validation(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool IsJobObjectiveDeadLineValid(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId)) return false;
            bool result =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(d => d.EmployeeId == employeeId && d.JobObjectiveDeadline >= DateTime.Today)
                    .OrderByDescending(a => a.JobObjectiveDeadline)
                    .Any();
            return result;
        }

        public bool IsSelfAppraisalDeadLineValid(string employeeId)
        {
            if (String.IsNullOrEmpty(employeeId)) return false;
            bool result =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(d => d.EmployeeId == employeeId && d.SelfAppraisalDeadline >= DateTime.Today)
                    .OrderByDescending(a => a.JobObjectiveDeadline)
                    .Any();
            return result;
        }

        public bool IsSelfAppraisalDeadLineNull(string employeeId)
        {
            if (String.IsNullOrEmpty(employeeId)) return false;
            bool result =
                GetUnitOfWork()
                    .EmployeeRepository.Get()
                    .Where(d => d.EmployeeId == employeeId && d.SelfAppraisalDeadline == null)
                    .OrderByDescending(a => a.JobObjectiveDeadline)
                    .Any();
            return result;
        }

        public bool HasSetJobDescription(string employeeId)
        {
            if (String.IsNullOrEmpty(employeeId)) return false;
            bool result =
                GetUnitOfWork()
                    .JobDescriptionRepository
                    .Get()
                    .Any(d => d.EmployeeId == employeeId);
            return result;
        }

        public bool IsValidToUpdateJobDescription(Guid jobDescriptionId)
        {
            if (jobDescriptionId == Guid.Empty) return false;
            bool result =
                GetUnitOfWork()
                    .JobDescriptionRepository
                    .Get()
                    .Where(a=>a.Id == jobDescriptionId)
                    .Select(s=>s.IsReportToConfirmed).FirstOrDefault()??false;
            return result;
        }

        public bool IsOSubApproved(string id)
        {
            bool app = GetUnitOfWork().ObjectiveSubRepository.Get()
                .Where(a => a.Id == id)
                .Select(s => s.IsObjectiveApproved??false).FirstOrDefault();
            return app;
        }

        public bool IsJobDescriptionConfirmed(string employeeId)
        {
            bool result = GetUnitOfWork().JobDescriptionRepository.Get().Any(a => a.IsReportToConfirmed == true && a.EmployeeId == employeeId);
            return result;
        }

        public ReportToInfo GetReportToByEmployeeId(string id)
        {
            var rId = GetUnitOfWork().EmployeeRepository.Get().Where(a => a.EmployeeId == id).Select(s => new ReportToInfo {
                ReportToId = s.ReportTo,
                ReportToEmail = s.Employee2.Email
            }).FirstOrDefault();
            return rId;
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
