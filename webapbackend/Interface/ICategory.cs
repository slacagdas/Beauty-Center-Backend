using webapbackend.Models;

namespace webapbackend.Interface
{
    public interface ICategory
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
       
        bool CategoryExists(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
