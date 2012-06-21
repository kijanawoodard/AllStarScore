Feature: Average Score Reporting
	In order that teams know how they did against the average of their division
	As a Tabulator
	I want to average the total of each scoring category by division

@mytag
Scenario: Average a Divisions
	Given a set of Performances to average
	| Id             | Division Id |
	| performances-1 | divisions-1 |
	| performances-2 | divisions-1 |
	| performances-3 | divisions-2 |
	| performances-4 | divisions-2 |
	| performances-5 | divisions-2 |
	| performances-6 | divisions-3 |
	| performances-7 | divisions-3 |
		And a set of scores
		| Performance Id | Stunts | Pyramids | Tosses | Deductions | Legalities |
		| performances-1 | 10     | 10       | 10     |            |            |
		| performances-1 | 20     | 20       | 20     |            |            |
		| performances-2 | 10     | 5        | 30     |            |            |
		| performances-2 | 20     | 10       | 40     |            |            |
		| performances-1 |        |          |        | 1          |            |
		| performances-1 |        |          |        |            | 1          |
		| performances-2 |        |          |        | 2          |            |
		| performances-2 |        |          |        |            | .5         |
	When I Average the Scores
	Then divisions-1 should have Stunts score equal 15
	Then divisions-1 should have Pyramids score equal 11.3
	Then divisions-1 should have Tosses score equal 25
	Then divisions-1 should have Deductions score equal 1.5
	Then divisions-1 should have Legalities score equal .8

Scenario: Average a few Divisions
	Given a set of Performances to average
	| Id             | Division Id |
	| performances-1 | divisions-1 |
	| performances-2 | divisions-1 |
	| performances-3 | divisions-2 |
	| performances-4 | divisions-2 |
	| performances-5 | divisions-2 |
	| performances-6 | divisions-3 |
	| performances-7 | divisions-3 |
		And a set of scores
		| Performance Id | Stunts | Pyramids | Tosses | Deductions | Legalities |
		| performances-1 | 10     | 10       | 10     |            |            |
		| performances-1 | 20     | 20       | 20     |            |            |
		| performances-2 | 10     | 5        | 30     |            |            |
		| performances-2 | 20     | 10       | 40     |            |            |
		| performances-1 |        |          |        | 1          |            |
		| performances-1 |        |          |        |            | 1          |
		| performances-2 |        |          |        | 2          |            |
		| performances-2 |        |          |        |            | .5         |
		| performances-3 | 10     | 10       | 10     |            |            |
		| performances-3 | 20     | 20       | 20     |            |            |
		| performances-4 | 10     | 5        | 30     |            |            |
		| performances-4 | 20     | 10       | 40     |            |            |
		| performances-3 |        |          |        | 1          |            |
		| performances-3 |        |          |        |            | 1          |
		| performances-4 |        |          |        | 2          |            |
		| performances-4 |        |          |        |            | .5         |
		| performances-6 | 10     | 10       | 10     |            |            |
		| performances-6 | 20     | 20       | 20     |            |            |
		| performances-7 | 10     | 5        | 30     |            |            |
		| performances-7 | 20     | 10       | 40     |            |            |
		| performances-6 |        |          |        | 1          |            |
		| performances-6 |        |          |        |            | 1          |
		| performances-7 |        |          |        | 2          |            |
		| performances-7 |        |          |        |            | .5         |
	When I Average the Scores
	Then divisions-1 should have Stunts score equal 15
	Then divisions-1 should have Pyramids score equal 11.3
	Then divisions-1 should have Tosses score equal 25
	Then divisions-1 should have Deductions score equal 1.5
	Then divisions-1 should have Legalities score equal .8
	Then divisions-2 should have Stunts score equal 15
	Then divisions-2 should have Pyramids score equal 11.3
	Then divisions-2 should have Tosses score equal 25
	Then divisions-2 should have Deductions score equal 1.5
	Then divisions-2 should have Legalities score equal .8
	Then divisions-3 should have Stunts score equal 15
	Then divisions-3 should have Pyramids score equal 11.3
	Then divisions-3 should have Tosses score equal 25
	Then divisions-3 should have Deductions score equal 1.5
	Then divisions-3 should have Legalities score equal .8