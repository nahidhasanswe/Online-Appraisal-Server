using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryPattern;

namespace Appraisal.BusinessLogicLayer.Core
{
    public class CompanyDll : IDisposable
    {
        private UnitOfWork _unitOfWork;

        public CompanyDll()
        {
            _unitOfWork = new UnitOfWork();
        }

        public void Save(Company company)
        {
            var co = GetUnitOfWork().CompanyRepository.GetById(company.Id);
            if (co != null)
            {
                co.Id = company.Id;
                co.Name = company.Name;
                GetUnitOfWork().CompanyRepository.Update(co);
            }
            else
            {
                GetUnitOfWork().CompanyRepository.Insert(company);
            }
            GetUnitOfWork().Save();
        }

        public object Get()
        {
            return GetUnitOfWork().CompanyRepository.Get().Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();
        }
        public object GetById(string id)
        {
            return GetUnitOfWork().CompanyRepository.Get().Where(a=>a.Id == id).Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();
        }

        public void Delete(string id)
        {
            GetUnitOfWork().CompanyRepository.Delete(id);
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
