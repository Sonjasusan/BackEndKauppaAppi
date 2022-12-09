using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RuokaAppiBackend.Models;
namespace RuokaAppiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class KauppaostoksetController : ControllerBase
    {
        private kauppalistadbContext db = new kauppalistadbContext();

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
        public bool StartStop(KauppaOperation op)
        {
            if (op == null)
            {
                return (false);
            }

            KauppaOstokset ostos = (from k in db.KauppaOstoksets
                                    where (k.Active == true) &&
                                    (k.IdKauppaOstos == op.KauppaOstosID)
                                    select k).FirstOrDefault();

            if (ostos == null)
            {
                return (false);
            }

            // Start
            if (op.OperationType == "start")
            {

                if (ostos.Inprogress == true || ostos.Completed == true)
                {
                    return (false);
                }

                ostos.Inprogress = true;
                //ostos.WorkStartedAt = DateTime.Now.AddHours(2);
                ostos.LastModifiedAt = DateTime.Now.AddHours(2);

                db.SaveChanges();

                Timesheet newEntry = new Timesheet()
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

                db.Timesheets.Add(newEntry);

                db.SaveChanges();

                return true;
            }


            // Stop
            else
            {

                if (ostos.Inprogress == false || ostos.Completed == true)
                {
                    return (false);
                }

                ostos.Inprogress = false;
                //ostos.CompletedAt = DateTime.Now.AddHours(2);
                ostos.Completed = true;
                ostos.LastModifiedAt = DateTime.Now;
                db.SaveChanges();

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
            if (kauppaostos == null)
            {
                return BadRequest("Tuotetta ei löydy");
            }
            else
            {
                try
                {
                    var ostos = db.KauppaOstoksets.Find(id);

                    if (ostos != null) //Jos kauppaostoksia on (ei ole null)
                    {
                        //Muokataan -> Korvataan tiedot uusilla tiedoilla
                        ostos.Title = kauppaostos.Title;
                        ostos.Description = kauppaostos.Description;
                        ostos.Active = kauppaostos.Active;
                        ostos.CreatedAt = kauppaostos.CreatedAt;
                        ostos.Active = kauppaostos.Active;
                        ostos.Inprogress = kauppaostos.Inprogress;
                        ostos.Completed = kauppaostos.Completed;

                        db.SaveChanges();
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
        public ActionResult Delete(int id)
        {
            var ostos = db.KauppaOstoksets.Find(id);
            if (ostos == null)
            {
                return NotFound("Poistettavaa tuotetta ei löytynyt");
            }

            else
            {
                try
                {
                    db.KauppaOstoksets.Remove(ostos);
                    db.SaveChanges();
                    return Ok("Poistettiin onnistuneesti tuote: " + ostos.Title + " kauppalistalta.");
                }
                catch (Exception e)
                {

                    return BadRequest($"{e.Message}");
                }
            }
        }

    }
}
