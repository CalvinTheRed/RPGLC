# MaximizeDamage

```c#
{
  "function": "maximize_damage",
  "damage_type": <string = "*">
}
```

Causes all damage dice of a specified damage type to roll their largest value. Note that the `"damage_type"` field is optional, and will default to a value of `"*"` if not supplied. This value causes the function to apply to all damage dice, rather than damage dice of a single damage type.

###### Applicable Subevents
- `DamageRoll`
- `DamageDelivery`