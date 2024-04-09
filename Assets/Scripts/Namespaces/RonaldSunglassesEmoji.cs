using UnityEngine;

namespace RonaldSunglassesEmoji.Interaction
{
    public interface IInteractable
    {
        void Interact(GameObject interactObject);
        void Salvage(GameObject interactObject);
        void CreateTooltip(GameObject interactObject);
        void DestroyTooltip(GameObject interactObject);
    }
}

namespace RonaldSunglassesEmoji.Personalities
{
    public enum SporePersonalities
    {
        Energetic,
        Lazy,
        Friendly,
        Curious,
        Playful
    }
}
