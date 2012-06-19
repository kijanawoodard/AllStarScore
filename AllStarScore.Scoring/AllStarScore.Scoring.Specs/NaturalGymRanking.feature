Feature: Natural Gym Ranking
	In give awards to teams
	As an Event Producer
	I want to rank the teams by their scores

@mytag
Scenario: Rank Division with a tie amongst non-winners
	Given a Natural Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-1 | levels-1 |
		| Another Large Gym | false      | 41.397      | registrations-5 | divisions-1 | levels-1 |
		| A Small Gym       | true       | 41.397      | registrations-6 | divisions-1 | levels-1 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-1 | levels-1 |
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And A Large Gym should be ranked 2
		And High Spirit should be ranked 3
		And A Small Gym should be ranked 4
		And Another Large Gym should be ranked 4
		And A Small Gym should be 4th
		And Another Large Gym should be 5th
		And Tiger Cheer should be ranked 5
		And A New Gym should be ranked 6

Scenario: Rank Division with a tie amongst winners
	Given a Natural Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score | Registration Id | Division Id | Level Id |
		| Tiger Cheer       | true       | 40.200      | registrations-1 | divisions-1 | levels-1 |
		| High Spirit       | true       | 42.293      | registrations-2 | divisions-1 | levels-1 |
		| Division Winner   | false      | 45.933      | registrations-3 | divisions-1 | levels-1 |
		| A Large Gym       | false      | 43.397      | registrations-4 | divisions-1 | levels-1 |
		| Another Large Gym | false      | 45.933      | registrations-5 | divisions-1 | levels-1 |
		| A Small Gym       | true       | 45.933      | registrations-6 | divisions-1 | levels-1 |
		| A New Gym         | true       | 38.397      | registrations-7 | divisions-1 | levels-1 |
	When the TeamScores are ranked
	Then Division Winner should be 3rd
		And Division Winner should be ranked 1
		And A Large Gym should be ranked 2
		And High Spirit should be ranked 3
		And A Small Gym should be ranked 1
		And Another Large Gym should be ranked 1
		And A Small Gym should be 1st
		And Another Large Gym should be 2nd
		And Tiger Cheer should be ranked 4
		And A New Gym should be ranked 5
