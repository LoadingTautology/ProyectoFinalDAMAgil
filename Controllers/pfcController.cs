﻿using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalDAMAgil.Controllers
{
    public class pfcController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Gantt()
        {
            return View("~/Views/pfc/Gantt.cshtml");
        }
    }
}
