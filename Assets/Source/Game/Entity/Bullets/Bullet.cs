using UnityEngine;

namespace Source.Game.Entity.Bullets
{
    public class Bullet : UniBullet
    {
        private Vector3 _dir;
        
        protected override void Launch()
        {
            transform.position = From;
            transform.up = _dir = (To - From).normalized;
        }

        private void FixedUpdate()
        {
            transform.position += _dir * (Speed * Time.fixedDeltaTime);
            
            if ((transform.position - From).magnitude < 3f) Destroy(gameObject);
        }
    }
}