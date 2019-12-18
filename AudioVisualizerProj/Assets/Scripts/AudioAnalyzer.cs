using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioAnalyzer : MonoBehaviour {

    public bool useMic = false;
    public AudioClip clip;
    AudioSource a;
    public AudioMixerGroup amgMic;
    public AudioMixerGroup amgMaster;

    public string selectedDevice;

    public static int frameSize = 512;
    public static float[] spectrum;
    public static float[] bands;

    public float binWidth;
    public float sampleRate;
    
    /*
     * 20-60 - Subbase
     * 60-250 - Bass
     * 250-500 - Low midrange
     * 500 - 2Khz - Midrange
     * 2Khz - 4Khz - Upper midrange
     * 4Khz - 6Khz - Presence
     * 6Khz - 20Khz - Brilliance
     */

    private void Awake()
    {
        a = GetComponent<AudioSource>();
        spectrum = new float[frameSize];
        bands = new float[(int) Mathf.Log(frameSize, 2)];
        
        if (useMic)
        {
            if (Microphone.devices.Length > 0)
            {
                selectedDevice = Microphone.devices[0].ToString();
                a.clip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);

                a.outputAudioMixerGroup = amgMic;
            }
        }
        else
        {
            a.clip = clip;
            a.outputAudioMixerGroup = amgMaster;
        }
        a.Play();
    }

    // Use this for initialization
    void Start () {        
        sampleRate = AudioSettings.outputSampleRate;
        binWidth = AudioSettings.outputSampleRate / 2 / frameSize;
    }
    void GetFrequencyBands()
    {        
        for (int i = 0; i < bands.Length; i++)
        {
            int start = (int)Mathf.Pow(2, i) - 1;
            int width = (int)Mathf.Pow(2, i);
            int end = start + width;
            float average = 0;
            for (int j = start; j < end; j++)
            {
                average += spectrum[j] * (j + 1);
            }
            average /= (float) width;
            bands[i] = average;
            //Debug.Log(i + "\t" + start + "\t" + end + "\t" + start * binWidth + "\t" + (end * binWidth));
        }

    }
    
    
    // Update is called once per frame
    void Update () {
        a.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        GetFrequencyBands();
    }
}
