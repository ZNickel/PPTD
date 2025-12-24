using System;
using UnityEngine;

namespace Source.Game.Entity.Bullets
{
    public class Bullet : UniBullet
    {
        private Vector3 _dir;
        private bool _hit;
        
        protected override void Launch()
        {
            transform.position = From;
            transform.up = _dir = (To - From).normalized;
            _hit = true;
        }

        private void FixedUpdate()
        {
            transform.position += _dir * (Speed * Time.fixedDeltaTime);

            if ((transform.position - From).magnitude > 64f)
            {
                _hit = false;
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_hit) SuccessAction?.Invoke();
        }
    }
}