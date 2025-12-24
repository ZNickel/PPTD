using UnityEngine;

namespace Source.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerSkill", menuName = "Entities/Tower Skill")]
    public class TowerSkillParams : ScriptableObject
    {
        [Header("Main"), SerializeField] private string skillName;
        [SerializeField] private TowerSkillType skillType;
        [SerializeField] private int clickCount = 32;
        [SerializeField] private float clickTime = 5f;
        [SerializeField] private int powerCost = 10;

        public string SkillName => skillName;
        public TowerSkillType SkillType => skillType;
        public int ClickCount => clickCount;
        public float ClickTime => clickTime;
        public int PowerCost => powerCost;
    }
}