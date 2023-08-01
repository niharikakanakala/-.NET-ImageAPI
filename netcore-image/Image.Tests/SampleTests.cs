using System.Net.Mime;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Image.WebAPI.Data; // Add this using directive
using Image.WebAPI; // Add this using directive
using Xunit;

namespace IntegrationTests.Tests
{
    public class SampleIntegrationTests
    {
        private TestServer _server;
        private HttpClient _client;

        public SampleIntegrationTests()
        {
            SetUpClient();
        }

        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var context = new ImageContext(new DbContextOptionsBuilder<ImageContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(ImageContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task TestYourIntegrationScenario()
        {
            // Add your integration test scenario here
            // For example:
            var response = await _client.GetAsync("/api/image");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var details = JsonConvert.DeserializeObject<List<ImageDetail>>(content);
            details.Should().NotBeNull();
        }

        [Fact]
        public async Task TestAddImageDetail()
        {
            var newImage = new ImageDetail
            {
                ImageName = "TestImage",
                ImageURL = "someurljustfetchfromgoogle.jpg",
                ImageDescription = "Image is very beautiful"
            };

            var jsonString = JsonConvert.SerializeObject(newImage);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/image", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task TestGetAllImageDetails()
        {
            // Add test data to the database
            var context = _server.Host.Services.GetRequiredService<ImageContext>();
            context.Details.Add(new ImageDetail { ImageName = "Cake", ImageURL = "cakeimage.jpg", ImageDescription = "YummyCake" });
            context.Details.Add(new ImageDetail { ImageName = "Pastry", ImageURL = "pastryimage.jpg", ImageDescription = "YummyPastry" });
            context.SaveChanges();

            var response = await _client.GetAsync("/api/image");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var images = JsonConvert.DeserializeObject<List<ImageDetail>>(content);
            images.Should().HaveCount(2);
        }

        [Fact]
        public async Task TestUpdateImageDetails()
        {
            // Add test data to the database
            var context = _server.Host.Services.GetRequiredService<ImageContext>();
            var imagedetails = new ImageDetail {  ImageName = "Pastry", ImageURL = "pastryimage.jpg", ImageDescription = "YummyPastry" };
            context.Details.Add(imagedetails);
            context.SaveChanges();

            imagedetails.ImageDescription = "YummyYummyPastry";
            var jsonString = JsonConvert.SerializeObject(imagedetails);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/image/{imagedetails.Id}", httpContent);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent); // Change this line

            var updatedImagedetails = await context.Details.FindAsync(imagedetails.Id);
            updatedImagedetails.ImageDescription.Should().Be("YummyYummyPastry");
        }

        [Fact]
        public async Task TestDeleteImageById()
        {
            // Add test data to the database
            var context = _server.Host.Services.GetRequiredService<ImageContext>();
            var detail = new ImageDetail { ImageName = "Pastry", ImageURL = "pastryimage.jpg", ImageDescription = "YummyPastry" };
            context.Details.Add(detail);
            context.SaveChanges();

            var response = await _client.DeleteAsync($"/api/image/{detail.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var deleted = await context.Details.FindAsync(detail.Id);
            deleted.Should().BeNull();
        }
    }
}
