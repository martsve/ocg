using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            ViewBag.Message = "Blessed vs. Cursed";

            var view = new GameViewModel()
            {
                CardViewModel = CardTemplate(),
            };

            return View(view);
        }

        public CardViewModel CardTemplate()
        {
            var template = new CardViewModel();
            template.Name = "[[Name]]";
            template.Artist = "[[Artist]]";
            template.BG = "[[BG]]";
            template.Border = "[[Border]]";
            template.Colors = "[[Colors]]";
            template.Cost = "[[Cost]]";
            template.Number = "[[Number]]";
            template.Power = "[[Power]]";
            
            template.Rarity = "[[Rarity]]";
            template.RarityCode = "[[RarityCode]]";
            template.SetCode = "[[SetCode]]";
            template.Text = "[[Text]]";
            template.Thoughness = "[[Thoughness]]";
            template.Type = "[[Type]]";
            return template;
        }
    }
}