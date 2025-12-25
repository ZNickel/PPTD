using Source.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.UI.Map
{
    public class LevelFrame : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelName;
        [SerializeField] private LevelSelectionContainer selectionContainer;

        private LevelData _data;

        public void Open(LevelData data)
        {
            _data = data;
            levelName.text = $"Level: {data.Index}";
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Play()
        {
            selectionContainer.lvlData = _data;
            SceneManager.LoadScene(1);
        }
    }
}