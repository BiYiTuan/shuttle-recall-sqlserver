SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeStore](
	[Id] [uniqueidentifier] NOT NULL,
	[AssemblyQualifiedName] [varchar](900) NOT NULL,
 CONSTRAINT [PK_TypeStore] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_TypeStore] ON [dbo].[TypeStore] 
(
	[AssemblyQualifiedName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE TABLE [dbo].[KeyStore](
	[MD5Hash] [binary](16) NOT NULL,
	[SHA256Hash] [binary](32) NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_KeyStore] PRIMARY KEY NONCLUSTERED 
(
	[MD5Hash] ASC,
	[SHA256Hash] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[EventStore](
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [int] NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[SequenceNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[DateRegistered] [datetime] NOT NULL,
 CONSTRAINT [PK_EventStore] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Version] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EventStore] ADD  CONSTRAINT [DF_EventStore_DateRegistered]  DEFAULT (getdate()) FOR [DateRegistered]
GO
ALTER TABLE [dbo].[EventStore]  WITH CHECK ADD  CONSTRAINT [FK_EventStore_TypeStore] FOREIGN KEY([TypeId])
REFERENCES [dbo].[TypeStore] ([Id])
GO
ALTER TABLE [dbo].[EventStore] CHECK CONSTRAINT [FK_EventStore_TypeStore]
GO
CREATE TABLE [dbo].[SnapshotStore](
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [int] NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[DateRegistered] [datetime] NOT NULL,
 CONSTRAINT [PK_SnapshotStore] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SnapshotStore] ADD  CONSTRAINT [DF_SnapshotStore_DateRegistered]  DEFAULT (getdate()) FOR [DateRegistered]
GO
ALTER TABLE [dbo].[SnapshotStore]  WITH CHECK ADD  CONSTRAINT [FK_SnapshotStore_TypeStore] FOREIGN KEY([TypeId])
REFERENCES [dbo].[TypeStore] ([Id])
GO
ALTER TABLE [dbo].[SnapshotStore] CHECK CONSTRAINT [FK_SnapshotStore_TypeStore]
GO
