﻿using AIRCOM.Models;
using AIRCOM.Models.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AIRCOM.Services
{
    public class InstallationService
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        public InstallationService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InstallationDTO>> Get(string userId)
        {
            var installation = await _context.Installations.Where(i => i.AirportID == int.Parse(userId)).ToListAsync();
            return _mapper.Map<List<InstallationDTO>>(installation);
        }

        public async Task<SelectList> Select(string userId)
        {
            var installations = await Get(userId);
            return new SelectList(installations, "ID", "Name");
        }

        public async Task Create(InstallationDTO installation, string userId)
        {
            await Errors(installation, userId);

            var installationDB = _mapper.Map<Installation>(installation);

            installationDB.AirportID = int.Parse(userId);

            _context.Installations.Add(installationDB);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(InstallationDTO installation)
        {
            await Errors(installation, installation.AirportID.ToString());

            var installationDB = await GetInstallationById(installation.ID);

            installationDB.Name = installation.Name;
            installationDB.Ubication = installation.Ubication;
            installationDB.InstallationID = installation.InstallationID;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var installationDB = await GetInstallationById(id);

            _context.Installations.Remove(installationDB);
            await _context.SaveChangesAsync();
        }

        //--------------------------------------------------------------------
        public async Task<Installation> GetInstallationById(int? id)
        {
            var installationDB = await _context.Installations.SingleOrDefaultAsync(i => i.ID == id);

            if (installationDB is null)
                throw new ArgumentNullException("La instalación no existe");

            return installationDB;
        }

        private async Task Errors(InstallationDTO installation, string userId)
        {
            var installationDB = await _context.Installations.SingleOrDefaultAsync(i =>
            i.InstallationID == installation.InstallationID && i.AirportID == int.Parse(userId));

            if (installationDB is not null)
                throw new Exception("Este número de instalación ya existe en el aeropuerto");

            installationDB = await _context.Installations.SingleOrDefaultAsync(i =>
            i.Name == installation.Name && i.AirportID == int.Parse(userId));

            if (installationDB is not null)
                throw new Exception("Este nombre de instalación ya existe en el aeropuerto");
        }
    }
}
