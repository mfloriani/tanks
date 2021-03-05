using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlaySkirmish()
    {
        SceneManager.LoadScene("Skirmish_demo");
    }

    public void PlayBattleRoyale()
    {
        SceneManager.LoadScene("Battle_royale_demo");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
