USE [CRMClaims]
GO


SET IDENTITY_INSERT [dbo].[Customers] ON 
GO

INSERT INTO [dbo].[Customers]([Id], [UserId], [FirstName], [LastName], [Street], [City], [State], [Zip], [Email], [MobilePhone], [DriversLicenseNumber], [DOB], [PolicyId], [PolicyStart], [PolicyEnd])
VALUES (1, 'MicrosoftAccount:19cffcd129d3d407', 'Margaret', 'Au', '123 Somewhere Street', 'New York', 'NY', '10001', 'marga-demo@outlook.com', '607-555-1212', 'RL8972635', '1/11/1975', 'CIC6749726354', '5/1/2012', '5/1/2018')

INSERT INTO [dbo].[Customers]([Id], [UserId], [FirstName], [LastName], [Street], [City], [State], [Zip], [Email], [MobilePhone], [DriversLicenseNumber], [DOB], [PolicyId], [PolicyStart], [PolicyEnd])
VALUES (2, 'MicrosoftAccount:c1f2b8ad81b8957b', 'Chris', 'Johnson', '618 Someplace Avenue', 'Cooperstown', 'NY', '13326', 'chrisj-demo@outlook.com', '201-555-3434', 'JT7893022', '3/13/1973', 'CIC3007486372', '7/1/2010', '7/1/2020')

SET IDENTITY_INSERT [dbo].[Customers] OFF
GO


SET IDENTITY_INSERT [dbo].[CustomerVehicles] ON 
GO

DECLARE @imageUrlBase nvarchar(255) = 'https://contosoinsurancestorage.blob.core.windows.net/vehicle-images/vehicle-';


INSERT INTO [dbo].[CustomerVehicles]([Id], [CustomerId], [LicensePlate], [VIN], [ImageURL]) 
VALUES(1, 1, 'TSB-782', 'PFS78364572019836', @imageUrlBase + '983cbb99-44c7-4ddd-800c-aa3360f217d4')

INSERT INTO [dbo].[CustomerVehicles]([Id], [CustomerId], [LicensePlate], [VIN], [ImageURL]) 
VALUES(2, 1, 'CBM-243', 'JYP73640900281632', @imageUrlBase + 'db11eaa3-58a2-453d-ac9b-aea8fefbcffa')

INSERT INTO [dbo].[CustomerVehicles]([Id], [CustomerId], [LicensePlate], [VIN], [ImageURL]) 
VALUES(3, 2, 'PRE-011', 'VLN73644758019273', @imageUrlBase + '032c600e-bea9-46e3-ab4a-ec399a3ff0b1')

INSERT INTO [dbo].[CustomerVehicles]([Id], [CustomerId], [LicensePlate], [VIN], [ImageURL]) 
VALUES(4, 2, 'COM-157', 'POL66738192038908', @imageUrlBase + 'dc716f63-f99b-45f4-87e3-4296e812845c')

SET IDENTITY_INSERT [dbo].[CustomerVehicles] OFF
GO