using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Main.Player;

namespace Main.UI
{
    public class CanvasHandle : MonoBehaviour
    {
        private readonly List<IWindowControl> _escapeUIObjects = new();
        private readonly List<IWindowControl> _windowControls = new();


        [Header("Components")]
        public Canvas canvas;
        public PlayerController playerController;
        public CameraController cameraController;
        //public MonitorsManager graphsManager;

        [Header("Layers")]
        public int worldItemsLayer;

        [Header("Debug")]
        [ReadOnly] public bool isPointerOverGameObject;
        [ReadOnly] public bool isCanvasEnabled;
        [ReadOnly] public bool isDragging;


        private void Update()
        {
            isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
        }

        public void AddWindowToEscapeControl(IWindowControl window)
        {
            _escapeUIObjects.Add(window);
        }
        public void AddWindowToControl(IWindowControl window)
        {
            _windowControls.Add(window);
        }

        public void CloseUIWindow(bool forceMainMenu = true)
        {
            for (int i = 0; i < _escapeUIObjects.Count; i++)
            {
                var current = _escapeUIObjects[i];

                if (current.IsActive)
                {
                    current.ToggleWindow();
                    return;
                }
            }
            //if (forceMainMenu) ToggleWindow<MainMenu>();
        }

        public void ToggleWindow<T>() where T : IWindowControl
        {
            for (int i = 0; i < _windowControls.Count; i++)
            {
                var current = _windowControls[i];
                if (current is T) current.ToggleWindow();
            }
        }
    }
}
