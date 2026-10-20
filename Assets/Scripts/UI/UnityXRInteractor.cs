using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CraftSystem.Core;

public class UnityXRInteractor : MonoBehaviour, IXRInteractor
{
    [Header("UI Controls")]
    [SerializeField] private Button craftButton;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject loadingIndicator;

    private IAIController _controller;

    private void Start()
    {
        // Точка сборки зависимостей (Composition Root)
        IInputProcessor inputProcessor = new PromptInputProcessor();
        IPredictionService predictionService = new HardcodedDummyAiService();
        IDecisionMaker decisionMaker = new CraftDecisionMaker();

        _controller = new CraftAIController(inputProcessor, predictionService, decisionMaker, this);

        if (craftButton != null)
        {
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }
    }

    private async void OnCraftButtonClicked()
    {
        var ingredients = new List<ItemData>
        {
            new ItemData("1", "Ржавый меч", 1, new List<string> { "Старый" }),
            new ItemData("2", "Волчий клык", 2, new List<string> { "Острый" })
        };

        await _controller.ProcessCraftAsync(ingredients);
    }

    public List<ItemData> GetSelectedItems() => new();

    public void SetLoadingState(bool isLoading)
    {
        if (loadingIndicator != null) loadingIndicator.SetActive(isLoading);
        if (craftButton != null) craftButton.interactable = !isLoading;
    }

    public void DisplayResult(CraftResult result)
    {
        if (resultText != null)
        {
            resultText.text = $"Результат: {result.ItemName}\nТир: {result.Tier}\nОписание: {result.Description}";
        }
        Debug.Log($"[Craft System] Скрафчен предмет: {result.ItemName}");
    }
}