using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookApp.Model.FiltrationClasses
{
    public static class RecipeFiltrationHelper
    {
        public static IQueryable<Recipe> OrderRecipes(this IQueryable<Recipe> recipes,
         FiltrationOrder orderType)
        {
            return orderType switch

            {
                FiltrationOrder.Default => recipes.OrderBy(r => r.RecipeId),
                FiltrationOrder.ByYear => recipes.OrderBy(r => r.CreatedAt),
                _ => recipes.OrderBy(r => r.RecipeId)
            };
        }

        public static IQueryable<Recipe> FilterRecipes(this IQueryable<Recipe> recipes,
        FiltrationFilter filterType, string? filterData)
        {
            switch (filterType)
            {
                case FiltrationFilter.Default:
                    {
                        return recipes;
                    }
                case FiltrationFilter.ByYear:
                    {
                        if (filterData == null)
                            throw new ArgumentNullException("Date can't be null.");

                        DateOnly startDate = new(int.Parse(filterData), 1, 1);
                        DateOnly endDate = startDate.AddYears(1);
                        return recipes.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate);
                    }
                default:
                    {
                        goto case FiltrationFilter.Default;
                    }
            }
        }

        public static IQueryable<Recipe> Paginate(this IQueryable<Recipe> recipes, int pageNum)
        {
            if (pageNum < 1)
                throw new ArgumentException("Page num can't be less than 1.");

            return recipes.Skip((pageNum - 1) * 10).Take(10);
        }
    }
}