using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Image target;
    public Sprite normal,
        highlight,
        click;
    private AudioSource fxSource;
    public AudioClip buttonClick,
        buttonHover;
    private void OnEnable()
    {
        fxSource= GetComponentInParent<AudioSource>();
    }
    public void ButtonHighlight()
    {
        fxSource.PlayOneShot(buttonHover);
        target.sprite = highlight;
    }
    public void ButtonClick()
    {
        if (!fxSource.isPlaying)
            fxSource.PlayOneShot(buttonClick);
        target.sprite = click;
    }
    public void Normal()
    {
        target.sprite = normal;
    }
}
