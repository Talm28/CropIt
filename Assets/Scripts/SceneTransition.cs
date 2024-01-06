using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    Animator animator;

    [SerializeField] private AudioClip woshIn;
    [SerializeField] private AudioClip woshOut;
    [SerializeField] private GameObject leafEffectPrefab;
    private GameObject effectObj;

    void Start()
    {
        animator = GetComponent<Animator>();
        AudioManager.instence.PlaySound(woshIn);
    }

    public void EndScene(string nextScene)
    {
        // Get mouse position
        Vector2 mousePosition = GameUtils.GetyMousePos();
        // Instantiate effect
        effectObj = Instantiate(leafEffectPrefab, mousePosition, Quaternion.identity);
        effectObj.GetComponent<ParticleSystem>().Play();
        
        Time.timeScale = 1f;
        animator.SetBool("IsEnd", true);
        AudioManager.instence.PlaySound(woshOut);
        StartCoroutine(LoadNewScene(nextScene));    
    }

    IEnumerator LoadNewScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        Destroy(effectObj);
        SceneManager.LoadScene(sceneName);
    }
}
