# RerollTemporaryHitPointDice

```c#
{
  "function": "reroll_temporary_hit_point_dice",
  "lower_bound": <long = 0>,
  "upper_bound": <long = long.MaxValue>
}
```

Rerolls temporary hit point dice that rolled within a defined range in a healing roll. Note that the upper and lower bounds are inclusive for the purpose of determining if a value should be rerolled.

Note that the `"lower_bound"` field is optional, and will default to a value of `0` if not specified.

Note that the `"upper_bound"` field is optional, and will default to a value of `long.MaxValue` if not specified.

###### Applicable Subevents
- `TemporaryHitPointRoll`