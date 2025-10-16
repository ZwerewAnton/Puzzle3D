using System;
using System.Collections.Generic;
using _1_LEVEL_REWORK.New.Instances;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay.Movement
{
    public class DetailViewMover : MonoBehaviour
    {
        [SerializeField] private MovingDetailView movingDetailViewPrefab;
        [SerializeField] private GhostDetailView ghostDetailViewPrefab;
        
        [Header("Settings")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float magnetDistance = 0.5f;
        [SerializeField] private float ghostDistance = 20f;
        [SerializeField] private Vector3 screenOffset = new (0, 100, 0);

        private InputHandler _inputHandler;
        
        private MovingDetailView _movingDetail;
        private GhostDetailView _ghostDetail;

        private List<PointTransform> _connectionPoints;
        private bool _isMoving;
        private bool _isConnected;
        private float _zCoord;
        private int _bestPointIndex;

        private static readonly Color NearColor = new(1, 1, 1, 0.5f);
        private static readonly Color FarColor = new(1, 1, 1, 0.0f);

        public event Action<DetailPlacementResult> PlacementEnded;

        [Inject]
        private void Construct(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
        
        private void Awake()
        {
            _movingDetail = Instantiate(movingDetailViewPrefab, Vector3.zero, Quaternion.identity, transform);
            _movingDetail.Hide();
            _ghostDetail = Instantiate(ghostDetailViewPrefab, Vector3.zero, Quaternion.identity, transform);
            _ghostDetail.Hide();
            
            _inputHandler.DetailActions.Tap.canceled += OnTapCanceled;
        }

        private void Update()
        {
            if (!_isMoving)
                return;
            
            UpdateMove(GetCursorWorldPoint());
        }

        private void OnDestroy()
        {
            _inputHandler.DetailActions.Tap.canceled -= OnTapCanceled;
        }

        public void StartMove(Mesh mesh, Material material, List<PointTransform> connectionPoints)
        {
            _zCoord = mainCamera.WorldToScreenPoint(Vector3.zero).z;
            _isMoving = true;
            _isConnected = false;
            
            if (connectionPoints.Count == 0 || !_inputHandler.DetailActions.Tap.IsPressed())
            {
                StopMove();
            }
            
            _connectionPoints = connectionPoints;
            _movingDetail.Show(mesh, material);
            _ghostDetail.Show(mesh);
            _ghostDetail.SetMaterialColor(FarColor);
        }

        public void StopMove()
        {
            if (!_isMoving)
                return;

            _isMoving = false;

            var result = new DetailPlacementResult
            {
                Success = _isConnected,
                PointIndex = _bestPointIndex
            };

            PlacementEnded?.Invoke(result);

            _movingDetail.Hide();
            _ghostDetail.Hide();
        }
        
        private void UpdateMove(Vector3 cursorPosition)
        {
            var minDistance = float.MaxValue;
            var closestPoint = new PointTransform();
            var closestPointIndex = 0;

            for (var i = 0; i < _connectionPoints.Count; i++)
            {
                var point = _connectionPoints[i];
                var distance = Vector3.Distance(cursorPosition, point.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                    closestPointIndex = i;
                }
            }

            var distanceToBest = Vector3.Distance(cursorPosition, closestPoint.Position);
            _zCoord = mainCamera.WorldToScreenPoint(closestPoint.Position).z;

            if (distanceToBest <= magnetDistance)
            {
                _isConnected = true;
                _bestPointIndex = closestPointIndex;
                _movingDetail.SetPositionAndRotation(closestPoint.Position, closestPoint.Rotation);
                _ghostDetail.SetMaterialColor(FarColor);
            }
            else
            {
                _isConnected = false;
                _movingDetail.SetPositionAndRotation(cursorPosition, closestPoint.Rotation);
                _ghostDetail.SetPositionAndRotation(closestPoint.Position, closestPoint.Rotation);
                var t = Mathf.InverseLerp(0, ghostDistance, distanceToBest);
                var color = Color.Lerp(NearColor, FarColor, t);
                _ghostDetail.SetMaterialColor(color);
            }
        }

        private void OnTapCanceled(InputAction.CallbackContext callbackContext)
        {
            StopMove();
        }
        
        private Vector3 GetCursorWorldPoint()
        {
            Vector3 input = _inputHandler.DetailActions.Cursor.ReadValue<Vector2>();
            var cursor = input + screenOffset;
            cursor.z = _zCoord;
            return mainCamera.ScreenToWorldPoint(cursor);
        }
    }
}