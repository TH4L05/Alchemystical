using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotionType
{
    Invalid = -1,

    Clockwise,
    Counterclockwise,

    Count
}

public class Motion : MonoBehaviour
{
    public Action<MotionType> Finished;

    [SerializeField]
    private float splittingSize = 45;

    [SerializeField]
    private float maxDistance = 100;

    [SerializeField]
    private float minDistance = 1;

    [SerializeField]
    private GameObject morter;


    bool readInput = false;

    int currentDirection = 0;

    public int lastSplit = 0;

    void Update()
    {
        if (!readInput) return;
        if(Input.GetMouseButton(0))
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            float angle = Angle(mousePosition - position);

            if ((mousePosition - position).magnitude > maxDistance) return;

            morter.transform.position = Input.mousePosition;

            for (int i = 0; i < Mathf.RoundToInt(360 / splittingSize); i++)
            {
                if (angle > i * splittingSize && angle < (i+1) * (splittingSize))
                {
                    if(i == lastSplit)
                    {
                        break;
                    }
                    else
                    {
                        if (i == (lastSplit + 1) % 8 || ((lastSplit == 7) && (i == 1)))
                        {
                            if (currentDirection > 0)
                            {
                                currentDirection++;
                            }

                            if (currentDirection < 0)
                            {
                                currentDirection = 1;
                            }

                            if (currentDirection == 0)
                            {
                                currentDirection = 1;
                            }

                        }

                        else if (i == (lastSplit - 1) % 8 || ((lastSplit == 0) && (i == 7)))
                        {
                            if (currentDirection < 0)
                            {
                                currentDirection--;
                            }

                            if (currentDirection > 0)
                            {
                                currentDirection = -1;
                            }

                            if (currentDirection == 0)
                            {
                                currentDirection = -1;
                            }
                        }
                        else
                        {
                            currentDirection = 0;
                        }

                        lastSplit = i;
                    }
                }
            }

            if(currentDirection > 8)
            {
                Finished.Invoke(MotionType.Clockwise);
            }

            if (currentDirection < -8)
            {
                Finished.Invoke(MotionType.Counterclockwise);
            }

            //Debug.Log(currentDirection);
        }
    }

    public bool IsSplitSmaller(int start, int end)
    {
        bool value = false;

        if(start < 4 && end > 4)
        {
            if (start + 4 < end) value = true;
        }
        else
        {
            value = start < end;
        }
        return value;
    }

    public void StartInput()
    {
        currentDirection = 0;
        readInput = true;
    }

    public void EndInput()
    {
        readInput = false;
    }

    public static float Angle(Vector2 vector2)
    {
        if (vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }
}
