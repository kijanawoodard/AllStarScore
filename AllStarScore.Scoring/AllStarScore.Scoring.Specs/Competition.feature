Feature: Competition Definition
	In order to manage Competitions
	As an Event Producer
	I want to be able to manage competition details

@mytag
Scenario: Set the First Day and the Number of Days
	Given A Competition
	And The First Day is 7/5/2012
	When I set the Number of Days to 1
	Then the Last Day should be 7/5/2012
