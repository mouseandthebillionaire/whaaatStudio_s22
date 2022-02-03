using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunerManager : MonoBehaviour
{
    // Variables for the WaveDisplays
    // One for now, but possible add more later
    public LineRenderer[] tuningDisplays;
    public int points;
    public float amplitude = 1;
    public float freq = 1;
    public float moveSpeed;

    // Circle Image
    public SpriteRenderer[] indicators;
    public float rAmt, gAmt; 
    
    // Text Sprites
    public SpriteRenderer tuning;

    // Sound Control Variables
    public AudioSource voice, noise;
    public float pitch, filter, distortion, noiseAmt;
    public float[] noiseLocs = new float[3];
    public float[] noiseAmts = new float[3];
    private AudioHighPassFilter hpf;
    private AudioReverbFilter arf;
    
    // Input Variables
    private float knobVal, sliderVal, crankVal, button;
    
    // Start is called before the first frame update
    void Start() {
        hpf = voice.GetComponent<AudioHighPassFilter>();
        arf = voice.GetComponent<AudioReverbFilter>();
        hpf.cutoffFrequency = 2200;
        noiseAmt = 1;
        noise.volume = 1;
        for (int i = 0; i < noiseLocs.Length; i++) {
            noiseLocs[i] = UnityEngine.Random.Range(0, 1f);
            //sliders[i].value = UnityEngine.Random.Range(0, 1f);
            // Eventually set waves to random value based on this
        }
        
        // assign random number to the crank
        crankVal = UnityEngine.Random.Range(0, 1f);
        tuning.enabled = true;
    }

    // Get Inputs from Device
    public void GetInputs()
    {
        if (Input.GetKey(GlobalVariables.S.leftSlider) && (sliderVal > 0)) sliderVal -= .001f;
        if (Input.GetKey(GlobalVariables.S.rightSlider) && (sliderVal < 1)) sliderVal += .001f;

        if (Input.GetKey(GlobalVariables.S.upCrank) && (crankVal > 0)) crankVal -= .001f;
        if (Input.GetKey(GlobalVariables.S.downCrank) && (crankVal < 1)) crankVal += .001f;
        
        if (Input.GetKey(GlobalVariables.S.knobLeft) && (knobVal > 0)) knobVal -= .001f;
        if (Input.GetKey(GlobalVariables.S.knobRight) && (knobVal < 1)) knobVal += .001f;

        if (Input.anyKey) {
            tuning.color = new Color(.93f, .98f, .37f);
        }
        else {
            tuning.color = new Color(1, 1, 1);
        }

    }
    
    // Update is called once per frame
    void DrawSine()
    {
        float xStart = 0;
        float Tau = 2 * Mathf.PI;
        float xFinish = Tau;

        for (int i = 0; i < tuningDisplays.Length; i++) {
            tuningDisplays[i].positionCount = points;
            for (int j = 0; j < points; j++) {
                float progress = (float) j / (points - 1);
                float x        = Mathf.Lerp(xStart, xFinish, progress);
                float y        = amplitude * Mathf.Sin((Tau * freq * x * (i + 1)) + Time.time * moveSpeed );
                tuningDisplays[i].SetPosition(j, new Vector3(x, y, 0));
            }
        }
    }

    void Update()
    {
        GetInputs();
        
        // Update SineWaves
        amplitude = sliderVal * 3f;
        freq = crankVal * 2f;
        moveSpeed = 1 + (20 * knobVal);
        
        // Update Noise Amounts Via Input Positions
        noiseAmts[0] = Mathf.Abs(sliderVal - noiseLocs[0]);
        noiseAmts[1] = Mathf.Abs(crankVal - noiseLocs[1]);
        noiseAmts[2] = Mathf.Abs(knobVal - noiseLocs[2]);

        // Update Sounds
        hpf.cutoffFrequency = noiseAmts[0] * 2200;
        voice.pitch = 1 + (noiseAmts[1] * 3f);
        arf.dryLevel = 0 - (noiseAmts[2] * 10000);
        arf.reverbLevel = 0 + (noiseAmts[2] * 500);
        arf.room = -10000 + (noiseAmts[2] * 10000);

        noiseAmt = (noiseAmts[0] + noiseAmts[1] + noiseAmts[2]);
        noise.volume = noiseAmt;
        
        // Update the Indicators
        for (int i = 0; i < indicators.Length; i++) {
            indicators[i].color = new Color(1 - noiseAmts[i], 1 - noiseAmts[i], 01 - noiseAmts[i]);
        }

        DrawSine();

        if (noiseAmt < 0.3f) {
            tuning.enabled = false;
        }
        else {
            tuning.enabled = true;
        } 

    }
}