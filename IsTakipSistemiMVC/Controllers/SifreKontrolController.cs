using IsTakipSistemiMVC.Filters;
using IsTakipSistemiMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IsTakipSistemiMVC.Controllers
{
    public class SifreKontrolController : Controller
    {
        IsTakipDBEntities2 entity=new IsTakipDBEntities2();
        // GET: SifreKontrol
        public ActionResult Index()
        {
            int personelId = Convert.ToInt32(Session["PersonelId"]);

            if (personelId == 0) return RedirectToAction("Index", "Login");

            var personel = (from p in entity.Personeller where p.personelId == personelId select p).FirstOrDefault();

            ViewBag.mesaj = null;
            ViewBag.yetkiTurId = null;
            ViewBag.sitil = null;

            return View(personel);
        }

        [HttpPost,ActFilter("Parola Değiştirildi")]

        public ActionResult Index(int personelId,string eskiParola,string yeniParola,string yeniParolaKontrol)
        {
            var personel=(from p in entity.Personeller where p.personelId==personelId select p).FirstOrDefault();

			if (eskiParola != personel.personelParola)
			{
				ViewBag.mesaj = "Eski parolanızı yanlış girdiniz";
				ViewBag.sitil = "alert alert-danger";
				return View(personel);
			}

			if (yeniParola != yeniParolaKontrol)
            {
                ViewBag.mesaj = "Yeni parola ve Yeni parola tekrarı eşleşmedi";
                ViewBag.sitil = "alert alert-danger";

                return View(personel);
            }

            if (yeniParola.Length < 3 || yeniParola.Length > 15)
            {
				ViewBag.mesaj = "Yeni parola en az 3 karakter en çok 15 karakter olabilir.";
				ViewBag.sitil = "alert alert-danger";

				return View(personel);
			}

           personel.personelParola = yeniParola;
            personel.yeniPersonel = false;
            entity.SaveChanges();

            TempData["bilgi"] = personel.personelKullaniciAd;

            ViewBag.mesaj = "Parolanız başarıyla değiştirildi. Anasayfaya yönlendiriliyorsunuz.";
            ViewBag.sitil = "alert alert-success";
            ViewBag.yetkiTurId = personel.personelYetkiTurId; 
          
            return View(personel);
        }
            }
}
