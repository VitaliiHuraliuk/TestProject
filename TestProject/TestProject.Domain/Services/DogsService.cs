using TestProject.Domain.Interfaces;
using TestProject.Models;
using TestPtoject.DAL;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Domain.Services
{
    public class DogsService : IDogsService
    {
        private readonly DogContext _context;

        public DogsService(DogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DogEntity>> GetDogs(string attribute = "weight", string order = "asc", int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Invalid pageNumber or pageSize values.");
            }

            var query = _context.Dogs.AsQueryable();

            if (attribute == "weight")
            {
                query = query.OrderBy($"{attribute} {order}");
            }

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<DogEntity> CreateDog(DogEntity dog)
        {
            if (dog == null)
            {
                throw new ArgumentNullException("Dog object is null.");
            }

            if (string.IsNullOrWhiteSpace(dog.Name))
            {
                throw new ArgumentException("Dog name cannot be empty or null.");
            }

            if (dog.TailLength < 0)
            {
                throw new ArgumentException("Tail length cannot be a negative number.");
            }

            if (dog.Weight < 0)
            {
                throw new ArgumentException("Weight cannot be a negative number.");
            }

            if (_context.Dogs.Any(d => d.Name == dog.Name))
            {
                throw new ArgumentException("A dog with the same name already exists.");
            }

            try
            {
                await _context.Dogs.AddAsync(dog);
                await _context.SaveChangesAsync();

                var createdDog = new DogEntity
                {
                    Id = dog.Id,
                    Name = dog.Name,
                    Color = dog.Color,
                    TailLength = dog.TailLength,
                    Weight = dog.Weight
                };

                return createdDog;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }

                throw;
            }
        }

        public string Ping()
        {
            return "Dogshouseservice.Version1.0.1";
        }
    }
}
