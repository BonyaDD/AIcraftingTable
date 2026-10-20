using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CraftSystem.Core
{
    public class PromptInputProcessor : IInputProcessor
    {
        public string BuildPrompt(List<ItemData> ingredients)
        {
            var sb = new StringBuilder("Ингредиенты:\n");
            foreach (var item in ingredients)
            {
                sb.AppendLine($"- {item.Name} (Тир: {item.Tier})");
            }
            return sb.ToString();
        }
    }

    public class HardcodedDummyAiService : IPredictionService
    {
        public async Task<string> PredictAsync(string prompt, CancellationToken token)
        {
            await Task.Delay(1000, token); // Имитация задержки 1 сек
            return "{\"Name\": \"Ледяной Клинок\", \"Tier\": 3, \"Description\": \"Меч, сгенерированный ИИ-заглушкой.\", \"Effects\": [\"Заморозка\"]}";
        }
    }

    public class CraftDecisionMaker : IDecisionMaker
    {
        public CraftResult ValidateAndApplyRules(string rawAiOutput, int averageTier)
        {
            try
            {
                // Парсинг через встроенный JsonUtility
                RawAiCraftResponse response = JsonUtility.FromJson<RawAiCraftResponse>(rawAiOutput);

                if (response == null) return GetFallbackItem();

                string name = string.IsNullOrEmpty(response.Name) ? "Предмет" : response.Name;
                string desc = response.Description ?? "";
                List<string> effects = response.Effects != null ? new List<string>(response.Effects) : new List<string>();

                int maxAllowedTier = averageTier + 1;
                int finalTier = Math.Min(response.Tier, maxAllowedTier);

                return new CraftResult(name, finalTier, desc, effects, false);
            }
            catch
            {
                return GetFallbackItem();
            }
        }

        public CraftResult GetFallbackItem()
        {
            return new CraftResult("Куча пыли", 1, "Неудавшаяся попытка крафта.", new List<string>(), true);
        }
    }
}