using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventTwo : MonoBehaviour
{
    public Transform player;

    public Transform playerpoint;

    public Transform bar;

    public Transform leftpoint;
    public Transform rightPoint;
    private Transform currentPoint;

    public Transform enterArea;

    int maxRange = 65;

    public float pointSwitchTime;
    private float currentTime;

    public int playerSpeed;
    public int playerPointspeed;
    public float smoothTime;
    private Vector3 velocity = Vector3.zero;

    public float temperatur;

    void Start()
    {
        //gameObject.SetActive(false);

        float r = Random.RandomRange(-maxRange, maxRange);
        enterArea.transform.position = new Vector3(r, enterArea.position.y, enterArea.position.z);

        currentPoint = leftpoint;
        currentTime = pointSwitchTime;
    }

    private void Update()
    {
        playerpoint.position = Vector3.MoveTowards(playerpoint.position, currentPoint.position, playerPointspeed * Time.deltaTime);

        //player.position = Vector3.SmoothDamp(player.position, playerpoint.position, ref velocity, smoothTime * Time.deltaTime, playerPointspeed);

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            if (currentPoint == leftpoint)
            {
                currentPoint = rightPoint;
            }
            else
            {
                currentPoint = leftpoint;
            }
            currentTime = pointSwitchTime;
        }
    }

    private void OnFireButton()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, player.forward, out hit, 10f))
        {
            if (hit.collider.gameObject.name == enterArea.gameObject.name)
            {
                Debug.Log("Win");

                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Lose");
                this.gameObject.SetActive(false);
            }
        }
    }


    private void OnColdValue()
    {
        temperatur -= 1;
    }
    private void OnHotValue()
    {
        temperatur += 1;
    }

    private void OnTemperatur(InputValue value)
    {
        temperatur += value.Get<float>();
        Debug.Log(value.ToString());

        player.position += new Vector3(value.Get<float>(), 0, 0);
    }
}
