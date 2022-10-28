using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, DepartmentName from dbo.Department";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using ( SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                
                }
            
            }
            return new JsonResult(table);
        }



        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"insert into dbo.Department values (@DepartmentName)";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myCommad.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }

            }
            return new JsonResult("Added Successfully");
        }





        [HttpPut]
        public JsonResult Update(Department dep)
        {
            string query = @"update dbo.Department set DepartmentName = @DepartmentName
                            where DepartmentId = @DepartmentId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myCommad.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommad.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }

            }
            return new JsonResult("Updated Successfully");
        }


        [HttpDelete ("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.Department where DepartmentId = @DepartmentId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myCommad.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }

            }
            return new JsonResult("Deleted Successfully");
        }





    }
}
