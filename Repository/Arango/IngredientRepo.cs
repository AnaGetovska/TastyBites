﻿using ArangoDBNetStandard;
using ArangoDBNetStandard.Serialization;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Edges;
using TastyBytesReact.Models.Nodes;

namespace TastyBytesReact.Repository.Arango
{
    public class IngredientRepo
    {
        private readonly IArangoDBClient _client;
        public IngredientRepo(IArangoDBClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets all ingredients.
        /// </summary>
        public async Task<IEnumerable<IngredientModel>> GetAll()
        {
            var cursor = await _client.Cursor.PostCursorAsync<IngredientModel>(
                @"FOR doc IN Ingredient RETURN doc")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all ingredients including all related categories.
        /// </summary>
        public async Task<IEnumerable<ExtendedIngredientModel>> GetAllExtended()
        {
            var cursor = await _client.Cursor.PostCursorAsync<ExtendedIngredientModel>(
                @"FOR doc IN Ingredient
                    LET categories = (FOR v IN 1 OUTBOUND doc._id InCategory RETURN v)
                RETURN MERGE({Categories: categories}, doc)")
                .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all ingredients by char segment included in the Name.
        /// </summary>
        /// <param name="name">Character segment included in ingredient name</param>
        public async Task<IEnumerable<IngredientModel>> GetAllByWildcard(string segment)
        {
            var cursor = await _client.Cursor.PostCursorAsync<IngredientModel>(
               @"FOR doc IN Ingredient FILTER LOWER(doc.Name) LIKE CONCAT(""%"", @segment, ""%"") RETURN doc",
               new Dictionary<string, object>() {
                    { "segment", segment }
               })
               .ConfigureAwait(false);
            return cursor.Result;
        }

        /// <summary>
        /// Gets all ingredients by char segment included in the Name.
        /// </summary>
        /// <param name="name">Character segment included in ingredient name</param>
        public async Task<IEnumerable<IngredientModel>> GetAllByKeys(string[] keys)
        {
            var cursor = await _client.Cursor.PostCursorAsync<IngredientModel>(
               @"FOR i IN Ingredient
                   FILTER i._key IN @keys
                   RETURN i",
               new Dictionary<string, object>() {
                    { "keys", keys }
               })
               .ConfigureAwait(false);
            return cursor.Result;
        }

        public async Task<IsContainedModel> AddIngredientToRecipe(IngredientModel ingredient, string recipeKey)
        {
            return (await _client.Document.PostDocumentAsync("IsContained",
            new IsContainedModel()
            {
                _from = "Ingredient/" + ingredient._key,
                _to = "Recipe/" + recipeKey,
                MeasurementUnit = ingredient.MeasurementUnit,
                Quantity = float.Parse(ingredient.Quantity)
            })).New;
        }
    }
}
