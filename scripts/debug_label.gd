extends VBoxContainer

class_name DebugLabel

var sliders := {}
var labels := {}

func _ready() -> void:
	pass

func set_variable(key: String, value: float) -> void:
	if key in sliders:
		sliders[key].value = value
		labels[key].text = "%s: %.2f" % [key, value]

func add_slider(key: String, ref_value: Ref, minVal := 0.0, maxVal := 100.0) -> void:
	var hbox = HBoxContainer.new()

	var label = Label.new()
	label.text = "%s: %.2f" % [key, ref_value.value]
	label.size_flags_horizontal = Control.SIZE_EXPAND_FILL

	var slider = HSlider.new()
	slider.min_value = minVal
	slider.max_value = maxVal
	slider.step = 0.01
	slider.value = ref_value.value
	slider.size_flags_horizontal = Control.SIZE_EXPAND_FILL
	slider.mouse_filter = Control.MOUSE_FILTER_STOP

	slider.value_changed.connect(func(new_value: float) -> void:
		ref_value.value = new_value
		label.text = "%s: %.2f" % [key, ref_value.value]
	)

	hbox.add_child(label)
	hbox.add_child(slider)

	add_child(hbox)
	sliders[key] = slider
	labels[key] = label

class Ref:
	var value: float
	func _init(v: float) -> void:
		value = v
