using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.I.Scripts.Menu
{
    public class MenuHandler : MonoBehaviour
    {
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
