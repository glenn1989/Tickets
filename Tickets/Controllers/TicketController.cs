using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Tickets.Domain.Entities;
using Tickets.Extensions;
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
        public async Task<IActionResult> Select(TicketVM entityVM, int id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Wedstrijd wedstrijd = await _wedstrijdService.FindById(id);
            VakStadion vakStadion = await _vakStadionService.FindById(entityVM.VakId);

            CartVM item = new CartVM
            {
                WedstrijdId = id,
                AantalTickets = entityVM.aantalTickets,
                Prijs = (float)vakStadion.Prijs,
                Aankoopdatum = DateTime.Now,
                Stadion = vakStadion.Stadion.StadionNaam,
                Thuisploeg = wedstrijd.Thuisploeg.Clubnaam,
                Uitploeg = wedstrijd.Uitploeg.Clubnaam,
                VakId = vakStadion.VakId
                
            };


            ShoppingCartVM? shopping;

            if (HttpContext.Session.GetObject<ShoppingCartVM>("OrderCheck") != null)
            {
                shopping = HttpContext.Session.GetObject<ShoppingCartVM>("OrderCheck");
            }
            else
            {
                shopping = new ShoppingCartVM();
                shopping.Cart = new List<CartVM>();
            }
            shopping.Cart.Add(item);

            HttpContext.Session.SetObject("OrderCheck", shopping);


            return RedirectToAction("OrderCheck","Ticket");
        }

        public async Task<IActionResult> OrderCheck()
        {
            ShoppingCartVM? cartlist = HttpContext.Session.GetObject<ShoppingCartVM>("OrderCheck");

            return View(cartlist);
        }

        public IActionResult Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList
              = HttpContext.Session
              .GetObject<ShoppingCartVM>("ShoppingCart");

            CartVM? itemToRemove =
                cartList?.Cart?.FirstOrDefault(r => r.WedstrijdId == id);
            // db.bieren.FirstOrDefault (r => 

            if (itemToRemove != null)
            {
                cartList?.Cart?.Remove(itemToRemove);
                HttpContext.Session.SetObject("ShoppingCart", cartList);

            }

            return View("OrderCheck", cartList);

        }

        [Authorize]
        [HttpPost]

        public async Task<IActionResult> Validate(List<CartVM> carts)
        {
            string? userID = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            foreach(var item in carts)
            {
                
            }
            return View();
        }
        
    }
}
