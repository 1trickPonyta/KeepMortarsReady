$path = "D:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\KeepMortarsReady"

Remove-Item -Recurse "$path\*"
mkdir $path
@(
	"About",
	"Assemblies",
	"Defs",
	"Languages",
	"Patches",
	"Source",
	"Textures"
) | %{ Copy-Item -Recurse "..\$_" "$path\$_" }
Remove-Item -Recurse "$path\Source\bin"
Remove-Item -Recurse "$path\Source\obj"
Remove-Item "$path\Source\packages.config"
