Controls:
Ship follows cursor.
LMB fire current weapon.
	MultiShot hold to fire 3 shot bursts. 
	Pulse Laser deals all enemies on a raycast at high frequency.
	Homing Missiles, when held fires a laser that marks targets for tracking.
		On release, rapidly fires missiles each locked on to one marked target. 
		If the a target is destroyed or despawns, the missile loses tracking and goes inactive and despawns after a short time.
		Known bug: under some circumstances (possibly related to weapon swapping or attempting to start locking on while still firing missiles from a previous lock on) the icon signifying a marked target does not show up properly.
Scroll up/down to swap weapon, after picking up weapon powerup.
RMB activates shield, 1 second invulnerability, 5 second cooldown.



Demo consists of 30 second timeline of enemies spawning and moving in different patterns. Enemies will keep spawning at a low rate for about 30 seconds more after that.
Weapon pickups spawn at 8 & 14 seconds
Ship dies after 3 hits. However it is not cleanly handled and simply disables the player object.
NO UI.



