## Choice Models and Icons - 0.3.2

- Choice Band, Choice Specs and Choice Scarf
<br>(About the downside increase, this is to encourage careful strategies when sacrificing non-preferred skills for damage and/or movement)
    - They are now fully accessible with added models and icons.
    - Increased cooldown penalty: <b>15%</b> -> <b>20%</b>.
- EXP Share
    - Fixed a bug where players would continue to gain EXP in environments which time does not elapse.
- Adjusted language descriptions.
- Cfg support has now been implemented for Choice Band, Choice Specs, and Choice Scarf.
- New Buffs/Debuffs section has been added to explain the new buffs added into the mod.
- New credit added into Inspirations/Assets.

## 0.3 Hotfix - 0.3.1

- Fixed a compilation bug where Risk Of Options being disabled or not installed would crash the mod.

## Risk Of Options Support and Choice Items - 0.3.0

- Third alpha release.
- Risk Of Options is now compatible with this mod, allowing options to be visible in-game via the mod menu.
- FlameOrb
<br>(Originally I wanted to make this as equally effective as Tri-Tip Dagger. However, with the inclusion of Ignition Tank, I decided to nerf it as a slight trade-off)
    - Reduced proc percent: <b>10%</b> -> <b>7%</b>
- EXP Share
    - EXP rate amount now shows per minute instead of second. Amount remains the same.
- More early-access items are implemented (these items are currently in development and may change over patches)
    - Choice Band [LUNAR]: Deal more Primary and Secondary damage... BUT all non-preferred skills recharges slower.
    - Choice Specs [LUNAR]: Deal more Utility and Special damage... BUT all non-preferred skills recharges slower.
    - Choice Scarf [LUNAR]: Increase movement speed... BUT all non-preferred skills recharges slower.
- More changes in coding:
    - Added RiskOfOptionsManager
    - RiskOfOptions has been added as a soft dependency
    - Added ChoiceBuffs and ChoiceManager
    - ChoiceBand, ChoiceSpecs, and ChoiceScarf classes added
    - PrimaryLock, SecondaryLock, UtilityLock, and SpecialLock added in Buffs
    - Language file adjusted
    - UnfinishedItemsEnabled from cfg is now saved in a variable so that it remains unchanged from RiskOfOptions until restart
    - Readjusted ExponentialPercentReductionStacking formula
    - ConfigManager has been optimized to better handle switching between custom and default values
    - All item classes (except early-access) have been adjusted to adhere with the new optimization

## New Models and Re-Rendered Icons - 0.2.1

- EXP Share, Amulet Coin, and Heavy-Duty Boots now have models and icons. They are now fully accessible.
- Cfg support has now been implemented for EXP Share, Amulet Coin, and Heavy-Duty Boots.
- All new and existing icons are re-rendered to have proportional sizes and rarity outlines that closely resemble the official look.
- Fixed a cfg bug where in Air Balloon, custom values of fall speed limit and fall percent reduction per extra stack were ignored.
- Cfg descriptions have been slightly improved.
- As a result of the new icons, README and the mod icon has been changed.
- More changes in coding:
    - Cfg now contain variables borrowed from item classes instead of magic values.

## Config Support and Early-Access Items - 0.2.0

- Second alpha release.
- Cfg support is now implemented, allowing configurations for Spawn Mode (item spawning via F-keys will now need to be enabled first) and custom values for items.
- All custom items now have dithering and GPU instancing.
- Readjusted some colors for Leftovers.
- Early-access items are now implemented (these items are currently in development and may change over patches)
    - Heavy-Duty Boots [GREEN]: Reduce damage taken when grounded.
    - Amulet Coin [RED]: Gain more gold from rewards and kills.
    - EXP Share [RED]: Gain more EXP via rewards and kills. Also passively gain EXP per second.
- More changes in coding:
    - Added ConfigManager
    - All Manager classes are now inside Managers folder
    - Every variables defined in Items are now configurable (unless custom values are turned off)
    - ExpShare, AmuletCoin, and HeavyDutyBoots classes added
    - ConfigManager includes a setting to enable unfinished items
    - More F-Keys in use for unfinished items if enabled
    - Item hooks are renamed to be more unique

## Alpha Release (Hotfix) - 0.1.1

- Fixed a bug where explosions occasionally explode infinitely on itself (and damage itself somehow).
- Air Balloon logging for damaged hook has been repositioned.
- Readjusted the math logic for exponential reduction stacking.
- Flame Orb description now includes how long an individual burn debuff lasts.
- Added Testers in credit.
- Added a link to a Google Forms where users can provide reviews and suggestions to improve the mod.

## Alpha Release - 0.1.0

- Alpha release.
- New mod icon!
- AssetBundle is now being used. As a result, all Pokemon items have models and icons (including the popped Air Balloon, but just the icon). Now they don't have to be mysteries!
- Further completed the description of the mod.
- Forgot to credit shirograhm for some of the TooManyItems being borrowed and modified for this mod. Shoutout to shirograhm for the amazing work of bringing lots of their custom items to the game!
- More changes in coding:
    - Changed README to include more description of the plugin.
    - Set to current version number in manifest and PokeItems.
	- Added AssetBundle class to handle pathfinding to the assetbundle file.
    - Added Textures folder with item icons for use in description.
    - All tokens are renamed to have 'words with capital first letter' instead of all capitals. This is done to support the file namings in the AssetBundle.
    - Removed unused Potion class.
    - Renamed PoppedAirBalloon class to AirBalloonBroken.
    - ModelPanelParameter is added and is automatically added to custom items for display (coding based on shirograhm's mod).
    - Readjusted comments to necessary codes (in case other developers want to see and understand this).
    - Lore is now defaulted to 'Lore not found...'

## Developer Release with GitHub - 0.0.3

- Added GitHub link to the repository containing project files.
- Readjusted changelog to reverse the order (oldest first -> latest first).
- Added a Spawn Keys section in README to describe the use of F-keys to spawn the desired items.
- Slightly adjusted item descriptions.

## Developer Release (Hotfix) - 0.0.2

- Added R2API as a dependancy for this plugin.

## Developer Release - 0.0.1

- First release.
- 3 Pokemon items included:
    - Leftovers [GREEN]: Increase health regeneration by a flat amount.
	- Flame Orb [WHITE]: Chance to burn enemies on hit.
    - Air Balloon [WHITE]: Limits your falling speed, but will pop at low health.
- <b>NOTE:</b> None of the items have 3D models or sprites, so they tend to appear as mystery placeholders.