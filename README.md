# MMCS Game Jam 2024

## Добавленный 3rd party контент:
1. https://seansleblanc.itch.io/better-minimal-webgl-template

`Ideal for embedding on sites like itch.io which provide an external full-screen button.`

2. https://github.com/Cysharp/UniTask

`async/await чего угодно в юнити, я им обычно пользуюсь, если вам удобнее через корутины - ничто не мешает`

3. https://dotween.demigiant.com/

`Твиннер для простого создания простых анимаций кодом, я им обычно пользуюсь, если вам удобнее через аниматор юнити - ничто не мешает`

4. https://assetstore.unity.com/packages/vfx/shaders/retrowave-skies-lite-dynamic-skybox-asset-pack-282063

`RETROWAVE SKIES Lite - Dynamic Skybox Asset Pack`

## Структура проекта

`//TBA`

Если коротко, типы объектов сложены по-папками (Materials, Prefabs, Models и т.д.). Идея в том, чтобы мы 
наделали универсальных кирпичиков-префабов для всего контента игры, и на последнем этапе без проблем
собрали из них уровни. Примеры таких префабов: DefaultBackground, DefaultGeometryFloor, DefaultGeometryWall, 
DefaultLight.

Обратите внимание на компонент Depth Restriction на этих префабах! В нем выбирается вариант элемента игры, и этот
элемент насмерть приклеивается к его глубине (ось Z). Например, у всех стен должен быть выбран вариант 
Background Geometry - все стены будут всегда выравнены по глубине, как бы вы не хотели их передвинуть в редакторе - 
должно быть удобно собирать трехмерные уровни управляя только 2 осями и не задумываясь о третьей.

Настройка глубины различных вариантов делается в обьекте GameController/DepthController в SampleScene.

Так же успел сделать простейший скрипт следования камеры за персонажем и простейший основанный на физике юнити 
контроллер персонажа, который умеет ходить влево/вправо на клавиши AD и прыгать на W.