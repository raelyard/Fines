Feature: Overdue Fine Calculation Execution
	In order to inform library patrons of their current fines
	As a library operator
	I want to calculate updated fines on a daily basis
	
Scenario: Calculate Fine Daily Starting on the Third Day After the Due Date And Continuing Until the Thirtieth Day
	Given A loan has been established
	When The third day following the due date has arrived
	Then the fine should begin daily calculation
	And the daily calculation should stop after the thirtieth day after the due date
