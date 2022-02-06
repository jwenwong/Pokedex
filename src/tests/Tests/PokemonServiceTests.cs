using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PokeApiNet;
using Pokedex.Models;
using Pokedex.Proxies;
using Pokedex.Services;
using Refit;
using Xunit;

namespace Tests
{
    public class PokemonServiceTests
    {
        private readonly PokemonService _pokemonService;
        private readonly IFunTranslationsProxy _translationsProxy;

        public PokemonServiceTests()
        {
            var pokeApiClient = Substitute.For<PokeApiClient>();
            _translationsProxy = Substitute.For<IFunTranslationsProxy>();
            var logger = Substitute.For <ILogger<PokemonService>>();
            _pokemonService = new PokemonService(pokeApiClient, _translationsProxy, logger);
        }

        [Fact]
        public async Task GetTranslatedPokemon_WhenTooManyRequests_ReturnsDefaultDescription()
        {

            var refitSettings = new RefitSettings();
            var tooManyRequestsException = await ApiException.Create(new HttpRequestMessage(), HttpMethod.Post, 
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Content = refitSettings.ContentSerializer.ToHttpContent("toomany")
                }, refitSettings);
            _translationsProxy.TranslateYoda(Arg.Any<TranslationRequest>()).Throws(tooManyRequestsException);
            var result = await _pokemonService.GetTranslatedPokemon("mewtwo");
            result.Description.Should()
                .Be(
                    "It was created by a scientist after years of horrific gene splicing and DNA engineering experiments.");
        }

        [Fact]
        public async Task GetTranslatedPokemon_WhenPokemonIsLegendary_ShouldReturnYodaTranslation()
        {
            var name = "mewtwo";
            _translationsProxy.TranslateYoda(Arg.Any<TranslationRequest>()).Returns(new TranslationResponse() { Success = new Success() { Total = 1 }, Contents = new Contents() { Translated = "Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was." } });
            var result = await _pokemonService.GetTranslatedPokemon(name);
            result.Name.Should().Be(name);
            result.Habitat.Should().Be("rare");
            result.IsLegendary.Should().Be(true);
            result.Description.Should().Be("Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.");
        }

        [Fact]
        public async Task GetTranslatedPokemon_WhenHabitatIsCave_ShouldReturnYodaTranslation()
        {
            var name = "zubat";
            _translationsProxy.TranslateYoda(Arg.Any<TranslationRequest>()).Returns(new TranslationResponse() { Success = new Success() { Total = 1 }, Contents = new Contents() { Translated = "Forms colonies in perpetually dark places.Ultrasonic waves to identify and approach targets,  uses." } });
            var result = await _pokemonService.GetTranslatedPokemon(name);
            result.Name.Should().Be(name);
            result.Habitat.Should().Be("cave");
            result.IsLegendary.Should().Be(false);
            result.Description.Should().Be("Forms colonies in perpetually dark places.Ultrasonic waves to identify and approach targets,  uses.");
        }

        [Fact]
        public async Task GetTranslatedPokemon_WhenNotLegendaryOrCave_ShouldReturnShakespeareTranslation()
        {
            var name = "charmander";
            _translationsProxy.TranslateShakespeare(Arg.Any<TranslationRequest>()).Returns(new TranslationResponse() { Success = new Success() { Total = 1 }, Contents = new Contents() { Translated = "Obviously prefers hot places. At which hour 't rains,  steam is did doth sayeth to spout from the tip of its tail." } });
            var result = await _pokemonService.GetTranslatedPokemon(name);
            result.Name.Should().Be(name);
            result.Habitat.Should().Be("mountain");
            result.IsLegendary.Should().Be(false);
            result.Description.Should().Be("Obviously prefers hot places. At which hour 't rains,  steam is did doth sayeth to spout from the tip of its tail.");
        }
    }
}
