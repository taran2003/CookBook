using CookBoock.Models;
using RestSharp;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace CookBoock.Data
{
    public static class RecipeApi
    {        
        public static Recipe GetRecipe(string itemId)
        {
            var client = new RestClient($"https://tasty.p.rapidapi.com/recipes/get-more-info?id={itemId}");
            var request = new RestRequest($"https://tasty.p.rapidapi.com/recipes/get-more-info?id={itemId}");
            request.AddHeader("X-RapidAPI-Key", "3b3717f146msh62fd46c8cb27267p161fa7jsn716d8d4e7714");
            request.AddHeader("X-RapidAPI-Host", "tasty.p.rapidapi.com");
            var response = client.Execute(request);
            var JSONString = response.Content;
            var recipes = JsonSerializer.Deserialize<Result>(JSONString);
            return GetFromResult(recipes);
        }

        public static List<Recipe> GetRecipes(int size)
        {
            var client = new RestClient($"https://tasty.p.rapidapi.com/recipes/list?from=0&size={size}");
            var request = new RestRequest($"https://tasty.p.rapidapi.com/recipes/list?from=0&size={size}");
            request.AddHeader("X-RapidAPI-Key", "3b3717f146msh62fd46c8cb27267p161fa7jsn716d8d4e7714");
            request.AddHeader("X-RapidAPI-Host", "tasty.p.rapidapi.com");
            var response = client.Execute(request);
            var JSONString = response.Content;
            var recipes = JsonSerializer.Deserialize<Rootobject>(JSONString);
            var res = new List<Recipe>();
            Parallel.ForEach(recipes.results, (item) =>
            {
                res.Add(GetFromResult(item));
            });
            return res;
        }

        static Recipe GetFromResult(Result result)
        {
            var recipe = new Recipe();
            recipe.Id = result.id;
            recipe.Name = result.name;
            foreach (var item1 in result.instructions)
            {
                recipe.CookingProcess += item1.display_text;
            }
            var bufList = new ObservableCollection<Ingridients>();
            foreach (var item1 in result.sections)
            {
                foreach (var item2 in item1.components)
                {
                    bufList.Add(new Ingridients(item2.raw_text));
                }
            }
            recipe.Ingridients = bufList;
            recipe.ImageUrl = result.thumbnail_url;
            recipe.Image = ImageSource.FromUri(new Uri(result.thumbnail_url));
            return recipe;
        }
    }
}
