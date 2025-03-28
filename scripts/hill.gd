extends Node2D

@onready var label_node := get_parent().get_node("DebugLabel") as DebugLabel

var hill_generator := HillGenerator.new()

var hill_key_points := []
var x_offset := DebugLabel.Ref.new(0.0)
var scroll_rate := DebugLabel.Ref.new(300.0)
var normal_scroll_rate := 300
var fast_scroll_rate := 600

var line_width := 10
var up_color := Color.GREEN
var down_color := Color.RED
var neutral_color := Color.BLUE

func _ready() -> void:
	hill_key_points = hill_generator.generate_hills()
	label_node.add_slider("_scroll_rate", scroll_rate, 0, fast_scroll_rate)
	label_node.add_slider("_XOffset", x_offset, 0, hill_generator.max_x)

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("s_key"):
		scroll_rate.value = fast_scroll_rate
	elif event.is_action_released("s_key"):
		scroll_rate.value = normal_scroll_rate

func _process(delta: float) -> void:
	x_offset.value += scroll_rate.value * delta

	if x_offset.value > hill_generator.max_x:
		x_offset.value = 0

	label_node.set_variable("_XOffset", x_offset.value)
	label_node.set_variable("_scroll_rate", scroll_rate.value)

	queue_redraw()

func _draw() -> void:
	var w = hill_generator.window_size.x
	var h = hill_generator.window_size.y
	var pad_top = hill_generator.padding_top
	var pad_bottom = hill_generator.padding_bottom

	draw_line(Vector2(0, pad_top), Vector2(w, pad_top), neutral_color, line_width)
	draw_line(Vector2(0, h - pad_bottom), Vector2(w, h - pad_bottom), neutral_color, line_width)

	for i in range(1, hill_key_points.size()):
		var p0 = hill_key_points[i - 1] - Vector2(x_offset.value, 0)
		var p1 = hill_key_points[i] - Vector2(x_offset.value, 0)

		var h_segments = int((p1.x - p0.x) / hill_generator.k_hill_segment_width)
		var dX = (p1.x - p0.x) / h_segments
		var dA = PI / h_segments
		var y_mid = (p0.y + p1.y) / 2
		var ampl = (p0.y - p1.y) / 2

		var pt0 = p0
		for j in range(h_segments + 1):
			var pt1 = Vector2(p0.x + j * dX, y_mid + ampl * cos(dA * j))
			var segment_color = up_color if ampl > 0.0 else down_color
			draw_line(pt0, pt1, segment_color, line_width)
			pt0 = pt1
