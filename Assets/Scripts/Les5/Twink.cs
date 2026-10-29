using System.Collections;
using UnityEngine;

public class Twink : Enemy
{
	[SerializeField] private float speedtwink = 3f;
	[SerializeField] private int Hptwink = 60;
	private Renderer twinkRenderer;
	void Awake(){
		speed = speedtwink;
		Hp = Hptwink;
		twinkRenderer = GetComponentInChildren<Renderer>();
		StartCoroutine(Invisible());
	}

	private IEnumerator Invisible(){
		yield return new WaitForSeconds(3f);
		twinkRenderer.enabled = false;
		capsuleCollider.enabled = false;
		yield return new WaitForSeconds(0.5f);
	twinkRenderer.enabled = true;
	capsuleCollider.enabled = true;
		StartCoroutine(Invisible());
		yield return null;
	}
}
