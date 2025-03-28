extends Node

class_name HillGenerator

var window_size := Vector2.ZERO

var max_hill_key_points := 20
var k_hill_segment_width := 10
var max_x := 0

var padding_top := 30
var padding_bottom := 30

var _min_dx := 0
var _range_dx := 0

var _min_dy := 0
var _range_dy := 0

var _initialized := false

func init() -> void:
	window_size = Vector2(1200, 800)
	_min_dx = floori(window_size.x / 6)
	max_x = int(_min_dx * max_hill_key_points + window_size.x * 0.5)
	_range_dx = floori(window_size.x / 16)
	_min_dy = floori(window_size.y / 6)
	_range_dy = floori(window_size.y / 2)
	_initialized = true

func generate_hills() -> Array:
	if not _initialized:
		init()

	var points := []
	var x = -_min_dx
	var y = window_size.y / 2
	points.append(Vector2(x, y))

	x = 0
	y = window_size.y / 2
	points.append(Vector2(x, y))

	var direction := 1

	for i in max_hill_key_points - 2:
		var dx = _min_dx + randi() % (_range_dx + 1)
		var dy = direction * (_min_dy + randi() % (_range_dy + 1))
		direction *= -1

		x += dx
		y += dy
		y = clamp(y, padding_bottom, window_size.y - padding_top)

		points.append(Vector2(x, y))

	return points
