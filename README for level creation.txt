-Level geometry (walls, ceilings, floors) MUST be set on the layer "Level" for the player control script to properly detect if the player is on or off the ground

-The RoomParent prefab is a template for making individual rooms.
	-Be sure to place the main camera into each room's level controller script.
	-The "Entrance" object is where the player will respawn if killed in that room.
	-The "EscapeBlocker" turns solid when the player first touches the entrance.

-Enemies must be use the "Enemy" tag to properly interact with the player
	-Killable enemies use the "EnemyD" tag
	-And killzones must use the "Killzone" tag

-Chains (and other destructible objects) must use the "Obstacle" tag

-Enemies and obstacles MUST have a rigidbody2D component to detect collision with the club effect

-Chains, for whatever reason, can NOT be child objects of the chain parent collider object.

-Rotating cannons should have fire cooldown set equal to rotation duration