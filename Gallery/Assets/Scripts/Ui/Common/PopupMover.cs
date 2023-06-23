using UnityEngine;

namespace Ui
{
    using System;
    using DG.Tweening;

    [Serializable]
    public class PopupMover
    {
        public Tween Tween;
        
        [SerializeField]
        private RectTransform boxRectTransform;

        [SerializeField]
        private RectTransform popup;
        
        [SerializeField]
        private RectTransform popupTargetPosition;

        [SerializeField]
        private float animationTime = 0.25f;

        [SerializeField, Range(-1, 1)]
        private int horizontal;

        [SerializeField, Range(-1, 1)]
        private int vertical;

        private Vector2 _popupStartPosition;

        public void Initialize()
        {
            SetPopupStartPosition();
            SetTween();
        }

        public void Dispose() => Tween.Kill();

        private void SetTween()
        {
            Tween = popup.DOAnchorPos
            (
                popupTargetPosition.anchoredPosition,
                animationTime / 2
            )
            .From(_popupStartPosition);

            Tween.SetAutoKill(false);
        }
        
        private void SetPopupStartPosition()
        {
            var boxRect = boxRectTransform.rect;
            var popupRect = popup.rect;
            
            var x = boxRect.width / 2 + popupRect.width / 2;
            var y = boxRect.height / 2 + popupRect.height / 2;

            _popupStartPosition = new Vector2
            (
                horizontal == 0 ? popup.anchoredPosition.x : horizontal * x, 
                vertical == 0 ? popup.anchoredPosition.y : vertical * y
            );
        }
    }
}
