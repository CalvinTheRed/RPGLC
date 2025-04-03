# CheckAbilityScore

```c#
{
  "condition": "check_ability_score",
  "object": {
    "from": "subevent" | "effect",
    "object": "source" | "target"
  },
  "ability": <string>,
  "comparison": "<" | "<=" | ">" | ">=" | "=" | "!=",
  "compare_to": <long>
}
```

Returns true if the specified object's specified ability score meets the criterion defined by the condition.