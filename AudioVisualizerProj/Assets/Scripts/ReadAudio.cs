using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AudioEditor
{    
    public enum VisualizationMode
    {
        Ring,
        RingWithBeat
    }

    public class ReadAudio : MonoBehaviour
    {
        // create variables for capturing audio data
        private Vector2[] uv = { 
        new Vector2(0,0), 
        new Vector2(0,1), 
        new Vector2(1,1), 
        new Vector2(1,0) 
        };

        public int bufferSampleSize;
        public float samplePercentage;
        public float emphasisMultiplier;
        public float retractionSpeed;
        public float MovementS;
        
        private int amountOfSegments;
        public float radius;
        public float bufferSizeArea;
        public float maximumExtendLength;

        public GameObject lineRendererPrefab;
        public VisualizationMode visualizationMode;

        public Gradient colorGradientA = new Gradient();

        private float sampleRate;

        private float[] samples;
        private float[] spectrum;
        private float[] extendLengths;

        private LineRenderer[] lineRenderers;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sampleRate = AudioSettings.outputSampleRate;

            amountOfSegments = 12;

            samples = new float[bufferSampleSize];
            spectrum = new float[bufferSampleSize];

            switch (visualizationMode)
            {
                case VisualizationMode.Ring:
                    RingStart();
                    break;
            }

            ScriptGrabber();
        }

        public void ScriptGrabber()
        {
            GameObject MovementSpeed = GameObject.Find("CameraOne");
            CameraEdit.MoveForward mf = MovementSpeed.GetComponent<CameraEdit.MoveForward>();
            MovementS = mf.MovementSpeed;
        }

        public void RingStart()
        {
            extendLengths = new float[amountOfSegments + 1];
            lineRenderers = new LineRenderer[extendLengths.Length];

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                GameObject go = Instantiate(lineRendererPrefab);
                go.transform.position = Vector3.zero;

                LineRenderer lineRenderer = go.GetComponent<LineRenderer>();

                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = false;
                lineRenderers[i] = lineRenderer;
                float count =  0.08f * i;
                lineRenderers[i].GetComponent<Renderer>().material.color = Color.HSVToRGB(count,1,1);
            }
        }

        // Update is called once per frame
        public void Update()
        {
            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

            UpdateExtends();

            if (visualizationMode == VisualizationMode.Ring)
            {
                UpdateRing();
            }

        }

        public void UpdateExtends()
        {
            int iteration = 0;
            int indexOnSpectrum = 0;
            int avarageValue = (int) (Mathf.Abs(samples.Length * samplePercentage) / amountOfSegments);

            if (avarageValue < 1)
            {
                avarageValue = 1;
            }

            while (iteration < amountOfSegments)
            {
                int iterationIndex = 0;
                float sumValueY = 0;

                while (iterationIndex < avarageValue)
                {
                    sumValueY += spectrum[indexOnSpectrum];
                    indexOnSpectrum++;
                    iterationIndex++;
                }

                float y = sumValueY / avarageValue * emphasisMultiplier;
                extendLengths[iteration] -= retractionSpeed * Time.deltaTime;

                if (extendLengths[iteration] < y)
                {
                    extendLengths[iteration] = y;
                }

                if (extendLengths[iteration] > maximumExtendLength)
                {
                    extendLengths[iteration] = maximumExtendLength;
                }

                iteration++;
            }
        }

        public void UpdateRing()
        {
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                float t = i / (lineRenderers.Length - 2f);
                float a = t * Mathf.PI * 2f;
                
                Vector2 direction = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                float maximumRadius = (radius + bufferSizeArea + extendLengths[i]);

                lineRenderers[i].SetPosition(0, direction * radius);
                lineRenderers[i].SetPosition(1, direction * maximumRadius);

                lineRenderers[i].startWidth = Spacing(radius);
                lineRenderers[i].endWidth = Spacing(maximumRadius);

                lineRenderers[i].transform.Translate(Vector3.forward * MovementS / 2);
            }
        }

        private float Spacing(float radius)
        {
            float c = 1.5f * Mathf.PI * radius;
            float n = lineRenderers.Length;
            return c / n;
        }
    }
}
