using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClickDetector
{
    private readonly double clickInterval;
    private double lastClickTime;
    private int clickCount;

    public DoubleClickDetector(double clickInterval = 0.3)
    {
        this.clickInterval = clickInterval;
        lastClickTime = 0.0;
        clickCount = 0;
    }


    public bool IsDoubleClick()
    {
        double currentTime = Time.unscaledTimeAsDouble;

        if (currentTime <= lastClickTime + clickInterval)
        {
            clickCount += 1;
        }
        else
        {
            clickCount = 1;
        }

        lastClickTime = currentTime;

        if (clickCount == 2)
        {
            clickCount = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
