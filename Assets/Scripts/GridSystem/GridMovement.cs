using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
using Unity.Mathematics;
using UnityEngine;

namespace GridSystem {
    public class GridMovement : MonoBehaviour {
        private readonly AppSettingsDefinition _settings = DevSettings.Instance.appSettings;
        private Camera _camera;
        
        private void Awake() {
            _camera = Camera.main;
        }

        private void Update() {
            _camera.orthographicSize =
                Math.Clamp(_camera.orthographicSize - Input.mouseScrollDelta.y, _settings.cameraZoomRange.x, _settings.cameraZoomRange.y);

            var zoomMovement = 1f;
            if (_settings.slowWhenZoomed) {
                zoomMovement = 0.1f + ((_camera.orthographicSize - _settings.cameraZoomRange.x) /
                                       (_settings.cameraZoomRange.y - _settings.cameraZoomRange.x));
                zoomMovement = Math.Clamp(zoomMovement, 0f, 1f);
            }

            var movementAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            transform.Translate(_settings.cameraMovementSpeed * Time.deltaTime * 70f * zoomMovement * movementAxis);
        }
    }  
}
