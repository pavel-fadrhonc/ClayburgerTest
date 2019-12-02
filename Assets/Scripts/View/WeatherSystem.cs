using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace View
{
    public enum eWeatherTypes
    {
        Sunshine,
        Raining
    }
    
    public class WeatherSystem : MonoBehaviour
    {
        [Header("References")] 
        public Image DarkOverlay;

        public ParticleSystem RainPS;
        public ParticleSystem RainDropsPS;
        public ParticleSystem SunShaftsPS;
        public LightRays2D LightRays;

        public ScrollWaves Waves;
        
        public List<Material> grassMaterials = new List<Material>();

        [Header("Settings")] 
        public bool UseSmoothSunShafts;
        [Space]
        public float DarkOverlayAlpha;
        [Space]
        public Vector2 WavesScrollSpeedSunshine;
        public Vector2 WavesScrollSpeedRaining;
        [Space]
        public Vector2 SunshineGrassSpeedRandomSpan;
        public Vector2 RainingGrassSpeedRandomSpan;
        
        [Space] 
        public float GrassWavesAmplitude; 

        [Header("Time Settings")]
        public Vector2 weatherDurationSpan;
        public float transitionDuration;

        private float _nextTransitionTime;
        private bool _transitionFinished ;
        private eWeatherTypes _currentWeather = eWeatherTypes.Sunshine;
        private List<ValueTuple<float, float>> _grassMaterialTransitionSpeedValues = new List<ValueTuple<float, float>>();

        private void Awake()
        {
            grassMaterials.ForEach(gm => _grassMaterialTransitionSpeedValues.Add(new ValueTuple<float, float>(0,0)));
        }

        private void Start()
        {
            GenerateNextTransitionTime();
            
            EvaluateTransition(1.0f, true, _currentWeather);
            _transitionFinished = true;
        }

        private void Update()
        {
            if (Time.time > _nextTransitionTime && Time.time < (_nextTransitionTime + transitionDuration))
            {
                if (_transitionFinished)
                {
                    _currentWeather = _currentWeather == eWeatherTypes.Raining
                        ? eWeatherTypes.Sunshine
                        : eWeatherTypes.Raining;
                }                
                
                var transitionProgress = (Time.time - _nextTransitionTime) / transitionDuration;
                EvaluateTransition(transitionProgress, _transitionFinished, _currentWeather);
                
                _transitionFinished = false;
            }
            else if (Time.time > _nextTransitionTime + transitionDuration && !_transitionFinished)
            {

                GenerateNextTransitionTime();

                _transitionFinished = true;
            }
        }

        private void EvaluateTransition(float progress, bool init, eWeatherTypes weather)
        {
            var actualProgress = progress;
            if (weather == eWeatherTypes.Raining)
            {
                if (init)
                {
                    RainPS.Play();
                    RainDropsPS.Play();
                    if (!UseSmoothSunShafts)
                        SunShaftsPS.Stop();
                    for (int i = 0; i < _grassMaterialTransitionSpeedValues.Count; i++)
                    {
                        _grassMaterialTransitionSpeedValues[i] =
                        (
                            grassMaterials[i].GetFloat("_Frequency"),
                            Random.Range(RainingGrassSpeedRandomSpan.x, RainingGrassSpeedRandomSpan.y)
                        );
                    }
                }
                
                actualProgress = 1 - progress;
            }
            else
            {    
                if (init)
                {
                    RainPS.Stop();
                    RainDropsPS.Stop();
                    if (!UseSmoothSunShafts)
                        SunShaftsPS.Play();
                    else
                        SunShaftsPS.Stop();
                    for (int i = 0; i < _grassMaterialTransitionSpeedValues.Count; i++)
                    {
                        _grassMaterialTransitionSpeedValues[i]=
                        (
                            grassMaterials[i].GetFloat("_Frequency"),
                            Random.Range(SunshineGrassSpeedRandomSpan.x, SunshineGrassSpeedRandomSpan.y)
                        );
                    }
                }
            }
 
            DarkOverlay.color = new Color(DarkOverlay.color.r, DarkOverlay.color.g, DarkOverlay.color.b, 
                Mathf.Lerp(DarkOverlayAlpha, 0.0f, actualProgress));
            Waves.ScrollSpeed = Vector2.Lerp(WavesScrollSpeedRaining, WavesScrollSpeedSunshine, actualProgress);
            if (UseSmoothSunShafts)
                LightRays.color1 = new Color(LightRays.color1.r, LightRays.color1.g, LightRays.color1.b, actualProgress);
            else
                LightRays.color1 = new Color(LightRays.color1.r, LightRays.color1.g, LightRays.color1.b, 0);
            
            
            for (int i = 0; i < grassMaterials.Count; i++)
            {
                // this would be great but is causing serious "fast forward" effect that is not very desirably (unless you want to do fast forward effect :) )
//                grassMaterials[i].SetFloat("_Frequency", Mathf.Lerp(_grassMaterialTransitionSpeedValues[i].Item1,
//                    _grassMaterialTransitionSpeedValues[i].Item2, progress));

                if (progress > 0.5 && progress < 0.6 & !Mathf.Approximately(grassMaterials[i].GetFloat("_Frequency"), _grassMaterialTransitionSpeedValues[i].Item2))
                {
                    grassMaterials[i].SetFloat("_Frequency", _grassMaterialTransitionSpeedValues[i].Item2);
                }
                // potential lerping the amplitude to lower the hickup and skip in an offset? Nah... :)
//                if (progress < 0.5)
//                {
//                    grassMaterials[i].SetFloat("_Amplitude", Mathf.Lerp(0,
//                        GrassWavesAmplitude, progress * 2));
//                }
//
//                if (progress > 0.5)
//                {
//                    grassMaterials[i].SetFloat("_Amplitude", Mathf.Lerp(0,
//                        GrassWavesAmplitude, (progress - 0.5f) * 2f));
//                }
            }
        }

        private void GenerateNextTransitionTime()
        {
            _nextTransitionTime = Time.time + Random.Range(weatherDurationSpan.x, weatherDurationSpan.y);
        }
    }
}