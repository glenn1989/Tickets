using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tickets.Domain.Entities;
using Tickets.Services.Interfaces;
using Tickets.ViewModel;

namespace Tickets.Controllers
{
    public class WedstrijdController : Controller
    {

        private Iservices<Club> _clubService;
        private Iservices<Wedstrijd> _wedstrijdService;
        private readonly IMapper _mapper;

        public WedstrijdController(IMapper mapper, Iservices<Club> clubservice,
            Iservices<Wedstrijd> wedstrijdservice)
        {
            _mapper = mapper;
            _clubService = clubservice;
            _wedstrijdService = wedstrijdservice;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var list = await _wedstrijdService.GetAll();
            List<WedstrijdlistVM> wedstrijdlistVMs = _mapper.Map<List<WedstrijdlistVM>>(list);
            WedstrijdVM wedstrijdvm = new WedstrijdVM();
            wedstrijdvm.Club = new SelectList(await _clubService.GetAll(), "Stamnummer", "Clubnaam", wedstrijdvm.Club);
            wedstrijdvm.wedstrijdlistVMs = wedstrijdlistVMs;
            return View(wedstrijdvm);
        }

        [HttpPost]

        public async Task<IActionResult> Index(WedstrijdVM wVM)
        {

            var sel = Convert.ToInt32(wVM.Thuisploeg);
            var list = await _wedstrijdService.FindThuisWedstrijd(sel);
            List<WedstrijdlistVM> wedstrijdlistVMs = _mapper.Map<List<WedstrijdlistVM>>(list);
            wVM.wedstrijdlistVMs = wedstrijdlistVMs;
            wVM.Club = new SelectList(await _clubService.GetAll(), "Stamnummer", "Clubnaam", wVM.Club);

            return View(wVM);
        }
    }
}
