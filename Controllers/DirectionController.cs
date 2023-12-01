﻿using AIRCOM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIRCOM.Controllers
{
    public class DirectionController : Controller
    {
        private readonly InstallationService _aux;
        private readonly RepairService _aux2;
        private readonly ServiceInstallationService _aux3;
        private readonly RepairInstallationService _aux4;
        public DirectionController(InstallationService aux, RepairService aux2, ServiceInstallationService aux3, RepairInstallationService aux4)
        {
            _aux = aux;
            _aux2 = aux2;
            _aux3 = aux3;
            _aux4 = aux4;
        }
        public async Task<IActionResult> Index(int page = 3, int lugar_del_error = 0, string error = "")
        {
               
            ViewData["page"] = page;
            ViewData["lugar_del_error"] = lugar_del_error;
            ViewData["error"] = error;

            if (page == 1)
            {
                var installations = await _aux.Get("1");
                ViewData["installations"] = installations;
            }

            if (page == 2)
            {
                var installations = await _aux.Get("1");
                ViewData["installations"] = new SelectList(installations, "ID", "Name");
                var services = await _aux3.Get("1");
                ViewData["services"] = new SelectList(services, "Code", "Name");
            }

            if (page == 3)
            {
                var installations = await _aux.Get("1");
                ViewData["installations"] = new SelectList(installations, "ID", "Name");
                var repairs = await _aux2.Get();
                ViewData["rep"] = new SelectList(repairs, "RepairID", "Name");
                var ri = await _aux4.Get("1");
                ViewData["repairs"] = ri;
            }

            return View("Direction");
            
        }
    }
}