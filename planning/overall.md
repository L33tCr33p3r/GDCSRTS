**terrain:**
- dual heightmap, one for ground and one for water
- units decide which parts they are allowed on and which heightmap to use
- air units can always be considered above heightmap

**weapons:**
```ts
class Weapon:
    init:
    verticalMax: float
    verticalMin: float
    horizontalRange: float
    horizontalOffset: float
    fireRate: float
    targetableUnits: enum<UnitType>[]
    muzzles: Muzzle[]
    mutable:
    horizontalAngle: float
    verticalAngle: float
    fireCooldown: float
```
```ts
abstract class Muzzle
    init:
    damage: int
    deviance: float
```
```ts
class HitscanMuzzle:
    init:
    range: float
```
```ts
class ProjectileMuzzle:
    init:
    velocity: float
    projectile: class<Projectile>
```

**unit types:**
```ts
class Unit:
    mutable:
    position: Vector2
    speed: float
    health: int
    orders: Order[]
    posture: enum<FiringStance>
    interface Istealth: 
    mutable:
    stealthActive: bool
```

**gameplay:**

the goal is to destroy the opposing player’s command unit

**server / client:**
- server receives user commands and updates game state
- client handles user input and display of game state

- port: 4503-4533
- protocol:
    ```
    <- ADD(id: GUID, unitType: UnitType, position: Vector2, direction: float, weaponDirs: float[], orders: Order[], stance: FireStance, height: float)
    <- DEL(id: GUID)
    <- MOD(id: GUID, params*)
    <- FIRE(id: GUID, weapon: int)
    -> ORDER(ids: GUID[], order: Order)
    -> CHECK() <- CHECK(state: Hash)
    -> SYNC() <- SYNC(state: Unit[]
    ```


**resources:**
- power: create using generators, generators are better in certain areas
- mass: created using extractors built on mass nodes, potential purity factor
- randomly scattered one-time sources of mass and power

**pathfinding:**
- create terrainmap
- create main pathnodes
