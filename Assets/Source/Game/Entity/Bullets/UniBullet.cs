using UnityEngine;

namespace Source.Game.Entity.Bullets
{
    public abstract class UniBullet : MonoBehaviour
    {
        public Enemy Target { get; private set; }
        public float Damage { get; private set; }
        protected float Speed { get; private set; }
        protected Vector3 From { get; private set; }
        protected Vector3 To { get; private set; }

        public void Setup(Enemy target, Vector3 from, Vector3 to, float speed, float dmg)
        {
            Damage = dmg;
            Speed = speed;
            From = from;
            To = to;
            Target = target;
            Launch();
        }

        protected abstract void Launch();
    }
}