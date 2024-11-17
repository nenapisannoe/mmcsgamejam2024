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

## Сборка уровней
![image](https://github.com/user-attachments/assets/84908379-8199-4660-88f3-2133aa28aa57)

# Уровни
Уровни находятся в Assets/Prefabs/Levels

Имена произвольные, главное добавить уровень в компонент сцены GameController

Есть уровень BASE, который можно дублировать и брать за основу для новых уровней. Или взять любой другой готовый!

# Базовая структура уровней (полы, стены, фоновые стены)

Находятся в Assets/Prefabs/LevelGeometry - вытаскиваются в префаб уровней и формируют "локацию"

# Враг

Находится в префабе Assets/Prefabs/Characters/EnemyCharacter.prefab (не перепутайте с Enemy.prefab!)

Вытаскивается на уровень, после чего требуется настройка:

Если нужно чтобы враг смотрел в другую сторону, в компоненте Transform задаем Rotation Y = 180.

В компоненте EnemyController основные настройки:

MoveSpeed - скорость передвижения, если будет двигаться

Color - цвет врага. Меняет визуал, а так же задает цвет, который выпадет с врага при его убийстве. Отображается в редакторе сцены цветом шарика над головой, а так же цветом конуса обзора врага:

![image](https://github.com/user-attachments/assets/57f086e7-87c9-465f-aa61-b8487f93d8cc)

Patrol Distance Left/Right - расстояние в единицах юнити, которое враг будет патрулировать влево-вправо от своей первоначальной позиции. Отображается в редакторе сцены цветными стенами:

![image](https://github.com/user-attachments/assets/a5ba5556-9450-40df-9fe7-32623ab95f72)

# Движущаяся платформа

Находится в префабе Assets/Prefabs/LevelGeometry/MovingPlatform.prefab

Вытаскивается на уровень, после чего требуется настройка:

В компоненте MovingPlatform основные настройки:

Speed - скорость передвижения, если будет двигаться

Distance Left/Right - расстояние в единицах юнити, которое платформа будет пролетать влево-вправо от своей первоначальной позиции. **Платформа меняет направление только когда центр платформы достигает точки!** Отображается в редакторе сцены стенами:

![image](https://github.com/user-attachments/assets/3f9ac1ec-27f8-4c1b-a9ae-0f9098278cbc)

# Краски

Находится в префабе Assets/Prefabs/Paints/PaintBucket.prefab

Вытаскивается на уровень, после чего требуется настройка:

В компоненте Paint:

Color - задает цвет краски. Меняет визуал, а так же задает цвет, в который покрасится игрок при касании. Отображается в редакторе сцены цветом шарика над краской:

![image](https://github.com/user-attachments/assets/10e592d7-b1e0-4517-9e31-f79b73198704)

# Прожектор

Находится в префабе Assets/Prefabs/Lights/Projector.prefab

Вытаскивается на уровень, после чего требуется настройка.

Дальность луча: в компоненте Light внутри прожектора в обьетке Spotlight, поле Range. Настраивать осторожно, может влиять на визуал.

![image](https://github.com/user-attachments/assets/1f0cff8f-50a8-4dde-bd7d-b9a5b8cd215c)

**Все остальное настраивается только через компонент пульта! Если нужно настроить прожектор не управляемый игроком, то на вытащенный префаб нужно добавить компонент LightProjectorController, после чего произвести настройку в нем (см. ниже)**

# Пульт прожектора

Находится в префабе Assets/Prefabs/Lights/ProjectorControl.prefab

Вытаскивается на уровень либо добавляется компонентом на прожектор, после чего требует настройки:

![image](https://github.com/user-attachments/assets/577be11d-b8c9-41b2-96f1-3cf08bdaaef4)

Light Projector - обязательно указываем прожектор, которым управляет пульт. Если это не пульт, а компонент пульта на прожекторе - нужно указать этот же прожектор, чтобы настройки передались прожектору

Is Enabled - включен ли прожектор по умолчанию.

Player Can Modify Is Enabled - может ли игрок менять эту настройку через пульт

Current Angle Delta - наклон направления прожектора влево-вправо относительно строго вертикального направления. Невозможно задать не изменив следующие два параметра:

Min/Max Angle Delta - минимальный и максимальный наклон прожектора. Задать текущий наклон прожектора, не входящий в рамки этих двух значений, не получится.

Player Can Modify Current Angle Delta - может ли игрок менять угол прожектора через пульт

Color - цвет прожектора

Player Can Modity Red/Green/Blue - может ли игрок менять цвет прожектора, и какие каналы именно

# Декор

Может лежать где угодно, те что есть сейчас лежат в Assets/Prefabs/Props

Вытаскивается на уровень и размещается произвольно. Если хочется выровнять все декоры - можно добавить вытащенному префабу компонент DepthRestriction - в нем есть 3 специальных варианта для фиксации декора по глубине

## ВАЖНО

Кажется есть косяк, изза которого параметры задаваемые в некоторых префабах могут не сохраниться автоматически. Тут не знаю как быстро поправить (в коде вызываю OnValidate, который как будто за этим должен следить), и может быть мне вообще показалось, но на всякий случай после изменения параметров рекомендую сохранять префаб вручную (Ctrl+S)
