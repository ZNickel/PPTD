using Source.Event;
using UnityEngine;

namespace Source.Game.Controllers
{
    public class ResourceController : MonoBehaviour
    {
        public int Coins { get; private set; }

        public int Power { get; private set; }

        public int Health { get; private set; }

        public void Setup(int startCoins, int startPower, int startHealth)
        {
            ChangeCoin(startCoins);
            ChangePower(startPower);
            ChangeHealth(startHealth);
        }

        public bool ChangeCoin(int value)
        {
            if (value < 0 && Mathf.Abs(value) > Coins) return false;
            Coins += value;
            UIEventBus.Instance.Trigger_EventCoinCountChanged(Coins);
            return true;
        }

        public bool ChangePower(int value)
        {
            if (value < 0 && Mathf.Abs(value) > Power) return false;
            Power += value;
            UIEventBus.Instance.Trigger_EventPowerCountChanged(Power);
            return true;
        }

        public void ChangeHealth(int value)
        {
            Health = Mathf.Max(Health + value, 0);
            UIEventBus.Instance.Trigger_EventHealthCountChanged(Health);

            if (Health <= 0)
                UIEventBus.Instance.Trigger_EventGameOver(false);
        }
    }
}