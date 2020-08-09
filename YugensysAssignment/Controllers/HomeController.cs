using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YugensysAssignment.Implementation;
using YugensysAssignment.Interface;
using YugensysAssignment.Models;

namespace YugensysAssignment.Controllers
{

    public class HomeController : Controller
    {
        private readonly IHomeService _iHomeService = null; 
        public HomeController()
        {
            _iHomeService = new HomeService();
        }
        public ActionResult Index()
        {
            fruitdetailModel model = new fruitdetailModel();
            model.selectList = _iHomeService.GetSelectListItems("");
            List<SelectListItem> items = new List<SelectListItem>();
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveDetail(fruitdetailModel model)
        {
            _iHomeService.SaveDetail(model);
            return RedirectToAction("OnSuccess", "Home", new { sProperty = model.sProperty, fruitId = model.fruitId});
        }

        public ActionResult OnSuccess(String sProperty, int fruitId)
        {
            ViewBag.sProperty = sProperty;
            ViewBag.fruitName = _iHomeService.GetSelectListItems("").Find(x => x.Value == fruitId.ToString()).Text;
            return View();
        }

        public PartialViewResult _dropdownData(String sProperty)
        {
            List<SelectListItem> selectList = _iHomeService.GetSelectListItems(sProperty);
            return PartialView("_dropdownData", selectList);
        }
    }
}