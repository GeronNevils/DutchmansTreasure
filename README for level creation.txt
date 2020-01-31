-Level geometry (walls, ceilings, floors) MUST be set on the layer "Level" for the player control script to properly detect if the player is on or off the ground

-The RoomParent prefab is a template for making individual rooms.
	-The "Entrance" object is where the player will respawn if killed in that room.
	-The "EscapeBlocker" turns solid when the player first touches the entrance.