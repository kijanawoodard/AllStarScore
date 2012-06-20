Feature: Score Grouping
	In order give Awards based on Divisions and Levels
	As a Tabulator
	I want the Teams to be Grouped by Division and Level

@mytag
Scenario: Group performances all in the same division
	Given a set of Performances to be grouped:
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
		And the count of overall will be 7

Scenario: Group performances all in the same level but different divisions
	Given a set of Performances to be grouped:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-2 | levels-1 |
		| Another Large Gym | false      | 41.397      | registrations-5 | divisions-2 | levels-1 |
		| A Small Gym       | true       | 41.397      | registrations-6 | divisions-3 | levels-1 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-3 | levels-1 |
	When Performances are Grouped
	Then the count of divisions-1 will be 3
		And the count of divisions-2 will be 2
		And the count of divisions-3 will be 2
		And the count of levels-1 will be 7
		And the count of overall will be 7

Scenario: Group performances all in the different levels and different divisions
	Given a set of Performances to be grouped:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-2 | levels-2 |
		| Another Large Gym | false      | 41.397      | registrations-5 | divisions-2 | levels-2 |
		| A Small Gym       | true       | 41.397      | registrations-6 | divisions-3 | levels-3 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-3 | levels-3 |
	When Performances are Grouped
	Then the count of divisions-1 will be 3
		And the count of divisions-2 will be 2
		And the count of divisions-3 will be 2
		And the count of levels-1 will be 3
		And the count of levels-2 will be 2
		And the count of levels-3 will be 2
		And the count of overall will be 7

Scenario: Group performances all in the different levels and different divisions in random order
	Given a set of Performances to be grouped:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| A Small Gym       | true       | 41.397      | registrations-6 | divisions-3 | levels-3 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-2 | levels-2 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-3 | levels-3 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| Another Large Gym | false      | 41.397      | registrations-5 | divisions-2 | levels-2 |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
	When Performances are Grouped
	Then the count of divisions-1 will be 3
		And the count of divisions-2 will be 2
		And the count of divisions-3 will be 2
		And the count of levels-1 will be 3
		And the count of levels-2 will be 2
		And the count of levels-3 will be 2
		And the count of overall will be 7


Scenario: Group performances all in the different levels and different divisions in random order and rank them
	Given a set of Performances to be grouped:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| A Small Gym       | true       | 41.397      | registrations-6 | divisions-3 | levels-2 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-2 | levels-1 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-3 | levels-2 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| Another Large Gym | false      | 41.397      | registrations-5 | divisions-2 | levels-1 |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
		And a Natural Gym Ranking Calculator
	When Performances are Grouped
		And the TeamScores are ranked
	Then the count of divisions-1 will be 3
		And the count of divisions-2 will be 2
		And the count of divisions-3 will be 2
		And the count of levels-1 will be 5
		And the count of levels-2 will be 2
		And the count of overall will be 7
		And Division Winner should be ranked 1 in division and 1 in level and 1 overall
		And High Spirit should be ranked 2 in division and 3 in level and 3 overall
		And Tiger Cheer should be ranked 3 in division and 5 in level and 5 overall
		And A Large Gym should be ranked 1 in division and 2 in level and 2 overall
		And Another Large Gym should be ranked 2 in division and 4 in level and 4 overall
		And A Small Gym should be ranked 1 in division and 1 in level and 4 overall
		And A New Gym should be ranked 2 in division and 2 in level and 6 overall