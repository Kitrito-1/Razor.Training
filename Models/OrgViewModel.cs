﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Razor.Training.Models
{
    public enum Sex { male = 1, Female = 2 }


    public class Position
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }
    }

    public class Employee
    {
        public Guid Id { get; set; }

        [StringLength(10, ErrorMessage = "欄位 {0} 長度必須介於2與10字元之間", MinimumLength = 2)]
        [Required(ErrorMessage = "欄位 {0} 必須輸入")]
        [Display(Name = "姓名", Prompt = "請輸入姓名")]
        public String Name { get; set; }

        [Display(Name = "性別")]
        public Sex Sex { get; set; }

        [StringLength(10, ErrorMessage = "欄位 {0} 長度必須介於6與10字元之間", MinimumLength = 6)]
        [Required(ErrorMessage = "欄位 {0} 必須輸入")]
        [Display(Name = "員工編號", Prompt = "請輸入員工編號")]
        public String Empno { get; set; }

        [Required(ErrorMessage = "欄位 {0} 必須輸入")]
        [Display(Name = "到職日")]
        public DateTime EnterDate { get; set; }

        [EmailAddress(ErrorMessage = "欄位 {0} 格式不正確")]
        [Display(Name = "電子郵件", Prompt = "admin@doublegreen.com")]
        public String Email { get; set; }

        [Display(Name = "職位")]
        public Guid PositionId { get; set; }

        [Display(Name = "自傳", Prompt = "請輸入您生平的經歷")]
        public String Autobiography { get; set; }

        public (DateTime?, Guid?) Init { get; set; }
    }

    public class OrgViewModel
    {
        public OrgViewModel()
        {

        }

        public List<Employee> GetEmployeeData()
        {
            List<Employee> result = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(new SqlConnectionStringBuilder() { DataSource = "129.129.1.90", UserID = "smoothnetdev", Password = "smoothnetdev", InitialCatalog = "SmoothEnterprise33_Dev", }.ConnectionString))
            {
                using (SqlCommand dc = connection.CreateCommand())
                {
                    dc.CommandText = @"SELECT TOP 15 dgpersonal.namezhtw, dgemployee.id, dgemployee.empno, dgemployee.enterdate, dgemployee.email, dgpersonal.sex , dgemployee.positionid
                                       FROM dgemployee 
                                       INNER JOIN dgpersonal ON dgpersonal.id = dgemployee.personid 
                                       WHERE LEN(dgemployee.empno) = 6 AND dgemployee.empno LIKE '100%' 
                                       ORDER BY dgemployee.empno ";

                    dc.Connection.Open();
                    using (SqlDataReader reader = dc.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Employee() { Id = Guid.Parse(reader["id"].ToString()), Name = reader["namezhtw"].ToString(), Empno = reader["empno"].ToString(), EnterDate = DateTime.Parse(reader["enterdate"].ToString()), Email = reader["email"].ToString(), Sex = String.Compare(reader["sex"].ToString(), "M", true) == 0 ? Sex.male : Sex.Female, PositionId = !String.IsNullOrEmpty(reader["positionid"].ToString()) ? Guid.Parse(reader["positionid"].ToString()) : default(Guid) });
                        }
                    }

                    dc.Connection.Close();
                }
            }

            return result;
        }

        public Employee GetEmployeeData(Guid eid)
        {
            Employee result = null;

            using (SqlConnection connection = new SqlConnection(new SqlConnectionStringBuilder() { DataSource = "129.129.1.90", UserID = "smoothnetdev", Password = "smoothnetdev", InitialCatalog = "SmoothEnterprise33_Dev", }.ConnectionString))
            {
                using (SqlCommand dc = connection.CreateCommand())
                {
                    dc.CommandText = @"SELECT dgpersonal.namezhtw, dgemployee.id, dgemployee.empno, dgemployee.enterdate, dgemployee.email, dgpersonal.sex , dgemployee.positionid, dgpersonal.autobiography, dgpersonal.initdate, dgpersonal.inituid
                                       FROM dgemployee 
                                       INNER JOIN dgpersonal ON dgpersonal.id = dgemployee.personid 
                                       WHERE dgemployee.id = @eid ";

                    dc.Connection.Open();
                    dc.Parameters.Add("@eid", SqlDbType.UniqueIdentifier).Value = eid;

                    using (SqlDataReader reader = dc.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime? initdate = null;
                            Guid? inituid = null;

                            if (!String.IsNullOrEmpty(reader["initdate"].ToString()))
                                initdate = DateTime.Parse(reader["initdate"].ToString());

                            if (!String.IsNullOrEmpty(reader["inituid"].ToString()))
                                inituid = Guid.Parse(reader["inituid"].ToString());

                            result = new Employee()
                            {
                                Id = Guid.Parse(reader["id"].ToString()),
                                Name = reader["namezhtw"].ToString(),
                                Empno = reader["empno"].ToString(),
                                EnterDate = DateTime.Parse(reader["enterdate"].ToString()),
                                Email = reader["email"].ToString(),
                                Sex = String.Compare(reader["sex"].ToString(), "M", true) == 0 ? Sex.male : Sex.Female,
                                PositionId = !String.IsNullOrEmpty(reader["positionid"].ToString()) ? Guid.Parse(reader["positionid"].ToString()) : default(Guid),
                                Autobiography = reader["autobiography"].ToString(),
                                Init = (initdate, inituid)
                            };
                        }
                    }

                    dc.Connection.Close();
                }
            }

            return result;
        }

        public List<Position> GetPositionData()
        {
            var result = new List<Position>();

            using (SqlConnection connection = new SqlConnection(new SqlConnectionStringBuilder() { DataSource = "129.129.1.90", UserID = "smoothnetdev", Password = "smoothnetdev", InitialCatalog = "SmoothEnterprise33_Dev", }.ConnectionString))
            {
                using (SqlCommand dc = connection.CreateCommand())
                {
                    dc.CommandText = @"SELECT * FROM dgposition ";

                    dc.Connection.Open();
                    using (SqlDataReader reader = dc.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Position() { Id = Guid.Parse(reader["id"].ToString()), Code = reader["code"].ToString(), Name = reader["namezhtw"].ToString() });
                        }
                    }

                    dc.Connection.Close();
                }
            }

            return result;
        }

        public void UpdateEmployeeData(Guid eid)
        {
            using (SqlConnection connection = new SqlConnection(new SqlConnectionStringBuilder() { DataSource = "129.129.1.90", UserID = "smoothnetdev", Password = "smoothnetdev", InitialCatalog = "SmoothEnterprise33_Dev", }.ConnectionString))
            {
                using (SqlCommand dc = connection.CreateCommand())
                {
                    dc.CommandText = @"UPDATE dgpersonal SET namezhtw = namezhtw + '(更新)' FROM dgpersonal
                                       INNER JOIN dgemployee ON dgemployee.personid = dgpersonal.id
                                       WHERE dgemployee.id = @id ";

                    dc.Parameters.Clear();
                    dc.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = eid;

                    dc.Connection.Open();
                    dc.ExecuteNonQuery();
                    dc.Connection.Close();
                }
            }
        }

        public void UpdateEmployeeData(Employee emp)
        {
            if (emp.Id != Guid.Empty)
            {
                using (SqlConnection connection = new SqlConnection(new SqlConnectionStringBuilder() { DataSource = "129.129.1.90", UserID = "smoothnetdev", Password = "smoothnetdev", InitialCatalog = "SmoothEnterprise33_Dev", }.ConnectionString))
                {
                    using (SqlCommand dc = connection.CreateCommand())
                    {
                        String personcolumn = "";

                        if (!String.IsNullOrEmpty(emp.Name))
                        {
                            if (!String.IsNullOrEmpty(personcolumn))
                                personcolumn += ",";
                            personcolumn += "namezhtw=N'" + emp.Name + "'";
                        }

                        if (!String.IsNullOrEmpty(personcolumn))
                            personcolumn += ",";
                        personcolumn += "sex=N'" + (emp.Sex == Sex.male ? "M" : "F") + "'";

                        if (!String.IsNullOrEmpty(personcolumn))
                            personcolumn += ",";
                        personcolumn += "autobiography=N'" + emp.Autobiography + "'";

                        if (!String.IsNullOrEmpty(personcolumn))
                        {
                            dc.CommandText = @"UPDATE dgpersonal SET " + personcolumn + @" FROM dgpersonal
                                               INNER JOIN dgemployee ON dgemployee.personid = dgpersonal.id
                                               WHERE dgemployee.id = @id ";

                            dc.Parameters.Clear();
                            dc.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = emp.Id;

                            dc.Connection.Open();
                            dc.ExecuteNonQuery();
                            dc.Connection.Close();
                        }
                    }
                }
            }
        }
    }
}
