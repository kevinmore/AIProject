If you are updating to a new version: before importing the new updated package, please delete the ancient "Advanced AI" folder from your project hierarchy.

After importing the package, It might be possible that layers/tags names are lost and not well assigned, so if you find that the Showcase scenes are not acting properly then be sure that:

- There is a "Proectile" tag associated to Projectile prefab in Advanced AI/Showcase/Prefabs folder.
-"First Person Controller" is tagged as "Player".
- Layer 8 is for Target (First Person Controller) only, name it Target.
- Layer 9 is for AI's view obstruction, name it "Los Obstacle", it is assigned to walls and floor game objects in the scene.
- Layer 10 is for enemies, name it "Enemy", it is important that Enemy AI game object are under this layer so that the "Defender" AI can attack them.
- Layer 11 is for "Companion" and "Defender" AI, it is important so that enemies can recognize your companions and/or defenders and attack them.


When I import the package into a blank new project everything worked fine, all layers and tags were assigned properly, but I'm just supposing if this happens with some users.

You Can find the demo scenes in Advanced AI/Showcase/Scenes folder.

You can find the PDF documentation in Advanced AI/Documentation folder.