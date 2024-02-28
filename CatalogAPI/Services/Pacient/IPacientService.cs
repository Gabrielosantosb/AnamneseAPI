using CatalogAPI.Models;

namespace CatalogAPI.Services.Pacient
{
    public interface IPacientService
    {
        List<PacientModel> GetAllPacients();
        PacientModel GetPacientById(int id);
        PacientModel CreatePacient(PacientModel pacient);
        PacientModel UpdatePacient(int id, PacientModel pacient);
        PacientModel DeletePacient(int id);
    }
}
