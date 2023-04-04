# Run Game Graphical Update
# Proposal
<details><summary>DETAILS HERE</summary>
 Elevator Pitch Link: https://youtu.be/qER0GKwGnwc </br>
 Pitch Slides Link: https://docs.google.com/presentation/d/14rdcaKjrJyhEeIMO3NwmRdRuwgVImMxVlJH7nQYCQqg/edit#slide=id.g1cf01f46c23_0_70 </br>
 Pitch Deck PDF:
 [Game Proposal.pdf](https://github.com/JayceLovell/Run-Game-Graphical-Update/files/10418794/Game.Proposal.pdf)</br>
 Kanban Board(Trello): https://trello.com/b/YNe2jzOI/gdc </br> 
 # Roles
 Jayce Lovell: Programmer & Game Designer - Lead programmer who will be taking the core programming role in terms of backend code development and game design. </br></br>
 Jelani Garnes: Programmer, Art Designer and Sound Designer - Second programmer who  will also be responsible for code updates,along with game level design and updates, VFX design, SFX updates and game art updates.
</details>
 
 # Group Assignment
 <details><summary>DETAILS HERE</summary>
 
 ### Presentaion:
 https://youtu.be/rehzmnpWkK4
 
 ### Slide Deck:
 [RunGameGraphicalUpdatePresentation.pdf](https://github.com/JayceLovell/Run-Game-Graphical-Update/files/10835300/RunGameGraphicalUpdatePresentation.pdf)

 ### Download Game
 [Run Game v0.1.8-Beta](https://github.com/JayceLovell/Run-Game-Graphical-Update/releases/tag/v0.1.8-beta)
 ### Part 1: Game Base & Description
The objective is for the player to find his/her way out of a dark maze using a flashlight with a depleting battery life and enemies who attack or runaway if the light is placed on them, if the flashlight battery dies before the player escapes or the layer is killed by an enemy they lose. Around the map are moving collectable batteries to replenish the flashlight battery life.

 #### Controls
 Movement - WASD </br>
 Look - Mouse pointer </br>
 Jump - Spacebar </br>
 Sprint - Shift </br>
 Pause - P </br>
 Flashlight Toggle - Left Button (Mouse) </br>
 Recharge Battery - R </br>
 View Switch/Color grading) - 1 </br>
 
 #### Integrated Shaders
 ##### Simple Specular
 ![Screenshot 2023-02-26 162345](https://user-images.githubusercontent.com/35810049/221447313-7ad7ea6f-3f9c-42cf-aab1-11f369319c80.png)
 
 ##### Normal Mapping
 ![walls](https://user-images.githubusercontent.com/35810049/221447683-ae436a14-b412-4746-83cb-73774f9c3c7f.png) </br>
 For the normal map shader on the walls and platform ground is creates an illusion of surface details by altering the surface normal using a normal map texture. Which encodes the direction of the surface normal in rgb color values.

 ##### Toon Ramp
 ![image2](https://user-images.githubusercontent.com/35810049/221447780-de9d573b-f26b-4ae3-9bc4-18bac2d71522.jpg)</br>
 We used toon ramp as an additional visual indicator that the enemy/spook is chasing the player
 
 ##### Color Grading
 ![image0](https://user-images.githubusercontent.com/35810049/221447507-9b6533fd-84b6-4d8d-a656-bb858e0f8fb2.jpg)</br>
 The color grading shader was imlemented using the color grading scripts done in the lecture exercise where the script defines the amount of colors on the lookup table, gets the scene color, add precision to the sampling so it doesn't go beyond the LUT limits and calculates the offset to map the image to the LUT. Additionally, using the a c# script to copies source texture into destination render texture with a shader by using Graphics.Blit. </br>
 
 We used a black and white color grade effect as it best fits the theme of the game. 
 
### Part 2: Shadows
![image](https://user-images.githubusercontent.com/35810049/221447139-74a78f80-ee1f-41b2-9da6-01975d35b4c9.png)</br>

The shader on the flashlight and spook models, is basically implemented where reflection on the material takes the lights color and direction on the object's albedo since specular itself doesn't have a color and produces somewhat of a shininess also producing a shadow based on the light direction which in our case was a spot light above the game object.

### Part 3: Visual Effect
#### Particle Effect - Battery Erosion
https://user-images.githubusercontent.com/35810049/221447954-21bfee06-2fde-4134-b130-a1d34518e2bd.mp4
</br>The particle effect implemented is an erosion type effect custom shader which is used in conjunction with the unity particle system. The effect plays after the player collects a battery.

#### Decal
![image5](https://user-images.githubusercontent.com/35810049/221448316-848d7cf0-b2b6-4c36-8921-3fbc228ac153.jpg)</br>
https://user-images.githubusercontent.com/35810049/221448448-2749532d-8283-4863-8adc-870569b72b82.mp4
</br>We added a breadcrumb decal system in the gaame so the player knows where that have travelled before in the maze

#### Lens Flare
![lens](https://user-images.githubusercontent.com/35810049/221448618-8ef2a7a2-ad45-4737-bb85-90bab54fcd1b.jpg)
</br>The code takes the lens flare component and calculates the brightness and falloff, applies it to the spotlight at the end of the game. Also allows the changing of the color of the spotlight

### Part 4: Postprocessing Effects
#### Rim Lighting
![Screenshot 2023-02-04 145409](https://user-images.githubusercontent.com/35810049/221448849-fd8752b4-e86b-420a-8d7e-d494ee265028.jpg)
</br>The rim lighting shader effect was added to the collectable battery object using the shader script done in the lecture with a few updates to take a texture. 

#### Bloom
https://user-images.githubusercontent.com/35810049/221449062-9a721bda-f358-4d89-92bc-e9d0a347556b.mp4
</br>When the player mouse hovers over the car it glows, achieved by using the bloom effect shader and the threshold is adjusted by the MouseOn method being called.


#### Vignette
 ![vin](https://user-images.githubusercontent.com/35810049/221449114-e5512bc7-f8a7-4f01-9c3b-ba7d042c1cd2.jpg)
 </br>The shader finds the center of the screen and calculates how far each pixel is from the center. Each pixel becomes darker the urther it is away from the center giving the scene a more horror type feel

### References
[unity documents](https://docs.unity3d.com/Manual/Shaders.html) 
</details>
 
 # Final Project Update
  <details><summary>DETAILS HERE</summary>
 
### Project Build:
 https://github.com/JayceLovell/Run-Game-Graphical-Update/releases/tag/v1.0.0
### Presentaion Slides:

### Game Demo Video: 
https://www.youtube.com/watch?v=aYY3ExYlNtU&ab_channel=JayceLovell
 
### Team Contributions:
 
### Final Updates
#### Basic Texturing
 ##### Simple Specular
 ![Screenshot 2023-02-26 162345](https://user-images.githubusercontent.com/35810049/221447313-7ad7ea6f-3f9c-42cf-aab1-11f369319c80.png)
 
 ##### Normal Mapping
 ![walls](https://user-images.githubusercontent.com/35810049/221447683-ae436a14-b412-4746-83cb-73774f9c3c7f.png) </br>
 For the normal map shader on the walls and platform ground is creates an illusion of surface details by altering the surface normal using a normal map texture. Which encodes the direction of the surface normal in rgb color values.

#### Post Processing Color Correction
For an update from the group assignment we used the black and white LUT as a game addition. When the player goes into peek mode which basically gives them the ability to see through the entire maze, it uses the black and white LUT to give the effect that the players view is changed and also fits the theme of the game more.</br>
![window](https://user-images.githubusercontent.com/35810049/229875694-d6c9810d-8a0c-47ce-8779-5990c57f139b.png)

#### Visual Effects
For some of the visual effects we have our particles effect when a battery is collected, decals for bread crumbs and rim lighting on battery.
##### Particle Effect - Battery Erosion </br>
https://user-images.githubusercontent.com/35810049/221447954-21bfee06-2fde-4134-b130-a1d34518e2bd.mp4
</br>The particle effect implemented is an erosion type effect custom shader which is used in conjunction with the unity particle system. The effect plays after the player collects a battery.

##### Decal
![image5](https://user-images.githubusercontent.com/35810049/221448316-848d7cf0-b2b6-4c36-8921-3fbc228ac153.jpg)</br>
https://user-images.githubusercontent.com/35810049/221448448-2749532d-8283-4863-8adc-870569b72b82.mp4
</br>We added a breadcrumb decal system in the gaame so the player knows where that have travelled before in the maze</br>

##### Rim Lighting
![Screenshot 2023-02-04 145409](https://user-images.githubusercontent.com/35810049/221448849-fd8752b4-e86b-420a-8d7e-d494ee265028.jpg)
</br>The rim lighting shader effect was added to the collectable battery object using the shader script done in the lecture with a few updates to take a texture. 

#### Additional Effects
##### Toon Shading with Shadows
![toonShadow2](https://user-images.githubusercontent.com/35810049/229911043-0ddf0726-eee9-4108-a065-b1cedc62698f.png)
###### Shader Explanation
![toonShadow](https://user-images.githubusercontent.com/35810049/229911153-fe248ea2-fbad-4e51-a7c7-b3e7a9bc910b.png)
##### Outline with Bump Map
![outline2](https://user-images.githubusercontent.com/35810049/229911254-9763754e-516c-4992-ae1a-2259bcb02fa1.png)
###### Shader Explanation
![outline](https://user-images.githubusercontent.com/35810049/229911299-b7e949b4-2a8d-4008-a8be-2ad7d173168a.png)
##### Sky (Water)
![sky2](https://user-images.githubusercontent.com/35810049/229911409-8336b41f-db2b-4931-ba86-fc7398275966.png)
###### Shader Explanation
![sky](https://user-images.githubusercontent.com/35810049/229911447-8d16c2b8-d796-428b-b314-25f103db2333.png)
##### Windows (Stencil Shader) [Peek]
![window](https://user-images.githubusercontent.com/35810049/229912033-1f5379fa-1d63-4c6e-8d6e-118fa018e024.png)
###### Shader Explanation
![window2](https://user-images.githubusercontent.com/35810049/229912208-9f558590-68e8-4212-9d90-d698a77f4a2f.png)


#### Additional Post Processing Effect
##### Pixilation Blur
![pixel](https://user-images.githubusercontent.com/35810049/229912449-c265d89d-af1a-4704-9967-8b79021716d7.png)
###### Shader Explanation
From beginning on render image then set the width and height of the texture then we create initial destination texture then Blit the source texture to the initial destionation texture next is a for loop through the iterations.
Check if the heigh is less than 2 to stop the for loop if not proceed to blit the texture.
After the forloop we loop again through any remaining iterations and bit the textures.
Then when finish we loop through in reverse order to create the final blurred image then Blit the final image to the Render.Now we will play a demo showing all the newly implemented effects.
This effect only happens when the player gets hit by ghost as a hit feedback before being respawned at the beginning of the maze.


 </details>
 

