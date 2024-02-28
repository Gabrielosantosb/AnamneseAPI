using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using static CatalogAPI.Repository.Repository;

namespace CatalogAPI.Services.Pacient
{
    public class PacientService : IPacientService
    {
        private readonly MySQLContext _context;
        private readonly BaseRepository<PacientModel> _pacientRepository;

        public PacientService(MySQLContext context, BaseRepository<PacientModel> pacientRepository)
        {
            _context = context;
            _pacientRepository = pacientRepository;
        }

        public List<PacientModel> GetAllPacients()
        {
            return _pacientRepository.FindAll();
        }

        public PacientModel GetPacientById(int id)
        {
            return _pacientRepository.FindById(id);
        }

        public PacientModel CreatePacient(PacientModel pacient)
        {
            return _pacientRepository.Create(pacient);
        }

        public PacientModel UpdatePacient(int id, PacientModel pacient)
        {
            return _pacientRepository.Update(pacient);
        }

        public PacientModel DeletePacient(int id)
        {
            return _pacientRepository.Delete(id);
        }
    }
}
