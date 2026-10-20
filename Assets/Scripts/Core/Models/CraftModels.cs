using System;
using System.Collections.Generic;

namespace CraftSystem.Core
{
    [Serializable]
    public class ItemData
    {
        public string Id;
        public string Name;
        public int Tier;
        public List<string> Properties;

        public ItemData(string id, string name, int tier, List<string> properties)
        {
            Id = id;
            Name = name;
            Tier = tier;
            Properties = properties ?? new List<string>();
        }
    }

    [Serializable]
    public class CraftResult
    {
        public string ItemName;
        public int Tier;
        public string Description;
        public List<string> Effects;
        public bool IsFallback;

        public CraftResult(string itemName, int tier, string description, List<string> effects, bool isFallback)
        {
            ItemName = itemName;
            Tier = tier;
            Description = description;
            Effects = effects ?? new List<string>();
            IsFallback = isFallback;
        }
    }

    // ¬спомогательный класс дл€ парсинга JSON через JsonUtility в Unity
    [Serializable]
    public class RawAiCraftResponse
    {
        public string Name;
        public int Tier;
        public string Description;
        public string[] Effects;
    }
}