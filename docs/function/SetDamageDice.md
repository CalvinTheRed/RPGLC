# SetDamageDice

```c#
{
  "function": "set_damage_dice",
  "set": <set formula>,
  "damage_type": <string = "*">,
  "lower_bound": <long = 0>,
  "upper_bound": <long = long.MaxValue>
}
```

Sets the value of damage dice of a specified damage type that rolled within a defined range in a damage roll. Note that the upper and lower bounds are inclusive for the purpose of determining if a value should be rerolled.

Note that the `"damage_type"` field is optional, and will default to a value of `"*"` if not specified. This value causes the function to reroll eligible dice of all damage types, rather than of only a particular damage type.

Note that the `"lower_bound"` field is optional, and will default to a value of `0` if not specified.

Note that the `"upper_bound"` field is optional, and will default to a value of `long.MaxValue` if not specified.

###### Applicable Subevents
- `DamageRoll`