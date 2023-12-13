using BeautyPoly.Common;
using BeautyPoly.Data.ViewModels;
using BeautyPoly.DBContext;
using BeautyPoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyPoly.Data.Repositories
{
    public class CategoryRepo : GenericRepo<Category>
    {
        public CategoryRepo(BeautyPolyDbContext dbContext) : base(dbContext)
        {
        }
        public List<CategoryViewModel> GetAllCate()
        {
            return SQLHelper<CategoryViewModel>.ProcedureToList("spGetCategory", new string[] { }, new object[] { });
        }
    }
}
