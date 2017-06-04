using System;
using System.Collections.Generic;
using System.Linq;
using Appraisal.BusinessLogicLayer.Core;
using AppraisalSystem.Models;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Admin
{
    public class JobObjective
    {
        private readonly UnitOfWork _unitOfWork;

        public string CreatedBy { get; set; }

        public JobObjective(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string GetReportTo(string id)
        {
            return GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == id)?.Employee2.EmployeeName;
        }

        public void SaveObjective(ObjectiveSub sub)
        {
            EmailNotifier notifier = new EmailNotifier();
            InsertObjectiveSub(sub);
            GetUnitOfWork().Save();

            string email = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.Employee2?.Email;
            string sender = GetUnitOfWork().EmployeeRepository.Get().FirstOrDefault(a => a.EmployeeId == CreatedBy)?.EmployeeName;
            if (String.IsNullOrEmpty(email))
            notifier.Send("/#/othersObjectives?id=" + CreatedBy, "Dear sir,<br/> I have submited my job objective on " + DateTime.Now.Date + ".", email, sender);
        }

        public void SavePerformanceAppraisal(List<PerformanceAppraisalPoco> list)
        {
            var id = list.Select(s => s.ObjectiveId).FirstOrDefault();
            var mainId =
                GetUnitOfWork()
                    .ObjectiveSubRepository.Get()
                    .Where(a => a.Id == id)
                    .Select(s => s.ObjectiveMainId)
                    .FirstOrDefault();
            int totalScore = 0;
            foreach (var poco in list)
            {
                ObjectiveSub sub =
                    GetUnitOfWork().ObjectiveSubRepository.Get().FirstOrDefault(a => a.Id == poco.ObjectiveId);
                if (sub != null)
                {
                    sub.Weight = poco.Weight;
                    sub.PerfomenseAppraisal = Convert.ToDecimal(poco.PerformanceAppraisal);
                    sub.Score = Convert.ToInt32((poco.Weight / 5.00) * Convert.ToDouble(sub.PerfomenseAppraisal));
                    totalScore += sub.Score ?? 0;
                    sub.UpdatedDate = DateTime.Now;
                    sub.UpdatedBy = CreatedBy;
                    GetUnitOfWork().ObjectiveSubRepository.Update(sub);
                }
                else
                {
                    throw new Exception("We can't find objective with ID: " + poco.ObjectiveId);
                }
            }
            var main = GetUnitOfWork().ObjectiveMainRepository.GetById(mainId);
            if (main != null)
            {
                main.TotalScore = totalScore;
                GetUnitOfWork().ObjectiveMainRepository.Update(main);
            }
            GetUnitOfWork().Save();
        }

        private void InsertObjectiveMain(ObjectiveMain main)
        {
            if (main.Id == Guid.Empty)
            {
                main.CreatedBy = CreatedBy;
                main.CreatedDate = DateTime.Now;
                main.IsActive = true;
                main.Id = Guid.NewGuid();
                GetUnitOfWork().ObjectiveMainRepository.Insert(main);
            }
        }

        private void InsertObjectiveSub(ObjectiveSub sub)
        {
            var mainId =
                GetUnitOfWork()
                    .ObjectiveMainRepository.Get()
                    .Where(a => a.EmployeeId == CreatedBy && a.IsActive == true)
                    .Select(s => s.Id)
                    .FirstOrDefault();
            int weight = 0;
            if(mainId != Guid.Empty)
            weight =  GetUnitOfWork().ObjectiveSubRepository.Get().Where(a => a.ObjectiveMainId == mainId).Sum(s => s.Weight??0) + sub.Weight??0;

            if (sub.Id != null)
            {
                if (weight - sub.Weight > 100)
                {
                    throw new Exception("Weight should not be greater then 100");
                }
                ObjectiveSub ob = GetUnitOfWork().ObjectiveSubRepository.Get().FirstOrDefault(a => a.Id == sub.Id);
                if (ob != null)
                {
                    ob.KPI = sub.KPI;
                    ob.Note = sub.Note;
                    ob.Target = sub.Target;
                    ob.Weight = sub.Weight;
                    ob.Title = sub.Title;
                    ob.UpdatedBy = CreatedBy;
                    ob.UpdatedDate = DateTime.Now;
                }
                GetUnitOfWork().ObjectiveSubRepository.Update(ob);
            }
            else
            {
                if (weight > 100)
                {
                    throw new Exception("Weight should not be greater then 100");
                }
                if (mainId == Guid.Empty)
                {
                    ObjectiveMain main = new ObjectiveMain()
                    {
                        EmployeeId = CreatedBy
                    };
                    InsertObjectiveMain(main);
                    mainId = main.Id;
                }
                sub.CreatedBy = CreatedBy;
                sub.CreatedDate = DateTime.Now;
                sub.ObjectiveMainId = mainId;
                sub.IsObjectiveApproved = false;
                sub.Status = true;
                sub.Id = UniqueNumber.GenerateUniqueNumber();
                GetUnitOfWork().ObjectiveSubRepository.Insert(sub);
            }

        }

        public void InsertSeflAppraisalToMain(ObjectiveMain main)
        {
            var m = GetUnitOfWork().ObjectiveMainRepository.Get().FirstOrDefault(a => a.Id == main.Id);
            if (m != null)
            {
                m.UpdatedBy = CreatedBy;
                m.UpdatedDate = DateTime.Now;
                m.OverallScore = main.OverallScore;
                m.OverallComment = main.OverallComment;
                m.IsActive = true;
                GetUnitOfWork().ObjectiveMainRepository.Update(m);
                InsertSelfAppraisal(main.ObjectiveSub, main.Id);
            }
            else
            {
                throw new Exception("Objective is not available");
            }
            GetUnitOfWork().Save();
        }

        private void InsertSelfAppraisal(ICollection<ObjectiveSub> objectiveSub, Guid mainId)
        {
            foreach (ObjectiveSub sub in objectiveSub)
            {
                if (sub.Id != null || sub.Id != "")
                {
                    ObjectiveSub ob = GetUnitOfWork().ObjectiveSubRepository.Get().FirstOrDefault(a => a.Id == sub.Id);
                    if (ob != null)
                    {
                        ob.SelfAppraisal = sub.SelfAppraisal;
                        ob.Comments = sub.Comments;
                        ob.ObjectiveMainId = mainId;
                        ob.UpdatedBy = CreatedBy;
                        ob.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        throw new Exception("Objective details is not available");
                    }
                    GetUnitOfWork().ObjectiveSubRepository.Update(ob);
                }
            }
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
