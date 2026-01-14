# Poly Legends (Graduation Project)
https://www.bilibili.com/video/BV1iar4B6E9D/?spm_id_from=333.1387.homepage.video_card.click&vd_source=459b9d328158060b3e8b9881ead4cd51

Action-RPG prototype built in Unity 2022.3.53f1c1. The project features point-and-click navigation, stat-based combat, quest and dialogue systems, inventory with drag-and-drop equipment, scene transitions with save/load, and lightweight UI for health/XP and quest tracking.

**Editor version**: 2022.3.53f1c1 (LTS)  
**Render pipeline**: URP assets are present; keep Universal Render Pipeline in Graphics settings.  
**Packages in use**: Cinemachine (free-look follow), DOTween (UI text tweening), Unity NavMesh (agent movement).

## Gameplay & Controls
- Left Mouse: move to ground; click enemy/attackable to engage. Cursor changes by tag via [Assets/Scripts/Managers/MouseManager.cs](Assets/Scripts/Managers/MouseManager.cs).
- Right Mouse: start dialogue when inside an NPC trigger ([Assets/Scripts/Dialogue/DialogueController.cs](Assets/Scripts/Dialogue/DialogueController.cs)).
- E: interact with portals/transition points ([Assets/Scripts/Transition/TransitionPoint.cs](Assets/Scripts/Transition/TransitionPoint.cs)).
- B: toggle backpack and stats panels ([Assets/Scripts/Inventory/Logic/MonoBehavior/InventoryManager.cs](Assets/Scripts/Inventory/Logic/MonoBehavior/InventoryManager.cs)).
- Q: toggle quest log ([Assets/Scripts/Quest/UI/QuestUI.cs](Assets/Scripts/Quest/UI/QuestUI.cs)).
- S / L: manual save/load (PlayerPrefs) during play ([Assets/Scripts/Managers/SaveManager.cs](Assets/Scripts/Managers/SaveManager.cs)).
- Esc: return to main menu scene via SceneController.
- Action bar keys: configured per button in the inspector, handled by [Assets/Scripts/UI/ActionButton.cs](Assets/Scripts/UI/ActionButton.cs).

## Core Systems
- **Player & Camera**: Player is registered to the singleton GameManager so Cinemachine FreeLook follows and looks at the shoulder target ([Assets/Scripts/Managers/GameManager.cs](Assets/Scripts/Managers/GameManager.cs)).
- **Movement & Combat**: Player and enemies use NavMeshAgent. Player attacks on approach; critical chance and cooldown come from AttackData. Enemies run a finite-state controller (Guard/Patrol/Chase/Dead) and face targets before applying damage ([Assets/Scripts/Characters/PlayerController.cs](Assets/Scripts/Characters/PlayerController.cs), [Assets/Scripts/Characters/EnemyController.cs](Assets/Scripts/Characters/EnemyController.cs)).
- **Stats & Damage**: Runtime stats are cloned from ScriptableObject templates. Damage = attacker roll minus defender defence; critical hits play hit reactions. Kills grant EXP and can level-up max health ([Assets/Scripts/Charactor Stats/MonoBehavior/CharacterStats.cs](Assets/Scripts/Charactor%20Stats/MonoBehavior/CharacterStats.cs), [Assets/Scripts/Charactor Stats/Scriptable Object/CharacterData_SO.cs](Assets/Scripts/Charactor%20Stats/Scriptable%20Object/CharacterData_SO.cs), [Assets/Scripts/Combat/AttackData_SO.cs](Assets/Scripts/Combat/AttackData_SO.cs)).
- **Inventory & Equipment**: Three containers (bag/action/equipment) with drag-and-drop swapping and stacking. Weapons override animator and AttackData; consumables heal the player. UI refresh is centralized in InventoryManager ([Assets/Scripts/Inventory/Logic/MonoBehavior/InventoryManager.cs](Assets/Scripts/Inventory/Logic/MonoBehavior/InventoryManager.cs), [Assets/Scripts/Inventory/UI/SlotHolder.cs](Assets/Scripts/Inventory/UI/SlotHolder.cs)).
- **Items & Loot**: Items are ScriptableObjects defining type, icon, stack, prefab, and weapon data. Pickups auto-add to inventory and update quests. Enemies can drop weighted loot via [Assets/Scripts/Inventory/Item/MonoBehavior/LootSpawner.cs](Assets/Scripts/Inventory/Item/MonoBehavior/LootSpawner.cs).
- **Dialogue**: DialogueData stores pieces, portraits, and branching options. DialogueUI types text with DOTween and instantiates option buttons. Options can accept/complete quests and branch to other pieces ([Assets/Scripts/Dialogue/DialogueController.cs](Assets/Scripts/Dialogue/DialogueController.cs), [Assets/Scripts/Dialogue/UI/DialogueUI.cs](Assets/Scripts/Dialogue/UI/DialogueUI.cs), [Assets/Scripts/Dialogue/UI/OptionUI.cs](Assets/Scripts/Dialogue/UI/OptionUI.cs)).
- **Quests**: QuestManager tracks a list of QuestTask instances, persisting them with PlayerPrefs. Progress updates on enemy death or item pickup; completion unlocks rewards and can consume required items. QuestUI shows quest list, requirements, and rewards ([Assets/Scripts/Quest/Logic/QuestManager.cs](Assets/Scripts/Quest/Logic/QuestManager.cs), [Assets/Scripts/Quest/Logic/QuestData_SO.cs](Assets/Scripts/Quest/Logic/QuestData_SO.cs)).
- **Saving/Loading**: SaveManager serializes ScriptableObject data to PlayerPrefs (JSON) and records the last scene. SceneController saves player, inventory, and quests before transitions, then reloads after spawning the player at destination tags ([Assets/Scripts/Managers/SaveManager.cs](Assets/Scripts/Managers/SaveManager.cs), [Assets/Scripts/Transition/SceneController.cs](Assets/Scripts/Transition/SceneController.cs)).
- **Scene Flow**: TransitionPoint listens for E to trigger SceneController, which may fade via SceneFader and load another scene or warp within the same one. Destination transforms are tagged through [Assets/Scripts/Transition/TransitionDestination.cs](Assets/Scripts/Transition/TransitionDestination.cs).
- **UI & HUD**: Player HUD shows level/health/EXP sliders; world-space health bars appear for characters and face the camera. Tooltips follow the cursor. Main menu buttons play a timeline before starting new game or continuing ([Assets/Scripts/UI/PlayerHealthUI.cs](Assets/Scripts/UI/PlayerHealthUI.cs), [Assets/Scripts/UI/HealthBarUI.cs](Assets/Scripts/UI/HealthBarUI.cs), [Assets/Scripts/UI/MainMenu.cs](Assets/Scripts/UI/MainMenu.cs)).
- **Audio & Camera Utilities**: BGM and player audio helpers live under Assets/Scripts/HMJY; free camera controller available for debug (see that folder for details).

## Data Assets (ScriptableObject)
- Character stats templates: [Assets/Scripts/Charactor Stats/Scriptable Object/CharacterData_SO.cs](Assets/Scripts/Charactor%20Stats/Scriptable%20Object/CharacterData_SO.cs)
- Attack parameters: [Assets/Scripts/Combat/AttackData_SO.cs](Assets/Scripts/Combat/AttackData_SO.cs)
- Items and consumables: [Assets/Scripts/Inventory/Item/ScriptableObject/ItemData_SO.cs](Assets/Scripts/Inventory/Item/ScriptableObject/ItemData_SO.cs), [Assets/Scripts/Inventory/Logic/ScriptableObject/UseableItemData_SO.cs](Assets/Scripts/Inventory/Logic/ScriptableObject/UseableItemData_SO.cs)
- Inventories: [Assets/Scripts/Inventory/Logic/ScriptableObject/InventoryData_SO.cs](Assets/Scripts/Inventory/Logic/ScriptableObject/InventoryData_SO.cs)
- Dialogue: [Assets/Scripts/Dialogue/Logic/DialogueData_SO.cs](Assets/Scripts/Dialogue/Logic/DialogueData_SO.cs)
- Quests: [Assets/Scripts/Quest/Logic/QuestData_SO.cs](Assets/Scripts/Quest/Logic/QuestData_SO.cs)

## Project Structure (high level)
- Assets/Scenes: Main menu, Forest level, Room level ([Assets/Scenes](Assets/Scenes)).
- Assets/Scripts: managers, combat, characters, inventory, dialogue, quests, UI, transitions, tools.
- Assets/Game Data: ScriptableObject instances for attack/character/dialogue/inventory/item/quest data.
- Assets/Prefabs, Materials, Animations, Audio, etc. for content.

## Running the Project
1. Install Unity 2022.3.53f1c1 (or compatible 2022.3 LTS) and open the project folder in Unity Hub.
2. Open scene Assets/Scenes/Main/Main.unity as the entry point. Ensure scenes you want are in Build Settings (Main, Forest, Room).
3. Press Play. Use Main Menu to start a new game or continue (PlayerPrefs-based).
4. To build, configure your target platform in Build Settings; URP assets are already present.

## Saving & Persistence
- Player, inventory, and quest state are saved to PlayerPrefs as JSON. Save on S key or when SceneController transitions. Clear PlayerPrefs to reset progress (New Game already clears).
- Saved scene name is stored to resume via Continue.

## Known Limitations / Notes
- PlayerPrefs storage is unencrypted and limited; not suitable for large or sensitive data.
- Enemy AI is simple chase/guard/patrol; no avoidance or flocking.
- No pooling for loot or UI elements; heavy spawn counts may impact performance.
- Input is hardcoded to keyboard/mouse; remapping and gamepad are not implemented.

## Assets & Licensing
- This repository uses Git LFS for large assets (.fbx, .png, .wav, .mp4, .zip, .asset, etc.). Ensure LFS is installed before cloning or pulling.
- Third-party assets (e.g., hero packs, fonts) remain under their original licenses; review source packages before redistribution.
