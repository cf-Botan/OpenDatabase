# OpenDatabase

OpenDatabase is a mod for [**Valheim**](https://store.steampowered.com/app/892970/Valheim/) it requires [**BepInEx for Valheim**](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).
With this mod you are able to controll all recipes via [JSON](https://en.wikipedia.org/wiki/JSON) files.

## Installation

Download and extract the latest version of OpenDatabase into the BepInEx plugin folder (usually Valheim/BepInEx/plugins )

Now run Valheim and join a world. After that go to Valheim/BepInEx/plugins/. There should be a folder called OpenDatabase, inside of that folder are currently two folders /Items/ and /Recipes/. Inside those folders are JSON files that you can now modify.

## Configuration file BepInEx/config/Botan.OpenDatabase.cfg

Enable or disable the mod.
If ShowZerosInJSON is set true, Zeros inside of a json file are not removed on generation.

## Recipes

The Club recipe for example looks like this:
```json
{
	"result_item_id": "Club",
	"result_amount": 1,
	"RepairStation": "$piece_workbench",
	"CraftingStation": "",
	"minStationLevel": 1,
	"ingredients": [
		{
			"id": "Wood",
			"amount": 6
		},
		{
			"id": "BoneFragments",
			"amount": 0
		}
	]
}
```
You will however not see "CraftingStation": "" empty values are not included in the json generation. This also applies to int/float by default if the value is 0.

## Items

This is the Club.json file inside of BepInEx/plugins/OpenDatabase/Items

> "name": ...  is the ItemId

> "m_name": ... is the displayed name 

```json
{
	"name": "Club",
	"itemData": {
		"m_name": "$item_club",
		"m_description": "$item_club_description",
		"m_weight": 2,
		"m_maxStackSize": 1,
		"m_armor": 20,
		"m_armorPerLevel": 1,
		"m_blockPower": 10,
		"m_canBeReparied": true,
		"m_damages": {
			"m_blunt": 12
		},
		"m_damagesPerLevel": {
			"m_blunt": 6
		},
		"m_timedBlockBonus": 2,
		"m_deflectionForce": 20,
		"m_deflectionForcePerLevel": 5,
		"m_destroyBroken": false,
		"m_dodgeable": true,
		"m_maxDurability": 100,
		"m_durabilityPerLevel": 50,
		"m_maxQuality": 4,
		"m_useDurability": true,
		"m_useDurabilityDrain": 1,
		"m_questItem": false,
		"m_teleportable": true
	}
}
```

### Damage types

> | 	Damage Name   	| 	Value Type    	|
> | 	-------------	|	:-------------:	|
> |	m_blunt		|	float		|
> |	m_chop		|	float		|
> |	m_damage	|	float		|
> |	m_fire		|	float		|
> |	m_frost		|	float		|
> |	m_lightning	|	float		|
> |	m_pickaxe	|	float		|
> |	m_pierce	|	float		|
> |	m_poison	|	float		|
> |	m_slash		|	float		|
> |	m_spirit	|	float		|

### Itemdata

> | 	Data Value   				| Value Type    |
> | 		------------- 			|:-------------:|
> |	m_armor					|	float	|
> |	m_armorPerLevel				|	float	|
> |	m_blockPower				|	float	|
> |	m_blockPowerPerLevel			|	float	|
> |	m_canBeReparied				|	bool	|
> |	m_damages				|DamageType	|
> |	m_damagesPerLevel			|DamageType	|
> |	m_destroyBroken				|	bool	|
> |	m_deflectionForce			|	float	|
> |	m_deflectionForcePerLevel		|	float	|
> |	m_dodgeable				|	bool	|
> |	m_durabilityDrain			|	float	|
> |	m_durabilityPerLevel			|	float	|
> |	m_description				|	string	|
> |	m_equipDuration				|	float	|
> |	m_food					|	float	|
> |	m_foodBurnTime				|	float	|
> |	m_foodColor				|	hex	|
> |	m_foodRegen				|	float	|
> |	m_foodStamina				|	float	|
> |	m_holdDurationMin			|	float	|
> |	m_holdStaminaDrain			|	float	|
> |	m_maxDurability				|	float	|
> |	m_maxQuality				|	float	|
> |	m_maxStackSize				|	int	|
> |	m_name					|	string	|
> |	m_questItem				|	bool	|
> |	m_teleportable				|	bool	|
> |	m_timedBlockBonus			|	float	|
> |	m_toolTier				|	int	|
> |	m_useDurability				|	bool	|
> |	m_useDurabilityDrain			|	float	|
> |	m_value					|	int	|
> |	m_weight				|	float	|
