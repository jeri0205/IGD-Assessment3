using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel : MonoBehaviour
{
    public void loadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
