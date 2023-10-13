using TestProject.Models;

namespace TestProject.Domain.Interfaces
{
    public interface IDogsService
    {
        Task<IEnumerable<DogEntity>> GetDogs(string attribute, string order, int pageNumber, int pageSize);
        Task<DogEntity> CreateDog(DogEntity dog);
        string Ping();
    }
}
