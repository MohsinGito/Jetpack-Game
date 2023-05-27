using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour
{
    public float fadeSpeed = 0.1f;
    public float maxEmission = 20f; // max emission you want

    private ParticleSystem ps;
    private Coroutine fadeCoroutine;

    void Start()
    {
        // Get all ParticleSystems in this object and its children
        ps = GetComponent<ParticleSystem>();
    }

    public void Fade(bool fadeIn)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeParticles(fadeIn));
    }

    IEnumerator FadeParticles(bool fadeIn)
    {
        float targetEmission = fadeIn ? maxEmission : 0f;
        bool completed = true;
        var emission = ps.emission;

        while (true)
        {
            float newRate = Mathf.MoveTowards(emission.rateOverTime.constant, targetEmission, fadeSpeed * Time.deltaTime);
            emission.rateOverTime = newRate;

            if (newRate != targetEmission)
                completed = false;

            if (completed) break;

            yield return null;
        }
    }
}