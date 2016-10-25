using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenerateGrid : MonoBehaviour {

	/*references*/
	#region
	public bool execute;
	public bool hasComputeButtonPressed;//when compute button pressed,the landscape starts to generate
	public bool hasComptationStopped;

	public GameObject cube;             //the building cube
	public GameObject blackScreen;		//black screen to prevent anything from viewing
	public GameObject dropCube;         //the model being dropped after the landscape is generated
	public GameObject cubeParent;
	public GameObject[] totalCubes;
	public GameObject[] totalBlocks;    //holds the total blocks i.e. the buttons
	public GameObject[] cubeTypes;      //ground, low grass, high grass, snow

	public int columnLength;
	public int rowLength;
	public int bloackSeperation;        //the seperation between the blocks
	public int totalBlockCount;         //keeps track of total buttons
	public int totalBlocksCount;        //the total blocks
	public int totalCubeCount;

	public float cubeSeperationDistance;//the seperation between the cubes (make sure you remember the cube scale also )
	public float currentTime = 2f;

	public Vector3 initialSpawnPoint;   //the initial spawn point for the buttons to spawn
	public Vector3 initialCubeSpawnPoint;
	public Vector3 dropCubeSpawnSpoint;

	public Canvas canvas;               //to set the blocks as the parent of the canvas

	public Button block;                //the building button
	public Button button_generate;  //generates the landscape
	public Button button_exit;		//exit the game

	#endregion  //references	 //refernces

	void Start() {
		execute = false;
		hasComptationStopped = false;
		SpawnButtonGrid();
		button_exit.transform.gameObject.SetActive(false);
    }

	void Update() {
		if (hasComputeButtonPressed == true) {
			if (hasComptationStopped == false) {
				GenerateLandscape(totalBlocksCount);
			}
		}

		if (execute == true) {
			CollectTotalButtons();
			execute = false;
		}
	}

	/*will generate a grid of buttons and base hexagons*/
    void SpawnButtonGrid() {
        for (int i = 0; i < columnLength; i++) {
            for (int j = 0; j < rowLength; j++) {
                Button b = Instantiate(block, new Vector3((initialSpawnPoint.x + 400) + i * bloackSeperation, (initialSpawnPoint.y + 650) - j * bloackSeperation, initialSpawnPoint.z), Quaternion.identity) as Button;
                b.transform.SetParent(canvas.transform);
                GameObject c = null;
                if(j%2 == 0)    {
                    c = Instantiate(cube, new Vector3((initialCubeSpawnPoint.x) + i * cubeSeperationDistance, initialCubeSpawnPoint.y, (initialCubeSpawnPoint.z) - j * 2.7f), cube.transform.rotation) as GameObject;
                    //that 2.7 in Y will shift the octagons so that they merge into other octagons
                } else {
                    c = Instantiate(cube, new Vector3((initialCubeSpawnPoint.x + 1.55f) + i * cubeSeperationDistance, initialCubeSpawnPoint.y, (initialCubeSpawnPoint.z) - j * 2.7f), cube.transform.rotation) as GameObject;
                    //that 1.55 in X parameter will shift the octagons by some positions since they need to merge with each other
                }
              
				c.transform.SetParent(cubeParent.transform);
            }
        }
    }

	/*the following functions are followed when the compute landscape button is pressed*/
	/*----------------------------------------------------------------------------------*/
   public void CollectTotalButtons() {

		//disable the black screen
		blackScreen.SetActive(false);

		//enable the exit button
		button_exit.transform.gameObject.SetActive(true);

		totalBlocks = GameObject.FindGameObjectsWithTag("Button");
		totalBlockCount = totalBlocks.Length;

		totalCubes = GameObject.FindGameObjectsWithTag("Cube");
		totalCubeCount = totalCubes.Length;

		AssignCubeHeights();

		DestroyComputeButton();
    }

	/*maps the heatmap to the respective hexagon position and removes the excess buttons*/
    void AssignCubeHeights() {
		totalBlocksCount = rowLength * columnLength;

		/*destroy all the buttons*/
		for (int i = 0; i < totalBlocksCount; i++) {
			Destroy(totalBlocks[i].gameObject);	
		}


		for(int i = 0; i < totalBlocksCount; i++) {
            totalCubes[i].GetComponent<Cube>().height = totalBlocks[i].GetComponent<ButtonTapCount>().tapCount;

            //code responsibe for changing materials during runtime
            //but sice we have cubes with already set colours, we wont need the below code

			/*//assigning materials based on height (ground, low grass, high grass, snow
			if (totalCubes[i].GetComponent<Cube>().height < 2) {
				totalCubes[i].GetComponent<Renderer>().material = totalCubes[i].GetComponent<Cube>().selectedMaterial[0];
			}else if (totalCubes[i].GetComponent<Cube>().height < 5) {
				totalCubes[i].GetComponent<Renderer>().material = totalCubes[i].GetComponent<Cube>().selectedMaterial[1];
			}else if(totalCubes[i].GetComponent<Cube>().height < 10) {
				totalCubes[i].GetComponent<Renderer>().material = totalCubes[i].GetComponent<Cube>().selectedMaterial[2];
			} else if(totalCubes[i].GetComponent<Cube>().height < 15) {
				totalCubes[i].GetComponent<Renderer>().material = totalCubes[i].GetComponent<Cube>().selectedMaterial[3];
			}*/
		}
		/*make sure the mapping has been done..We dont want this compute intensive function to run again*/
		hasComputeButtonPressed = true;
    }

	/*pretty self explanatory*/
    void GenerateLandscape(int totalBlocksCount) {
		
        for (int i = 0; i < totalBlocksCount; i++) {
			//only execute if the cubes have not reached their height
			if (totalCubes[i].GetComponent<Cube>().hasReachedHeight == false) {
				float currentHeight = totalCubes[i].GetComponent<Cube>().currentHeight * 10f;
				float targetHeight = totalCubes[i].GetComponent<Cube>().height * 10f;
				
				if (currentHeight <= targetHeight) {
					//totalCubes[i].transform.localScale = new Vector3(
						//totalCubes[i].transform.localScale.x,
                        //totalCubes[i].transform.localScale.y,
                        //totalCubes[i].transform.localScale.z);
					totalCubes[i].GetComponent<Cube>().currentHeight += 0.05f;
				} else {
					totalCubes[i].GetComponent<Cube>().hasReachedHeight = true;
					if(totalCubes[i].GetComponent<Cube>().hasModelDropped == false) {
						/*only enable the box collider when you have reached your height. 
						This is the recommended way instead of scaling the cube and keeping the collider on*/
						//totalCubes[i].GetComponent<BoxCollider>().enabled = true;

						//DropModels(totalCubes[i].transform.position);
						//DetailedDropModels(totalCubes[i], totalCubes[i].transform.position);

						//the BIG DADDY sits here
						StartCoroutine(GenerateLandscape(totalCubes[i], totalCubes[i].transform.position));
                        totalCubes[i].GetComponent<Cube>().hasModelDropped = true;
                    }
                }
			}
        }
    }

	//and thats how u drop models like a pro.
	void DropModels(Vector3 position) {
		Vector3 actualSpawnPosition = new Vector3(position.x, position.y + 100, position.z);
		GameObject c = Instantiate(dropCube, actualSpawnPosition, Quaternion.identity) as GameObject;
	}

	//but hey, this is uber pro than the previous one in dropping models
	void DetailedDropModels(GameObject detailedCube, Vector3 position) {

		int totalCubesToBeDropped = (int)detailedCube.GetComponent<Cube>().height;
		for(int i = 0; i < totalCubesToBeDropped; i++) {
			GameObject cube = CubeDecider(i);
			Vector3 actualSpawnPosition = new Vector3(position.x, position.y + 100, position.z);
		}
	}

	/*drops heatmap specific blocks into their respective position*/
	IEnumerator GenerateLandscape(GameObject detailedCube, Vector3 position) {

		int totalCubesToBeDropped = (int)detailedCube.GetComponent<Cube>().height;
		for (int i = 0; i < totalCubesToBeDropped; i++) {
			GameObject cube = CubeDecider(i);
			Vector3 actualSpawnPosition = new Vector3(position.x, position.y + 100, position.z);
			GameObject c = Instantiate(cube, actualSpawnPosition, cube.transform.rotation) as GameObject;
			yield return new WaitForSeconds(1.5f);
		}
	}

	/*spits out a hexagon specific to heatmap*/
    GameObject CubeDecider(int cubeNumber) {
		//if (cubeNumber) {
		if(cubeNumber < 17)	
			return cubeTypes[cubeNumber];
		else
			return cubeTypes[17];
		//}
		/*
        if (cubeNumber == 0) {           //drop ground cube
            return cubeTypes[0];
        } else if (cubeNumber == 1) {     //drop a low grass cube
            return cubeTypes[1];
        } else if (cubeNumber == 2) {   //drop a high grass cube
            return cubeTypes[2];
        } else if (cubeNumber == 3) {
            return cubeTypes[3];
        } else if (cubeNumber == 4) {
            return cubeTypes[4];
        } else if (cubeNumber == 5) {
            return cubeTypes[5];
        } else if (cubeNumber == 6) {
            return cubeTypes[6];
        } else if (cubeNumber == 7) {
            return cubeTypes[7];
        } else if (cubeNumber == 8) {
            return cubeTypes[8];
        } else if (cubeNumber == 9) {
            return cubeTypes[9];
        } else if (cubeNumber == 10) {
            return cubeTypes[10];
        } else if (cubeNumber == 11) {
            return cubeTypes[11];
        } else if (cubeNumber == 12) {
            return cubeTypes[12];
        }else if (cubeNumber == 13) {
            return cubeTypes[13];
        } else if (cubeNumber == 14) {
            return cubeTypes[14];
        } else if (cubeNumber == 15) {
            return cubeTypes[15];
        } else if (cubeNumber == 16) {
            return cubeTypes[16];
        }else {
            return cubeTypes[17];
        }*/
	}
	/*----------------------------------------------------------------------------------*/
	
	/*destroys the generated grid of buttons*/
	void DestroyComputeButton() {
		//always delete button first and then image, to avoid warning spikes from Button gameobject
		Destroy(button_generate);
		Destroy(button_generate.GetComponent<Image>());
	}
}
