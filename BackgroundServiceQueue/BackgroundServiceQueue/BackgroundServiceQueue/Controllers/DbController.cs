using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackgroundServiceQueue.Models;
using Microsoft.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackgroundServiceQueue.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DbController : Controller
    {

        Context dbContext = new Context();
        //SqlConnection con = new SqlConnection("Server = 10.41.150.78; initial catalog = BackgroundServiceTest; Persist Security Info=True;user id = sa; password=112233on!");

        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var result = dbContext.Workers.ToList();
            return Ok(result);
        }

        DateTime date = DateTime.Now;

        [HttpGet("GetDateIfNow")]
        public ActionResult GetDate()
        {
            var futureDateList = dbContext.Workers.Select(x => x.FutureDate == date);
            return Ok(futureDateList);
        }

        private readonly IBackgroundQueue<string> future_date_queue;
        private readonly IBackgroundQueue<string> message_queue;

        public DbController(IBackgroundQueue<string> future_date_queue,IBackgroundQueue<string> message_queue)
        {
            this.future_date_queue = future_date_queue;
            this.message_queue = message_queue;
        }

        [HttpPost]
        public async Task<IActionResult> AddQueue()
        {
            
            List<DateTime> futureDateList = dbContext.Workers.Where(x => x.FutureDate == DateTime.Now).Select(x => x.FutureDate).ToList();

            foreach (DateTime date in futureDateList)
            {
                var worker1 = dbContext.Workers.Where(x => x.FutureDate == DateTime.Now && x.IsActive == true).SingleOrDefault();
                if (worker1 != null)
                {

                    await future_date_queue.AddQueueFutureDate(Convert.ToString(date));
                    await message_queue.AddQueueMessage(worker1.Message);
                    worker1.RunDate = date;

                    if (worker1.Frequency == 1)
                    {
                        worker1.FutureDate = worker1.FutureDate;
                        //worker1.FutureDate = worker1.FutureDate.AddDays(Convert.ToDouble(worker1.Frequency));
                    }
                    else if (worker1.Frequency == 30)
                    {
                        worker1.FutureDate = worker1.FutureDate.AddMonths(worker1.Frequency);
                    }


                    dbContext.SaveChanges();
                }
                List<Worker> closestWorkerList = dbContext.Workers.Where(x => x.FutureDate >= DateTime.Now && x.FutureDate < DateTime.Now.AddMinutes(15)).ToList();
                closestWorkerList.Sort();

                while (closestWorkerList != null)
                {
                    Worker closestWorker = closestWorkerList.First();
                    //await Task.Delay(closestWorker.FutureDate.Subtract(DateTime.Now));

                    await future_date_queue.AddQueueFutureDate(Convert.ToString(date));
                    await message_queue.AddQueueMessage(closestWorker.Message);
                    closestWorker.RunDate = date;

                    if (closestWorker.Frequency == 1)
                    {
                        worker1.FutureDate = worker1.FutureDate;
                        //worker1.FutureDate = worker1.FutureDate.AddDays(Convert.ToDouble(worker1.Frequency));
                    }
                    else if (closestWorker.Frequency == 30)
                    {
                        closestWorker.FutureDate = closestWorker.FutureDate.AddMonths(closestWorker.Frequency);
                    }


                    dbContext.SaveChanges();
                }
            }

            return Ok();
        }





        /*
        [HttpPost]
        public async Task<IActionResult> AddQueue()
        {
            String dateTime = "2022-08-20 14:51:44.043"; // Futuredate in database


            List<DateTime> futureDateList = dbContext.Workers.Where(x=>x.FutureDate==Convert.ToDateTime(dateTime)).Select(x => x.FutureDate).ToList();

            DateTime dateNow = DateTime.Now;

            foreach (DateTime date in futureDateList)
            {
                var worker1 = dbContext.Workers.Where(x => x.FutureDate == Convert.ToDateTime(dateTime) && x.IsActive == true).SingleOrDefault();
                if (worker1 != null)
                {

                    await future_date_queue.AddQueueFutureDate(Convert.ToString(date));
                    await message_queue.AddQueueMessage(worker1.Message);
                    worker1.RunDate = date;

                    if (worker1.Frequency == 1)
                    {
                        worker1.FutureDate = worker1.FutureDate;
                        //worker1.FutureDate = worker1.FutureDate.AddDays(Convert.ToDouble(worker1.Frequency));
                    }
                    else if (worker1.Frequency == 30)
                    {
                        worker1.FutureDate = worker1.FutureDate.AddMonths(worker1.Frequency);
                    }


                    dbContext.SaveChanges();
                }
            }

            return Ok();
        }*/

    }
}



