using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelArtRotation.Internal;

namespace PixelArtRotation
{
    public class PixelRotation : MonoBehaviour
    {
        public int Angle;
        public FilterMode Filter;
        public int PixelsPerUnit;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

            _originalSprite = _renderer.sprite;

            _possibleRotations = new Dictionary<int, Sprite>();
            _currentKey = 0;

            _oldFilter = Filter;
            _useAnimator = _animator != null ? _animator.enabled : false;
        }

        void Update()
        {
            CheckFilter();
            CheckAnimator();
        }

        void LateUpdate()
        {
            Rotate();
        }

        /// <summary>
        /// To allow every rotation with the selected filter.
        /// </summary>
        private void CheckFilter()
        {
            if (Filter != _oldFilter)
            {
                ResetDictionary();

                _oldFilter = Filter;
            }
        }

        /// <summary>
        /// To avoid the sprite being rewritten.
        /// </summary>
        private void CheckAnimator()
        {
            _useAnimator = (_animator != null && _animator.enabled);
        }

        /// <summary>
        /// Resets every value of the dictionary.
        /// </summary>
        private void ResetDictionary()
        {
            _possibleRotations.Clear();
            _currentKey = 0;
        }

        /// <summary>
        /// This methods rotates the sprite and stores every rotation in a dictionary
        /// to avoid recalculating the same sprite again and again.
        /// </summary>
        private void Rotate()
        {
            //Always use the original sprite as the sprite to rotate if the animator is off or missing.
            //If not, we take the sprite from the renderer, that way in case there's any animation,
            //we'll have the right sprite.
            _spriteToRotate = _useAnimator ? _renderer.sprite : _originalSprite;

            //Calculate value of current key, for the dictionary.
            _currentKey = (Angle * 31) + (_spriteToRotate.name.GetHashCode() * 17);

            //Check if the sprite is already rotated.
            if (!_possibleRotations.ContainsKey(_currentKey))
            {
                //Create a blank texture.
                _currentTexture = new Texture2D((int)_spriteToRotate.rect.width, (int)_spriteToRotate.rect.height);

                //Set name and filter.
                _currentTexture.name = _spriteToRotate.name;
                _currentTexture.filterMode = Filter;

                //Set pixels of the selected sprite.
                _currentTexture.SetPixels(_spriteToRotate.texture.GetPixels((int)_spriteToRotate.rect.position.x, (int)_spriteToRotate.rect.position.y, (int)_spriteToRotate.rect.width, (int)_spriteToRotate.rect.height));

                //Rotate the texture.
                _rotator = new Rotation(_currentTexture, _spriteToRotate.pivot, PixelsPerUnit);

                //Create new sprite with the rotation.
                Sprite newSprite = _rotator.RotateTexture(Angle);
                newSprite.name = _currentTexture.name;

                //Add to the dictionary.
                _possibleRotations.Add(_currentKey, newSprite);
            }

            //Set the renderer to the sprite calculated.
            _renderer.sprite = _possibleRotations[_currentKey];
        }

        private Dictionary<int, Sprite> _possibleRotations;
        private SpriteRenderer _renderer;
        private Animator _animator;
        private Rotation _rotator;

        private Sprite _spriteToRotate;
        private Texture2D _currentTexture;
        private int _currentKey;

        private FilterMode _oldFilter;
        private Sprite _originalSprite;
        private bool _useAnimator;
    }
}
