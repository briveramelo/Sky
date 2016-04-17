using System;

public interface ITriggerSpawnable{
	void TriggerSpawnEvent();
}

public class EagleFriends : Wave, ITriggerSpawnable {

	private enum SpawnEvent{
		Bats5 = 0,
		Pigeons5 =1,
		Shoebill6=2,
		Ducks3 =3,
		Seagull3=4,
		Albatross2=5
	}

	void ITriggerSpawnable.TriggerSpawnEvent(){
		SpawnEvent dice = (SpawnEvent)UnityEngine.Random.Range(0,Enum.GetNames(typeof(SpawnEvent)).Length);
		switch (dice){
		case SpawnEvent.Bats5:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat],5));
			break;
		case SpawnEvent.Pigeons5:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 3));
			break;
		case SpawnEvent.Shoebill6:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill], 6));
			break;
		case SpawnEvent.Ducks3:
			StartCoroutine(ProduceDucks(3));
			break;
		case SpawnEvent.Seagull3:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Seagull], 3));
			break;
		case SpawnEvent.Albatross2:
			SpawnBirds(BirdType.Albatross,SpawnPoint(right,lowHeight));
			SpawnBirds(BirdType.Albatross,SpawnPoint(left,lowHeight));
			break;
		}
	}
}
