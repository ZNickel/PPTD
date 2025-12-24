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

        [Header("Show in UI")]
        [SerializeField] private bool showRadius = true;
        [SerializeField] private bool showCount = true;
        [SerializeField] private bool showTime = true;

        [Header("Skill TTP"), SerializeField, Range(.1f, 10f)]
        private float damage = 30f;

        [SerializeField, Range(.1f, 10f)] private float radius = 2f;
        [SerializeField, Range(1, 50)] private int count = 3;
        [SerializeField, Range(.001f, 10f)] private float time = 1f;
        [SerializeField, Range(1f, 0f)] private float maxFalloff = 1f;
        [SerializeField, Range(0f, 1f)] private float minFalloff = .25f;

        [Header("Skill Upgrade"), SerializeField, Range(1f, 2f)]
        private float lvlDamage = 1.1f;

        [SerializeField, Range(1f, 2f)] private float lvlRadius = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlCount = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlTime = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlMaxFall = 1.1f;
        [SerializeField, Range(1f, 2f)] private float lvlMinFall = 1.1f;

        [Header("Skill Complete"), SerializeField, Range(.25f, 1f)]
        private float perfDamage = 1f;

        [SerializeField, Range(.25f, 1f)] private float perfRadius = 1f;
        [SerializeField, Range(.25f, 1f)] private float perfCount = 1f;
        [SerializeField, Range(.25f, 1f)] private float perfTime = 1f;
        [SerializeField, Range(.25f, 1f)] private float perfMaxFall = 1f;
        [SerializeField, Range(.25f, 1f)] private float perfMinFall = 1f;

        public bool ShowRadius => showRadius;
        public bool ShowCount => showCount;
        public bool ShowTime => showTime;
        
        public string SkillName => skillName;
        public TowerSkillType SkillType => skillType;
        public int ClickCount => clickCount;
        public float ClickTime => clickTime;
        public int PowerCost => powerCost;
        
        public float GetDamage(int level, float perf) => CalcPerf(damage, lvlDamage, level, perfDamage, perf);
        public float GetRadius(int level, float perf) => CalcPerf(radius, lvlRadius, level, perfRadius, perf);
        public float GetCount(int level, float perf) => CalcPerf(count, lvlCount, level, perfCount, perf);
        public float GetTime(int level, float perf) => CalcPerf(time, lvlTime, level, perfTime, perf);
        public float GetMaxFall(int level, float perf) => CalcPerf(maxFalloff, lvlMaxFall, level, perfMaxFall, perf);
        public float GetMinFall(int level, float perf) => CalcPerf(minFalloff, lvlMinFall, level, perfMinFall, perf);

        public (float, float) GetDamageDelta(int level) => CalcDelta(damage, lvlDamage, level, perfDamage);
        public (float, float) GetRadiusDelta(int level) => CalcDelta(radius, lvlRadius, level, perfRadius);
        public (float, float) GetCountDelta(int level) => CalcDelta(count, lvlCount, level, perfCount);
        public (float, float) GetTimeDelta(int level) => CalcDelta(time, lvlTime, level, perfTime);
        public (float, float) GetMaxFallDelta(int level) => CalcDelta(maxFalloff, lvlMaxFall, level, perfMaxFall);
        public (float, float) GetMinFallDelta(int level) => CalcDelta(minFalloff, lvlMinFall, level, perfMinFall);

        //n0 (* fac)n
        private static float CalcLvl(float baseV, float f, int lvl)
        {
            if (lvl == 0) return baseV;
            var x = baseV;
            for (var i = 1; i < lvl; i++) x *= f;
            return x;
        }

        private static float CalcPerf(float baseV, float lvlF, int lvl, float perfF, float perf)
        {
            var leveled = CalcLvl(baseV, lvlF, lvl);
            return Mathf.Lerp(leveled * perfF, leveled, perf);
        }

        private static (float, float) CalcDelta(float baseV, float lvlF, int level, float perf)
        {
            var leveled = CalcLvl(baseV, lvlF, level);
            return (perf * leveled, leveled);
        }
    }
}