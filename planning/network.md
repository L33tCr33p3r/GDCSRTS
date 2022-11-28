- server receives user commands and updates game state
- client handles user input and display of game state

- port: 4503-4533

the following commands can be sent between clients and the server (`->` is sent from a client, `<-` is sent from the server)

```
<- ADD(id: GUID, unitType: UnitType, position: Vector2, direction: float, weaponDirs: float[], orders: Order[], stance: FireStance, height: float)
<- DEL(id: GUID)
<- MOD(id: GUID, flags: bool[], changes...) ; the flags tell which attributes are getting changed
<- FIRE(id: GUID, weapon: int)
-> ORDER(order: Order, ids: GUID[])
-> CHECK() <- CHECK(state: Hash)
-> SYNC() <- SYNC(state: Unit[]
```