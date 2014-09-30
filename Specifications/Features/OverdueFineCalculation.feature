Feature: Overdue Fine Calculation
	In order to incentivize the return of library property
	As a library operator
	I want to charge fines for overdue media

Scenario Outline: Calculate Fines For Overdue Days Less Than Replacement Cost Deadline
	Given A loan is overdue by <days> days
	When I calculate the overdue fine
	Then I should see a fine of $<fine>

	Examples:
	| days | fine |
	| 0    | 0.00 |
	| 1    | 0.00 |
