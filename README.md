# GOAP-AI-RTS-Project
An RTS project with focus on enemy AI. In this project I used the goal oriented action planning method (GOAP) to create an enemy AI that builds its own base and attacks the player. 

# What is goal oriented action planning?


Goal oriented action planning short (GOAP) is an AI programming pattern that doesn't rely on a simple state machine but rather uses 
so called Actions which are being executed in a specific order that is defined by the ActionPlanner. 


<img width="1152" height="648" alt="Goal_Oriented_Action_Planning_Graph" src="https://github.com/user-attachments/assets/1af68850-58ad-466d-b83c-9d168d654ce5" />



AI Agent that controls the enemy faction:
[GoapAgent](https://github.com/AnubisDev161/GOAP-AI-RTS-Project/blob/c9134d9b7146bca2f035e81d6f864a126d914d47/Assets/Scripts/GOAP/GoapAgent.cs)

Goals - what the agent tries to achieve with action plans:
[AgentGoal](https://github.com/AnubisDev161/GOAP-AI-RTS-Project/blob/bcc0add75dcf24f63f01e6dfe39cc10a43645e37/Assets/Scripts/GOAP/AgentGoal.cs)

Beliefs - Represents the current world state of the agent to track if a goal as been achieved or needs to be discarded:
[AgentBelief](https://github.com/AnubisDev161/GOAP-AI-RTS-Project/blob/3193fc97c1ee49c02e322f886552ed2cdb1ded3e/Assets/Scripts/GOAP/AgentBelief.cs)

Action - A simple action that can be combined with others to create a plan:
[AgentAction](https://github.com/AnubisDev161/GOAP-AI-RTS-Project/blob/6acf3f3d0d790854e32c8714bf0663f538515150/Assets/Scripts/GOAP/AgentAction.cs)

Action planner - Creates action plans which consist of one or more actions needed to satisfy an agents goal:
[ActionPlanner](https://github.com/AnubisDev161/GOAP-AI-RTS-Project/blob/43bb094071edbdaa9f77f39e0b4af4ca39710554/Assets/Scripts/GOAP/GoapPlanner.cs)

Disclaimer: I'm still a student and when I started this project I was very new to Unity and didn't have any experience in making RTS games. 
My goal was just to figure out whether it is possible to implement GOAP in an RTS AI. 
If I had had the time, I would have rewritten most of the code, especially everything related to the player.

Reference: https://www.youtube.com/watch?v=T_sBYgP7_2k

Sources: https://www.youtube.com/watch?v=PaOLBOuyswI

