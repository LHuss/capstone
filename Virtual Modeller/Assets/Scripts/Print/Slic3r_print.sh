gcode="$1";
obj="$2";
echo "Object selected: $obj";
echo "Output path: $gcode";
$slic3r -load Virtual_Modeller_config_bundle.ini -scale 150 --support-material --output "$gcode" "$obj";

sleep 10;