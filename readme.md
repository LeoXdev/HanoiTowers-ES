### HanoiTowers in Unity (ES)

An interactive HanoiTowers minigame made in 2 days school project, also my first work on 3D.

'Builds' and 'Library' folders are not included as they are very storage-expensive and are not mandatory to start editing the project in unity.
I'll edit this readme when an online demo is available.

Made with LTS 2020.3.26f1 version.

#### Changelog (in Features, Changes, Bugfixes order):

##### v1:
- Initial functional version.

#### v1.1:
- Lights trigger when you win a play to indicate victory.
- Game no longer stops working properly when winning a first play (player is able to spawn disks again).

#### v1.2:
- Each disk numeric value is now visible at naked eye in case disk size is not enough to calculate a valid move.
- Tweaked game camera angle to a closer view.
- Fixed a bug where trying to move a disk into the same origin tower made it go upward a bit.

#### v1.3:
- Added an exit button.
- Finishing a play will no longer restart the game automatically after a while
- When player tries to move a disk into the same origin tower, the disk will stop being selected anymore (instead of keeping the game still pending of a movement).
- Fixed a bug introduced in v1.1 where restart button couldn't work properly, not allowing players to move disks after restarting and selecting an amount of disks to spawn.
