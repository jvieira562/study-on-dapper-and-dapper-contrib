INSERT INTO [Tag] 
VALUES 
	('ASP.NET', 'aspnet'),
	('SQL Server', 'sqlserver'),
	('Angular', 'angular');

SELECT * FROM [User];
SELECT * FROM [Role];
SELECT * FROM [UserRole];

INSERT INTO [UserRole]
VALUES 
	(1, 1),
	(1, 2);


SELECT 
	[User].*,
	[Role].*
FROM 
	[User]
	LEFT JOIN [UserRole]
		ON [UserRole].[UserId] = [User].[Id]
	LEFT JOIN [Role] ON [UserRole].[RoleId] = [Role].[Id];


