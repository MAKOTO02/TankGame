using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField]
    private SoundManager soundManager; //サウンドマネージャー

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //左クリック
        {
            soundManager.Play("shot"); //サウンドマネージャーを使用して効果音再生
        }
        if (Input.GetMouseButtonDown(1)) //右クリック
        {
            soundManager.Play("hit"); //サウンドマネージャーを使用して効果音再生
        }
    }
}