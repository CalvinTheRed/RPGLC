# GrantResistance

```c#
{
  "function": "grant_resistance",
  "damage_type": <string>
}
```

Grants resistance to a specified damage type. Note that the `"damage_type"` field is optional, and will default to a value of `"*"` if not specified. This value causes the function to grant resistance to all damage types, rather than to a single damage type.

###### Applicable Subevents
- `DamageAffinity`