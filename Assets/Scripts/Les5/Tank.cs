using UnityEngine;

public class Tank : Enemy
{
	[SerializeField] private float speedTank = 1f;
	[SerializeField] private int HpTank = 200;
	void Awake(){
	speed = speedTank;
	Hp = HpTank;
	}
}