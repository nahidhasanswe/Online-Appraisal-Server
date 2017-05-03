using System;
using System.Linq;
using Appraisal.BusinessLogicLayer.Core;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.HBOU
{
    public class HboActivities : IDisposable
    {
        private UnitOfWork _unitOfWork;
        public string CreatedBy { get; set; }

        public HboActivities()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void ApproveJobDescriptionByHOBU(string employeeId)
        {
            var desc = GetUnitOfWork().JobDescriptionRepository.Get().FirstOrDefault(a => a.EmployeeId == employeeId);
            if (desc != null)
            {
                desc.IsHOBUConfirmed = true;
                desc.HOBUConfirmedDate = DateTime.Now;
                GetUnitOfWork().Save();
            }
            else
            {
                throw new Exception("This employee has not set his/her job description yet!");
            }
        }

        public void ApproveJobDescriptionByReportee(string employeeId)
        {
            var desc = GetUnitOfWork().JobDescriptionRepository.Get().FirstOrDefault(a => a.EmployeeId == employeeId);
            if (desc != null)
            {
                //EmailNotifier notifier = new EmailNotifier();
                desc.IsReportToConfirmed = true;
                desc.IsHOBUConfirmed = true;
                desc.HOBUConfirmedDate = DateTime.Now;
                GetUnitOfWork().Save();
                string email = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.Employee2?.Email;
                string sender = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.EmployeeName;
                //if(email != null)
                //notifier.Send("", "Dear employee,\n I have reviewed your job description on " + DateTime.Now.Date + ".", email, sender);
            }
            else
            {
                throw new Exception("This employee has not set his/her job description yet!");
            }
        }

        public void ApproveObjectiveByReportee(string id)
        {
            var desc = GetUnitOfWork().ObjectiveSubRepository.Get().FirstOrDefault(a => a.Id == id);
            if (desc != null)
            {
                //EmailNotifier notifier = new EmailNotifier();
                desc.IsObjectiveApproved = true;
                GetUnitOfWork().Save();
                //string email = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.Employee2?.Email;
                //string sender = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.EmployeeName;
                //notifier.Send("", "Dear employee,\n I have reviewed your job objective on " + DateTime.Now.Date + ".", email, sender);

            }
            else
            {
                throw new Exception("This employee has not set his/her job description yet!");
            }
        }

        public UnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
