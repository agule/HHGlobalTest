using Microsoft.AspNetCore.Mvc;

namespace HHGlobalTest.Controllers
{
    public class JobController : ControllerBase
    {
        const decimal cSalesTax     = 0.07m;     // 7% sales tax if applicable
        const decimal cBaseMargin   = 0.11m;     // 11% margin
        const decimal cExtraMargin  = 0.05m;     // +5% extra margin if applicable

        public class PrintItemOutput
        {
            public string? Name { get; set; }
            public decimal Cost { get; set; }
        }
        public class PrintItem : PrintItemOutput
        {
            public bool IsTaxFree { get; set; }
        }

        public class JobRequest
        {
            public PrintItem[]? Items { get; set; }
            public bool HasExtraMargin { get; set; }
        }

        public class JobResponse
        {
            public PrintItemOutput[]? Items { get; set; }
            public decimal TotalCost { get; set; }
        }

        [HttpPost]
        [Route("calculateJobCost")]
        public IActionResult CalculateJobCost([FromBody] JobRequest request)
        {
            if (request == null || request.Items == null || request.Items.Length == 0) {
                return BadRequest("Invalid input format");
            }
            var response = CalculateTotalCost(request.Items, request.HasExtraMargin);
            return Ok(response);
        }

        private JobResponse CalculateTotalCost(PrintItem[] items, bool hasExtraMargin)
        {
            decimal baseMargin = cBaseMargin;
            decimal extraMargin = hasExtraMargin ? cExtraMargin : 0;
            decimal totalCost = 0;
            var response = new JobResponse{ Items = new PrintItemOutput[items.Length] };

            foreach (var item in items){
                decimal itemCost = item.Cost;
               
                if (!item.IsTaxFree) {
                    itemCost += item.Cost * cSalesTax;              // Apply sales tax if not tax-free
                }
                response.Items[items.ToList().IndexOf(item)] = new PrintItemOutput { Name = item.Name, Cost = itemCost };

                itemCost += item.Cost * (baseMargin + extraMargin); // Apply margin
                itemCost = Math.Round(itemCost * 100) / 100;        // Round to the nearest cent
                totalCost += itemCost;
            }
            
            totalCost = Math.Round(totalCost * 50) / 50;            // Round the total cost to the nearest even cent
            response.TotalCost = totalCost;
            return response;
        }
    }

    //public class JobController : Controller
    //{
    //    // GET: JobController
    //    public ActionResult Index()
    //    {
    //        return View();
    //    }

    //    // GET: JobController/Details/5
    //    public ActionResult Details(int id)
    //    {
    //        return View();
    //    }

    //    // GET: JobController/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: JobController/Create
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create(IFormCollection collection)
    //    {
    //        try
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }

    //    // GET: JobController/Edit/5
    //    public ActionResult Edit(int id)
    //    {
    //        return View();
    //    }

    //    // POST: JobController/Edit/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit(int id, IFormCollection collection)
    //    {
    //        try
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }

    //    // GET: JobController/Delete/5
    //    public ActionResult Delete(int id)
    //    {
    //        return View();
    //    }

    //    // POST: JobController/Delete/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Delete(int id, IFormCollection collection)
    //    {
    //        try
    //        {
    //            return RedirectToAction(nameof(Index));
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }
    //}
}
