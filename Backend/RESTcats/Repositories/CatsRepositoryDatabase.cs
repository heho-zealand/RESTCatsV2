using RESTcats.Models;

namespace RESTcats.Repositories
{
    public class CatsRepositoryDatabase: ICatsRepository
    {
        private readonly CatsDbContext _context;
        public CatsRepositoryDatabase(CatsDbContext context)
        {
            _context = context;
        }

        public Cat AddCat(Cat cat)
        {
            if (cat is null)
            {
                throw new ArgumentNullException(nameof(cat));
            }
            _context.Cats.Add(cat);
            _context.SaveChanges();
            return cat;
        }

        public IEnumerable<Cat> GetAllCats(int? minimumweight, int? maximumweight, string? nameFilter)
        {
            return _context.Cats;
        }

        public Cat? GetCatById(int id)
        {
            return _context.Cats.Find(id);
        }

        public Cat? RemoveCat(int id)
        {
            var cat = GetCatById(id);
            if (cat != null)
            {
                _context.Cats.Remove(cat);
                _context.SaveChanges();
                return cat;
            }
            return null;
        }

        public Cat? UpdateCat(int id, Cat updatedCat)
        {
            var existingCat = GetCatById(id);
            if (existingCat != null)
            {
                existingCat.Name = updatedCat.Name;
                existingCat.Weight = updatedCat.Weight;
                _context.SaveChanges();
                return existingCat;
            }
            return null;
        }
    }
}
