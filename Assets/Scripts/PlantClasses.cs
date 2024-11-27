using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

    // Plant class
    public class Plant: MonoBehaviour{
        [SerializeField] private string tilesetPath;
        private Dictionary<string, int> gridLocation;
        private int waterLevel;
        private int sunLevel;
        private int plantStage; // plant stage goes from 0->3. 0 = dead, 1 = seed, 3 = max/grown up
        private int plantID;
        protected Sprite[] stageSprites;
        private bool spritesLoaded;

        
        // Constructor
        public Plant(){
            gridLocation = new Dictionary<string, int>();
            gridLocation.Add("x", 0);
            gridLocation.Add("y", 0);

            waterLevel = 0;
            sunLevel = 0;
            plantStage = 1;
            plantID = -11111; // The only plantID with this ID should be the "Parent", PlantObject
            tilesetPath = "Assets/Sprites/GRASS+.png";
            spritesLoaded = false;
        }

        public System.Collections.IEnumerator WaitForSpritesAndInitialize(){
            // Wait until sprites are loaded
            while (!AreSpritesLoaded()){
                Debug.Log("Sprites not loaded yet, waiting...");
                yield return null; // Wait one frame
            }

            Debug.Log("Sprites successfully loaded! Initializing...");
        }

        //HELPERS//

        // Location Functions
        public void updateX(int x){
            gridLocation["x"] = x;
        }
        public void updateY(int y){
            gridLocation["y"] = y;
        }
        public void updateXY(int x, int y){
            gridLocation["x"] = x;
            gridLocation["y"] = y;
        }

        public int getPlantX(){
            return gridLocation["x"];
        }
        public int getPlantY(){
            return gridLocation["y"];
        }

        /////!!!!//////

        // Value Functions
        public void updateWater(int w){
            waterLevel = w;
        }
        public int getWater(){
            return waterLevel;
        }

        public void updateSun(int s){
            sunLevel = s;
        }

        public int getSun(){
            return sunLevel;
        }


        public int getPlantStage(){
            return plantStage;
        }
        public void changeLevel(int l){
            if (l < 0 || l > 3) {
                Debug.LogWarning($"plant.changeLevel() ERROR: Level is {l}, but range is 0 to 3.");
            }
            plantStage = l;
        }

        public void setID(int ID){
            plantID = ID;
        }

        public void LoadTilesetSprites(){
            
            // Load the tileset asynchronously
            Addressables.LoadAssetAsync<Sprite[]>(tilesetPath).Completed += OnTilesetLoaded;

            if (stageSprites.Length == 0){
                Debug.LogError($"Could not load sprites. Ensure the tileset is in a Resources folder and correctly sliced!");
            }
        }

        private void OnTilesetLoaded(AsyncOperationHandle<Sprite[]> handle){
            if (handle.Status == AsyncOperationStatus.Succeeded){
                // Assign the loaded sprites to the stageSprites array
                stageSprites = handle.Result;
                Debug.Log($"Successfully loaded {stageSprites.Length} sprites!");
                spritesLoaded = true;
            }
            else{
                Debug.LogError("Failed to load tileset via Addressables.");
            }
        }

        protected Sprite GetStageSprite(int index){
            if (stageSprites == null || index < 0 || index >= stageSprites.Length){
                Debug.LogWarning($"Invalid sprite index: {index}. Range 0 to {stageSprites.Length}");
                return null;
            }
            return stageSprites[index];
        }

        public bool AreSpritesLoaded(){
            return spritesLoaded;
        }
    }

public class PlantClass : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){
        Plant MotherPlant = new Plant();
    }
}
    

