using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class IrisFlower: Plant {

        const int DEAD = 257;
        const int stage1 = 335;
        const int stage2 = 336;
        const int stage3 = 337;
        private SpriteRenderer irisSprite;

        //CONSTRUCTOR
        public IrisFlower(int x, int y){
            updateXY(x, y);
            LoadTilesetSprites();
            setID( (int)Mathf.Floor((203 * (x * y)) + x/y) );
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
                irisSprite.sprite = stageSprites[DEAD];
            } else if (getPlantStage() == 1) { //If plant stage 1
                irisSprite.sprite = stageSprites[stage1];
            } else if (getPlantStage() == 2) { //If plant stage 2
                irisSprite.sprite = stageSprites[stage2];
            } else if (getPlantStage() == 3) { //If plant stage 3
                irisSprite.sprite = stageSprites[stage3];


            }else{
                Debug.LogWarning($"Stage ID {getPlantStage()} is out of range or sprites not loaded");
            }
        }

        public void SetSpriteRenderer(SpriteRenderer sr){
            irisSprite = sr;
        }
    }
public class flower_Iris : MonoBehaviour{

    [SerializeField] SpriteRenderer spriteRenderer; // SpriteRenderer component for the flower

    // Start is called before the first frame update
    void Start(){
        IrisFlower flower = new IrisFlower(5, 5);
        flower.SetSpriteRenderer(spriteRenderer);
        flower.updateAppearance();
    }
}

