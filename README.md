There is a few procedural generation methods:
- [SimpleRoomPlacement](#simpleroomplacement)
- [BinarySpacePartition](#binaryspacepartition)
- [CellularAutomata](#cellularautomata)
- [VoronoiNoise](#voronoinoise)

To use any of them, place the ProceduralGridGenerator script in your scene, and place either of these's scriptable objects inside it.
Its settings can then be modified from the inspector.

<img width="492" height="482" alt="Screenshot 2025-11-14 164414" src="https://github.com/user-attachments/assets/4966bdcf-2e62-4ce4-bfd7-e39371ae2851" />


## SimpleRoomPlacement

Places rooms and links them with corridors, much less logic than the others.
<img width="543" height="538" alt="image" src="https://github.com/user-attachments/assets/0167fbb7-119d-4f00-96c0-be485a05f04b" />

## BinarySpacePartition

Does not work right now

## CellularAutomata

Cells that increasingly grow in size if they're near each other.                
<img width="539" height="539" alt="image" src="https://github.com/user-attachments/assets/b59880e9-29b9-4f6d-a726-cacd45d97818" />

## VoronoiNoise

Voronoi noise based generation, with layers!                                  
<img width="539" height="539" alt="image" src="https://github.com/user-attachments/assets/e89eef48-7c6b-4ef7-863e-e314006a15bb" />
<img width="469" height="629" alt="image" src="https://github.com/user-attachments/assets/48d52034-ee1d-4739-a2fd-913a424eb6e7" />

Feel free to implement these later

<img width="541" height="384" alt="image" src="https://github.com/user-attachments/assets/57c8875c-eb1d-47db-80c8-b551379bd6cf" />


