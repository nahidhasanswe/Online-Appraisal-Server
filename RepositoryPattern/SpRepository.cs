using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Appraisal.BusinessLogicLayer;
using RepositoryPattern.Repository;

namespace RepositoryPattern
{
    public class SpRepository
    {
        public object GetEmployeeByReportToId(string id)
        {
            List<OrganoramPoco> oragList = new List<OrganoramPoco>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SP_GetEmployeeByReportToForOrganogram", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (SqlDataReader rdr = command.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        OrganoramPoco poco = new OrganoramPoco();
                        poco.EmployeeId = rdr["EmployeeId"].ToString();
                        poco.EmployeeName = rdr["EmployeeName"].ToString();
                        poco.ReportToId = rdr["reportTo"].ToString();
                        poco.ReportToName = rdr["ReportToName"].ToString();
                        poco.Designation = rdr["Designation"].ToString();
                        poco.GroupName = rdr["GroupName"].ToString();
                        oragList.Add(poco);
                    }
                }
            }
            connection.Close();
            using (AppraisalDbContext db = new AppraisalDbContext())
            {

                OrganoramPoco empPoco = db.Employee.Where(a => a.EmployeeId == id).Select(s => new OrganoramPoco()
                {
                    EmployeeId = s.EmployeeId,
                    EmployeeName = s.EmployeeName,
                    Designation = s.Designation.Name,
                    ReportToId = s.ReportTo,
                    ReportToName = s.Employee2.EmployeeName,
                    GroupName = s.groups
                }).FirstOrDefault();
                OrganoramPoco supPoco = db.Employee.Where(a => a.EmployeeId == empPoco.ReportToId).Select(s => new OrganoramPoco()
                {
                    EmployeeId = s.EmployeeId,
                    EmployeeName = s.EmployeeName,
                    Designation = s.Designation.Name,
                    ReportToId = s.ReportTo,
                    ReportToName = s.Employee2.EmployeeName,
                    GroupName = s.groups
                }).FirstOrDefault();

                oragList.Add(empPoco);
                oragList.Add(supPoco);

                return oragList;
            }
        }

        public object GetJobObjectiveDataForChart()
        {
            List<JobChart> oragList = new List<JobChart>();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("spJobDescriptionForChart", connection);
            command.CommandType = CommandType.StoredProcedure;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            using (SqlDataReader rdr = command.ExecuteReader())
            {
                if (rdr.HasRows)
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        JobChart poco = new JobChart();
                        poco.Deprtment = rdr["Department"].ToString();
                        poco.Submited = Convert.ToInt32(rdr["submited"]) ;
                        poco.Unsubmited = Convert.ToInt32(rdr["unsubmited"]);
                        oragList.Add(poco);
                    }
                }
            }
            connection.Close();
            return oragList;
        }
        
    }
}
