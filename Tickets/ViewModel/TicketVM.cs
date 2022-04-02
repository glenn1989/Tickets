using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Tickets.ViewModel
{
    public class TicketVM
    {
        public int wedstrijdID { get; set; }
        public string? Thuisploeg { get; set; }
        public string? Uitploeg { get; set; }

        [Range(0,4,ErrorMessage ="Maximum 4 kaarten per persoon.")]
        public int aantalTickets { get; set; }
        public IEnumerable<SelectListItem> Vak { get; set; }

        public double Prijs { get; set; }

    }
}
