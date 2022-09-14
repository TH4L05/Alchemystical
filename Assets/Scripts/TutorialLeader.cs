using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialLeader : MonoBehaviour
{
    public UnityEvent StartEvent;
    public UnityEvent EndEvent;

    [SerializeField] private GameObject lastObj;

    private void OnEnable()
    {
        TriggerStartEvent();
    }

    public void Skip()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        lastObj.SetActive(false);
        gameObject.SetActive(false );
    }

    public void TriggerStartEvent()
    {
       StartEvent?.Invoke();
    }

    public void TriggerEndEvent()
    {
        EndEvent?.Invoke();
    }

}
