using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //���N���b�N
        {
            SoundManager.Play("shot"); //�T�E���h�}�l�[�W���[���g�p���Č��ʉ��Đ�
        }
        if (Input.GetMouseButtonDown(1)) //�E�N���b�N
        {
            SoundManager.Play("hit"); //�T�E���h�}�l�[�W���[���g�p���Č��ʉ��Đ�
        }
    }
}