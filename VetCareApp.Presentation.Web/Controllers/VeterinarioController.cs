using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VetCareApp.Presentation.Web.Controllers
{
    public class VeterinarioController : Controller
    {
        // GET: VeterinarioController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VeterinarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VeterinarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VeterinarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VeterinarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VeterinarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VeterinarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VeterinarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
