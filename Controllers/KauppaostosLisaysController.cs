using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;
using System.Linq;

namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KauppaostosLisaysController : ControllerBase
    {
        //private readonly kauppalistadbContext db = new kauppalistadbContext();

        //dependency injection
        private readonly kauppalistadbContext db;

        public KauppaostosLisaysController(kauppalistadbContext dbparam)
        {
            db = dbparam;
        }

        [HttpPost]
        public bool Lisaa(KauppaostosData kauppaostos) //Lisätään kauppaostos
        {
            try
            {

                KauppaOstokset kauppaostokset = new KauppaOstokset()
                {
                    IdKauppaOstos = kauppaostos.IdKauppaOstos,
                    Active = kauppaostos.Active,
                    Title = kauppaostos.Title,
                    Description = kauppaostos.Description,
                    Inprogress = kauppaostos.Inprogress,
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now
                };

                db.KauppaOstoksets.Add(kauppaostokset); //lisätään
                db.SaveChanges(); //tallennetaan
                return (true);

            }
            catch (Exception)
            {

                return (false);
            }
        }


    }
}
