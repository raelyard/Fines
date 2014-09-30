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
	| 2    | 0.00 |
	| 3    | 0.75 |
	| 4    | 1.00 |
	| 10   | 2.50 |
	| 29   | 7.25 |
	
Scenario Outline: Calculate Fines For Overdue Days At Or Beyond Replacement Cost Deadline
	Given A loan for an item with replacement value $<replacementValue> is overdue by <days> days
	When I calculate the overdue fine
	Then I should see a fine of $<fine>

	Examples:
	| days | replacementValue | fine       |
	| 30   | 3.00             | 3.00       |
	| 30   | 9.95             | 9.95       |
	| 30   | 1000000.00       | 1000000.00 |
	| 60   | 3.00             | 3.00       |
	| 60   | 9.95             | 9.95       |
	| 60   | 1000000.00       | 1000000.00 |
	| 90   | 3.00             | 3.00       |
	| 90   | 9.95             | 9.95       |
	| 90   | 1000000.00       | 1000000.00 |
