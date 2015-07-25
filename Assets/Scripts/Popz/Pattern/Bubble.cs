using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {

	bool justOnce = false;

	public Sprite bubbleEnd;

	public void CorrectChoice()
	{
		this.GetComponent<Animator>().SetBool("Correct",true);

	}
	public void InCorrectChoice()
	{
		this.GetComponent<Animator>().SetBool("InCorrect",true);

	}
	
	// Update is called once per frame
	void Update () {
	
		if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BubbleOk"))
		{
			if(!justOnce)
			{
				if (this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= .9)
				{
					justOnce = true;
					//Play collectible going up anim!
					//Debug.Log ("Play going up!");
					gameObject.GetComponent<SpriteRenderer>().sprite = bubbleEnd;
					this.transform.parent.gameObject.GetComponent<Collectible>().collectibleGoUp();

				}
			}
		}
	}
}
