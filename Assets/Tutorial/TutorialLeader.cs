using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLeader : MonoBehaviour
{
    [SerializeField] private GameObject lastObj;

    private void OnEnable()
    {
        //Time.timeScale = 0.1f;
    }

    public void Skip()
    {
        StartCoroutine(Wait());
        //Time.timeScale = 1;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        lastObj.SetActive(false);
        gameObject.SetActive(false );
    }

}
