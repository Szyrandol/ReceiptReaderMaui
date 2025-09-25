using System.Net.Http.Json;

namespace ReceiptReader.ApplicationLayer.Services;

public class ReceiptService
{
    HttpClient httpClient;
    public ReceiptService()
    {
        httpClient = new HttpClient();
    }
    public async Task<Receipt> GetReceiptAsync(string id)
    {
        var url = $"http://localhost:5001/receipt?receipt_id={id}";
        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var receipt = JsonSerializer.Deserialize<Receipt>(json); // newtonsoft might be better
            return receipt;
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
    }
    public async Task<Receipt> PostReceiptAsync(Receipt receipt)
    {
        // adding user to header field "From" after addition of users
        var jsonContent = JsonContent.Create(receipt, typeof(Receipt));
        var response = await httpClient.PostAsync("http://localhost:8000/receipt", jsonContent);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            
            //trzeba zmienić co zwraca API
            return JsonSerializer.Deserialize<Receipt>(result);
        }
        else
        {
            throw new Exception($"Error: {response.ReasonPhrase}");
        }
    }
    public async Task<List<Receipt>> GetReceiptsAsync(string owner)
    {
        var url = $"http://localhost:5001/receipt?user_name={owner}";
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error: {response.StatusCode}");
        }
        var receipts = await response.Content.ReadFromJsonAsync<List<Receipt>>();
        return receipts;
    }
}
