﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;

public class Pelican : Bird {

	[SerializeField] private Animator pelicanAnimator;
	[SerializeField] private Collider2D pelicanCollider;

	private Vector2[] setPositions = new Vector2[]{
		new Vector2 (0f, -.8f),
		new Vector2 (.3f, .1f),
		new Vector2 (-.3f, .1f),
		new Vector2 (0f, .5f)
	};

	private Vector2 targetPosition;
	private Vector2 moveDir;
	
	private float[] moveSpeeds = new float[]{
		2.5f, 2.5f, 2.5f, 2.5f, 9f
	};
	private float[] curveAngles = new float[]{
		20f, 30f, 30f, 5f
	};

	const float distanceThreshold = 0.25f;
	private float angleDelta;

	private enum PP{  //pelican positions
		Below =0, Right =1, Left =2, Above=3, End=4
	}
	private Dictionary<PP, Vector2> pelPosts; //pelican positions
	private PP pelPost;
	private PP PelPost {
		get{return pelPost;}
		set{
			if (pelPost== PP.Right && value == PP.Left){
				pelPost = PP.Above;
			}
			else{
				pelPost = value;
			}
			if (pelPost == PP.End){
				rigbod.velocity = Vector2.zero;
			}
		}
	}

	private bool[] spotsHit = new bool[4];
	private bool[] clockwiseSteps = new bool[4];

	protected override void Awake () {
		birdStats = new BirdStats(BirdType.Pelican);

		pelPosts = new Dictionary<PP, Vector2>();
		for (int i=0; i<setPositions.Length; i++){
			setPositions[i] = new Vector2 (setPositions[i].x * Constants.WorldDimensions.x, setPositions[i].y * Constants.WorldDimensions.y);
			pelPosts.Add((PP)i, setPositions[i]);
		}
		StartCoroutine(SwoopAround());
		base.Awake();
	}
		
	//Move from one checkpoint to another
	IEnumerator SwoopAround(){
		PelPost = 0;
		spotsHit = new bool[4];
		DetermineFlightPattern();
		while ((int)PelPost<setPositions.Length){	
			while (!spotsHit[(int)PelPost]) {
				targetPosition = (Vector2)Constants.balloonCenter.position + pelPosts[PelPost];
				CheckToMoveOn();
				moveDir = CurveItIn();
				rigbod.velocity = moveDir * moveSpeeds[(int)PelPost];
				yield return null;
			}
			PelPost++;
			yield return null;
		}
		yield return new WaitForSeconds (0.3f);
		//pelicanAnimator.SetInteger("AnimState",1);
		yield return new WaitForSeconds (0.2f);
		StartCoroutine (DiveBomb ());
	}

	//set his flight pattern from the getgo
	void DetermineFlightPattern(){
		if (targetPosition.x>transform.position.x){
			if (targetPosition.y<transform.position.y){
				clockwiseSteps = new bool[]{false,false,false,false}; 	//downright
			}
			else{
				clockwiseSteps = new bool[]{true,false,false,false}; 	//upright
			}
		}
		else{
			if (targetPosition.y<transform.position.y){
				clockwiseSteps = new bool[]{true,true,true,true}; 		//downleft
			}
			else{
				clockwiseSteps = new bool[]{false,true,true,true};		//upleft
			}
		}
	}

	//bring in that nice curve to his flight pattern
	Vector2 CurveItIn(){
		Vector2 initialMoveDir = (targetPosition - (Vector2)transform.position).normalized;
		float inputAngle = ConvertAnglesAndVectors.ConvertVector2FloatAngle (initialMoveDir);
		angleDelta = curveAngles[(int)PelPost];
		if (!clockwiseSteps[(int)PelPost]){
			angleDelta = -angleDelta;
		}

		float outputAngle = inputAngle + angleDelta;
		return ConvertAnglesAndVectors.ConvertAngleToVector2 (outputAngle).normalized;
	}
		
	//determine if he should move on to the next checkpoint in his flight
	void CheckToMoveOn(){
		if (PelPost == PP.Below || PelPost == PP.Above){
			if (Mathf.Abs (transform.position.x-targetPosition.x)<distanceThreshold){
				spotsHit[(int)PelPost] = true;
			}
		}
		else if (PelPost == PP.Left || PelPost == PP.Right){
			if (transform.position.y>targetPosition.y-.1f){
				spotsHit[(int)PelPost] = true;
			}
		}
	}

	//plunge to (un)certain balloon-popping glory
	IEnumerator DiveBomb(){
		rigbod.velocity = Vector2.down * moveSpeeds [4];
		while (transform.position.y>-Constants.WorldDimensions.y-1f){
			yield return null;
		}
		//pelicanAnimator.SetInteger("AnimState",0);
		rigbod.velocity = Vector2.zero;
		pelicanCollider.enabled = false;
		yield return new WaitForSeconds (2f);
		StartCoroutine (SwoopAround ());
		while (transform.position.y<-Constants.WorldDimensions.y){
			yield return null;
		}
		pelicanCollider.enabled = true;
	}
}