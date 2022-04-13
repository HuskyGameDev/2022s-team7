using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    public Animator transition;
    public Animator sceneFade;

    IEnumerator LoadLevel ()
    {
        transition.SetTrigger("elevate");
        yield return new WaitForSeconds(1);              
        sceneFade.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1);        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
        StartCoroutine(LoadLevel());        
    }
}
}
