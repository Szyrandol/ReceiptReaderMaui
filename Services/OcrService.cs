namespace ReceiptReader.Services
{
    public class OcrService
    {
        HttpClient httpClient;
        public OcrService()
        {
            httpClient = new HttpClient();
        }

        List<Item> itemList = new();
        public async Task<List<Item>> GetItems(string imagePath)
        {
            if (itemList.Count > 0)
                return itemList;

            using var formData = new MultipartFormDataContent();
            formData.Add(new ByteArrayContent(File.ReadAllBytes(imagePath)), "image", "image.jpg");

            var response = await httpClient.PostAsync("http://localhost:5000/ocr", formData);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonStringToItemList(result);
            }
            else
            {
                throw new Exception($"Error: {response.ReasonPhrase}");
            }
        }

        private List<Item> JsonStringToItemList(string jsonText)
        {
            Console.WriteLine("jsonText");
            List<Item> result = new();
            using (JsonDocument doc = JsonDocument.Parse(jsonText))
            {
                JsonElement root = doc.RootElement;
                foreach (JsonElement jsonElement in root.EnumerateArray())
                {
                    string jsonValue = jsonElement.GetProperty("newline").GetString();
                    Item item = new(jsonValue);
                    result.Add(item);
                }
            }
            result[result.Count - 1].Value *= 10;
            result[result.Count - 1].Name = "SUMA";
            result[result.Count - 1].StringValue += "0 PLN";
            return result;
        }
    }
}
