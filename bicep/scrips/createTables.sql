IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' 
      AND TABLE_NAME = 'Location'
)
BEGIN
    CREATE TABLE [dbo].[Location] (
        [Id]                   INT            IDENTITY (1, 1) NOT NULL,
        [Department]           NVARCHAR (100) NULL,
        [DepartmentCode]       INT            NULL,
        [City]                 NVARCHAR (100) NULL,
        [PostalCode]           INT            NULL,
        [Latitude]             FLOAT (53)     NULL,
        [Longitude]            FLOAT (53)     NULL,
        [GenerationTimeMs]     FLOAT (53)     NULL,
        [UtcOffsetSeconds]     INT            NULL,
        [Timezone]             NVARCHAR (50)  NULL,
        [TimezoneAbbreviation] NVARCHAR (10)  NULL,
        [Elevation]            FLOAT (53)     NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END


IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' 
      AND TABLE_NAME = 'Weather'
)
BEGIN
CREATE TABLE [dbo].[Weather] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [LocationId]    INT           NULL,
    [Time]          NVARCHAR (50) NULL,
    [Interval]      INT           NULL,
    [Temperature]   FLOAT (53)    NULL,
    [Windspeed]     FLOAT (53)    NULL,
    [Winddirection] INT           NULL,
    [IsDay]         INT           NULL,
    [Weathercode]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id])
);

END