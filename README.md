# Jumping Bread
Jumping Bread is a Unity-based mobile game that brings delightful tile-hitting gameplay to life. This comprehensive technical overview will dive into its key components, design choices, and optimizations, highlighting the integration of tools like Firebase Analytics and the dreamlo library for leaderboards.

**Note:** If you want to see my code, look at the C# files in the [assets folder](https://github.com/iFralex/JumpingBread/tree/main/Assets).

## 1. Unity Engine:
Jumping Bread harnesses the power of the Unity game engine, providing an ideal platform for crafting a responsive and immersive mobile gaming experience. The game is developed in Unity 2020, ensuring compatibility and performance on various devices.

## 2. Gameplay Mechanics:
![Level selection](https://github.com/iFralex/JumpingBread/assets/61825057/df9127ed-fac2-4d0e-ac94-b99ed055890c)
![Gameplay screen](https://github.com/iFralex/JumpingBread/assets/61825057/7c964fc1-7985-442a-b4b7-d1f7812f0c76)

The core gameplay revolves around players tossing slices of bread to hit colored tiles on a wall, strategically aiming to avoid tiles that deduct points. The colorful tiles represent various points, with different colors corresponding to different values. Jumping Bread offers three distinct levels, of which two need to be unlocked progressively by achieving a minimum score within a set time limit. Each level consists of five rounds, making the gameplay engaging and challenging.

## 3. Graphics and Animation:
![Round end screen](https://github.com/iFralex/JumpingBread/assets/61825057/ee0612b4-625c-4277-bd29-8d1a10b15fd3)

The game's visual elements are entirely based on Scalable Vector Graphics (SVG), a feat achieved through the use of the Vector Graphics package. Unity's native support for vector graphics is limited, and the use of SVGs required innovative solutions, ensuring high-quality visuals.

Jumping Bread boasts numerous meticulously designed animations, elevating the gaming experience from the menu to the gameplay itself. The various house-themed backgrounds create immersive in-game environments, ranging from a study to a kitchen.

![Menu animation](https://github.com/iFralex/JumpingBread/assets/61825057/07ed95b0-4020-4d64-8fa0-029c0714692e)


## 4. Firebase Analytics
Incorporating [Firebase Analytics](https://firebase.google.com/docs/analytics), Jumping Bread meticulously tracks and analyzes player interactions, offering a wealth of data that unveils user preferences and patterns. The integration of Firebase Analytics empowers developers to enhance the game's performance, identify areas for improvement, and fine-tune the overall player experience.

## 5. Leaderboards with Dreamlo:
![Leaderboard screen](https://github.com/iFralex/JumpingBread/assets/61825057/e317904d-2484-48cc-9aca-a5985c395839)
Leaderboards are essential for fostering competition and engagement. For this purpose, the [dreamlo](https://assetstore.unity.com/packages/tools/network/dreamlo-com-free-instant-leaderboards-and-promocode-system-3862) library is integrated into the game to create instant leaderboards. This library facilitates tracking player scores and globally ranking their achievements. 

## 6. Advertising Integration with AdMob:
To support the game's monetization, [AdMob](https://admob.google.com/home/) is seamlessly integrated, allowing in-game ads to provide revenue. This ensures a smoother gaming experience, while ads are strategically implemented to enhance user engagement without causing disruption.

## 7. Versatile Platform Support:
Jumping Bread is developed with versatile platform support in mind. The game at the time was published on the appStore and Google PlayStore. Today the owner of the game removed them from the stores.

## 8. Context
Jumping Bread is a project I personally developed, commissioned by a client during the summer of 2021. My client, a graphic designer, asked me to turn their vision into a mobile game.
Jumping Bread remains a tangible opportunity to showcase my development skills and expertise in creating engaging gaming experiences.
