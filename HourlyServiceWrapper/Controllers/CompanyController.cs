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
    public class CompanyController : ApiController
    {
        HourlyDBEntities DB = new HourlyDBEntities();

        public CompanyController()
        {
            DB.Configuration.LazyLoadingEnabled = false;
            DB.Configuration.ProxyCreationEnabled = false;
            
        }

        public IEnumerable<Company> Get()
        {
            using (HourlyDBEntities entities = new HourlyDBEntities())
            {
                return entities.Companies.Include("Departments").ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (HourlyDBEntities entities = new HourlyDBEntities())
            {
                var entity = entities.Companies.Include("Departments").FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Company with ID " + id + " not found.");
                }
            }
        }
        public HttpResponseMessage Post([FromBody] Company company)
        {
            try
            {
                using (HourlyDBEntities entities = new HourlyDBEntities())
                {
                    entities.Companies.Add(company);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, company);
                    message.Headers.Location = new Uri(Request.RequestUri + company.ID.ToString());
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
                using(HourlyDBEntities entities = new HourlyDBEntities()){
                    var entity = entities.Companies.FirstOrDefault(e => e.ID == id);
                    if (entity != null)
                    {
                        entities.Companies.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Company with ID " + id.ToString() + " not found");
                    }
                }
                
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(int id, [FromBody] Company company)
        {
            try
            {
                using (HourlyDBEntities entities = new HourlyDBEntities())
                {
                    var entity = entities.Companies.FirstOrDefault(e => e.ID == id);
                    if(entity != null)
                    {
                        entity.name = company.name;
                        entity.description = company.description;
                        entity.isactive = company.isactive;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Company with ID " + entity.ID + " not found.");
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
