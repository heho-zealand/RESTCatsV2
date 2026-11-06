using RESTcats.Models;

namespace RESTcats.Repositories
{
    public interface ICatsRepository
    {
        Cat AddCat(Cat cat);
        IEnumerable<Cat> GetAllCats(int? minimumweight, int? maximumweight, string? nameFilter);
        Cat? GetCatById(int id);
        Cat? RemoveCat(int id);
        Cat? UpdateCat(int id, Cat updatedCat);
    }
}
