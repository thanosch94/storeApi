IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [Name] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NOT NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        [Country] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [PostalCode] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [Phone1] nvarchar(max) NULL,
        [Phone2] nvarchar(max) NULL,
        [Notes] nvarchar(max) NULL,
        [BirthDay] datetime2 NULL,
        [Occupation] nvarchar(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115162408_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251115162408_Initial', N'9.0.11');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    ALTER TABLE [AspNetRoles] ADD [DateAdded] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    CREATE TABLE [store_affiliate_programs] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_affiliate_programs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    CREATE TABLE [store_brand] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_brand] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    CREATE TABLE [store_import_settings] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [FolderToSave] nvarchar(max) NULL,
        [GetUrl] nvarchar(max) NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_import_settings] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    CREATE TABLE [store_products] (
        [Id] uniqueidentifier NOT NULL,
        [AffiliateId] nvarchar(max) NULL,
        [Sku] nvarchar(max) NULL,
        [Name] nvarchar(255) NOT NULL,
        [Barcode] nvarchar(255) NULL,
        [Description] nvarchar(max) NULL,
        [AffiliateUrl] nvarchar(max) NULL,
        [AffiliateProgramId] uniqueidentifier NULL,
        [IsInStock] bit NULL,
        [BrandId] uniqueidentifier NULL,
        [FeatureImageUrl] nvarchar(max) NULL,
        [Price] decimal(18,2) NULL,
        [DiscountPrice] decimal(18,2) NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_store_products_store_affiliate_programs_AffiliateProgramId] FOREIGN KEY ([AffiliateProgramId]) REFERENCES [store_affiliate_programs] ([Id]),
        CONSTRAINT [FK_store_products_store_brand_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [store_brand] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    CREATE INDEX [IX_store_products_AffiliateProgramId] ON [store_products] ([AffiliateProgramId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    CREATE INDEX [IX_store_products_BrandId] ON [store_products] ([BrandId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251115221200_AddedProductsAndImportSettings'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251115221200_AddedProductsAndImportSettings', N'9.0.11');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251116185921_AddedImportSettingsAndLogs'
)
BEGIN
    EXEC sp_rename N'[store_import_settings].[FolderToSave]', N'Folder', 'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251116185921_AddedImportSettingsAndLogs'
)
BEGIN
    ALTER TABLE [store_import_settings] ADD [ImportType] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251116185921_AddedImportSettingsAndLogs'
)
BEGIN
    ALTER TABLE [store_import_settings] ADD [MatchProperty] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251116185921_AddedImportSettingsAndLogs'
)
BEGIN
    ALTER TABLE [store_import_settings] ADD [UpdateExistingEntities] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251116185921_AddedImportSettingsAndLogs'
)
BEGIN
    CREATE TABLE [store_logs] (
        [Id] uniqueidentifier NOT NULL,
        [LogName] nvarchar(max) NOT NULL,
        [LogType] int NOT NULL,
        [LogOrigin] int NOT NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_logs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251116185921_AddedImportSettingsAndLogs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251116185921_AddedImportSettingsAndLogs', N'9.0.11');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251118204655_AddedMoreImportSettings'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[store_import_settings]') AND [c].[name] = N'MatchProperty');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [store_import_settings] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [store_import_settings] DROP COLUMN [MatchProperty];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251118204655_AddedMoreImportSettings'
)
BEGIN
    ALTER TABLE [store_import_settings] ADD [DbMatchProperty] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251118204655_AddedMoreImportSettings'
)
BEGIN
    ALTER TABLE [store_import_settings] ADD [FileMatchProperty] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251118204655_AddedMoreImportSettings'
)
BEGIN
    ALTER TABLE [store_import_settings] ADD [Title] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251118204655_AddedMoreImportSettings'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251118204655_AddedMoreImportSettings', N'9.0.11');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260502125535_AddedCategoriesAndAnalytics'
)
BEGIN
    CREATE TABLE [store_analytics] (
        [Id] uniqueidentifier NOT NULL,
        [PageId] uniqueidentifier NOT NULL,
        [EntityType] nvarchar(max) NOT NULL,
        [TrackingMode] int NOT NULL,
        [Device] int NOT NULL,
        [Referer] nvarchar(max) NULL,
        [Platform] nvarchar(max) NULL,
        [CountryCode] nvarchar(max) NULL,
        [Country] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [SessionHash] nvarchar(max) NOT NULL,
        [VisitorHash] nvarchar(max) NULL,
        [AffiliateUrlClick] nvarchar(max) NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_analytics] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260502125535_AddedCategoriesAndAnalytics'
)
BEGIN
    CREATE TABLE [store_categories] (
        [Id] uniqueidentifier NOT NULL,
        [ParentId] uniqueidentifier NULL,
        [Name] nvarchar(max) NOT NULL,
        [Slug] nvarchar(max) NOT NULL,
        [ImagePath] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [SeoTitle] nvarchar(max) NULL,
        [MetaDescription] nvarchar(max) NULL,
        [SerialNumber] int NULL,
        [Code] nvarchar(25) NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [IsSeeded] bit NOT NULL,
        [DateAdded] datetime2 NOT NULL,
        [UserAdded] uniqueidentifier NULL,
        [DateUpdated] datetime2 NULL,
        [UserUpdated] uniqueidentifier NULL,
        CONSTRAINT [PK_store_categories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260502125535_AddedCategoriesAndAnalytics'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260502125535_AddedCategoriesAndAnalytics', N'9.0.11');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515175545_AnalyticsChanges'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[store_analytics]') AND [c].[name] = N'EntityType');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [store_analytics] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [store_analytics] DROP COLUMN [EntityType];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515175545_AnalyticsChanges'
)
BEGIN
    ALTER TABLE [store_analytics] ADD [Action] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515175545_AnalyticsChanges'
)
BEGIN
    ALTER TABLE [store_analytics] ADD [Controller] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515175545_AnalyticsChanges'
)
BEGIN
    ALTER TABLE [store_analytics] ADD [Source] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260515175545_AnalyticsChanges'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260515175545_AnalyticsChanges', N'9.0.11');
END;

COMMIT;
GO

