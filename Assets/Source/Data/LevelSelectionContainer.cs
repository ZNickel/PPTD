using UnityEngine;

namespace Source.Data
{
    [CreateAssetMenu(menuName = "Game/Runtime/Selected Level")]
    public class LevelSelectionContainer : ScriptableObject
    {
        public LevelData lvlData;
    }
}