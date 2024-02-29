using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] string _soundTag;
    [SerializeField] GameObject _broken;

    public void Die()
    {
        Debug.Log("게임 오버!");
        SoundManager.Instance.PlayAudioClip(_soundTag);
        _broken.SetActive(true);

        _broken.transform.position = transform.position;
        gameObject.SetActive(false);

        // UI 띄워 주세용
    }
}
