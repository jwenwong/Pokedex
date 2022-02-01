using FluentAssertions;
using NSubstitute;
using Pokedex.Controllers;
using Services;
using Services.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    [Collection("Shared Server Collection")]
    public class PokemonControllerTests
    {
        private HttpClient _client;
        private TestServerContainer _server;

        public PokemonControllerTests(TestServerContainer testServerContainer)
        {
            _server = testServerContainer;
            _client = _server.Server.CreateClient();
        }

        [Fact]
        public async Task GetPokemon_WhenNameIsValid_ShouldReturnSuccess()
        {
            var name = "mewtwo";
            var result = await _client.GetAsync($"/pokemon/{name}");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsAsync<PokemonResponse>();
            content.Name.Should().Be(name);
            content.Habitat.Should().Be("rare");
            content.IsLegendary.Should().Be(true);
            content.Description.Should().Be("It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("fakename")]
        public async Task GetPokemon_WhenNameIsNull_ShouldReturnNotFound(string name)
        {
            var result = await _client.GetAsync($"/pokemon/{name}");
            result.IsSuccessStatusCode.Should().Be(false);
        }
    }
}
