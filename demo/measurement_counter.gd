extends Node

@export
var property_name: StringName
@export
var delta: float


var label: Label

func _ready():
	label = $HBoxContainer/Label
	update_label()

func increment():
	var measurement: float = VRUserMeasurements.get(property_name)
	VRUserMeasurements.set(property_name, measurement + delta)
	update_label()
	
func decrement():
	var measurement: float = VRUserMeasurements.get(property_name)
	VRUserMeasurements.set(property_name, measurement - delta)
	update_label()

func update_label():
	var measurement: float = VRUserMeasurements.get(property_name)
	label.text = "%s: %.2f" % [property_name, measurement]
