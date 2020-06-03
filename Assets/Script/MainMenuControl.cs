using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuControl : MonoBehaviour
{
    //GameObject level1, level2, level3;
    //GameObject lock1, lock2, lock3;
    GameObject levels,locks;
    void Start()
    {
        levels = GameObject.Find("Levels");
        locks = GameObject.Find("Locks");
         //PlayerPrefs.DeleteAll();

        for (int i=0; i < levels.transform.childCount; i++)
        {
            levels.transform.GetChild(i).gameObject.SetActive(false);
        }
        for(int i=0; i < locks.transform.childCount; i++)
        {
            locks.transform.GetChild(i).gameObject.SetActive(false);
        }
        

       

        for (int i=0; i < PlayerPrefs.GetInt("whichLevel"); i++)
        {
            levels.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }

    }

    public void SelectButton(int getButton)
    {
        if(getButton == 1)
        {
            
            if(PlayerPrefs.GetInt("whichLevel") > 1)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("whichLevel"));
            }
            else
            {
                SceneManager.LoadScene(1);
            }
            
        }
        else if (getButton == 2)
        {
            for(int i=0; i<locks.transform.childCount; i++)
            {
                locks.transform.GetChild(i).gameObject.SetActive(true);
            }

            for(int i=0; i< levels.transform.childCount; i++)
            {
                levels.transform.GetChild(i).gameObject.SetActive(true);
            }

            for(int i=0; i < PlayerPrefs.GetInt("whichLevel"); i++)
            {
                locks.transform.GetChild(i).gameObject.SetActive(false);
            }

            


        }
        else if (getButton == 3)
        {
            Application.Quit();
        }
    }

    public void LevelToGo(int levelValue)
    {
        SceneManager.LoadScene(levelValue);
    }

  
}
