# GrantImmunity

```c#
{
  "function": "grant_immunity",
  "damage_type": <string = "*">
}
```

Grants immunity to a specified damage type. Note that the `"damage_type"` field is optional, and will default to a value of `"*"` if not specified. This value causes the function to grant immunity to all damage types, rather than to a single damage type.

###### Applicable Subevents
- `DamageAffinity`