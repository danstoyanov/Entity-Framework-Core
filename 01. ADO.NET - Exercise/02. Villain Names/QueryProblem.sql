-- first test query ! 
SELECT 
	vill.[Name],
	COUNT(vill.Id) AS [MinionsCount]
		FROM Villains vill
		JOIN MinionsVillains mv ON mv.VillainId = vill.Id
		JOIN Minions m ON mv.MinionId = m.Id
	GROUP BY vill.[Name], vill.Id
	HAVING COUNT(vill.[Name]) > 3
	ORDER BY COUNT(vill.Id)

-- second test query !

-- minions 
SELECT 
	m.[Name] AS [NAME],
	m.Age AS [Age]
		FROM Minions m
		JOIN MinionsVillains mv ON mv.MinionId = m.Id
		WHERE VillainId = 2

-- villain name
SELECT TOP (1) v.[Name] AS [Name]
	FROM Villains v
	JOIN MinionsVillains mv ON mv.VillainId = v.Id
	WHERE VillainId = 1
