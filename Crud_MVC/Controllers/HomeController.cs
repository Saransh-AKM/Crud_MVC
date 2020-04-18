using Crud_MVC.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crud_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Student> stu = new List<Student>();
            DataTable dt = new DataTable();
            using (SqlConnection conn=new SqlConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Record",conn);
                SqlDataAdapter cd = new SqlDataAdapter(cmd);
                cd.Fill(dt);
                conn.Close();
            }
            for(int i=0;i<dt.Rows.Count;i++)
            {
                stu.Add(new Student()
                {
                    id = Convert.ToInt32(dt.Rows[i]["id"]),
                    Name=dt.Rows[i]["Name"].ToString(),
                    Age=Convert.ToInt32(dt.Rows[i]["Age"]),
                    Sex=dt.Rows[i]["Sex"].ToString()
                });
            }
                return View(stu);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student s)
        {
            if (ModelState.IsValid == true)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into Record Values(@Name,@Age,@Sex)", conn);
                    cmd.Parameters.AddWithValue("@Name", s.Name);
                    cmd.Parameters.AddWithValue("@Age", s.Age);
                    cmd.Parameters.AddWithValue("@Sex", s.Sex);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            Student st = new Student();
            DataTable dt = new DataTable();
            
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Record where id=@ID",conn);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.SelectCommand.Parameters.AddWithValue("@ID", id);
                ad.Fill(dt);
                conn.Close();   

            }
            if (dt.Rows.Count == 1)
            {
                st.id = Convert.ToInt32(dt.Rows[0][0].ToString());
                st.Name = dt.Rows[0][1].ToString();
                st.Age = Convert.ToInt32(dt.Rows[0][2].ToString());
                st.Sex = dt.Rows[0][3].ToString();
            }
            return View(st);
        }
        [HttpPost]
        public ActionResult Edit(Student s)
        {
            Student stu = new Student();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update Record Set Name=@Name,Age=@Age,Sex=@Sex where id=@ID",conn);
                cmd.Parameters.AddWithValue("@ID",s.id);
                cmd.Parameters.AddWithValue("@Name", s.Age);
                cmd.Parameters.AddWithValue("@Age", s.Age);
                cmd.Parameters.AddWithValue("@Sex", s.Sex);
              //  cmd.ExecuteNonQuery();
                conn.Close();

            }
                return View();
        }
        public ActionResult Delete(int id)
        {
            Student stu = new Student();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from Record where id=@ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("Index");
        }
    }
}