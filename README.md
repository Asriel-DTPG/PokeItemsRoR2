# PokeItems - RoR2
This is a work-in-progress mod that adds Pokemon items into Risk of Rain 2.
Since this mod is still in development, it will continue to change and will
be balanced throughout the patches.

## Inspiration/Assets
- [<b>TooManyItems</b> by shirograhm](https://thunderstore.io/c/riskofrain2/p/shirograhm/TooManyItems/)
- [<b>Lock SVG Vector</b> by SVG Repo](https://staging.svgrepo.com/svg/99424/lock)

## Items
<table>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/AirBalloon.png" alt="Air Balloon" width="192px"/>
		</td>
		<td>
			<b>Air Balloon</b>
		</td>
		<td>
			<b>WHITE</b>
		</td>
		<td>
			Limits your falling speed by <b>50</b> m/s (-<b>10</b>% per stack). However, it will pop when under <b>35</b>% HP.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/FlameOrb.png" width="192px"/>
		</td>
		<td>
			<b>Flame Orb</b>
		</td>
		<td>
			<b>WHITE</b>
		</td>
		<td>
			<b>7</b>% (+<b>7</b>% per stack) chance to burn an enemy on hit. Debuff stack individually lasts for <b>4</b> seconds.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/HeavyDutyBoots.png" width="192px"/>
		</td>
		<td>
			<b>Heavy-Duty Boots</b>
		</td>
		<td>
			<b>GREEN</b>
		</td>
		<td>
			Gain <b>20</b> (+<b>10</b> per stack) armor when on ground.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/Leftovers.png" width="192px"/>
		</td>
		<td>
			<b>Leftovers</b>
		</td>
		<td>
			<b>GREEN</b>
		</td>
		<td>
			Increase health regeneration by <b>4</b> HP/s (+<b>2</b> HP/s per stack).
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/AmuletCoin.png" width="192px"/>
		</td>
		<td>
			<b>Amulet Coin</b>
		</td>
		<td>
			<b>RED</b>
		</td>
		<td>
			Gain <b>100</b>% (+<b>100</b>% per stack) more gold.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/EXPShare.png" width="192px"/>
		</td>
		<td>
			<b>EXP Share</b>
		</td>
		<td>
			<b>RED</b>
		</td>
		<td>
			Gain <b>200</b>% (+<b>100%</b> per stack) more EXP. Also gain <b>30</b>% required EXP per minute (Early levels offer more EXP due to rounding).
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/ChoiceBand.png" width="192px"/>
		</td>
		<td>
			<b>Choice Band</b>
		</td>
		<td>
			<b>LUNAR</b>
		</td>
		<td>
			Deal <b>100</b>% (+<b>50</b>% per stack) more damage for Primary and Secondary skills. Choosing a skill will create a choice lock for that skill, and all other skills will recharge <b>20</b>% (+<b>20</b>% per stack) slower. Choice lock resets next stage or respawn.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/ChoiceScarf.png" width="192px"/>
		</td>
		<td>
			<b>Choice Scarf</b>
		</td>
		<td>
			<b>LUNAR</b>
		</td>
		<td>
			Increase movement speed by <b>30</b>% (+<b>20</b>% per stack). Choosing a skill will create a choice lock for that skill, and all other skills will recharge <b>20</b>% (+<b>20</b>% per stack) slower. Choice lock resets next stage or respawn.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Textures/ChoiceSpecs.png" width="192px"/>
		</td>
		<td>
			<b>Choice Specs</b>
		</td>
		<td>
			<b>LUNAR</b>
		</td>
		<td>
			Deal <b>100</b>% (+<b>50</b>% per stack) more damage for Utility and Special skills. Choosing a skill will create a choice lock for that skill, and all other skills will recharge <b>20</b>% (+<b>20</b>% per stack) slower. Choice lock resets next stage or respawn.
		</td>
	</tr>
</table>

## Buffs/Debuffs
This also introduces new buffs and debuffs along with this mod.
<table>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/PrimaryLock.png" width="128px"/>
		</td>
		<td>
			<b>Primary Lock</b>
		</td>
		<td>
			All skill cooldowns (except Primary) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/SecondaryLock.png" width="128px"/>
		</td>
		<td>
			<b>Secondary Lock</b>
		</td>
		<td>
			All skill cooldowns (except Secondary) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/SpecialLock.png" width="128px"/>
		</td>
		<td>
			<b>Special Lock</b>
		</td>
		<td>
			All skill cooldowns (except Special) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/UtilityLock.png" width="128px"/>
		</td>
		<td>
			<b>Utility Lock</b>
		</td>
		<td>
			All skill cooldowns (except Utility) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
</table>

## Buffs/Debuffs
This also introduces new buffs and debuffs along with this mod.
<table>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/PrimaryLock.png" width="128px"/>
		</td>
		<td>
			<b>Primary Lock</b>
		</td>
		<td>
			All skill cooldowns (except Primary) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/SecondaryLock.png" width="128px"/>
		</td>
		<td>
			<b>Secondary Lock</b>
		</td>
		<td>
			All skill cooldowns (except Secondary) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/SpecialLock.png" width="128px"/>
		</td>
		<td>
			<b>Special Lock</b>
		</td>
		<td>
			All skill cooldowns (except Special) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
	<tr>
		<td>
			<img src="https://raw.githubusercontent.com/Asriel-DTPG/PokeItemsRoR2/refs/heads/main/Buffs/UtilityLock.png" width="128px"/>
		</td>
		<td>
			<b>Utility Lock</b>
		</td>
		<td>
			All skill cooldowns (except Utility) have slower recharge. The effect is strengthened based on extra number of choice items.
		</td>
	</tr>
</table>

## Early-Access Items
Items within this section are currently in development and so will include default sprites and models (mystery question marks).

No early-access items are listed at this time.

## Spawn Mode
Press the following keys to spawn the item (early-access items will need to be enabled first):
- <b>F2</b>: Air Balloon
- <b>F3</b>: Flame Orb
- <b>F4</b>: Leftovers
- <b>F5</b>: EXP Share
- <b>F6</b>: Amulet Coin
- <b>F7</b>: Heavy-Duty Boots
- <b>F8</b>: Choice Band
- <b>F9</b>: Choice Specs
- <b>F10</b>: Choice Scarf

## Suggestions?
If you like to share your reviews and suggestions about this mod, please post your responses under this link. I'd be more than happy to listen to your ideas!

[Click here to view the form!](https://docs.google.com/forms/d/e/1FAIpQLSf3XJybH-HHiqVKOapujpag7YSNg0_WbX8M-D5hPSU6mHRc2g/viewform?usp=dialog)

## Credits
- <b>Programming</b>: Asriel_DTPG
- <b>Modelling</b>: EclipticCosmos
- <b>Assets & Shading</b>: Asriel_DTPG
- <b>Testers</b>: Asriel_DTPG, 4adamninja
- <b>Special Thanks</b>: CrashMate, Sensler, WackyZackyBoy