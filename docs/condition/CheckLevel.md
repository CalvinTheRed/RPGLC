# CheckLevel

```c#
{
  "condition": "check_level",
  "object": {
    "from": "subevent" | "effect",
    "object": "source" | "target"
  },
  "class": "*" | <class datapack id>,
  "comparison": "<" | "<=" | ">" | ">=" | "=" | "!=",
  "compare_to": <long>
}
```

Returns true if the specified object's level in the specified class meets the criterion defined by the condition.

Note: if `class` is assigned the value of `"*"`, the condition resolves using the specified object's total level.