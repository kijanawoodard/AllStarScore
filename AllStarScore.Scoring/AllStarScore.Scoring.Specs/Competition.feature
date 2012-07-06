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

Scenario: Set the number of panles
	Given A Competition
	When I set the Number of Panels to 2
	Then there should be 2 panels
		And the panels should be A, B

Scenario: Competition Defaults
	Given A Competition
	Then the Number of Days should be 1
		And the Number of Panels should be 1

Scenario: Competition Created
	Given A Competition
		And A Competition Create Command
	When The Create Command is processed by Update
	Then The ICanBeUpdatedByCommand Properties are Correct
