using UnityEngine;

namespace Source.Game.Entity.Bullets
{
    public class BulletBallistic : UniBullet
    {
        [SerializeField] private float arcHeight = 1.5f;

        private float _time;
        private float _duration;

        protected override void Launch()
        {
            transform.position = From;

            var distance = Vector3.Distance(From, To);
            _duration = distance / Speed;
            _time = 0f;
        }

        private void Update()
        {
            if (_time >= _duration)
            {
                OnHit();
                return;
            }

            _time += Time.deltaTime;
            float t = _time / _duration;

            // Линейная позиция
            Vector3 pos = Vector3.Lerp(From, To, t);

            // Парабола (навес)
            float height = 4f * arcHeight * t * (1f - t);
            pos.y += height;

            // Поворот по направлению движения
            Vector3 nextPos = Vector3.Lerp(From, To, t + 0.01f);
            float nextHeight = 4f * arcHeight * (t + 0.01f) * (1f - (t + 0.01f));
            nextPos.y += nextHeight;

            Vector2 dir = nextPos - pos;
            transform.right = dir;

            transform.position = pos;
        }

        private void OnHit()
        {
            if (Target) Target.Hit(Damage);
            Destroy(gameObject);
        }
    }
}