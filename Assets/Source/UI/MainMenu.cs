using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.UI
{
    //Temp
    public class MainMenu : MonoBehaviour
    {
        //Temp
        public void LaunchFirst() => SceneManager.LoadScene(1);
        
        //Temp
        public void OpenMap() => SceneManager.LoadScene(2);
        
        //Temp
        public void Exit() => Application.Quit();
    }
}