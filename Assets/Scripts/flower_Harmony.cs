using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class HarmonyFlower: Plant {

        const int DEAD = 257;
        const int stage1 = 289;
        const int stage2 = 291;
        const int stage3 = 299;
        private SpriteRenderer harmonySprite;

        //CONSTRUCTOR
        public HarmonyFlower(int x, int y){
            updateXY(x, y);
            LoadTilesetSprites();
            StartCoroutine(WaitForSpritesAndInitialize());
            setID( (int)Mathf.Floor((1218 * (x * y)) + x/y) );
        }

        public void updateFlower(){

            bool updateCondition = true;  //INSERT CONDITIONS LATER
            if (updateCondition) {
                changeLevel(getPlantStage()+1);
            } else {
                changeLevel(getPlantStage()-1);
            }
            updateAppearance();
        }

        public void updateAppearance(){
            if (getPlantStage() == 0) { //If plant dead
                harmonySprite.sprite = GetStageSprite(DEAD);
            } else if (getPlantStage() == 1) { //If plant stage 1
                harmonySprite.sprite = GetStageSprite(stage1);
            } else if (getPlantStage() == 2) { //If plant stage 2
                harmonySprite.sprite = GetStageSprite(stage2);
            } else if (getPlantStage() == 3) { //If plant stage 3
                harmonySprite.sprite = GetStageSprite(stage3);


            }else{
                Debug.LogWarning($"Stage ID {getPlantStage()} is out of range or sprites not loaded");
            }
            Debug.Log("Success");
        }

        public void SetSpriteRenderer(SpriteRenderer sr){
            harmonySprite = sr;
        }
    }
public class flower_Harmony : MonoBehaviour{

    [SerializeField] SpriteRenderer spriteRenderer; // SpriteRenderer component for the flower

    // Start is called before the first frame update
    void Start(){
        HarmonyFlower flower = new HarmonyFlower(5, 5);
        flower.SetSpriteRenderer(spriteRenderer);
        flower.updateAppearance();
    }
}

