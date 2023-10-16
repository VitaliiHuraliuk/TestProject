using Microsoft.EntityFrameworkCore;
using TestProject.Domain.Services;
using TestProject.Models;
using TestPtoject.DAL;

public class DogsServiceTests
{
    [Fact]
    public async Task GetDogs_WithValidInput_ReturnsDogs()
    {
        var options = new DbContextOptionsBuilder<DogContext>()
            .UseSqlServer("Server=.;Database=Dogs;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;")
            .Options;

        using var context = new DogContext(options);
        context.Dogs.Add(new DogEntity { Name = "Dog1", Weight = 10, TailLength = 5 });
        context.Dogs.Add(new DogEntity { Name = "Dog2", Weight = 15, TailLength = 6 });
        context.SaveChanges();

        var service = new DogsService(context);

        var result = await service.GetDogs("weight", "asc", 1, 2);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateDog_WithValidInput_CreatesDog()
    {
        var options = new DbContextOptionsBuilder<DogContext>()
            .UseSqlServer("Server=.;Database=Dogs;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;")
            .Options;

        using var context = new DogContext(options);
        var service = new DogsService(context);

        var existingDog = await context.Dogs.FirstOrDefaultAsync(d => d.Name == "NewDog");

        if (existingDog != null)
        {
            try
            {
                var result = await service.CreateDog(existingDog);
            }
            catch (ArgumentException ex)
            {

                Assert.Contains("already exists", ex.Message);
            }
        }
        else
        {
            var dog = new DogEntity
            {
                Name = "NewDog",
                Weight = 12,
                TailLength = 7
            };

            var result = await service.CreateDog(dog);

            Assert.NotNull(result);
            Assert.Equal("NewDog", result.Name);
        }
    }

    [Fact]
    public async Task CreateDog_WithInvalidInput_ThrowsException()
    {
        var options = new DbContextOptionsBuilder<DogContext>()
            .UseSqlServer("Server=.;Database=Dogs;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;")
            .Options;

        using var context = new DogContext(options);
        var service = new DogsService(context);

        var dog = new DogEntity();

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateDog(dog));
    }

    [Fact]
    public async Task CreateDog_WithDuplicateName_ThrowsException()
    {
        var options = new DbContextOptionsBuilder<DogContext>()
            .UseSqlServer("Server=.;Database=Dogs;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;")
            .Options;

        using var context = new DogContext(options);
        context.Dogs.Add(new DogEntity { Name = "ExistingDog" });
        context.SaveChanges();

        var service = new DogsService(context);

        var dog = new DogEntity
        {
            Name = "ExistingDog"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateDog(dog));
    }
}
