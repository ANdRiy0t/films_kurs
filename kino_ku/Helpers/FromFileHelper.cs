namespace kino_ku.Helpers;

public static class FromFileHelper
{
    public static async Task<string> ConvertToBase64(this IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}