using System.Collections.Generic;
using Source.Data.Towers;
using Source.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.TowerInfo
{
    public class TowerInfoLarge : MonoBehaviour
    {
        private static readonly int HashOpen = Animator.StringToHash("Open");
        [Header("Editor"), SerializeField] private GameObject levelButton;
        
        [Header("Struct"), SerializeField] private Image towerIcon;
        [SerializeField] private Image skillIcon;
        [SerializeField] private GameObject lvlSelectContainer;
        
        [Header("TTP"), SerializeField] private TMP_Text tName;
        [SerializeField] private TMP_Text damage;
        [SerializeField] private TMP_Text range;
        [SerializeField] private TMP_Text atkSpeed;
        [SerializeField] private TMP_Text projectileSpeed;
        [SerializeField] private TMP_Text hp;

        [Header("Skill"), SerializeField] private TMP_Text skillName;
        [SerializeField] private TMP_Text skillType;
        [SerializeField] private TMP_Text skillDamage;
        [SerializeField] private TMP_Text skillRadius;
        [SerializeField] private TMP_Text skillCount;
        [SerializeField] private TMP_Text skillTime;
        [SerializeField] private TMP_Text skillClickCount;
        [SerializeField] private TMP_Text skillClickTime;

        private Animator _animator;
        private TowerData _data;
        private int _level;
        private TowerStats[] _stats;
        private List<TowerInfoLevelBtn> _buttons;

        private bool _isOpened;

        private int Level
        {
            get => _level;
            set
            {
                _level = value;
                RedrawStats();
            }
        }

        private TowerStats Stats => _stats[_level];

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            UIEventBus.Instance.OpenTowerInfoLarge.AddListener(Setup);
        }

        private void Setup(TowerData data, int level = 0)
        {
            _level = level;
            _data = data;
            _stats = new TowerStats[data.LevelCount];
            for (var i = 0; i < data.LevelCount; i++) _stats[i] = data.GetStats(i);
            RedrawStats();
            SetupLevelSelectionBtn();
            _animator.SetBool(HashOpen, true);
            _isOpened = true;
        }

        private void SetupLevelSelectionBtn()
        {
            while (_buttons.Count < _data.LevelCount)
            {
                var btn = Instantiate(levelButton, lvlSelectContainer.transform);
                _buttons.Add(btn.GetComponent<TowerInfoLevelBtn>());
            }
            
            for (var i = 0; i < _buttons.Count; i++)
            {
                var active = i < _data.LevelCount;
                _buttons[i].gameObject.SetActive(active);
                if (active) _buttons[i].Setup(i, Handle_LvlJump);
            }

            var bs = _buttons[0].GetComponent<RectTransform>().sizeDelta;
            var rtCont = lvlSelectContainer.GetComponent<RectTransform>();
            rtCont.sizeDelta = bs * new Vector2(_data.LevelCount, 1);
        }
        
        private void RedrawStats()
        {
            tName.text = _data.Tname;
            damage.text = $"{Stats.Damage:N1}";
            range.text = $"{Stats.Range:N1}";
            atkSpeed.text = $"{Stats.AtkSpeed:N1}";
            projectileSpeed.text = $"{Stats.ProjectileSpeed:N1}";
            hp.text = $"{Stats.Hp:N1}";

            skillName.text = _data.Skill.SkillName;
            skillType.text = _data.Skill.SkillType.ToString();
            skillDamage.text = DeltaFormat(_data.Skill.GetDamageDelta(_level));
            skillRadius.text = DeltaFormat(_data.Skill.GetRadiusDelta(_level));
            skillRadius.gameObject.SetActive(_data.Skill.ShowRadius);
            skillCount.text = DeltaFormat(_data.Skill.GetCountDelta(_level));
            skillCount.gameObject.SetActive(_data.Skill.ShowCount);
            skillTime.text = DeltaFormat(_data.Skill.GetTimeDelta(_level));
            skillTime.gameObject.SetActive(_data.Skill.ShowTime);
            skillClickCount.text = $"{_data.Skill.ClickCount:N1}";
            skillClickTime.text = $"{_data.Skill.ClickTime:N1}";
        }

        private static string DeltaFormat((float a, float b) delta) =>
            Mathf.Abs(delta.a - delta.b) < .01f ? $"{delta.b:N1}" : $"{delta.a:N1} - {delta.b:N1}";

        public void Handle_LvlDec()
        {
            if (!_isOpened) return;
            Level = Level <= 0 ? _data.LevelCount - 1 : Level - 1;
            Reselect(Level);
        }

        public void Handle_LvlInc()
        {
            if (!_isOpened) return;
            Level = Level >= _data.LevelCount ? 0 : Level + 1;
            Reselect(Level);
        }

        private void Handle_LvlJump(int level)
        {
            if (!_isOpened) return;
            Level = level;
            Reselect(Level);
        }
        
        public void Handle_Close()
        {
            _animator.SetBool(HashOpen, false);
            _isOpened = false;
        }
        
        private void Reselect(int level)
        {
            for(var i = 0; i < _data.LevelCount; i++)
                _buttons[i].Selected = level == i;
        }
    }
}