Feature: Score Grouping
	In order give Awards based on Divisions and Levels
	As a Tabulator
	I want the Teams to be Grouped by Division and Level

@mytag
Scenario: Group performances all in the same division
	Given a set of Performances:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-1 | levels-1 |
		| Another Large Gym | false      | 41.397      | registrations-5 | divisions-1 | levels-1 |
		| A Small Gym       | true       | 41.397      | registrations-6 | divisions-1 | levels-1 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-1 | levels-1 |
	When Performances are Grouped
	Then the count of divisions-1 will be 7
		And the count of levels-1 will be 7
