alter table [dbo].[ApplicationUsers]
add GmailLoginAccount nvarchar(max) NULL


alter table CSPGoalCatalogs
add TextOrder int NOT NULL default(0)

alter table CSPGoalCatalogs
add SubTextOrder int NOT NULL default(0)


alter table GoalCatalogs
add TextOrder int NOT NULL default(0)

alter table GoalCatalogs
add SubTextOrder int NOT NULL default(0)