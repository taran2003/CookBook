using Android.App;
using CookBoock.Helpers;
using CookBoock.Models;
using RestSharp;
using System.Collections.ObjectModel;
using System.Text.Json;
using Result = CookBoock.Models.Result;

namespace CookBoock.Data
{
    public static class RecipeApi
    {        
        public static Recipe GetRecipe(string itemId)
        {
            string JSONString;
            Result recipes;
            try
            {
                string url = $"{Constants.Texts.Url}get-more-info?id={itemId}";
                var client = new RestClient(url);
                var request = new RestRequest(url);
                request.AddHeader(Constants.Texts.HederKeyName, Constants.Texts.HederKey);
                request.AddHeader(Constants.Texts.HederHostName, Constants.Texts.HederHost);
                var response = client.Execute(request);
                JSONString = response.Content;
                client.Dispose();
                recipes = JsonSerializer.Deserialize<Result>(JSONString);

            }
            catch (Exception)
            {
                throw;
            }
            return GetFromResult(recipes);
        }

        public static List<Recipe> GetRecipes(int from, int size)
        {
            string JSONString;
            Rootobject recipes;
            try
            {
                string url = $"{Constants.Texts.Url}list?from={from}&size={size}";
                var client = new RestClient(url);
                var request = new RestRequest(url);
                request.AddHeader(Constants.Texts.HederKeyName, Constants.Texts.HederKey);
                request.AddHeader(Constants.Texts.HederHostName, Constants.Texts.HederHost);
                var response = client.Execute(request);

                var t= response.IsSuccessStatusCode;
                if(!t) {
                    throw new Exception("asdjnaskdjnask jdnkjd");
                }
                JSONString = response.Content;
                client.Dispose();
                recipes = JsonSerializer.Deserialize<Rootobject>(JSONString);
            }
            catch (Exception)
            {
                throw;
            }
            var res = new List<Recipe>();
            Parallel.ForEach(recipes.results, (item) => {
                res.Add(GetFromResultShort(item));
            });
            return res;
        }

        static Recipe GetFromResultShort(Result result)
        {
            var recipe = new Recipe();
            recipe.Id = result.id;
            recipe.Name = result.name;
            //recipe.ImageUrl = result.thumbnail_url;
            recipe.Image = ImageGeter.GetImageFromUrl(result.thumbnail_url);
            return recipe;
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
            recipe.Image = ImageGeter.GetImageFromUrl(result.thumbnail_url);
            return recipe;
        }
    }
}
