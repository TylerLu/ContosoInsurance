using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

public static class Utils
{
    public static async Task<string> GetBlobUriAsync(ICloudBlob blob)
    {
        return await blob.ExistsAsync() ? blob.Uri.AbsoluteUri : null;
    }

    public static async Task<HttpResponseMessage> PostTo(string url, string content, string mediaType = "text/plain")
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        requestMessage.Content = new StringContent(content, Encoding.UTF8, mediaType);

        var client = new HttpClient();
        return await client.SendAsync(requestMessage);
    }

    public static Task<HttpResponseMessage> PostTo(string url, object obj)
    {
        var content = JsonConvert.SerializeObject(obj);
        return PostTo(url, content, "application/json");
    }
}