# Vampirism Formula

The following is the vampirism formula recognized by RPGLC. Note that all `"scale"` fields are optional, and will default to a value of `{ "numerator": 1, "denominator": 1, "round_up": false }` if not specified.

```c#
{
  "damage_type": <string = "*">,
  "scale": {
    "numerator": <long>,
    "denominator": <long>,
    "round_up": <bool = false>
  }
}
```

Note that the `"damage_type"` field is optional, and will default to a value of `"*"` if not specified. This value indicates that the vampirism formula applies across all damage types, rather than only applying to damage of a single type.