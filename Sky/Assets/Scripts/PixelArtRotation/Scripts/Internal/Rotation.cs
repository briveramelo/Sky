using UnityEngine;
using System.Collections;

namespace PixelArtRotation.Internal
{
    public class Rotation
    {
        /// <summary>
        /// Receives the original texture so it doesn't have to instantiate it every time.
        /// </summary>
        /// <param name="original"></param>
	    public Rotation(Texture2D original, Vector2 pivot, int pixelsPerUnit)
        {
            _newTexture = (Texture2D)MonoBehaviour.Instantiate(original);

            _oldArray = original.GetPixels32();
            _newArray = new Color32[original.GetPixels32().Length];

            _width = original.width;
            _height = original.height;

            _pivot = pivot;
		    _pixelsPerUnit = pixelsPerUnit;
        }

        /// <summary>
        /// Rotate the texture inside the sprite.
        /// </summary>
        public Sprite RotateTexture(int angle)
        {
            RotateSquare(Mathf.Deg2Rad * angle);

            //Then return the rotation.
            return ApplyRotation(_newArray);
        }

        /// <summary>
        /// Apply the new texture to the sprite.
        /// </summary>
        private Sprite ApplyRotation(Color32[] pix)
        {
            //No change performance-wise
            //Texture2D newTexture = new Texture2D(_width, _height);
            //newTexture.filterMode = FilterMode.Point;
        
            _newTexture.SetPixels32(pix);
            _newTexture.Apply();
		    return Sprite.Create(_newTexture, new Rect(0, 0, _width, _height), new Vector2((_pivot.x / _width), (_pivot.y / _height)), _pixelsPerUnit);
        }

        /// <summary>
        /// Rotate the texture by a rotation and traslation matrix.
        /// </summary>
        /// <param name="textureArray"></param>
        /// <param name="phi"></param>
        /// <returns></returns>
        private void RotateSquare(float phi)
        {
            int x, y, xc, yc;
            float sin, cos;

            x = 0;
            y = 0;
            sin = Mathf.Sin(phi);
            cos = Mathf.Cos(phi);

            //pivot centered for the rotation
            //xc = width / 2;
            //yc = height / 2;

            //custom pivot
            xc = (int)_pivot.x;
            yc = (int)_pivot.y;

            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    _newArray[j * _width + i] = new Color32(0, 0, 0, 0);

                    x = (int)(cos * (i - xc) + sin * (j - yc) + xc);
                    y = (int)(-sin * (i - xc) + cos * (j - yc) + yc);

                    if ((x > -1) && (x < _width) && (y > -1) && (y < _height))
                    {
                        _newArray[j * _width + i] = _oldArray[y * _width + x];
                    }
                }
            }       
        }

        private Texture2D _newTexture;

        private Color32[] _newArray;
        private Color32[] _oldArray;

        private int _width;
        private int _height;

        private Vector2 _pivot;
	    private int _pixelsPerUnit;
    }
}
