Feature: Registration
	In order organize teams from gyms
	As an Event Producer
	I want to record team Registrations

@mytag
Scenario: Registration Id is formatted correctly
	Given A new Registration
	And The company id is company/1
	And The competition id is company/1/competition/1
	And The gym id is company/1/gym/1
	When The Id is Generated
	Then The result should be company/1/competition/1/gym/1/registration/
