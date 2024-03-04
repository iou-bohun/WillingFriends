using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSourceObject : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip;
    public float voulme;

    [SerializeField]
    private bool isActiveTrigger = false;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
    private void OnEnable()
    {
        isActiveTrigger = true;
    }
    public void Return()
    {
        audioSource.Stop();
        ClearSetting();
        SoundManager.Instance.ReturnAudioSource(gameObject);
    }
    private void ClearSetting()
    {
        //audioSource = null;
        audioSource.clip = null;
        clip = null;
        isActiveTrigger = false;
        voulme = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (isActiveTrigger)
        {
            audioSource.clip = clip;
            audioSource.volume = voulme;
            audioSource.Play();
            isActiveTrigger = false;
            
        }
        if (!audioSource.isPlaying)
            Return();
        //if (gameObject.activeSelf)
        //{
        //    if (!audioSource.isPlaying)
        //        Return();
        //}
        //else
        //{
        //    Return();
        //}
    }
}
