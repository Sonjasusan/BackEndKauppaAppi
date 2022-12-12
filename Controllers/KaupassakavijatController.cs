using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;
using System.Text.Json.Serialization;



namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaupassakavijatController : ControllerBase
    {
        private readonly kauppalistadbContext db = new kauppalistadbContext();

        [HttpGet]
        public List<Kaupassakavijat> GetAllActive() //luodaan lista
        {
            var kaupassakavijat = db.Kaupassakavijats.Where(k => k.Active == true);//Haetaan kaikki aktiiviset kaupassakävijät

            return kaupassakavijat.ToList(); //palautetaan kaupassakävijät listana
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

        //MUOKKAUS
        [HttpPut]
        [Route("{id}")]
        public ActionResult PutEdit(int id, [FromBody] Kaupassakavijat kaupassakavijat)
        {
            if (kaupassakavijat == null)
            {
                return BadRequest("Kaupassakävijää ei löydy.");
            }

            else
            {
                try
                {
                    var kavija = db.Kaupassakavijats.Find(id); //etsitään kaupassakävijä id:llä
                    if (kavija != null) //jos kaupassakävijöitä on (ei ole null)
                    {
                        //Muokataan - korvataan tiedot uusilla käyttäjän antamilla tiedoilla
                        kavija.Nimi = kaupassakavijat.Nimi;
                        kavija.Active = kaupassakavijat.Active;
                        kavija.CreatedAt = kaupassakavijat.CreatedAt;

                        db.SaveChanges(); //tallennetaan
                        return Ok("Muokattiin onnistuneesti kaupassakävijää: " + kavija.Nimi + " id:llä: " + kavija.IdKavija);
                    }
                    else
                    {
                        //Jos kaupassakävijää ei löydy
                        return NotFound("Muokattavaa kaupassakävijää ei löytynyt.");
                    }
                }
                catch (Exception e)
                {
                    //Jokin menee pieleen
                    return BadRequest("Jokin meni pieleen." + e.Message);
                }
            }
        }
        //POISTO
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var kaupassakavija = db.Kaupassakavijats.Find(id);//etsitään id:llä

            if (kaupassakavija == null) //jos kaupassakävijää /kaupassakävijöitä ei löydy -> on null
            {
                return NotFound("Poistettavaa kaupassakävijää ei löytynyt!");
            }
            else
            {
                try
                {
                    db.Kaupassakavijats.Remove(kaupassakavija);
                    db.SaveChanges(); //tallenetaan
                    return Ok("Poistettiin onnistuneesti tuote: " + kaupassakavija.Nimi + " .");

                }
                catch (Exception e)
                {
                    //palautetaan virheen message
                    return BadRequest($"{e.Message}");
                }
            }
        }
    }
}
