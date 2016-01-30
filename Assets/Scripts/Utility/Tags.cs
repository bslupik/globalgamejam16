using UnityEngine;
using System.Collections;


// for the autocomplete!

public class Tags{
    public const string player = "Player";
    public const string levelManager = "LevelManager";
    public const string soundManager = "SoundManager";
    public const string fileNameInput = "FileNameInput";
    public class Scenes
    {
    }

    public class Layers
    {
        public const string swipable = "Swipable";
    }

    public class AnimatorParams
    {
    }

    public class PlayerPrefKeys
    {
    }

    public class Options
    {
        public const string SoundLevel = "SoundLevel";
        public const string MusicLevel = "MusicLevel";
    }

    public class ShaderParams
    {
        public static int color = Shader.PropertyToID("_Color");
        public static int emission = Shader.PropertyToID("_EmissionColor");
        public static int cutoff = Shader.PropertyToID("_Cutoff");
        public static int noiseStrength = Shader.PropertyToID("_NoiseStrength");
        public static int effectTexture = Shader.PropertyToID("_EffectTex");
        public static int rangeMin = Shader.PropertyToID("_RangeMin");
        public static int rangeMax = Shader.PropertyToID("_RangeMax");
        public static int imageStrength = Shader.PropertyToID("_ImageStrength");
        public static int alpha = Shader.PropertyToID("_MainTexAlpha");
    }
}