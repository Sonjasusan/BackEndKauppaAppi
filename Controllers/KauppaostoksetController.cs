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
                return BadRequest("Error happened: " + e);
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

                if (ostos.InProgress == true || ostos.Completed == true)
                {
                    return (false);
                }

                ostos.InProgress = true;
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

                if (ostos.InProgress == false || ostos.Completed == true)
                {
                    return (false);
                }

                ostos.InProgress = false;
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


    }
}
