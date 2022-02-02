using System.Threading.Tasks;
using Pokedex.Models;
using Refit;

namespace Pokedex.Proxies
{
    public interface IFunTranslationsProxy
    {
        [Post("/yoda")]
        Task<TranslationResponse> TranslateYoda(TranslationRequest request);

        [Post("/shakespeare")]
        Task<TranslationResponse> TranslateShakespeare(TranslationRequest request);
    }
}
