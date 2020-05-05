using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductDataAccess;

namespace ProductDemoService.Controllers
{
    public class ProductController : ApiController
    {
        public IEnumerable<Product> Get()
        {
            using (ProductDemoServiceEntities1 entities = new ProductDemoServiceEntities1())
            {
                return entities.Products.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (ProductDemoServiceEntities1 entities = new ProductDemoServiceEntities1())
            {
                var entity = entities.Products.FirstOrDefault(e => e.Id == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with ID" + id.ToString() + " Not found.");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Product product)
        {
            try
            {
                using (ProductDemoServiceEntities1 entities = new ProductDemoServiceEntities1())
                {
                    entities.Products.Add(product);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, product);
                    message.Headers.Location = new Uri(Request.RequestUri + product.Id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
               return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ProductDemoServiceEntities1 entities = new ProductDemoServiceEntities1())
                {
                    var entity = entities.Products.Remove(entities.Products.FirstOrDefault(e => e.Id == id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with ID" + id.ToString() + " Not found.");
                    }
                    else
                    {
                        entities.Products.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] Product product)
        {
            try
            {
                using (ProductDemoServiceEntities1 entities = new ProductDemoServiceEntities1())
                {
                    var entity = entities.Products.Remove(entities.Products.FirstOrDefault(e => e.Id == id));

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with ID" + id.ToString() + " Not found to update.");
                    }
                    else
                    {
                        entity.CategoryId = product.CategoryId;
                        entity.Name = product.Name;
                        entity.Description = product.Description;
                        entity.LastUpdatedBy = product.LastUpdatedBy;
                        entity.LastUpdatedDate = product.LastUpdatedDate;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
