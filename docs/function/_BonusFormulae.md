# Bonus Formulae

The following is a list of bonus formulae recognized by RPGLC. Note that all `"scale"` fields are optional, and will default to a value of `{ "numerator": 1, "denominator": 1, "round_up": false }` if not specified.

Note that if these formulas are used to represent damage bonuses, each must have an additional `"damage_type"` field storing a string representing the damage type being added.

## range

Rolls a collection of dice and adds a defined bonus to the sum. Final sum can be scaled.

```c#
{
  "formula": "range",
  "bonus": <long>,
  "dice": [
    { "count": <long>, "size": <long> }
  ],
  "scale": {
    "numerator": <long>,
    "denominator": <long>,
    "round_up": <bool = false>
  }
}
```

## modifier

Calculates an object's modifier for a given ability. Result can be scaled.

```c#
{
  "formula": "modifier",
  "ability": <string>,
  "object": {
    "from": "effect" | "subevent",
    "object": "source" | "target",
    "as_origin": <bool = false>
  },
  "scale": {
    "numerator": <long>,
    "denominator": <long>,
    "round_up": <bool = false>
  }
}
```

## ability

Calculates an object's score for a given ability. Result can be scaled.

```c#
{
  "formula": "ability",
  "ability": <string>,
  "object": {
    "from": "effect" | "subevent",
    "object": "source" | "target",
    "as_origin": <bool = false>
  },
  "scale": {
    "numerator": <long>,
    "denominator": <long>,
    "round_up": <bool = false>
  }
}
```

## proficiency

Calculates an object's proficiency bonus. Result can be scaled.

```c#
{
  "formula": "proficiency",
  "object": {
    "from": "effect" | "subevent",
    "object": "source" | "target",
    "as_origin": <bool = false>
  },
  "scale": {
    "numerator": <long>,
    "denominator": <long>,
    "round_up": <bool = false>
  }
}
```

## level

Calculates an object's level in a specified class. Result can be scaled.

Note that the `"class"` field is optional, and will default to a value of `"*"` if not specified. This value represents the total level of the object, rather than the object's level in a particular class.

```c#
{
  "formula": "level",
  "class": <string = "*">,
  "object": {
    "from": "effect" | "subevent",
    "object": "source" | "target",
    "as_origin": <bool = false>
  },
  "scale": {
    "numerator": <long>,
    "denominator": <long>,
    "round_up": <bool = false>
  }
}
```