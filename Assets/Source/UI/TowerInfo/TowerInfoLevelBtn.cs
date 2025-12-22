using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Source.UI.TowerInfo
{
    public class TowerInfoLevelBtn : MonoBehaviour
    {
        [SerializeField] private TMP_Text number;
        [SerializeField] private GameObject border;
        
        private UnityAction<int> _action;
        private int _value;

        private bool _selected;
        
        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected == value) return; 
                number.color = _selected ? Color.green : Color.lightBlue;
                border.SetActive(_selected);
                _selected = value;
            }
        }
        
        public void Setup(int level, UnityAction<int> action)
        {
            _action = action;
            _value = level;
            number.text = $"{level + 1}";
        }

        public void OnClick()
        {
            if (Selected) return;
            _action?.Invoke(_value);
        }
    }
}