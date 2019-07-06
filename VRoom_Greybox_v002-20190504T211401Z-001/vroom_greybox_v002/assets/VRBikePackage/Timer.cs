using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    #region Variables

    private bool gameEnded = false;
    [HideInInspector]
    public bool bikeCalibrated = false;

    public float gameDuration = 3.0f;
    private float gameTime;

    //public GameObject fadeSphere;
    //public Material fadeMaterial;
    public Renderer rend;
    public float fadeTime = 2.0f;

    public GameObject endGameTextAnimation;

    #endregion

    #region Unity Methods

    private void Start()
    {
        gameTime = 0f;
        StartCoroutine(TimerUpdate());
        //fadeMaterial = fadeSphere.gameObject.GetComponent<Material>();
    }

    private IEnumerator TimerUpdate()
    {
        yield return new WaitUntil(() => bikeCalibrated);
        while(gameTime < gameDuration)
        {
            gameTime += Time.deltaTime;
            yield return null;
        }
        EndGame();
    }

    /*
     * Performs the actions indicating the game has ended
     */
    private void EndGame()
    {
        gameEnded = true;

        StartCoroutine(FadeOutTest());

        StartCoroutine(WaitToActivateAnimation(endGameTextAnimation));

        Debug.Log("End Game");
    }


    /*
     * Change alpha on black sphere to fade out for VR scene
     */
    private IEnumerator FadeOutTest()
    {
        Debug.Log("FadeOutTest Started");

        for(float f = 0f; f <= 1; f += 0.05f)
        {
            Color fadeColor = rend.material.color;
            fadeColor.a = f;
            rend.material.color = fadeColor;
            yield return new WaitForSeconds(fadeTime/20f);
        }
    }


    /*
     * Activate a gameObject after a designated amount of time
     */
    private IEnumerator WaitToActivateAnimation(GameObject objectToActivate)
    {
        yield return new WaitForSeconds(fadeTime);

        objectToActivate.SetActive(true);
    }

    #endregion
}
