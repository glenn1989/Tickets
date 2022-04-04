using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tickets.Domain.Entities;
using Tickets.Services.Interfaces;
using Tickets.ViewModel;

namespace Tickets.Controllers
{
    public class TicketController : Controller
    {
        private Iservices<Club> _clubService;
        private Iservices<Wedstrijd> _wedstrijdService;
        private Iservices<Vak> _vakService;
        private Iservices<VakStadion> _vakStadionService;
        private readonly IMapper _mapper;

        public TicketController(IMapper mapper, Iservices<Club> clubservice,
            Iservices<Wedstrijd> wedstrijdservice, Iservices<Vak> vakservice, Iservices<VakStadion> vakstadionservice)
        {
            _mapper = mapper;
            _clubService = clubservice;
            _wedstrijdService = wedstrijdservice;
            _vakService = vakservice;
            _vakStadionService = vakstadionservice;
        }

        public async Task<IActionResult> Ticketselect(int id)
        {

            if(id == null)
            {
                return NotFound();
            }

            Wedstrijd wedstrijd = await _wedstrijdService.FindById(id);

            var ticket = new TicketVM();

            ticket.wedstrijdID = wedstrijd.WedstrijdId;
            ticket.Thuisploeg = wedstrijd.Thuisploeg.Clubnaam;
            ticket.Uitploeg = wedstrijd.Uitploeg.Clubnaam;
            ticket.Vak = new SelectList(await _vakService.GetAll(), "VakId", "VakNaam", ticket.Vak);

            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> OrderCheck(TicketVM entityVM, int id)
        {

            return View();
        }
        
    }
}
