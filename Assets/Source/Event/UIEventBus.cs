using Source.Data;
using Source.Data.Towers;
using Source.Game.Controllers;
using Source.Game.Entity;
using UnityEngine.Events;

namespace Source.Event
{
    public class UIEventBus
    {
        private static UIEventBus _instance;
        public static UIEventBus Instance => _instance ??= new UIEventBus();

        // public UnityEvent<MouseClickData> EventMouseClick { get; } = new ();
        // public void Trigger_EventMouseClick(MouseClickData d) => EventMouseClick.Invoke(d);
        
        public UnityEvent<float> EventClickPowerChanged { get; } = new ();
        public void Trigger_EventClickPowerChanged(float v) => EventClickPowerChanged.Invoke(v);
        
        public UnityEvent<int> EventCoinCountChanged { get; } = new ();
        public void Trigger_EventCoinCountChanged(int v) => EventCoinCountChanged.Invoke(v);
        
        public UnityEvent<int> EventPowerCountChanged { get; } = new ();
        public void Trigger_EventPowerCountChanged(int v) => EventPowerCountChanged.Invoke(v);
        
        public UnityEvent<int> EventHealthCountChanged { get; } = new ();
        public void Trigger_EventHealthCountChanged(int v) => EventHealthCountChanged.Invoke(v);
        
        public UnityEvent<TowerData> EventSelectShopItem { get; } = new();
        public void Trigger_EventSelectShopItem(TowerData data) => EventSelectShopItem.Invoke(data);
        
        public UnityEvent<TowerController, Tower> EventShowTowerPopup { get; } = new();
        public void Trigger_EventShowTowerPopup(TowerController tc, Tower t) => EventShowTowerPopup.Invoke(tc, t);
        
        public UnityEvent EventHideTowerPopup { get; } = new();
        public void Trigger_EventHideTowerPopup() => EventHideTowerPopup.Invoke();
        
        public UnityEvent<bool> EventGameOver { get; } = new();
        public void Trigger_EventGameOver(bool win) => EventGameOver.Invoke(win);

        #region Tower Info panels
        
        public UnityEvent<TowerData, int> OpenTowerInfoLarge { get; } = new();
        public void Trigger_OpenTowerInfoLarge(TowerData data, int level) => OpenTowerInfoLarge.Invoke(data, level);
        
        public UnityEvent<TowerData, int> OpenTowerInfoSmall { get; } = new();
        public void Trigger_OpenTowerInfoSmall(TowerData data, int level) => OpenTowerInfoSmall.Invoke(data, level);
        
        #endregion
        
        #region Enemy HP bar
        
        public UnityEvent<Enemy> AttachHpBar { get; } = new();
        public void Trigger_AttachHpBar(Enemy e) => AttachHpBar.Invoke(e);
        
        public UnityEvent<Enemy> DetachHpBar { get; } = new();
        public void Trigger_DetachHpBar(Enemy e) => DetachHpBar.Invoke(e);
        
        public UnityEvent<Enemy, float> UpdateEnemyHp { get; } = new();
        public void Trigger_UpdateEnemyHp(Enemy e, float hpNormalized) => UpdateEnemyHp.Invoke(e, hpNormalized);
        
        #endregion
    }
}