using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXDriver : MonoBehaviour
{
    public AudioClip[] clips;
    public float spatialBlend = 0.75f;

    private IEnumerator InstantiateSound(AudioClip clip)
    {
        GameObject obj = new GameObject("SFX");
        obj.transform.SetParent(transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = spatialBlend;
        source.Play();
        yield return new WaitForSeconds(clip.length);
        Destroy(obj);
    }

    public void PlayRandomSound()
    {
        int i = Random.Range(0, clips.Length);
        PlaySound(i);
    }

    public void PlaySound(int i)
    {
        StartCoroutine(InstantiateSound(clips[i]));
    }
}
