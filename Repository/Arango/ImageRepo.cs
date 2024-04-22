using ArangoDBNetStandard;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System;
using TastyBytesReact.Models;
using TastyBytesReact.Models.Nodes;
using TastyBytesReact.Models.Requests;

namespace TastyBytesReact.Repository.Arango
{
    public class ImageRepo
    {
        private readonly IArangoDBClient _client;
        public ImageRepo(IArangoDBClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ImageModel>> GetImageByKey(string key)
        {
            var cursor = await _client.Cursor.PostCursorAsync<ImageModel>(
             @"let doc = DOCUMENT(CONCAT(""Image/"", @key))
                RETURN doc", new Dictionary<string, object>() { { "key", key } })
             .ConfigureAwait(false);
            return cursor.Result;
        }

        public async Task<IEnumerable<ImageModel>> SaveImage(string imageBase64, string mimeType)
        {
            long seconds = (long)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
            var cursor = await _client.Cursor.PostCursorAsync<ImageModel>(
             @"INSERT {
                Body : @body,
                LastModified: @unixTime,
                MimeType: @mimeType
             } INTO Image RETURN NEW", new Dictionary<string, object>() {
                { "body", imageBase64 },
                {"unixTime", seconds },
                {"mimeType", mimeType }
             })
             .ConfigureAwait(false);
            return cursor.Result;
        }
    }
}
