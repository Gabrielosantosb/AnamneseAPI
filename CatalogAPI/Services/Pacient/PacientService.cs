using CatalogAPI.Context;
using CatalogAPI.Models;
using CatalogAPI.Services.Token;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static CatalogAPI.Repository.Repository;

namespace CatalogAPI.Services.Pacient
{
    public class PacientService : IPacientService
    {
        private readonly MySQLContext _context;
        private readonly BaseRepository<PacientModel> _pacientRepository;        
        private ITokenService _tokenService { get; }
        private readonly DbSet<PacientModel> dataset;

        public PacientService(MySQLContext context, BaseRepository<PacientModel> pacientRepository, ITokenService tokenService)
        {
            _context = context;
            _pacientRepository = pacientRepository;
            _tokenService = tokenService;
            dataset = _context.Set<PacientModel>();
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
            //pacient.DoctorId = 5;
            pacient.DoctorId = _tokenService.GetUserId();
            dataset.Add(pacient);            
            _context.SaveChanges();                
            return pacient;
            //return _pacientRepository.Create(pacient);            
        }

        public PacientModel UpdatePacient(int id, PacientModel pacient)
        {
            return _pacientRepository.Update(pacient);
        }

        public PacientModel DeletePacient(int id)
        {
            var pacientToRemove = _context.Pacients.Find(id);

            if (pacientToRemove != null)
            {
                _context.Pacients.Remove(pacientToRemove);
                _context.SaveChanges();

                return pacientToRemove;
            }
            return null;

        }
    }
}
