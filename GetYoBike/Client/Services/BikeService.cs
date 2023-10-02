using GetYoBike.Shared.Models;
using System.Net.Http.Json;

namespace GetYoBike.Client.Services
{
    public class BikeService
    {
        private readonly HttpClient _httpClient;

        public BikeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BikeModel> GetAnAvailableBike(DateTime dateTimeStart, DateTime dateTimeEnd, int bikeTypeNr)
        {
            string startDateTimeString = dateTimeStart.ToString("yyyy-MM-dd-HH-mm");
            string endDateTimeString = dateTimeEnd.ToString("yyyy-MM-dd-HH-mm");

            try
            {
                List<BikeModel>? availableBikes = await _httpClient.GetFromJsonAsync<List<BikeModel>>(
                    $"api/Bikes/availableBikesInInterval?" +
                    $"startDateTime={startDateTimeString}" +
                    $"&endDateTime={endDateTimeString}" +
                    $"&bikeTypeId={bikeTypeNr}"
                );

                if (availableBikes != null)
                {
                    int randBikeId = new Random().Next(0, availableBikes.Count - 1);
                    BikeModel bike = availableBikes[randBikeId];

                    return bike;
                }
                else
                {
                    throw new Exception("Couldn't check for available bikes!");
                }

            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("No bikes available");
                }
                else
                {
                    throw e;
                }
            }
        }
    }
}
