<h1 align="center">Tic-tac-toe game with AI</h1>
  
<p align="center">  <img src="https://user-images.githubusercontent.com/39067344/112275804-ac5aba00-8c88-11eb-85e6-d508bb6c2317.png"> </p>

<h2>Description</h2>

<img src="https://user-images.githubusercontent.com/39067344/112382739-e792d280-8cf4-11eb-95c4-e8e019d9b037.gif" align="right" >
<p>This app is intended to be the first time AI is being used.</p>
<p>From a task that implied a textual implementation of tic-tac-toe, it grew into a full-fledged application for a phone with a visual component.</p>
<p><a href="https://clck.ru/TtJAw">Reinforcement learning</a> is used as an algorithm for AI. Since this method is good for a small number of variations of the match outcome. For convenience and understanding of the work, a line of the best states of the AI has been added to the application, as well as its recalculated values after a positive / negative outcome of the match.</p>
<p>Since the game is intended for educational purposes to show how <a href="https://clck.ru/TtJAw">Reinforcement learning</a> works, many points (visual, gameplay and logical) are not taken into account</p>

<p>To download the demo version, click on the link below (only for android)</p>
<a href="https://github.com/Poochie18/Tic-tac-toe-AI/releases/download/0.1.0/tic-tac-toe.v0.1.0r.apk">Tic-Tac-Toe</a>

<h2>How to play</h2>

<img src="https://user-images.githubusercontent.com/39067344/112382877-101acc80-8cf5-11eb-9b19-03b9125e1448.gif" align="left">
<p>The main point of the game is to arrange your three signs (X) on the field in one row. You will be confronted by a <strike>genius</strike> dumb AI. Throughout all matches, give the AI ​​a chance to win and learn from its wins and losses.</p> 

<h3> Victorious case</h3>

<div>
<img src="https://user-images.githubusercontent.com/39067344/112382478-8f5bd080-8cf4-11eb-9d5d-ac6ac3ec8fd8.jpg"  width="138" height="192" align="right" >
<p align="left">So in case of victories in the status bars of his moves for this match there will be an increase. This means that the AI ​​will remember them as high-ranking the next time it is similar. </p>
</div>
<h3> Losing case</h3>
<div>
<img src="https://user-images.githubusercontent.com/39067344/112382504-9aaefc00-8cf4-11eb-91d0-981d62f013f6.jpg" width="147" height="192" align="right">
<p>In case of defeats in the status bars of his moves for this match, there will be a decrease in coefficients. This means the AI ​​will remember them as negative moves on the next similar move.</p>
  </div>
<br>
<br>
<br>
<h2>Technologies</h2>
<p>The main AI technology used was <a href="https://clck.ru/TtJAw">Reinforcement learning</a>. The main point of which is to praise the AI ​​for victories and punish for defeats. The implementation of these awards is in the txt file.</p>
<p align="center"> <img src="https://user-images.githubusercontent.com/39067344/112297671-dcad5300-8c9e-11eb-86d5-5a79f448f1e9.png" width="500" height="500"></p>
<p>For the software and visual component, the unity game engine and the C # programming language were used.</p>
<p align="center"> 
  <img src="https://user-images.githubusercontent.com/39067344/112299290-542fb200-8ca0-11eb-9a2a-2b882f13c08a.png" width="128" height="128">
  <img src="https://user-images.githubusercontent.com/39067344/112299320-5b56c000-8ca0-11eb-8fbf-8d59f7a66cd1.png" width="114" height="128">
</p>

<h2>Future features</h2>

<p>In the next releases, all current bugs will be fixed and added:</p>
 <ul>
  <li><strike>Add move order changes</strike></li>
  <li><strike>Updatе visual design</strike></li>
  <li><strike>Main menu and settings menu</strike></li>
  <li><strike>Visualization of AI weights of all possible cells</strike></li>
  <li>Gomoku game implementation (in the distant future)</li>
</ul>

<h2>New versions features</h2>

<h3>Version 0.1.0</h3>
<p>This version is the first working version with the implementation of a sufficient minimum amount of functionality. It includes: playing for crosses, restarting the game, exiting and a table of bot moves.</p>
<h3>Version 0.2.0</h3>
<p>This version is an addition to the previous one, with the implementation of the intended functionality. Added move change (you can now play as X or O). 
All this is included in the settings menu. They also include the number of bot training iterations. </p>
<p>In the mathematical part, the coefficient of random moves has been changed. Since the bot is initially untrained, its moves should be more random in order to explore more possibilities. But over time, this coefficient decreases. This also applies to anyone's. Since draws are a high priority for O, they can be rewarded for not "losing"</p>
<h3>Version 0.3.0</h3>
<p>The main innovation in this version is the addition of a AI vs AI mode. Now you can watch the actions of two bots.</p>
<p>Redesigned settings menu. Now you can change everything in it using switches. Added hiding of the bottom panel with the bot's moves. Now, instead of iterations, you can watch the number of victories X and O.</p>
<p>Completely redesigned and UI(maybe not for the better)</p>
<h3>Version 1.0.0</h3>

<p>This is the release version. It fixes all bugs and implements all the desired features.</p>
<p align="middle">
<img src="https://user-images.githubusercontent.com/39067344/114400540-bf3b1d00-9baa-11eb-98a7-1af4d951a350.jpg" >
<img src="https://user-images.githubusercontent.com/39067344/114400594-cb26df00-9baa-11eb-8ff4-7e37a0a8ffe1.gif" >
<img src="https://user-images.githubusercontent.com/39067344/114400608-ce21cf80-9baa-11eb-9ac9-0bd16800c5af.gif" >
<img src="https://user-images.githubusercontent.com/39067344/114400629-d24ded00-9baa-11eb-9d6b-7d5b9ea72f25.jpg" >
</p>
<h2>Epilogue</h2>


<p>Thanks to everyone who pays attention to this repository. For all questions and wishes, please contact us by <a href="mailto:3030cocacola@gmail.com">mail</a></p>
<p>You can download the game from the <a href="https://github.com/Poochie18/Tic-tac-toe-AI/releases/tag/1.0.0">link</a></p>
<p>You can also visit the page at <a href="https://poochieco.itch.io/tic-tac-toe">itch.io</a> and download the game from there.</p>
