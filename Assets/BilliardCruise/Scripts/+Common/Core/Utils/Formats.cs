using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class Formats 
{
    public static string Resources(ResourcePack resources)
    {
        List<string> result = new List<string>();
        foreach(ContentObject<IResource> res in resources)
        {
            result.Add($"<sprite name={res.entity}>{Formats.Number(res.count)}");
        }
        return string.Join(" ", result.ToArray());
    }
    
    public const double fDay = 60 * 60 * 24;
    public const double fHour = 60 * 60;
    public const double fMinute = 60;

    public static string HumanTime(double seconds)
    {

        if (seconds > fDay)
        {
            int d = (int)(seconds / fDay);
            int h = (int)((seconds - (d * fDay)) / fHour);
            return d + Localization.Translate("time.d") + " " + h + Localization.Translate("time.d");
        }
        else if (seconds > fHour)
        {
            int h = (int)(seconds / fHour);
            int m = (int)((seconds - (h * fHour)) / fMinute);
            return h + Localization.Translate("time.h") + " " + m + Localization.Translate("time.m");
        }
        else if (seconds > fMinute)
        {
            int m = (int)(seconds / fMinute);
            int s = (int)(seconds - (m * fMinute));
            return m + Localization.Translate("time.m") + " " + s + Localization.Translate("time.s");
        }
        else
        {
            if ((int)seconds < 1)
            {
                return "";
            }
            return ((int)seconds) + Localization.Translate("time.s");
        }
    }

    /**
     * Format number as "1 000 000"
     */ 
    public static string NumberFull(double score)
    {
        return string.Format("{0:n0}", score);
    }

    /**
     * Format number like "1.5K", "123", "1.7AB"
     */
    public static string Number(double Score)
    {
        string result;
        string[] ScoreNames = new string[] { "", "K", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
        int i;

        for (i = 0; i < ScoreNames.Length; i++)
            if (Score < 900)
                break;
            else Score = System.Math.Floor(Score / 100f) / 10f;

        if (Score == System.Math.Floor(Score))
            result = Score.ToString() + ScoreNames[i];
        else result = Score.ToString("F1") + ScoreNames[i];
        return result.Replace(",", ".");
    }

    public static string TwoNumbers(double a, double b)
    {
        if (a >= b) // enough
        {
            return Number(b);
        }
        else // not enough
        {
            return "<color=#fcc>" + Number(a) + "</color>/" + Number(b);
        }
    }

    public static string Multiplier(double m)
    {
        if (m < 1)
        {
            return "-" + (100 - (int)(m * 100f)) + "%";
        }
        else
        {
            return "+" + ((int)((m - 1) * 100f)) + "%";
        }
    }

    public static string FormatTime(int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
    }

    public static string FormatTimeShort(int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    }

    public static string FormatTime(double seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
    }
}