using System;

public interface ITriggerSpawnable{
	void TriggerSpawnEvent();
}

public class EagleFriends : Wave, ITriggerSpawnable {

	private enum SpawnEvent{
		Bats = 0,
		Pigeons =1,
		Shoebill6=2,
		Ducks =3,
		Seagulls=4,
		Albatrosses=5
	}

	void ITriggerSpawnable.TriggerSpawnEvent(){
		SpawnEvent dice = (SpawnEvent)UnityEngine.Random.Range(0,Enum.GetNames(typeof(SpawnEvent)).Length);
		switch (dice){
		case SpawnEvent.Bats:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat],5));
			break;
		case SpawnEvent.Pigeons:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 5));
			break;
		case SpawnEvent.Shoebill6:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill], 6));
			break;
		case SpawnEvent.Ducks:
			StartCoroutine(ProduceDucks(4));
			break;
		case SpawnEvent.Seagulls:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Seagull], 3));
			break;
		case SpawnEvent.Albatrosses:
			SpawnBirds(BirdType.Albatross,SpawnPoint(Right,LowHeight));
			SpawnBirds(BirdType.Albatross,SpawnPoint(Left,LowHeight));
			break;
		}
	}
}
