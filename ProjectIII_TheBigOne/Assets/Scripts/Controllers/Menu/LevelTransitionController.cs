using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransitionController : MonoBehaviour
{
    [SerializeField] private GameObject m_BlackFadeOut;
    [SerializeField] private GameObject m_LoadingScreen;
    [SerializeField] private Slider m_Slider;

    private void Start()
    {
        BlackFadeOut();
    }

    public void BlackFadeOut()
    {
        m_BlackFadeOut.GetComponent<Animator>().SetBool("WhiteFadeOut", true);
    }

    public void FadeToNextScene()
    {
        StartCoroutine(FadeOut()); 
    }

    IEnumerator FadeOut()
    {
        //AsyncOperation l_AsyncLoad = SceneManager.LoadSceneAsync("SCENE_Gold_Composition");
        SceneManager.LoadScene(2);

        //m_LoadingScreen.SetActive(true);

        //while (!l_AsyncLoad.isDone)
        //{
        //    float progress = Mathf.Clamp01(l_AsyncLoad.progress / .9f);

        //    m_Slider.value = progress;

        //    yield return null;
        //}
        yield return null;
    }
}
