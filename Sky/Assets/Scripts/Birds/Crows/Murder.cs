using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;

public interface ICrowToMurder {
	void SendNextCrow();
	void ReportCrowDown(IMurderToCrow crowDown);
	int Cycle{get;}
}

public class Murder : MonoBehaviour, ICrowToMurder {

	[SerializeField] private Crow[] crows;
	List<IMurderToCrow> crowsAlive, crowsToSwoop;
	private ICrowToMurder me;

	private Vector2[] crowPositions  = new Vector2[]{
		new Vector2(0f  					  			,  Constants.WorldDimensions.y * 1.4f),
		new Vector2(Constants.WorldDimensions.x * 1.08f ,  Constants.WorldDimensions.y * 1.2f),
		new Vector2(Constants.WorldDimensions.x * 1.08f , -Constants.WorldDimensions.y * 1.2f),
		new Vector2(0f  					  			, -Constants.WorldDimensions.y * 1.4f),
		new Vector2(-Constants.WorldDimensions.x * 1.08f, -Constants.WorldDimensions.y * 1.2f),
		new Vector2(-Constants.WorldDimensions.x * 1.08f,  Constants.WorldDimensions.y * 1.2f)
	};

	private int maxCycles = 10;
	private int cycle;

	void Awake () {
		crowsAlive = new List<IMurderToCrow>((IMurderToCrow[])crows);
		crowsToSwoop = new List<IMurderToCrow>(crowsAlive);
		for (int j=0; j<crowsAlive.Count; j++){
			crowsAlive[j].InitializeCrow(j, crowPositions[j]);
		}
		cycle = 1;
		me = (ICrowToMurder)this;
		me.SendNextCrow ();
	}

	#region ICrowToMurder Interface
	void ICrowToMurder.SendNextCrow(){
		if (crowsToSwoop.Count>0){
			int luckyCrow = Random.Range (0,crowsToSwoop.Count-1);
			crowsToSwoop[luckyCrow].TakeFlight();
			crowsToSwoop.Remove(crowsToSwoop[luckyCrow]);
		}
		else if (crowsAlive.Count>0) {
			StartCoroutine ( ResetTheCycle());
        }
	}

	void ICrowToMurder.ReportCrowDown(IMurderToCrow crowDown){
		crowsAlive.Remove(crowDown);
		if (crowsAlive.Count==0){
			StopAllCoroutines();
			Destroy(gameObject);
		}
	}
		
	int ICrowToMurder.Cycle {get{ return cycle;}}
	#endregion
	
	IEnumerator ResetTheCycle(){
		while (!crowsAlive.Any(crow => crow.ReadyToFly)){
			yield return null;	
		}
		crowsToSwoop = new List<IMurderToCrow>(crowsAlive);
		yield return new WaitForSeconds (3f);
		cycle++;
		if (cycle>=maxCycles) {
            Destroy(gameObject);
        }
		me.SendNextCrow();
	}
}
