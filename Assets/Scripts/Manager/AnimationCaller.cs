using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{
    public static AnimationCaller Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #region Menu animation ended
    public event Action OnMenuAnimationEnded = null;
    public void MenuAnimationEnded() => OnMenuAnimationEnded?.Invoke();
    #endregion

}
