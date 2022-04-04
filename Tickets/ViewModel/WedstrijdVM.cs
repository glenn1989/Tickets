using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Tickets.Domain.Entities;

namespace Tickets.ViewModel
{
    public class WedstrijdVM
    {
        public string? Thuisploeg { get; set; }
        public string? Uitploeg { get; set; }

        [Display(Name = "Selecteer thuisploeg")]
        [Required(ErrorMessage = "Verplicht")]
        public IEnumerable<SelectListItem>? Club { get; set; }

        
        public List<Wedstrijd> WedstrijdenList { get; set; }
       

    }
}
