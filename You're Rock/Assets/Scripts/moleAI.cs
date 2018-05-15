using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PositionX1 ve PositionX2 degerleri her mole icin ayri olacak. 08.03.2018
*/  

public class moleAI : MonoBehaviour {
	  

	private Animator anim;
	private BoxCollider2D boxCollider;
	public GameObject moleGameObject;     
	private bool endOfTheRoad = false;  
	private float speed = 0.1f;    
	private bool moleDirectionIsRight = true;
	private bool isMoleTurn = true;
	private bool isMoleOutOfRoad = false;  
	private bool collisionOccur = false;
	private bool isObjectDestroyed = false;
	public float chaseRange;

	//Mole'nin devriye gezdigi x duzlemindeki min ve max degerler
	public float positionX1 = 3417.134f; // 0.2f icin 3417.392
	public float positionX2 = 3366.484f; // 0.2f icin 338.803 

	//The Rock
	public Transform theRock;
	public BoxCollider2D theRockBoxCollider; 
	public rockController rockController;
	public GameObject theRockGameObject;


	//Mole Tasi yakalarsa... dot dot dot
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "TheRock")
			collisionOccur = true;
	}


	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> (); 
		boxCollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
				
		if(!isObjectDestroyed)
		{

		//Chasing THE ROCK
		//Mole ile Tas arasindaki mesafeye bak, eger yeterince yakinlarsa magic
		float distanceToTarget = Vector3.Distance(transform.position, theRock.position);

		if (distanceToTarget < chaseRange && !collisionOccur) 
		{

			if (transform.position.x < theRock.position.x) // Tas Mole'nin onundeyse (Tasin x degeri daha buyukse)
			{
				if (moleDirectionIsRight) // Eger Mole tasa bakiyorsa
				{
					

					if (isRoadEndsWhileChase()) // Mole icin yolun sonuysa, Tas chaseRange'den cikana kadar ona bakacak mal mal.
					{ 
						transform.Translate (0f, 0f, 0f);
					} else 
					{
						transform.Translate (speed, 0f, 0f);
					}

				} else // Eger Mole'nin gotu tasa bakiyorsa
				{ 
					
					isMoleTurn = false;
					isMoleOutOfRoad = true;

					if (!isMoleTurn)  
					{
						MoleTurnToChase ();
						moleDirectionIsRight = true;
					}

					if (isRoadEndsWhileChase()) // Mole icin yolun sonuysa, Tas chaseRange'den cikana kadar ona bakacak mal mal.
					{ 
						transform.Translate (0f, 0f, 0f);

					} else 
					{ 
						transform.Translate (speed, 0f, 0f);
					}
						
				}
			} 

			if (transform.position.x > theRock.position.x) // Tas Mole'nin arkasindaysa (Tasin x degeri daha kucukse)
			{
				if (moleDirectionIsRight)// eger Mole'nin gotu tasa bakiyorsa
				{ 

					isMoleTurn = false;
					isMoleOutOfRoad = true;

					if (!isMoleTurn)  
					{
						MoleTurnToChase ();
						moleDirectionIsRight = false;

						if (isRoadEndsWhileChase()) // Mole icin yolun sonuysa, Tas chaseRange'den cikana kadar ona bakacak mal mal.
						{ 
							transform.Translate (0f, 0f, 0f);

						} else 
						{ 
							transform.Translate (speed, 0f, 0f);
						}
					}

				} else//  Eger Mole tasa bakiyorsa  
				{ 
					

					if (isRoadEndsWhileChase())  // Mole icin yolun sonuysa, Tas chaseRange'den cikana kadar ona bakacak mal mal.
					{
						transform.Translate (0f, 0f, 0f);
					} else 
					{
						transform.Translate (speed, 0f, 0f);
					}
				}
				
			}

		} 
		else // Mole devriye
		{
				
			if (isMoleOutOfRoad) 
			{
				transform.RotateAround (transform.position, transform.up, 180f);
				isMoleOutOfRoad = false;

				if (moleDirectionIsRight) {
					moleDirectionIsRight = false;	
				} else 
				{
					moleDirectionIsRight = true;
				}

			}
			isRoadEnds();
			transform.Translate (speed, 0f, 0f);
		}
			  

		//Mole tasi sirtlayip yuvasina goturecek, theRock.MaxSpeed == 0
		if (collisionOccur) 
		{
			if (moleDirectionIsRight) 
			{
					transform.RotateAround (transform.position, transform.up, 180f);
					moleDirectionIsRight = false;
			}

				Vector3 newPos = new Vector3 ();
				newPos = transform.position;
				newPos.y = newPos.y + 1.5674f;
				theRock.transform.position = newPos;
				rockController.maxSpeed = -1;

			if (transform.position.x > positionX2 - 0.003f && transform.position.x < positionX2 + 0.006f) 
			{
					Destroy (moleGameObject);
					Destroy (theRockGameObject);  
					isObjectDestroyed = true;   
			}
		}

		}
	}






	// FUNCTIONS
		
	public void isRoadEnds() 
	{   

		if (transform.position.x < positionX1 + 0.003f && transform.position.x > positionX1 - 0.006f) 
		{//Saga giderken kontrol
			transform.RotateAround (transform.position, transform.up, 180f);
			moleDirectionIsRight = false;
			endOfTheRoad = true;
		} else if (transform.position.x > positionX2 - 0.003f && transform.position.x < positionX2 + 0.006f) 
		{//Sola giderken kontrol  
			transform.RotateAround (transform.position, transform.up, 180f);
			moleDirectionIsRight = true;
		}
	}

	public void MoleTurnToChase()
	{
		transform.RotateAround (transform.position, transform.up, 180f);
		isMoleTurn = true;
	}
		

	public bool isRoadEndsWhileChase()
	{
		if (transform.position.x < positionX1 + 0.003f && transform.position.x > positionX1 - 0.001f) 
		{//Saga giderken kontrol
			endOfTheRoad = true;
		} else if (transform.position.x > positionX2 - 0.003f && transform.position.x < positionX2 + 0.001f) 
		{//Sola giderken kontrol  
			endOfTheRoad = true;
		} else 
		{
			endOfTheRoad = false;
		}

		return endOfTheRoad;  
	}

}
