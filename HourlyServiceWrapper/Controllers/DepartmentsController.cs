using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HourlyDataAccess;
using System.Data.Entity;

namespace HourlyServiceWrapper.Controllers
{
    public class DepartmentsController : ApiController
    {
        HourlyDBEntities DB = new HourlyDBEntities();
        public DepartmentsController()
        {
            DB.Configuration.LazyLoadingEnabled = false;
            DB.Configuration.ProxyCreationEnabled = false;

        }
        public IEnumerable<Department> Get()
        {
            using (HourlyDBEntities entities = new HourlyDBEntities())
            {
                IList<Department> departments = entities.Departments.ToList<Department>();
                return departments;
                //return entities.Departments;
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (HourlyDBEntities entities = new HourlyDBEntities())
            {
                var entity = entities.Departments.FirstOrDefault(e => e.ID == id);
                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department with ID " + id.ToString() + " not found.");
                }
            }
        }
        public HttpResponseMessage GetByCompanyID(int companyid)
        {
            using (HourlyDBEntities entities = new HourlyDBEntities())
            {
                var entity = entities.Departments.Where(e => e.ID == companyid);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department with Comapny ID " + companyid.ToString() + " not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody] Department department)
        {
            try
            {
                using(HourlyDBEntities entities = new HourlyDBEntities())
                {
                    entities.Departments.Add(department);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, department);
                    message.Headers.Location = new Uri(Request.RequestUri + department.ID.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using(HourlyDBEntities entities = new HourlyDBEntities())
                {
                    var entity = entities.Departments.FirstOrDefault(e => e.ID == id);
                    if(entity != null)
                    {
                        entities.Departments.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department with ID " + id.ToString() + " not found.");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(int id, [FromBody] Department department)
        {
            try
            {
                using(HourlyDBEntities entities = new HourlyDBEntities())
                {
                    var entity = entities.Departments.FirstOrDefault(e => e.ID == id);
                    if(entity != null)
                    {
                        entity.name = department.name;
                        entity.description = department.description;
                        entity.isactive = department.isactive;
                        entity.companyID = department.companyID;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Department with ID " + id.ToString() + " not found.");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
