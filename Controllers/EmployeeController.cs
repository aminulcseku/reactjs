using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env; 

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, convert(varchar(10),
                             DateOfJoining,120) as dateofjoining, PhotoFileName
                            from dbo.employee";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
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
        public JsonResult Post(Employee dep)
        {
            string query = @"insert into dbo.employee
                            (EmployeeName, Department, DateOfJoining, PhotoFileName)
                            values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myCommad.Parameters.AddWithValue("@EmployeeName", dep.EmployeeName);
                    myCommad.Parameters.AddWithValue("@Department", dep.Department);
                    myCommad.Parameters.AddWithValue("@DateOfJoining", dep.DateOfJoining);
                    myCommad.Parameters.AddWithValue("@PhotoFileName", dep.PhotoFileName);
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }

            }
            return new JsonResult("Added Successfully");
        }





        [HttpPut]
        public JsonResult Update(Employee dep)
        {
            string query = @"update dbo.employee set 
                 EmployeeName = @EmployeeName,
                 Department = @Department, 
                 DateOfJoining = @DateOfJoining, 
                 PhotoFileName = @PhotoFileName
                  where EmployeeId = @EmployeeId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myCommad.Parameters.AddWithValue("@EmployeeId", dep.EmployeeId);
                    myCommad.Parameters.AddWithValue("@EmployeeName", dep.EmployeeName);
                    myCommad.Parameters.AddWithValue("@Department", dep.Department);
                    myCommad.Parameters.AddWithValue("@DateOfJoining", dep.DateOfJoining);
                    myCommad.Parameters.AddWithValue("@PhotoFileName", dep.PhotoFileName);
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }

            }
            return new JsonResult("Updated Successfully");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.employee where EmployeeId = @EmployeeId";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommad = new SqlCommand(query, myCon))
                {
                    myCommad.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = myCommad.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();

                }

            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                //var physicalPath = _env.ContentRootPath + "/Photos" + filename;
                var physicalPath = "G:/C#Practice/WebApplication1/" + "Photos/" + filename;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            { 
            
            }

            return new JsonResult("anonymousrerere.png");
        }

    }
}
