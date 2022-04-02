﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Domain.Entities;
using Tickets.Repository.Interfaces;
using Tickets.Services.Interfaces;

namespace Tickets.Services
{
    public class VakStadionServices : Iservices<VakStadion>
    {
        

        private IDAO<VakStadion> _VakStadionDAO;

        public VakStadionServices(IDAO<VakStadion> vakstadion)
        {
            _VakStadionDAO = vakstadion;
        }

        public async Task<VakStadion> FindById(int id)
        {
            return await _VakStadionDAO.FindById(id);
        }

        public Task<IEnumerable<VakStadion>> FindThuisWedstrijd(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VakStadion>> GetAll()
        {
            return await _VakStadionDAO.GetAll();
        }
    }
}
