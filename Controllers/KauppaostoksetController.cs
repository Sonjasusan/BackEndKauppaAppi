using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;
namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class KauppaostoksetController : ControllerBase
    {
        //private readonly kauppalistadbContext db = new kauppalistadbContext();

        //dependency injection
        private readonly kauppalistadbContext db;

        public KauppaostoksetController(kauppalistadbContext dbparam)
        {
            db = dbparam;
        }

        //Haetaan kaikki kauppaostokset taulussa olevat ostokset
        [HttpGet]
        public ActionResult GetKauppaostokset()
        {
            try
            {
                var kop = (from k in db.KauppaOstoksets where k.Active == true && k.Completed == false select k).ToList();

                return Ok(kop);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe: " + e);
            }
        }


        [HttpPost]
        [Route("")]
        public bool StartStop(KauppaOperation op) //Kauppaostosten tekemisen alotus/lopetus
        {
            if (op == null)
            {
                return (false);
            }

            //Haetaan kaikki ostokse, jotka on aktiivisia
            KauppaOstokset ostos = (from k in db.KauppaOstoksets
                                    where (k.Active == true) &&
                                    (k.IdKauppaOstos == op.KauppaOstosID)
                                    select k).FirstOrDefault();

            if (ostos == null)
            {
                return (false);
            }

            // Start
            if (op.OperationType == "start") //Jos OperationType on "start" - eli aloitus
            {

                if (ostos.Inprogress == true || ostos.Completed == true) //jos ostos on Inprogress -tilassa
                                                                         //ja ostos merkattu ostetuksi
                {
                    return (false); //palautetaan false
                }

                ostos.Inprogress = true; //ostos on InProgress tilassa
                //ostos.WorkStartedAt = DateTime.Now.AddHours(2);
                ostos.LastModifiedAt = DateTime.Now.AddHours(2); //tallennetaan viimeksi muokattu pvm

                db.SaveChanges(); //tallennetaan 

                Timesheet newEntry = new Timesheet() //Timesheet tauluun uusi lisäys (kauppaostosten aloitus)
                {
                    IdKauppaOstos = op.KauppaOstosID,
                    StartTime = DateTime.Now.AddHours(2),
                    Active = true,
                    IdKavija = op.KavijaID,
                    CreatedAt = DateTime.Now.AddHours(2),
                    Comments = op.Comment,
                    StartLongitude = op.Longitude,
                    StartLatitude = op.Latitude,
                };

                db.Timesheets.Add(newEntry); //lisätään

                db.SaveChanges(); //tallennetaan

                return true;
            }


            // Stop - toiminto -> Lopetetaan kauppaostosten tekeminen
            else
            {

                if (ostos.Inprogress == false || ostos.Completed == true) //jos ostos ei ole Inprogress -tilassa
                                                                          //Ja ostos on Completed -> Ostettu
                {
                    return (false);
                }

                ostos.Inprogress = false; //laitetaan Inprogress falseksi - koska ei olla enää ostamassa
                //ostos.CompletedAt = DateTime.Now.AddHours(2);
                ostos.Completed = true; //ostos completed:ksi - ostos on valmis
                ostos.LastModifiedAt = DateTime.Now; //tallenetaan viimesin muokkaus pvm
                db.SaveChanges(); //tallennetaan tiedot

                //tallenetaan tiedot Timesheet tauluun

                Timesheet ts = (from t in db.Timesheets
                                where (t.Active == true) &&
                                (t.IdKauppaOstos == op.KauppaOstosID)
                                select t).FirstOrDefault();

                //ts.StopTime = DateTime.Now.AddHours(2);
                ts.LastModifiedAt = DateTime.Now.AddHours(2);
                ts.Comments = op.Comment;
                ts.StopLongitude = op.Longitude;
                ts.StopLatitude = op.Latitude;
                db.SaveChanges();

                return (true);
            }
        }

        //Tähän kauppaostosten poisto ja muokkaus mahdollisuus

        //MUOKKAUS
        [HttpPut]
        [Route("{id}")]
        public ActionResult PutEdit(int id, [FromBody] KauppaostosData kauppaostos)
        {
            if (kauppaostos == null) //Jos kauppaostosta ei löydy - palauttaa nullin
            {
                return BadRequest("Tuotetta ei löydy"); //ilmoitetaan ettei tuotetta löydy
            }
            else
            {
                try
                {
                    var ostos = db.KauppaOstoksets.Find(id); //etsitään tuote id:llä

                    if (ostos != null) //Jos kauppaostoksia on (ei ole null)
                    {
                        //Muokataan -> Korvataan tiedot uusilla käyttäjän syöttämillä tiedoilla
                        ostos.Title = kauppaostos.Title;
                        ostos.Description = kauppaostos.Description;
                        ostos.Active = kauppaostos.Active;
                        ostos.CreatedAt = kauppaostos.CreatedAt;
                        ostos.Active = kauppaostos.Active;
                        ostos.Inprogress = kauppaostos.Inprogress;
                        ostos.Completed = kauppaostos.Completed;

                        db.SaveChanges(); //tallennetaan muutokset
                        //Ja ilmoitetaan onnistuneesta muokkauksesta 
                        return Ok("Muokattu onnistuneesti tuotetta: " + ostos.Title + " id:llä: "+ ostos.IdKauppaOstos);
                    }
                    else
                    {
                        //Jos tuotetta ei löydy
                        return NotFound("Muokattavaa tuotetta ei löytynyt.");
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
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            var ostos = db.KauppaOstoksets.Find(id); //etsitään id:llä
            if (ostos == null) //jos null - ostosta ei löydy
            {
                return NotFound("Poistettavaa tuotetta ei löytynyt");
            }

            else
            {
                try
                {
                    db.KauppaOstoksets.Remove(ostos); //poistetaan ostos
                    db.SaveChanges(); //tallennetaan
                    //Ja ilmoitetaan onnistunut poisto
                    return Ok("Poistettiin onnistuneesti tuote: " + ostos.Title + " kauppalistalta.");
                }
                catch (Exception e) //tapahtuu virhe
                {
                    //palautetaan virheen message
                    return BadRequest($"{e.Message}");
                }
            }
        }
    }
}
