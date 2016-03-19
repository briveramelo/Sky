using UnityEngine;
using System.Collections;
using GenericFunctions;

public class MaskCamera : MonoBehaviour{

	[SerializeField] private Transform pooSliderTransform;
	[SerializeField] private Material eraserMaterial;
	[SerializeField] private Camera myCam;
	[SerializeField] private RenderTexture[] rts;
	private Vector3 startingPoint;
	private bool firstFrame;
    private Vector2? newHolePosition;

	void Awake(){
		startingPoint = pooSliderTransform.transform.position;
		//myCam.targetTexture = rts[Constants.TargetPooInt];
		firstFrame = true;
	}

    void Update(){
        newHolePosition = null;
		Vector2 touchSpot = Joyfulstick.Instance.TouchSpot;
		Rect worldRect = new Rect(-Constants.WorldDimensions.x + pooSliderTransform.position.x - startingPoint.x, -Constants.WorldDimensions.y + pooSliderTransform.position.y - startingPoint.y, Constants.WorldDimensions.x*2f, Constants.WorldDimensions.y*2f);
		if (worldRect.Contains(touchSpot)){
			newHolePosition = new Vector2(Constants.ScreenDimensions.x * (touchSpot.x - worldRect.xMin) / worldRect.width, Constants.ScreenDimensions.y * (touchSpot.y - worldRect.yMin) / worldRect.height);
		}
    }

	void OnPostRender(){
	    if (firstFrame){
	        firstFrame = false;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
	    }
        if (newHolePosition != null){
			CutHole(new Vector2(Constants.ScreenDimensions.x, Constants.ScreenDimensions.y), newHolePosition.Value);
		}
	}

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
}
