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
