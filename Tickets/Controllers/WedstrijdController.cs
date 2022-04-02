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
            WedstrijdVM wedstrijdvm = new WedstrijdVM();

            wedstrijdvm.Club = new SelectList(await _clubService.GetAll(), "Stamnummer", "Clubnaam", wedstrijdvm.Club);

            var wedlist = new List<Wedstrijd>();

            foreach(var item in list)
            {
                wedlist.Add(item);
            }

            wedstrijdvm.WedstrijdenList = wedlist;
            
           
           
            return View(wedstrijdvm);
        }

        [HttpPost]

        public async Task<IActionResult> Index(WedstrijdVM wVM)
        {


            var sel = Convert.ToInt32(wVM.Thuisploeg);
            var wedList = new List<Wedstrijd>();
            var list = await _wedstrijdService.GetAll();
            wVM.Club = new SelectList(await _clubService.GetAll(), "Stamnummer", "Clubnaam", wVM.Club);


            foreach (var item in list)
            {
                if (item.Thuisploeg.Stamnummer == sel)
                {

                    wedList.Add(item);
                }
            }

            wVM.WedstrijdenList = wedList;

            return View(wVM);
        }
    }
}
