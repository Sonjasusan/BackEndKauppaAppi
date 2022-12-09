using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;

namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaupassakavijatController : ControllerBase
    {
        private readonly kauppalistadbContext db = new kauppalistadbContext();

        [HttpGet]
        public List<Kaupassakavijat> GetAllActive()
        {
            var kaupassakavijat = db.Kaupassakavijats.Where(k => k.Active == true);//Haetaan kaikki aktiiviset kaupassakävijät

            return kaupassakavijat.ToList();
        }

        //Uuden kaupassakävijan lisäys
        [HttpPost]
        public ActionResult Post([FromBody] Kaupassakavijat kavija)
        {
            try
            {
                db.Kaupassakavijats.Add(kavija);
                db.SaveChanges();
                return Ok("Lisättiin kavija " + kavija.IdKavija);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut. Tässä lisätietoa: " + e);
            }
        }
        //Tähän kaupassakävijöiden poisto ja muokkaus mahdollisuus

    }
}
