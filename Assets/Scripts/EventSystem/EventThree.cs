using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventThree : MonoBehaviour
{
    int lookDirection;

    [SerializeField]
    GameObject leftObject;
    [SerializeField]
    GameObject rightObject;
    [SerializeField]
    Camera mainCamera;

    GameObject target;

    [SerializeField]
    Material selectetMaterial;

    Material normalMaterial;

    void Start()
    {
        lookDirection = Random.RandomRange(0, 2);
        SetDirection();
    }

    void SetDirection()
    {
        if (lookDirection == 0)
        {
            leftObject.SetActive(true);
            rightObject.SetActive(false);
            target = leftObject;
        }
        else
        {
            leftObject.SetActive(false);
            rightObject.SetActive(true);
            target = rightObject;
        }
    }

    public void OnLookLeft()
    {
        if (lookDirection == 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Lost");
        }
    }

    public void OnLookRight()
    {
        if (lookDirection == 1)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Lost");
        }
    }
}
