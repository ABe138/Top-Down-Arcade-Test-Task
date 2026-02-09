# Top-Down Arcade Game Prototype

Прототип аркадной игры с видом сверху на Unity DOTS (Entities).

## Запуск

Открыть и запустить сцену `Assets/Scenes/Main.unity`.

## Настройка параметров

### Игрок
Префаб: `Assets/Prefabs/PlayerCharacter.prefab`
- **UnitAuthoring**: HP, скорость движения, скорость поворота

### Оружие
Дочерние объекты Player prefab'а с компонентом `WeaponAuthoring`:
- Скорость атаки, скорость снаряда, префаб снаряда, урон, кол-во снарядов за залп, разброс

### Противники
Префабы в `Assets/Prefabs/Enemies`:
- **UnitAuthoring**: HP, скорость движения, скорость поворота
- **EnemyAuthoring**: дальность атаки, урон, кулдаун, вероятность дропа, настройки obstacle avoidance 

### Спавнеры
Находятся в SubScene:
- **EnemySpawnerAuthoring**: частота спавна