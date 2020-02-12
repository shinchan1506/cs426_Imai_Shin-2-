using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadOnClick : MonoBehaviour {

   // public GameObject loadingImage;
    public GameObject Button;
    public GameObject park;
    public GameObject food;
    public GameObject desert;
    public GameObject Title;
    public Canvas canvas;

    private Vector3 buttonStart;
    private Vector3 parkStart;
    private Vector3 foodStart;
    private Vector3 desertStart;
    private Vector3 titleStart;
    private float mtimer;
    private Vector3[] targetPos;
    private bool done = false;
    public void Start() {
        
        Invoke("tweenRight", 3f);
    }
    public void tweenRight() {
        float x = 33;
        mtimer = 0.0f;
        Vector3 transformer = new Vector3(x,0,0);
        buttonStart = Button.transform.position;
        parkStart = park.transform.position;
        foodStart = food.transform.position;
        desertStart = desert.transform.position;
        titleStart = Title.transform.position;
        targetPos = new Vector3[] {buttonStart + transformer, parkStart + transformer, foodStart + transformer, desertStart + transformer, titleStart + transformer};
        done = true;

        foreach (Animator a in Button.GetComponentsInChildren<Animator>()) {
            a.SetFloat("walking", 0.1f);
        }
    }

    public void Update() {
        if (!done)
            return;

        mtimer += Time.deltaTime;
        Button.transform.position = Vector3.Lerp(buttonStart, targetPos[0], mtimer);
        park.transform.position = Vector3.Lerp(parkStart, targetPos[1], mtimer);
        food.transform.position = Vector3.Lerp(foodStart, targetPos[2], mtimer);
        desert.transform.position = Vector3.Lerp(desertStart, targetPos[3], mtimer);
        Title.transform.position = Vector3.Lerp(titleStart, targetPos[4], mtimer);

        if (Title.transform.position.x - titleStart.x > 33f) {
            foreach (Animator a in Button.GetComponentsInChildren<Animator>()) {
            a.SetFloat("walking", -0.1f);
            }
        }
    }

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