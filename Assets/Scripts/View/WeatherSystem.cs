using System;
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

        [Header("Settings")] 
        public bool UseSmoothSunShafts;
        
        public float DarkOverlayAlpha;

        public Vector2 WavesScrollSpeedSunshine;
        public Vector2 WavesScrollSpeedRaining;

        [Header("Time Settings")]
        public Vector2 weatherDurationSpan;
        public float transitionDuration;

        private float _nextTransitionTime;
        private bool _transitionFinished ;
        private eWeatherTypes _currentWeather = eWeatherTypes.Sunshine;

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
                }
            }
 
            DarkOverlay.color = new Color(DarkOverlay.color.r, DarkOverlay.color.g, DarkOverlay.color.b, 
                Mathf.Lerp(DarkOverlayAlpha, 0.0f, actualProgress));
            Waves.ScrollSpeed = Vector2.Lerp(WavesScrollSpeedRaining, WavesScrollSpeedSunshine, actualProgress);
            if (UseSmoothSunShafts)
                LightRays.color1 = new Color(LightRays.color1.r, LightRays.color1.g, LightRays.color1.b, actualProgress);
            else
                LightRays.color1 = new Color(LightRays.color1.r, LightRays.color1.g, LightRays.color1.b, 0);
        }

        private void GenerateNextTransitionTime()
        {
            _nextTransitionTime = Time.time + Random.Range(weatherDurationSpan.x, weatherDurationSpan.y);
        }
    }
}