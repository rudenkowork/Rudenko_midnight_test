using System;
using Plugins.RContainer;
using Services.SceneManagement;
using Services.WindowsManagement;
using Services.WindowsManagement.Windows;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.Infrastructure
{
    public class MenuController : MonoBehaviour
    {
        private const string Gameplay = "Gameplay";
        
        public Button PlayButton;
        
        private IWindowService _windowService;
        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(IWindowService windowService, ISceneLoader sceneLoader)
        {
            _windowService = windowService;
            _sceneLoader = sceneLoader;
        }
        
        private void OnEnable()
        {
            PlayButton.onClick.AddListener(Play);
        }
        
        private void OnDisable()
        {
            PlayButton.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            _sceneLoader.Load(Gameplay);
        }
    }
}