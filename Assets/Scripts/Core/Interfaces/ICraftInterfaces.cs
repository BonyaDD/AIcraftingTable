using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CraftSystem.Core
{
    public interface IInputProcessor
    {
        string BuildPrompt(List<ItemData> ingredients);
    }

    public interface IPredictionService
    {
        Task<string> PredictAsync(string prompt, CancellationToken token);
    }

    public interface IDecisionMaker
    {
        CraftResult ValidateAndApplyRules(string rawAiOutput, int averageTier);
        CraftResult GetFallbackItem();
    }

    public interface IXRInteractor
    {
        List<ItemData> GetSelectedItems();
        void DisplayResult(CraftResult result);
        void SetLoadingState(bool isLoading);
    }

    public interface IAIController
    {
        Task<CraftResult> ProcessCraftAsync(List<ItemData> ingredients);
    }
}