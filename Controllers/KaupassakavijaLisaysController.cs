using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;
namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaupassakavijaLisaysController : ControllerBase //Toteutetaan lisäys omassa kontrollerissa
                                                                 //Koska jostain syystä sei onnistunut kaupassakävijät puolella
    {
        private readonly kauppalistadbContext db = new kauppalistadbContext();


        [HttpPost] //Kaupassakävijän lisäys 
        public bool Lisaa(Kaupassakavijat kavija)
        {
            try
            {
                //Uuden kaupassakävijän lisäys
                Kaupassakavijat kaupassakavija = new Kaupassakavijat()
                {
                    IdKavija = kavija.IdKavija,
                    Nimi = kavija.Nimi,
                    Active = kavija.Active,
                    CreatedAt = DateTime.Now,
                };

                db.Kaupassakavijats.Add(kavija); //lisätään tiedot
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
