using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Fader : MonoBehaviour
{
    [SerializeField] private bool fadeInOnAwake;
    [SerializeField] private float delay;
    [SerializeField] private float fadeDuration;
    [SerializeField] private UnityEvent afterFadeInCall;
    [SerializeField] private GameObject imageObj;
    private Image image;
    private GameObject niwatoriAnim;
    private GameObject niwatoriAnimCamera;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        niwatoriAnim = transform.GetChild(1).gameObject;
        niwatoriAnimCamera = transform.GetChild(2).gameObject;

        if (fadeInOnAwake)
        {
            StartCoroutine(FadeInCor(delay, fadeDuration, () =>
            {
                afterFadeInCall.Invoke();
            }));
        }
    }

    public void FadeIn(float delay, float fadeDuration, UnityAction afterFadeInCall)
    {
        StartCoroutine(FadeInCor(delay, fadeDuration, afterFadeInCall));
    }

    private IEnumerator FadeInCor(float delay, float fadeDuration, UnityAction afterFadeInCall)
    {
        niwatoriAnimCamera.SetActive(true);
        niwatoriAnim.SetActive(true);
        yield return new WaitForSeconds(delay);
        niwatoriAnimCamera.SetActive(false);
        niwatoriAnim.SetActive(false);
        image.DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        afterFadeInCall.Invoke();
        imageObj.SetActive(false);
    }

    public void FadeIn(float delay, float fadeDuration)
    {
        StartCoroutine(FadeInCor(delay, fadeDuration));
    }

    private IEnumerator FadeInCor(float delay, float fadeDuration)
    {
        niwatoriAnimCamera.SetActive(true);
        niwatoriAnim.SetActive(true);
        yield return new WaitForSeconds(delay);
        niwatoriAnimCamera.SetActive(false);
        niwatoriAnim.SetActive(false);
        image.DOFade(0f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        imageObj.SetActive(false);
    }

    public void FadeOut(float delay, float fadeDuration, UnityAction afterFadeOutCall)
    {
        StartCoroutine(FadeOutCor(delay, fadeDuration, afterFadeOutCall));
    }

    private IEnumerator FadeOutCor(float delay, float fadeDuration, UnityAction afterFadeOutCall)
    {
        yield return new WaitForSeconds(delay);
        imageObj.SetActive(true);
        image.DOFade(1f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        afterFadeOutCall.Invoke();
        niwatoriAnimCamera.SetActive(true);
        niwatoriAnim.SetActive(true);
    }


    public void FadeOut(float delay, float fadeDuration)
    {
        StartCoroutine(FadeOutCor(delay, fadeDuration));
    }

    private IEnumerator FadeOutCor(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);
        imageObj.SetActive(true);
        image.DOFade(1f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        niwatoriAnimCamera.SetActive(true);
        niwatoriAnim.SetActive(true);
    }

    public void FadeOutAndLoadScene(float delay, float fadeDuration, string sceneName)
    {
        StartCoroutine(FadeOutAndLoadSceneCor(delay, fadeDuration, sceneName));
    }

    private IEnumerator FadeOutAndLoadSceneCor(float delay, float fadeDuration, string sceneName)
    {
        yield return new WaitForSeconds(delay);
        imageObj.SetActive(true);
        image.DOFade(1f, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(sceneName);
    }

    public static void StartFadeIn(float delay, float fadeDuration, UnityAction afterFadeInCall)
    {
        GameObject.FindWithTag("Fader").GetComponent<Fader>().FadeIn(delay, fadeDuration, afterFadeInCall);
    }

    public static void StartFadeOut(float delay, float fadeDuration, UnityAction afterFadeOutCall)
    {
        GameObject.FindWithTag("Fader").GetComponent<Fader>().FadeOut(delay, fadeDuration, afterFadeOutCall);
    }

    public static void StartFadeIn(float delay, float fadeDuration)
    {
        GameObject.FindWithTag("Fader").GetComponent<Fader>().FadeIn(delay, fadeDuration);
    }

    public static void StartFadeOut(float delay, float fadeDuration)
    {
        GameObject.FindWithTag("Fader").GetComponent<Fader>().FadeOut(delay, fadeDuration);
    }

    public static void StartFadeOutAndLoadScene(float delay, float fadeDuration, string sceneName)
    {
        GameObject.FindWithTag("Fader").GetComponent<Fader>().FadeOutAndLoadScene(delay, fadeDuration, sceneName);
    }
}
