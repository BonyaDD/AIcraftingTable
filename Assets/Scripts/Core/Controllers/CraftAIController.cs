using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CraftSystem.Core
{
    public class CraftAIController : IAIController
    {
        private readonly IInputProcessor _inputProcessor;
        private readonly IPredictionService _predictionService;
        private readonly IDecisionMaker _decisionMaker;
        private readonly IXRInteractor _xrInteractor;

        public CraftAIController(
            IInputProcessor inputProcessor,
            IPredictionService predictionService,
            IDecisionMaker decisionMaker,
            IXRInteractor xrInteractor)
        {
            _inputProcessor = inputProcessor;
            _predictionService = predictionService;
            _decisionMaker = decisionMaker;
            _xrInteractor = xrInteractor;
        }

        public async Task<CraftResult> ProcessCraftAsync(List<ItemData> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0)
                return _decisionMaker.GetFallbackItem();

            _xrInteractor.SetLoadingState(true);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try
            {
                string prompt = _inputProcessor.BuildPrompt(ingredients);
                string rawJson = await _predictionService.PredictAsync(prompt, cts.Token);

                int avgTier = (int)Math.Round(ingredients.Average(i => i.Tier));
                CraftResult result = _decisionMaker.ValidateAndApplyRules(rawJson, avgTier);

                _xrInteractor.DisplayResult(result);
                return result;
            }
            catch
            {
                var fallback = _decisionMaker.GetFallbackItem();
                _xrInteractor.DisplayResult(fallback);
                return fallback;
            }
            finally
            {
                _xrInteractor.SetLoadingState(false);
            }
        }
    }
}