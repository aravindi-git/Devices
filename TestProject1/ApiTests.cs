using System.Text;
using FluentAssertions;
using Newtonsoft.Json;

namespace TestProject
{
    public class ApiTests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "https://api.restful-api.dev/objects";

        public ApiTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task GetAllObjects_ShouldReturnListOfObjects()
        {
            // Act
            var response = await _httpClient.GetAsync(_baseAddress);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AddObject_ShouldReturnCreatedObject()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object." };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync(_baseAddress, content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            responseData.Should().Contain("Test Object");
        }

        [Fact]
        public async Task GetObjectById_ShouldReturnObject()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object." };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");
            var postResponse = await _httpClient.PostAsync(_baseAddress, content);
            postResponse.EnsureSuccessStatusCode();

            var postResponseData = await postResponse.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<dynamic>(postResponseData);
            var objectId = createdObject.id;

            // Act
            var getResponse = await _httpClient.GetAsync($"{_baseAddress}/{objectId}");

            // Assert_baseAddress
            getResponse.EnsureSuccessStatusCode();
            var getResponseData = await getResponse.Content.ReadAsStringAsync();
            getResponseData.Should().Contain("Test Object");
        }

        [Fact]
        public async Task UpdateObject_ShouldReturnUpdatedObject()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object." };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");
            var postResponse = await _httpClient.PostAsync(_baseAddress, content);
            postResponse.EnsureSuccessStatusCode();

            var postResponseData = await postResponse.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<dynamic>(postResponseData);
            var objectId = createdObject.id;

            var updatedObject = new { name = "Updated Test Object", description = "This is an updated test object." };
            var updateContent = new StringContent(JsonConvert.SerializeObject(updatedObject), Encoding.UTF8, "application/json");

            // Act
            var putResponse = await _httpClient.PutAsync($"{_baseAddress}/{objectId}", updateContent);

            // Assert
            putResponse.EnsureSuccessStatusCode();
            var putResponseData = await putResponse.Content.ReadAsStringAsync();
            putResponseData.Should().Contain("Updated Test Object");
        }

        [Fact]
        public async Task DeleteObject_ShouldReturnNoFoundResponse()
        {
            // Arrange
            var newObject = new { name = "Test Object", description = "This is a test object." };
            var content = new StringContent(JsonConvert.SerializeObject(newObject), Encoding.UTF8, "application/json");
            var postResponse = await _httpClient.PostAsync(_baseAddress, content);
            postResponse.EnsureSuccessStatusCode();

            var postResponseData = await postResponse.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<dynamic>(postResponseData);
            var objectId = createdObject.id;

            // Act
            var deleteResponse = await _httpClient.DeleteAsync($"{_baseAddress}/{objectId}");

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Now let's try to get the deleted object from the get API call. It should return a Not Found response 

            // Act
            var getResponse = await _httpClient.GetAsync($"{_baseAddress}/{objectId}");

            // Assert
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

    }
}
