using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeConversion
{
    public static string ToTime(float time)
    {
        int hours = (int)time / 3600;
        int minutes = (int)time % 3600 / 60;
        int seconds = (int)time % 3600 % 60;

        return hours >= 1 ? $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}" : $"{minutes.ToString("00")}:{seconds.ToString("00")}";
    }
}
