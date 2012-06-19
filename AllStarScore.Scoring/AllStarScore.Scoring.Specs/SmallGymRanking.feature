Feature: Small Gym Ranking
	In order to give an equal chance to small gyms
	As an event producer
	I want the scores ranked with two champions for each scoring group

@mytag
Scenario: Rank Division with Large Gym Winner
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName         | IsSmallGym | Final Score |
		| Tiger Cheer     | true       | 40.200      |
		| High Spirit     | true       | 42.293      |
		| Division Winner | false      | 45.933      |
		| A Large Gym     | false      | 43.397      |
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And High Spirit should be ranked 1
		And A Large Gym should be ranked 2
		And Tiger Cheer should be ranked 3

Scenario: Rank Division with a tie amongst non-winners
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score |
		| Tiger Cheer       | true       | 40.200      |
		| High Spirit       | true       | 42.293      |
		| Division Winner   | false      | 45.933      |
		| A Large Gym       | false      | 43.397      |
		| Another Large Gym | false      | 41.397      |
		| A Small Gym       | true       | 41.397      |
		| A New Gym         | true       | 38.397      |
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And High Spirit should be ranked 1
		And A Large Gym should be ranked 2
		And Another Large Gym should be ranked 3
		And A Small Gym should be ranked 3
		And A Small Gym should be 4th
		And Another Large Gym should be 5th
		And Tiger Cheer should be ranked 4
		And A New Gym should be ranked 5

Scenario: Rank Division with a tie amongst winners
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score |
		| Tiger Cheer       | true       | 40.200      |
		| High Spirit       | true       | 42.293      |
		| Division Winner   | false      | 45.933      |
		| A Large Gym       | false      | 43.397      |
		| Another Large Gym | false      | 45.933      |
		| A Small Gym       | true       | 45.933      |
		| A New Gym         | true       | 38.397      |
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

Scenario: Rank Division with a tie small gym non-natural winners
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score |
		| Tiger Cheer       | true       | 40.200      |
		| High Spirit       | true       | 39.293      |
		| Division Winner   | false      | 45.933      |
		| A Large Gym       | false      | 43.397      |
		| Another Large Gym | false      | 44.933      |
		| A Small Gym       | true       | 40.200      |
		| A New Gym         | true       | 38.397      |
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And Tiger Cheer should be ranked 1
		And A Small Gym should be ranked 1
		And A Small Gym should be 2nd
		And Tiger Cheer should be 3rd
		And Another Large Gym should be ranked 2
		And A Large Gym should be ranked 3
		And High Spirit should be ranked 4
		And A New Gym should be ranked 5


Scenario: Rank Division with no Small Gyms
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score |
		| Tiger Cheer       | true       | 40.200      |
		| High Spirit       | true       | 39.293      |
		| Division Winner   | true       | 45.933      |
		| A Large Gym       | true       | 43.397      |
		| Another Large Gym | true       | 44.933      |
		| A Small Gym       | true       | 40.200      |
		| A New Gym         | true       | 38.397      |
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And Another Large Gym should be ranked 2
		And A Large Gym should be ranked 3
		And Tiger Cheer should be ranked 4
		And A Small Gym should be ranked 4
		And High Spirit should be ranked 5
		And A New Gym should be ranked 6

Scenario: Rank Division with no Large Gyms
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName           | IsSmallGym | Final Score |
		| Tiger Cheer       | false      | 40.200      |
		| High Spirit       | false      | 39.293      |
		| Division Winner   | false      | 45.933      |
		| A Large Gym       | false      | 43.397      |
		| Another Large Gym | false      | 44.933      |
		| A Small Gym       | false      | 40.200      |
		| A New Gym         | false      | 38.397      |
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And Another Large Gym should be ranked 2
		And A Large Gym should be ranked 3
		And Tiger Cheer should be ranked 4
		And A Small Gym should be ranked 4
		And High Spirit should be ranked 5
		And A New Gym should be ranked 6

Scenario: Rank Division with Large Gym Winner and a Small Gym winner far down the list
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName         | IsSmallGym | Final Score |
		| Tiger Cheer     | true       | 40.200      |
		| Bear Cheer      | true       | 41.200      |
		| Zebra Cheer     | true       | 39.200      |
		| Division Winner | false      | 45.933      |
		| A Large Gym     | false      | 43.397      |
		| A Large Gym2    | false      | 43.297      |
		| A Large Gym3    | false      | 43.197      |
		| A Large Gym4    | false      | 42.397      |
		| High Spirit     | true       | 42.293      |
		
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And High Spirit should be ranked 1
		And A Large Gym should be ranked 2
		And Bear Cheer should be ranked 6
		And Tiger Cheer should be ranked 7

Scenario: Rank Division with Large Gym Winner and a Small Gym winner far down the list With a lot of ties in between
	Given a Small Gym Ranking Calculator
		And a set of Performances:
		| GymName         | IsSmallGym | Final Score |
		| Tiger Cheer     | true       | 40.200      |
		| Bear Cheer      | true       | 41.200      |
		| Zebra Cheer     | true       | 39.200      |
		| Division Winner | false      | 45.933      |
		| A Large Gym     | false      | 43.397      |
		| A Large Gym2    | false      | 43.297      |
		| A Large Gym3    | false      | 43.297      |
		| A Large Gym4    | false      | 43.297      |
		| High Spirit     | true       | 42.293      |
		
	When the TeamScores are ranked
	Then Division Winner should be 1st
		And Division Winner should be ranked 1
		And High Spirit should be ranked 1
		And A Large Gym should be ranked 2
		And Bear Cheer should be ranked 4
		And Tiger Cheer should be ranked 5
