using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Entities;
using Tickets.Repository.Interfaces;

namespace Tickets.Repository
{
    public class TicketDAO : IDAO<Ticket>
    {
        public Task<Ticket> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> FindThuisWedstrijd(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
