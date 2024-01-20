using ArangoDBNetStandard;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Models.Requests;

namespace TastyBytesReact.Repository.Arango
{
    public class RecipeRepo
    {
        private readonly IArangoDBClient _client;
        public RecipeRepo(IArangoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets all recipes.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RecipeModel>> GetAll()
        {
            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
                @"FOR doc IN Recipe
                  RETURN UNSET(doc, ""Images"")")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        ///Creates new recipe.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RecipeModel>> AddRecipe(AddRecipeRequest newRecipe, string imgName)
        {

            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
                @"INSERT {
                    Name: @name,
                    PreparationTime: @prepTime,
                    Portions:@portions,
                    Description: @description,
                    DisplayImage: @displayImage
                } INTO Recipe
                  RETURN NEW", new Dictionary<string, object>(){
                {"name", newRecipe.Name },
                {"prepTime", newRecipe.PreparationTime },
                {"portions", newRecipe.Portions },
                {"description", newRecipe.Description },
                {"displayImage", imgName },
            })
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all extended recipes.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ExtendedRecipeModel>> GetAllExtended()
        {
            var cursor = await _client.Cursor.PostCursorAsync<ExtendedRecipeModel>(
                @"FOR doc IN Recipe
                    LET categories = (FOR v IN 1 OUTBOUND doc._id InCategory RETURN v)
                    LET rating = (FOR v,e IN 1 OUTBOUND doc._id HasRating RETURN e.Rating)
                    LET ingredients = (FOR v,e IN 1 INBOUND doc._id IsContained RETURN MERGE(e, v))
                    LET allergens = (FOR ingredient IN ingredients
                    FOR v IN 1 OUTBOUND ingredient._id InCategory RETURN v
                )
                RETURN MERGE(doc,{Categories: categories},{Ingredients: ingredients}, {Allergens: allergens}, {Rating: COUNT(rating) > 0 ? SUM(rating)/COUNT(rating) : 0}, {RatingCount: COUNT(rating)})")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets extended recipe by key.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ExtendedRecipeModel>> GetExtendedByKey(string key)
        {
            var cursor = await _client.Cursor.PostCursorAsync<ExtendedRecipeModel>(
                @"let doc = DOCUMENT(CONCAT(""Recipe/"", @key))
                    LET categories = (FOR v IN 1 OUTBOUND doc._id InCategory RETURN v)
                    LET ingredients = (FOR v,e IN 1 INBOUND doc._id IsContained RETURN MERGE(e, v))
                    LET rating = (FOR v,e IN 1 OUTBOUND doc._id HasRating RETURN e.Rating)
                    LET allergens = (FOR ingredient IN ingredients FOR v IN 1 OUTBOUND ingredient._id InCategory RETURN v)
                RETURN MERGE(doc,{Categories: categories},{Ingredients: ingredients}, {Allergens: allergens}, {Rating: COUNT(rating) > 0 ? SUM(rating)/COUNT(rating) : 0}, {RatingCount: COUNT(rating)})", new Dictionary<string, object>() { { "key", key } })
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all extended recipes filtered by collection of keys.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ExtendedRecipeModel>> GetAllExtendedByKeys(string[] keys)
        {
            var cursor = await _client.Cursor.PostCursorAsync<ExtendedRecipeModel>(
                @"FOR doc IN Recipe
                    FILTER POSITION(@keys, doc._key)
                    LET categories = (FOR v IN 1 OUTBOUND doc._id InCategory RETURN v)
                    LET rating = (FOR v,e IN 1 OUTBOUND doc._id HasRating RETURN e.Rating)
                    LET ingredients = (FOR v,e IN 1 INBOUND doc._id IsContained RETURN MERGE(e, v))
                    LET allergens = (FOR ingredient IN ingredients
                    FOR v IN 1 OUTBOUND ingredient._id InCategory RETURN v
                )
                RETURN MERGE(doc,{Categories: categories},{Ingredients: ingredients}, {Allergens: allergens}, {Rating: COUNT(rating) > 0 ? SUM(rating)/COUNT(rating) : 0}, {RatingCount: COUNT(rating)})", new Dictionary<string, object>() { { "keys", keys } })
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all recipes by char segment included in the Name.
        /// </summary>
        /// <param name="name">Character segment included in recipe name</param>
        public async Task<IEnumerable<RecipeModel>> GetAllByNameCut(string segment)
        {
            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
               @"FOR doc IN Recipe FILTER LOWER(doc.Name) LIKE CONCAT(""%"", @segment, ""%"") RETURN doc",
               new Dictionary<string, object>() {
                    { "segment", segment }
               })
               .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all recipes filtered by preparation time.
        /// </summary>
        /// <param name="time">Recipe preparation time to filter by.</param>
        /// <param name="comparison">Comparison type.See <see cref="ComparisonType"></see>.</param>
        public async Task<IEnumerable<RecipeModel>> GetByPrepTime(int time, ComparisonType comparison)
        {
            var comparisonTypes = new Dictionary<ComparisonType, string>() {
                {ComparisonType.Equal, "==" },
                {ComparisonType.NotEqual, "!=" },
                {ComparisonType.GreaterThanOrEqual, ">=" },
                {ComparisonType.LessThanOrEqual, "<=" },
                {ComparisonType.GreaterThan, ">" },
                {ComparisonType.LessThan, "<" },
            };

            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
                @"FOR doc IN Recipe FILTER doc.PreparationTime " + comparisonTypes[comparison] + @" @time
                RETURN doc", new Dictionary<string, object>() {
                    { "time", time }
                })
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all recipes that contains a collection of ingredients' keys.
        /// </summary>
        /// <param name="ingredientsKeys">The keys af all searched ingredients.</param>
        public async Task<IEnumerable<RecipeModel>> GetByIngredients(string[] ingredientsKeys)
        {
            var cursor = await _client.Cursor.PostCursorAsync<RecipeModel>(
                @"FOR r IN Recipe
                    LET ingredients = (FOR v IN 1 INBOUND r._id IsContained RETURN v._key)
                    LET matches = INTERSECTION(ingredients, @searchKeys)
                    FILTER COUNT(matches) == COUNT(@searchKeys)
                    RETURN r", new Dictionary<string, object>() {
                    { "searchKeys", ingredientsKeys }
                })
                .ConfigureAwait(false);
            return cursor.Result;
        }
    }
}
