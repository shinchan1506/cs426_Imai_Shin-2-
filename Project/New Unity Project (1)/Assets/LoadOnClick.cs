using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadOnClick : MonoBehaviour {

   // public GameObject loadingImage;

    public void GoToDesert()
    {
        //loadingImage.SetActive(true);
        SceneManager.LoadScene("desert");
    }

    public void GoToPark()
    {
        SceneManager.LoadScene("efwef");
    }

    public void GoToFood()
    {
        SceneManager.LoadScene("level 1");
    }
}