using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IPMButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Game1()
    {
         SceneManager.LoadScene("1DuckMode");
    }
    public void Game2()
    {
         SceneManager.LoadScene("1DuckMode");
    }

    public void Game3()
    {
        SceneManager.LoadScene("Clayshoot");
    }
    public void IsaiahPM() //This is just for development use.
    {
         SceneManager.LoadScene("IPM Main Menu");
    }
}
