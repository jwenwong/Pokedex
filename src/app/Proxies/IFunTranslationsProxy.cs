using Proxies.Models;
using Refit;
using System.Threading.Tasks;

namespace Proxies
{
    public interface IFunTranslationsProxy
    {
        [Post("/yoda")]
        Task<TranslationResponse> TranslateYoda(TranslationRequest request);

        [Post("/shakespeare")]
        Task<TranslationResponse> TranslateShakespeare(TranslationRequest request);
    }
}
