@tool
extends EditorPlugin


const AUTOLOAD_MEASUREMENTS_NAME = "VRUserMeasurements"


func _enter_tree():
	add_autoload_singleton(AUTOLOAD_MEASUREMENTS_NAME, "res://addons/Godot-VR-Measurements/scenes/VRUserMeasurements.tscn")

func _exit_tree():
	remove_autoload_singleton(AUTOLOAD_MEASUREMENTS_NAME)
