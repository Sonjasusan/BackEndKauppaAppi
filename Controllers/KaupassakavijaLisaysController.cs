using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;
namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaupassakavijaLisaysController : ControllerBase
    {
        private readonly kauppalistadbContext db = new kauppalistadbContext();

        [HttpPost]
        public bool Lisaa(Kaupassakavijat kavija)
        {
            try
            {

                Kaupassakavijat kaupassakavija = new Kaupassakavijat()
                {
                    IdKavija = kavija.IdKavija,
                    Nimi = kavija.Nimi,
                    Active = kavija.Active,
                    CreatedAt = DateTime.Now,
                };

                db.Kaupassakavijats.Add(kavija);
                db.SaveChanges();
                return (true);

            }
            catch (Exception)
            {

                return (false);
                
            }
        }
    }
}
