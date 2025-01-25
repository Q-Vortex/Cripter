using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class buttonFunc : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
