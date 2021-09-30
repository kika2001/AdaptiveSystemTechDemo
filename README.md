# AdaptiveSystem Tech Demo
## What is an Adaptive System
An adaptive game/dynamic game difficulty balancing is a system in games which adapts to the gameplay style of the player giving him a better experience or at least more challenging.
Some games use this system to change the narrative, which is very common in interactive storytelling where you can choose the path and take certain actions which use karma or similar features.
## Type of system which this system is based
The Adaptive system was created based on the DDA system (Dynamic Difficulty Adaptation) because its a concept easy to use when talking about small data to be analysed. The system uses a ratio for difficulty, where each player will have a different difficulty depending on how good they are. 
Player with low skill playing on hard will have it more easy than pro players playing on hard, but both will have difficulties because it adapts to their skill.
## Why?
I wanted to make this system because it can be very helpful to make the game playable for players with different skill levels. When players dont feel anxious because the game is to challenging for their skill, or when they dont feel bored due to the game being too easy for their skill, they stay on the flow zone. This zone is ideal to keep the player motivated to play the game.
## About the Gameplay
In this tech demo, the player is playing an endless wave system where he needs to kill crawling monsters. He can use doors to go to the opposite side, helping running away from the monsters when there is no escape. The game ends when he dies or quits the game. The player has infinite magazines but has 30 bullets per magazine.

![Gameplayv2](https://user-images.githubusercontent.com/47228282/135369161-dd1b75b9-4c83-4ced-91ea-192038f139dc.gif)
## Technical Explanation
Right now, the system is only treating this data:

- Fires Shots
- Hitted Shots

Other data was not included due to other priorites for the game experience. More data will be added in the future
After the game collects the data for the player skill, it will modify the:

- Enemy Speed
- Enemy Quantity per Round

The programmer just needs to call the calculate function and it returns the value between the minimum and maximum depending on the ratio.
![Screenshot_1](https://user-images.githubusercontent.com/47228282/135368515-eecbb0eb-fb47-40d1-ab98-e8dcee61d20d.png)
