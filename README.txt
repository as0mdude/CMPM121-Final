# README: Reap and Sow Game Development  
By Team 21


## Setting Up the Project  
1. Download and install **Unity Hub** if you haven’t already.  
2. Add this directory to Unity Hub to start working on the game.  

### Important Notes on Version Control  
- Do **not** commit the `Packages`, `Logs`, or `Temp` files to the GitHub repository.  
- Committing these files will prevent you from pushing changes to the remote repository.  
- Avoid editing the `.gitignore` file unless you are fully aware of the consequences.  

---

## Game Mechanics  
### Core Features  
- **Turn Management**:  
  - The game features a turn-based system managed by the **TurnManager** script.  
  - Press **"O"** to progress to the next turn.  

- **Reap and Sow**:  
  - Use **Mouse1** (left-click) to reap and sow plants.  
  - You can only reap **3 plants per turn**.  
  - Reap and sow actions are restricted to a **1-block radius** around the player, with the exception that diagonal actions are not allowed.  

- **Plant Growth**:  
  - Each plant progresses through **three growth stages**:  
    - Grass  
    - Shrub  
    - Tree  
  - Plants grow automatically as turns progress.  
  - Sowing is only allowed during the **Tree phase** of the plant's lifecycle.  

- **Winning Condition**:  
  - The game ends when **10 trees** have been successfully sowed.  

---

## Unity Configuration  
### Tags  
Tags in Unity are not stored in a specific file but within the Unity Editor.  
- Before making changes or testing the project, create a **Planted** tag in your Unity project.  

Failure to configure tags correctly may cause issues during development and testing.  

---

Enjoy developing the game! If you have questions or encounter any issues, feel free to reach out to the team.
