using UnityEngine;
using System.Collections;
using GenericFunctions;

public class MaskCamera : MonoBehaviour
{

	public Joyfulstick joyfulstickScript;
	public Transform pooSliderTransform;
	public Vector3 startDifference;
	public Material eraserMaterial;
    public bool firstFrame;
    private Vector2? newHolePosition;

    private void CutHole(Vector2 imageSize, Vector2 imageLocalPosition){
        Rect textureRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        Rect positionRect = new Rect(
			(imageLocalPosition.x - 0.5f * eraserMaterial.mainTexture.width) / imageSize.x,
			(imageLocalPosition.y - 0.5f * eraserMaterial.mainTexture.height) / imageSize.y,
			eraserMaterial.mainTexture.width / imageSize.x,
			eraserMaterial.mainTexture.height / imageSize.y
        );
        GL.PushMatrix();
        GL.LoadOrtho();
		for (int i = 0; i < eraserMaterial.passCount; i++){
			eraserMaterial.SetPass(i);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(textureRect.xMin, textureRect.yMax);
            GL.Vertex3(positionRect.xMin, positionRect.yMax, 0.0f);
            GL.TexCoord2(textureRect.xMax, textureRect.yMax);
            GL.Vertex3(positionRect.xMax, positionRect.yMax, 0.0f);
            GL.TexCoord2(textureRect.xMax, textureRect.yMin);
            GL.Vertex3(positionRect.xMax, positionRect.yMin, 0.0f);
            GL.TexCoord2(textureRect.xMin, textureRect.yMin);
            GL.Vertex3(positionRect.xMin, positionRect.yMin, 0.0f);
            GL.End();
        }
        GL.PopMatrix();
    }

    void Awake(){
        firstFrame = true;
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		eraserMaterial = Resources.Load ("Materials/EraserMaterial", typeof (Material)) as Material;
    }

    void Update(){
        newHolePosition = null;
		if (joyfulstickScript.pooOnYou>0){
			Vector2 touchSpot = joyfulstickScript.touchSpot;
			//Vector2 v = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			Rect worldRect = new Rect(-Constants.worldDimensions.x + pooSliderTransform.position.x - startDifference.x, -Constants.worldDimensions.y + pooSliderTransform.position.y - startDifference.y, Constants.worldDimensions.x*2f, Constants.worldDimensions.y*2f);
			if (worldRect.Contains(touchSpot)){
				newHolePosition = new Vector2(Constants.screenDimensions.x * (touchSpot.x - worldRect.xMin) / worldRect.width, Constants.screenDimensions.y * (touchSpot.y - worldRect.yMin) / worldRect.height);
			}
        }
    }

	void OnPostRender(){
	    if (firstFrame){
	        firstFrame = false;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
	    }
        if (newHolePosition != null){
			CutHole(new Vector2(Constants.screenDimensions.x, Constants.screenDimensions.y), newHolePosition.Value);
		}
	}
}
