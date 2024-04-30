extends Node

@export
var property_name: StringName
@export
var delta: float


var label: Label

func _enter_tree():
	label = $HBoxContainer/Label
	update_label()
	
	VRUserMeasurements.MeasurementsChange.connect(Callable(self, "update_label"))
	
func _exit_tree():
	VRUserMeasurements.MeasurementsChange.disconnect(Callable(self, "update_label"))

func increment():
	var measurement: float = VRUserMeasurements.get(property_name)
	VRUserMeasurements.set(property_name, measurement + delta)
	
func decrement():
	var measurement: float = VRUserMeasurements.get(property_name)
	VRUserMeasurements.set(property_name, measurement - delta)

func update_label():
	var measurement: float = VRUserMeasurements.get(property_name)
	label.text = "%s: %.2f" % [property_name, measurement]
