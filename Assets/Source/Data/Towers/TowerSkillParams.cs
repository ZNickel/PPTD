using UnityEngine;

namespace Source.Data.Towers
{
    [CreateAssetMenu(fileName = "TowerSkill", menuName = "Entities/Tower Skill")]
    public class TowerSkillParams : ScriptableObject
    {
        [Header("Main"), SerializeField] private string skillName;
        [SerializeField] private TowerSkillType skillType;
        [Header("Cast")] [SerializeField] private int clickCount = 32;
        [SerializeField] private float clickTime = 5f;
        [SerializeField] private int powerCost = 10;
        [Header("TTP")] [SerializeField] private float minPower = 0f;
        [SerializeField] private float maxPower = 1f;
        [SerializeField] private float minTime = 0f;
        [SerializeField] private float maxTime = 1f;
        [SerializeField] private int minCount = 0;
        [SerializeField] private int maxCount = 1;

        public float Power(float perf) => Mathf.Lerp(minPower, maxPower, perf);
        public float Time(float perf) => Mathf.Lerp(minTime, maxTime, perf);
        public (int, int) Count() => (minCount, maxCount);
        
        public string SkillName => skillName;
        public TowerSkillType SkillType => skillType;
        public int ClickCount => clickCount;
        public float ClickTime => clickTime;
        public int PowerCost => powerCost;
    }
}