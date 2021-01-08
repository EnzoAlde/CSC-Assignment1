using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Task2.Models
{
    public class ProductsV2Controller : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        //[Authorize] Start of GET method for getting all products
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/v3/products")]
        public IEnumerable<Product> GetAllProductsFromRepository()
        {
            return repository.GetAll();

        }

        //Start of GET method for specific product
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/v3/products/{id}", Name = "getProductByIdv3")]

        public Product retrieveProductfromRepository(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        //Start of GET method for Categories
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/v3/products", Name = "getProductByCategoryv3")]
        //http://localhost:44333/v3/products?category=
        //http://localhost:44333/v3/products?category=Groceries

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        //Start of POST method for creating new products
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/v3/products")]
        public HttpResponseMessage PostProduct(Product item)
        {
            if (ModelState.IsValid)
            {
                item = repository.Add(item);
                var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

                // Generate a link to the new product and set the Location header in the response.

                string uri = Url.Link("getProductByIdv3", new { id = item.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }


        //Start of PUT method for updating product
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/v3/products/{id:int}")]
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        //Start of DELETE method for removing product
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/v3/products/{id:int}")]
        public void DeleteProduct(int id)
        {
            repository.Remove(id);
        }
    }

}