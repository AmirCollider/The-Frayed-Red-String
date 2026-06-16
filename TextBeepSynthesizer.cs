// ==========================================
// TextBeepSynthesizer - Procedural Per-Character Dialogue Beep Matrix (GDD §1.2)
// AmirCollider Games - The Frayed Red String
// ==========================================

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class TextBeepSynthesizer : MonoBehaviour
{
    // ==========================================
    // Waveform - Base Oscillator Shape per Voice
    // ==========================================
    private enum Waveform { Sine = 0, Square = 1, Triangle = 2 }

    // ==========================================
    // Inspector — Source Typewriter (the spoken dialogue typewriter)
    // ==========================================
    [Header("Driver")]
    [SerializeField] private TypewriterEffect typewriter;

    // ==========================================
    // Inspector — Output
    // ==========================================
    [Header("Output")]
    [SerializeField] private AudioSource beepSource;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField][Range(0f, 1f)] private float beepVolume = 0.35f;

    // ==========================================
    // Inspector — Procedural Tone Settings
    // ==========================================
    [Header("Tone Generation")]
    [SerializeField] private float baseFrequency = 440f;
    [SerializeField] private float toneLength = 0.055f;

    // ==========================================
    // Private State — Generated Clips
    // ==========================================
    private AudioClip _sineClip;
    private AudioClip _squareClip;
    private AudioClip _triangleClip;
    private AudioClip _crushedClip;

    // ==========================================
    // Private State — Active Voice Profile
    // ==========================================
    private AudioClip _activeClip;
    private float _pitchMin = 0.98f;
    private float _pitchMax = 1.04f;
    private float _detune = 1f;
    private int _cadence = 2;            // beep every N visible characters
    private bool _microStutter;
    private int _charCounter;

    // ==========================================
    // Awake - Generate Clips, Configure Source, Subscribe to Typewriter
    // ==========================================
    private void Awake()
    {
        _sineClip = GenerateTone(Waveform.Sine, false);
        _squareClip = GenerateTone(Waveform.Square, false);
        _triangleClip = GenerateTone(Waveform.Triangle, false);
        _crushedClip = GenerateTone(Waveform.Square, true);

        _activeClip = _sineClip;

        if (beepSource == null)
            beepSource = gameObject.AddComponent<AudioSource>();
        beepSource.playOnAwake = false;
        beepSource.loop = false;
        if (sfxGroup != null) beepSource.outputAudioMixerGroup = sfxGroup;

        if (typewriter != null)
            typewriter.OnCharacterRevealed.AddListener(HandleCharacter);
    }

    // ==========================================
    // OnDestroy - Unsubscribe from Typewriter
    // ==========================================
    private void OnDestroy()
    {
        if (typewriter != null)
            typewriter.OnCharacterRevealed.RemoveListener(HandleCharacter);
    }

    // ==========================================
    // ConfigureForLine - Select Voice Profile by Speaker + Current Emotional State
    // ==========================================
    public void ConfigureForLine(string speakerId)
    {
        _charCounter = 0;

        // ---- Base voice by speaker ----
        if (speakerId == ConstantsConfig.SPEAKER_HARU)
        {
            _activeClip = _squareClip; _pitchMin = 0.85f; _pitchMax = 0.95f;
        }
        else if (speakerId == ConstantsConfig.SPEAKER_YUA)
        {
            _activeClip = _triangleClip; _pitchMin = 1.15f; _pitchMax = 1.25f;
        }
        else
        {
            _activeClip = _sineClip; _pitchMin = 0.98f; _pitchMax = 1.04f;
        }

        _detune = 1f;
        _cadence = 2;
        _microStutter = false;

        // ---- Emotional modulation (GDD §1.2 table) ----
        CharacterState state = ResolveState(speakerId);
        switch (state)
        {
            case CharacterState.HappyBlush:
                _pitchMin *= 1.15f; _pitchMax *= 1.15f; _cadence = 1;
                break;
            case CharacterState.Annoyed:
                float flat = (_pitchMin + _pitchMax) * 0.5f;
                _pitchMin = flat; _pitchMax = flat; _cadence = 2;
                break;
            case CharacterState.SadImploring:
                _pitchMin *= 0.80f; _pitchMax *= 0.80f; _microStutter = true;
                break;
            case CharacterState.InsaneSmile:
                _detune = 1f + 0.12f; _activeClip = _crushedClip;
                break;
        }
    }

    // ==========================================
    // ResolveState - Read Active CharacterState from the Scene Registry
    // ==========================================
    private CharacterState ResolveState(string speakerId)
    {
        if (CharacterRegistry.Instance == null) return CharacterState.WaitingToTalk;
        CharacterSpriteController c = CharacterRegistry.Instance.Get(speakerId);
        return c != null ? c.CurrentState : CharacterState.WaitingToTalk;
    }

    // ==========================================
    // HandleCharacter - Per-Character Reveal Callback from TypewriterEffect
    // ==========================================
    private void HandleCharacter(char c)
    {
        if (char.IsWhiteSpace(c)) return;
        if (c == '<' || c == '>' || c == '/') return;     // skip stray rich-text artifacts

        _charCounter++;
        if (_cadence > 1 && (_charCounter % _cadence) != 0) return;

        PlayBeep();
    }

    // ==========================================
    // PlayBeep - Pitch-Randomized One-Shot via the Active Voice Clip
    // ==========================================
    private void PlayBeep()
    {
        if (beepSource == null || _activeClip == null) return;

        float pitch = Random.Range(_pitchMin, _pitchMax) * _detune;
        beepSource.pitch = Mathf.Clamp(pitch, 0.3f, 3f);
        beepSource.PlayOneShot(_activeClip, beepVolume);

        if (_microStutter && Random.value < 0.25f)
            StartCoroutine(MicroStutter());
    }

    // ==========================================
    // MicroStutter - SadImploring Randomized Echo Tap
    // ==========================================
    private IEnumerator MicroStutter()
    {
        yield return new WaitForSeconds(Random.Range(0.025f, 0.05f));
        if (beepSource != null && _activeClip != null)
            beepSource.PlayOneShot(_activeClip, beepVolume * 0.5f);
    }

    // ==========================================
    // GenerateTone - Build a Short Enveloped Waveform Clip (optionally bit-crushed)
    // ==========================================
    private AudioClip GenerateTone(Waveform wave, bool crush)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.Max(1, (int)(sampleRate * toneLength));
        float[] data = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            float phase = baseFrequency * t;
            float frac = phase - Mathf.Floor(phase);

            float s;
            switch (wave)
            {
                case Waveform.Square: s = frac < 0.5f ? 1f : -1f; break;
                case Waveform.Triangle: s = 4f * Mathf.Abs(frac - 0.5f) - 1f; break;
                default: s = Mathf.Sin(phase * 2f * Mathf.PI); break;
            }

            float env = Mathf.Exp(-6f * (float)i / sampleCount);   // quick decay
            s *= env;

            if (crush) s = Mathf.Round(s * 3f) / 3f;               // amplitude quantization

            data[i] = Mathf.Clamp(s, -1f, 1f) * 0.5f;
        }

        AudioClip clip = AudioClip.Create($"beep_{wave}{(crush ? "_crush" : "")}", sampleCount, 1, sampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }
}