# EquippedItemHasTag

```c#
{
  "condition": "equipped_item_has_tag",
  "object": {
    "from": "subevent" | "effect",
    "object": "source" | "target"
  },
  "slot": "*" | <string>,
  "tag": <string>
}
```

Returns true if the specified object has an equipped item in the specified equipment slot with the specified tag.

Note: if `slot` is assigned the value of `"*"`, the condition returns true if any equipped item has the specified tag.