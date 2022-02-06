using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using Pokedex.Models;
using Xunit;

namespace Tests
{
    [Collection("Shared Server Collection")]
    public class PokemonControllerTests
    {
        private readonly HttpClient _client;

        public PokemonControllerTests(TestServerContainer testServerContainer)
        {
            _client = testServerContainer.Server.CreateClient();
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

        [Fact]
        public async Task GetTranslatedPokemon_WhenPokemonIsLegendary_ShouldReturnYodaTranslation()
        {
            var name = "mewtwo";
            var result = await _client.GetAsync($"/pokemon/translated/{name}");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsAsync<PokemonResponse>();
            content.Name.Should().Be(name);
            content.Habitat.Should().Be("rare");
            content.IsLegendary.Should().Be(true);
            content.Description.Should()
                .Match(x => (x ==
                    "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.") || (x == "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments."));
        }

        [Fact]
        public async Task GetTranslatedPokemon_WhenHabitatIsCave_ShouldReturnYodaTranslation()
        {
            var name = "zubat";
            var result = await _client.GetAsync($"/pokemon/translated/{name}");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsAsync<PokemonResponse>();
            content.Name.Should().Be(name);
            content.Habitat.Should().Be("cave");
            content.IsLegendary.Should().Be(false);
            content.Description.Should()
                .Match(x => (x ==
                             "Forms colonies in perpetually dark places.Ultrasonic waves to identify and approach targets,  uses.") ||
                            x ==
                            "Forms colonies in perpetually dark places. Uses ultrasonic waves to identify and approach targets.");
        }

        [Fact]
        public async Task GetTranslatedPokemon_WhenNotLegendaryOrCave_ShouldReturnShakespeareTranslation()
        {
            var name = "charmander";
            var result = await _client.GetAsync($"/pokemon/translated/{name}");
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsAsync<PokemonResponse>();
            content.Name.Should().Be(name);
            content.Habitat.Should().Be("mountain");
            content.IsLegendary.Should().Be(false);
            content.Description.Should()
                .Match(x => (x ==
                             "Obviously prefers hot places. At which hour 't rains,  steam is did doth sayeth to spout from the tip of its tail.") ||
                            (x ==
                             "Obviously prefers hot places. When it rains, steam is said to spout from the tip of its tail."));
        }
    }
}
