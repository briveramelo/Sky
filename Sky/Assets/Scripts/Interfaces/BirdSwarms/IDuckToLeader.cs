using UnityEngine;

public interface IDuckToLeader {
	void ReShuffle(ILeaderToDuck deadDuck);
	Vector2[] SetPositions{get;}
}
