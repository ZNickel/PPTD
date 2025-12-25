using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.UI.Hud
{
    public class TempExit : MonoBehaviour
    {
        public void LoadMenu() => SceneManager.LoadScene(0);
    }
}