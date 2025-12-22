namespace Source.Data.Towers
{
    public class TowerStats
    {
        public float Range {get; private set; }
        public float AtkSpeed {get; private set; }
        public float ProjectileSpeed {get; private set; }
        public float Hp {get; private set; }
        public float Damage {get; private set; }

        public TowerStats(float range, float atkSpeed, float projectileSpeed, float hp, float damage)
        {
            Range = range;
            AtkSpeed = atkSpeed;
            ProjectileSpeed = projectileSpeed;
            Hp = hp;
            Damage = damage;
        }
    }
}