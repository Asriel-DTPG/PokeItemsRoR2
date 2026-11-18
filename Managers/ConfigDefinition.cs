using BepInEx.Configuration;
using PokeItems.Items;

namespace PokeItems.Managers
{
    // Create a structure for config entries
    internal sealed class ConfigDefinition
    {
        public ConfigEntryBase Entry { get; }
        public float MinValue { get; }
        public float MaxValue { get; }

        public ConfigDefinition(
            ConfigEntryBase entry,
            float minValue = 0f,
            float maxValue = 100f)
        {
            Entry = entry;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
