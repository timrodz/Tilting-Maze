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
    public static string FindStringAndReturnIncrementedNumber(string _stringToFind)
    {
        // Find all numbers in the string and store it
        string number = Regex.Match(_stringToFind, @"\d+").Value;

        // parse the number from the string and add 1 to it
        int num = int.Parse(number) + 1;

        return (num.ToString());
    }

    public static void Fade(CanvasGroup _canvasGroup, bool _fadeIn = true, float _duration = 0.35f)
    {
        if (_fadeIn)
        {
            if (_duration == 0)
            {
                _canvasGroup.alpha = 1;
            }
            else
            {
                _canvasGroup.DOFade(1, _duration);
            }
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            if (_duration == 0)
            {
                _canvasGroup.alpha = 0;
            }
            else
            {
                _canvasGroup.DOFade(0, _duration);
            }
            _canvasGroup.blocksRaycasts = false;
        }

    }

}

[System.Serializable]
public class AnimationSettings
{
    [Range(0.0f, 2.0f)]
    public float duration = 1.0f;
    [Range(0.0f, 4.0f)]
    public float delay = 0.0f;
    public Ease ease = Ease.OutQuad;
    public bool animateOnStart = false;
    public bool loop = false;
    public LoopType loopType;
}

[System.Serializable]
public enum EndAction
{
    None,
    Destroy
}

// -------------------------------------------------------------------------------------------
[System.SerializableAttribute]
public enum GameState
{
    NULL,
    LoadingLevel,
    Play,
    Paused,
    LevelComplete,
}

public enum SceneNames
{
    MAIN_MENU = 0,
    LEVEL_SELECT = 1,
    GAME = 2
}