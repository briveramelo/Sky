using System.Collections;

public class Duck_Wave : Wave {

	protected override IEnumerator RunWave(){
		float[] heights = new float[]{-1,1};
		DuckDirection[] directions = new DuckDirection[]{DuckDirection.UpLeft, DuckDirection.DownLeft};

		DuckDelegate SpawnDucks = AtHeight(heights, directions);

		for (int i=0; i<heights.Length; i++){
			yield return StartCoroutine(Produce1Wait3(()=>SpawnDucks(i)));
		}
		yield return MirrorDucks(heights, directions);
		yield return MirrorDucks(new float[]{0,0}, directions);

		yield return StartCoroutine (base.RunWave());
	}

	IEnumerator MirrorDucks(float[] heights, DuckDirection[] directions){
		SpawnDelegate SpawnDucks = AtHeights(heights, directions);
		yield return StartCoroutine(Produce1Wait3(SpawnDucks));
	}

	public delegate void DuckDelegate(int i);
	DuckDelegate AtHeight(float[] myHeights, DuckDirection[] myDirections){
		return (int i)=>{
			SpawnBirds (BirdType.Duck, SpawnPoint(right,myHeights[i]),myDirections[i]);
		};
	}
	SpawnDelegate AtHeights(float[] myHeights, DuckDirection[] myDirections){
		return ()=>{
			for (int i=0; i<myHeights.Length; i++){
				SpawnBirds (BirdType.Duck, SpawnPoint(right,myHeights[i]),myDirections[i]);
			}
		};
	}
}