using System.Collections.Generic;
using RESTcats.Models;

namespace RESTcats.Repositories
{
    public class CatsRepositoryList : ICatsRepository
    {
        private readonly List<Cat> cats = new();
        private int nextId = 1;

        public CatsRepositoryList(bool includeData = false)
        {
            if (includeData)
            {
                AddCat(new Cat { Name = "Whiskers", Weight = 4 });
                AddCat(new Cat { Name = "Mittens", Weight = 5 });
                AddCat(new Cat { Name = "Shadow", Weight = 6 });
            }
        }

        public IEnumerable<Cat> GetAllCats(int? minimumweight, int? maximumweight, string? nameFilter)
        {
            if (minimumweight > maximumweight &&
                minimumweight != null && maximumweight != null)
            {
                throw new ArgumentException("Minimum weight " +
                    "cannot be greater than maximum weight.");
            }

            IEnumerable<Cat> result = cats.AsReadOnly();

            if (minimumweight != null)
            {
                result = result.Where(c => c.Weight >= minimumweight);
            }
            if (maximumweight != null)
            {
                result = result.Where(c => c.Weight <= maximumweight);
            }
            if (nameFilter != null)
            {
                result = result.Where(c => c.Name.
                Contains(nameFilter, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }
        public Cat? GetCatById(int id)
        {
            return cats.FirstOrDefault(c => c.Id == id);
        }
        public Cat AddCat(Cat cat)
        {
            if (cat is null)
            {
                throw new ArgumentNullException(nameof(cat));
            }
            cat.Id = nextId++;
            cats.Add(cat);
            return cat;
        }

        public Cat? RemoveCat(int id)
        {
            var cat = GetCatById(id);
            if (cat != null)
            {
                cats.Remove(cat);
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
                return existingCat;
            }
            return null;
        }
    }
}
