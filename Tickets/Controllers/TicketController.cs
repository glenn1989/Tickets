using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;
using Tickets.Domain.Entities;
using Tickets.Extensions;
using Tickets.Services.Interfaces;
using Tickets.ViewModel;

namespace Tickets.Controllers
{
    public class TicketController : Controller
    {
        
        private Iservices<Wedstrijd> _wedstrijdService;
        private Iservices<Vak> _vakService;
        private Iservices<VakStadion> _vakStadionService;
        private Iservices<Aankopen> _aankopenService;
        private Iservices<Ticket> _ticketService;
        private Iservices<Plaat> _plaatsService;
        private readonly IMapper _mapper;

        public TicketController(IMapper mapper,
            Iservices<Wedstrijd> wedstrijdservice, Iservices<Vak> vakservice,
            Iservices<VakStadion> vakstadionservice, Iservices<Aankopen> aankopenservice,
            Iservices<Ticket> ticketservice, Iservices<Plaat> plaatservice)
        {
            _mapper = mapper;
            _wedstrijdService = wedstrijdservice;
            _vakService = vakservice;
            _vakStadionService = vakstadionservice;
            _aankopenService = aankopenservice;
            _ticketService = ticketservice;
            _plaatsService = plaatservice;
            
        }

        private List<Plaat> GetPlaats(IEnumerable<Plaat> plaatsen, int? VakId, int? StadionId)
        {
            
            List<Plaat> p = new List<Plaat>();

            foreach (var i in plaatsen)
            {
                if (i.StadionId == StadionId && i.VakId == VakId)
                {
                    p.Add(i);
                }
            }

            return p;
        }

        private List<Plaat> GetId(IEnumerable<Plaat> numerable)
        {
            List <Plaat> p = new List<Plaat>();

            foreach(var i in numerable)
            {
                p.Add(i);
            }

            return p;
        }

        public async Task<IActionResult> Ticketselect(int id,int id2)
        {

            if(id == null)
            {
                return NotFound();
            }

            Wedstrijd wedstrijd = await _wedstrijdService.FindById(id,id2);

            var ticket = new TicketVM();

            ticket.wedstrijdID = wedstrijd.WedstrijdId;
            ticket.Thuisploeg = wedstrijd.Thuisploeg.Clubnaam;
            ticket.Uitploeg = wedstrijd.Uitploeg.Clubnaam;
            ticket.StadionId = wedstrijd.Thuisploeg.StadionId;
            
            ticket.Vak = new SelectList(await _vakService.GetAll(), "VakId", "VakNaam", ticket.Vak);

            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Select(TicketVM entityVM, int id,int id2)
        {
            if(id == null)
            {
                return NotFound();
            }

            Wedstrijd wedstrijd = await _wedstrijdService.FindById(id,id2);
            VakStadion vakStadion = await _vakStadionService.FindById(entityVM.VakId,id2);

            CartVM item = new CartVM
            {
                WedstrijdId = id,
                AantalTickets = entityVM.aantalTickets,
                Prijs = (float)vakStadion.Prijs,
                Aankoopdatum = DateTime.Now,
                Stadion = vakStadion.Stadion.StadionNaam,
                StadionId = vakStadion.StadionId,
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
        [HttpGet]
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
              .GetObject<ShoppingCartVM>("OrderCheck");

            CartVM? itemToRemove =
                cartList?.Cart?.FirstOrDefault(r => r.WedstrijdId == id);
            

            if (itemToRemove != null)
            {
                cartList?.Cart?.Remove(itemToRemove);
                HttpContext.Session.SetObject("OrderCheck", cartList);

            }

            return View("OrderCheck", cartList);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Validate(List<CartVM> cart)
        {
            string? userID = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            TicketOrderVM ticketorder = new TicketOrderVM();

            Aankopen aankoop;
            Ticket ticket;
            Plaat plaats;

            try
            {
                foreach (var item in cart)
                {
                    aankoop = new Aankopen();
                    plaats = new Plaat();
                    aankoop.ClientId = userID;
                    aankoop.Aankoopdatum = item.Aankoopdatum;
                    await _aankopenService.Add(aankoop);


                    var aantalTicket = item.AantalTickets;

                    VakStadion stadionvak = await _vakStadionService.FindById(item.VakId, item.StadionId);

                    IEnumerable<Plaat> plaatsen = await _plaatsService.GetAll();

                    var capaciteit = stadionvak.Capaciteit;
                    

                    var aantalBezet = GetPlaats(plaatsen, item.VakId, item.StadionId).Count();
                   

                    //int j = -1;

                    for (int i = 0; i < aantalTicket && aantalBezet <= capaciteit; i++)
                    {
                        if (aantalBezet == capaciteit)
                        {
                            TempData["status"] = "Vak is volzet.";
                            return RedirectToAction("OrderCheck", "Ticket");
                        }

                            
                            plaats.Plaatsnummer = ++aantalBezet;
                            plaats.StadionId = item.StadionId;
                            plaats.VakId = item.VakId;
                            await _plaatsService.Add(plaats);

                        }
                    }

                    //while(j <= aantalTicket && aantalBezet <= capaciteit)
                    //{
                        
                        
                    //    if (aantalBezet == capaciteit)
                    //    {
                    //        TempData["status"] = "Vak is volzet.";
                    //        return RedirectToAction("OrderCheck", "Ticket");
                    //    } 
                            
                    //        plaats.Plaatsnummer = (int?)++aantalBezet;
                    //        plaats.StadionId = item.StadionId;
                    //        plaats.VakId = item.VakId;
                    //        _plaatsService.Add(plaats);
                    //        ++j;

                    //}

                
            } 
            catch (DataException ex)
            {
                throw new DataException("error");
            } 
            catch (Exception ex)
            {
                throw new Exception("error");
            }
            
            return RedirectToAction("Index","Home");
        }
        
    }

}
