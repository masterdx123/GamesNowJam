using UnityEngine;
using System;

namespace UI.MainConsole {
    public class MachineArrow : MonoBehaviour
    {
        public GameObject UpgradeConsole
        {
            set => _upgradeConsole = value;
        }

        public float Offset
        {
            set => _offset = value;
        }

        public Transform Canvas
        {
            set => _canvas = value;
        }

        private GameObject _player;
        private GameObject _upgradeConsole;
        private Camera _mainCamera;
        private Vector2 _direction;
        private float _offset;
        private Transform _canvas;

        void Start() {
            _player = GameObject.FindGameObjectWithTag("Player");
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        // Update is called once per frame
        void Update()
        {
            RotateArrow();
            PositionArrow();
        }

        void RotateArrow() {

            _direction = _upgradeConsole.transform.position - _player.transform.position;

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        void PositionArrow() {

            RectTransform rectTransform = _canvas.GetComponent<RectTransform>();

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;

            RectTransform arrowRectTransform = GetComponent<RectTransform>();

            float calculatedX = height * Mathf.Abs(_direction.x) / Mathf.Abs(_direction.y);
            float calculatedY = width * Mathf.Abs(_direction.y) / Mathf.Abs(_direction.x);

            if (calculatedX <= width)
            {
                arrowRectTransform.anchoredPosition = new Vector2(Mathf.Sign(_direction.x) * calculatedX / 2, Mathf.Sign(_direction.y) * height / 2 );
            }
            else 
            {
                arrowRectTransform.anchoredPosition = new Vector2(Mathf.Sign(_direction.x) * width / 2, Mathf.Sign(_direction.y) * calculatedY / 2 );
            }
        }
        
    }
}
