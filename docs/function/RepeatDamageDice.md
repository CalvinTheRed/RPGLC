# RepeatDamageDice

```c#
{
  "function": "repeat_damage_dice",
  "count": <long = 1>
}
```

Repeats the first damage die contained within a damage collection a specified number of times. Note that the `"count"` field is optional, and will default to a value of `1` if not specified.

###### Applicable Subevents
- `CriticalHitDamageCollection`
- `DamageCollection`