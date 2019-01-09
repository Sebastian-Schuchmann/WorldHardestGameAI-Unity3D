So we need a strategy. 

This is going to be a multi part series.
This time we are analysing the existing approach and see how we could do better. 
 
The common approach is via the classical evolutionary algorithm. Most of you probably know this one. In this method a random set of moves is generated for each player. After each generation the moveset with the highest fitness score wins and then, random mutations of the winner set get calculated. Survival of the fittest.  This works well in this case but it is also a bit cheated.

You see what happend here? The Dots got reset to their original position after the player died. But if we look at the original game - this is not what happens. After you die the dots just stay at their position as if nothing has happend. 

Okay, so why does this matter? We can just remove this one line in the code at compare the results. 

So you can see after 100 Generations - the cheaters beat the game everytime with a nearly perfect route. But the non-cheaters didnt even reach the goal once. How can that be?

What you have to understand is that the players dont see anything. It is just a fancy table of moves. The Player with the best fitness in one generation is probably very bad in the next one because you have to play different each time.

Of course we could just code in a few lines that let the players wait till the dots are at their orignal position and then start the movement. But it still wouldnt help when trying to solve multiple levels at once. BECAUSE there probably isnt one set of moves that can complete every level. 

We need a system where the players can actually see and react. This is where neural networks come in.

There are two different popular ways we will explore in the next video:

The first one is called NEAT (Neuroevolution of augmenting topologies).. that is basically a genetic algorithm with neural networks. 

The other one is PPO (Proximal Policy Optimization). This is learning by trial and error. We just have one network that gets better over time - similar to a classification networks that does image recognition for example. 

Subscribe if you want to know which approach works and if we cant beat the world hardest game, properly




-- Explain why it is a bit cheated (Comparison) 3 Minute Mark

-- Path Ways to get forward

- NEAT
- Reinforcement Learning

- Evolutionary approach. Works well on a single level when there is no variation in the enviroment. 
- But it is impossible for it to solve multiple levels at once.

- 

We will try a reinforcement learning approach:


