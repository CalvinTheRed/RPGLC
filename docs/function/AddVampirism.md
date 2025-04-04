# AddVampirism

```c#
{
  "function": "add_vampirism",
  "vampirism": [
    <vampirism formula>
  ]
}
```

Applies a list of vampiric behaviors to a damage-dealing subevent.

Note that the field `"vampirism"` is optional, and will default to a value of `[{}]`, which will go on to become the default vampirism addition of `[ { "damage_type": "*", "scale": { "numerator": 1, "denominator": 1, "round_up": false } } ]`.

###### Applicable Subevents
- `AttackRoll`
- `DealDamage`
- `SavingThrow`