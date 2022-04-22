CREATE DATABASE markersdataa;
GO
 
USE markersdataa;
 
CREATE TABLE markers
(
    Id INT PRIMARY KEY IDENTITY,
    marker VARCHAR(50), 
    lat Float,
    len Float
)

GO

INSERT markers VALUES 
('traktor', 55,050842588449079, 82,932357788085938),
('car', 55,050852588449079, 82,932357788085938),
('truck', 55,050892588449079, 82,932357788085938)