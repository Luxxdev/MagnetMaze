using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
   public Sound[] sfxs;
   // Update is called once per frame
   void Awake()
   {
      foreach (Sound audio in sfxs)
      {
         audio.source = gameObject.AddComponent<AudioSource>();
         audio.source.clip = audio.clip;
         audio.source.volume = audio.volume;
         audio.source.pitch = audio.pitch;
         audio.source.loop = audio.loop;

      }
   }
   public void Play(string name)
   {
      Sound audio = Array.Find(sfxs, som => som.name == name);
      if (audio == null)
      {
         return;
      }
      audio.source.Play();
   }
}
