using System.Collections.Generic;
using System.Linq;
using Source.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private LevelSelectionContainer selector;
        [SerializeField] private Button next;
        [SerializeField] private TMP_Text text;
        
        public void Show(bool win)
        {
            text.text = win ? "Win!" : "Defeat...";
            next.interactable = win;
        }
        
        public void Repeat()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Map()
        {
            SceneManager.LoadScene(2);
        }
        
        public void Next()
        {
            var nextInd = selector.lvlData.Index + 1;
            
            foreach (var ld in LoadAllLevels())
            {
                if (ld.Index == nextInd) selector.lvlData = ld;
                SceneManager.LoadScene(1);
                return;
            }
            SceneManager.LoadScene(0);
        }
        
        private static List<LevelData> LoadAllLevels()
        {
            var levels = Resources.LoadAll<LevelData>("Levels").ToList();
            levels.Sort((a, b) => a.Index.CompareTo(b.Index));
            return levels;
        }
    }
}