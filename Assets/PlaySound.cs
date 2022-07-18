using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void _PlaySound(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }
}
