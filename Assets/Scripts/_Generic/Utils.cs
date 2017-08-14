using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using UnityEngine;

public static class Utils
{

    /// <summary>
    /// Finds numbers in a string (Level-1) and increments it (Level-2)
    /// </summary>
    /// <param name="_stringToFind"> The string to find </param>
    /// <returns> the incremented string </returns>
    public static string FindStringAndIncrementNumber(string _stringToFind)
    {

        // Find the first parentheses and delete everything
        // that follows after it
        int index = _stringToFind.IndexOf('-');

        // Find all numbers in the string and store it
        string number = Regex.Match(_stringToFind, @"\d+").Value;

        // parse the number from the string and add 1 to it
        int num = int.Parse(number) + 1;

        // Remove the number at the top
        _stringToFind = _stringToFind.Remove(index + 1);

        _stringToFind += (num);

        return _stringToFind;

    }

    /// <summary>
    /// Finds numbers in a string (Level-1) and increments it (Level-2)
    /// </summary>
    /// <param name="_stringToFind"> The string to find </param>
    /// <returns> the incremented string </returns>
    public static string FindAndIncrementNumberInString(string _stringToFind)
    {

        // Find all numbers in the string and store it
        string number = Regex.Match(_stringToFind, @"\d+").Value;

        // parse the number from the string and add 1 to it
        int num = int.Parse(number) + 1;

        return (num.ToString());

    }

    public static void Fade(CanvasGroup canvasGroup, bool fadeIn, float duration)
    {

        if (fadeIn)
        {

            canvasGroup.DOFade(1, duration);
            canvasGroup.blocksRaycasts = true;

        }
        else
        {

            canvasGroup.DOFade(0, duration);
            canvasGroup.blocksRaycasts = false;

        }

    }

}

// -------------------------------------------------------------------------------------------
[System.Serializable]
public enum Clip
{
    moveLeft,
    moveRight,
    slide,
    hit,
    triggerButton,
    pause

}

// -------------------------------------------------------------------------------------------
[System.SerializableAttribute]
public enum GameState
{
    LoadingLevel,
    Play,
    Paused,
    LevelComplete,
}