using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Task_4.Models;

namespace Task_4.Controllers
{
    public class TalentsController : ApiController
    {
        //Attempt to use Web API Dependency (could not understand)

        static readonly TalentRepository repository = new TalentRepository();
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/talents")]
        public IEnumerable<Talent> GetAllTalents()
        {
            return repository.GetAll();
        }

        //Security to make sure user uses https instead of http (s stands for secured i think)

        [Route("api/talents/{id:int}")]
        public Talent GetTalent(int id)
        {
            Talent item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

    }
}