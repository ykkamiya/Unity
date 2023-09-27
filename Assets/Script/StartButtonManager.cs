using UnityEngine;
using UnityEngine.SceneManagement;
 
public class StartButtonManager : MonoBehaviour {
 
    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        SceneManager.LoadScene("BasementofMain");
    }
 
}