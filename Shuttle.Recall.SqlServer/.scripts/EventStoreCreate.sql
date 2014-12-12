SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventStore](
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [int] NOT NULL,
	[AssemblyQualifiedName] [varchar](512) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[SequenceNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[DateRegistered] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventStore] ADD  CONSTRAINT [DF_EventStore_DateRegistered]  DEFAULT (getdate()) FOR [DateRegistered]
GO

CREATE TABLE [dbo].[UniqueHash](
	[IndexType] [int] NOT NULL,
	[Hash] [binary](32) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UniqueHash] PRIMARY KEY NONCLUSTERED 
(
	[IndexType] ASC,
	[Hash] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

