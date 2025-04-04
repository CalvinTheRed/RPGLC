# RevokeImmunity

```c#
{
  "function": "revoke_immunity",
  "damage_type": <string = "*">
}
```

Revokes immunity to a specified damage type. Note that the `"damage_type"` field is optional, and will default to a value of `"*"` if not specified. This value causes the function to revoke immunity to all damage types, rather than to a single damage type.

###### Applicable Subevents
- `DamageAffinity`