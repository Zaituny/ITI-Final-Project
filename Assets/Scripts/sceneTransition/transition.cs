using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transition : MonoBehaviour
{
    transition mytransition;
    private void Awake()
    {
        mytransition = GetComponent<transition>();
    }
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void level1()
    {
        SceneManager.LoadScene(2);
    }
    public void level2()
    {
        SceneManager.LoadScene(3);
    }
    public void level3()
    {
        SceneManager.LoadScene(4);
    }
    public void settings()
    {
        SceneManager.LoadScene(5);
    }
}
