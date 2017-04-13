
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/13/2017 13:20:45
-- Generated from EDMX file: D:\Freelancer\SinaiCSP\sinaicsp\Sinaicsp_MVC_V1\Sinaicsp_MVC_V1\Sinaicsp_API\SinaicspDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SinaicspDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ApplicationUserUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_ApplicationUserUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_ApplicationRoleUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_ApplicationRoleUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_SchoolTeacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teachers] DROP CONSTRAINT [FK_SchoolTeacher];
GO
IF OBJECT_ID(N'[dbo].[FK_SchoolStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_SchoolStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentClassStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_StudentClassStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentGradeStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_StudentGradeStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_CityStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_CityStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentInclusion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Inclusions] DROP CONSTRAINT [FK_StudentInclusion];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentAccommodation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accommodations] DROP CONSTRAINT [FK_StudentAccommodation];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subjects] DROP CONSTRAINT [FK_SubjectSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_SchoolSchoolYear]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SchoolYears] DROP CONSTRAINT [FK_SchoolSchoolYear];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentCSP]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CSPs] DROP CONSTRAINT [FK_StudentCSP];
GO
IF OBJECT_ID(N'[dbo].[FK_TeacherTeacherCSP]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeacherCSPs] DROP CONSTRAINT [FK_TeacherTeacherCSP];
GO
IF OBJECT_ID(N'[dbo].[FK_CSPTeacherCSP]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeacherCSPs] DROP CONSTRAINT [FK_CSPTeacherCSP];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectCSP]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CSPs] DROP CONSTRAINT [FK_SubjectCSP];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentLock]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locks] DROP CONSTRAINT [FK_StudentLock];
GO
IF OBJECT_ID(N'[dbo].[FK_SubjectLock]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Locks] DROP CONSTRAINT [FK_SubjectLock];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ApplicationUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationUsers];
GO
IF OBJECT_ID(N'[dbo].[ApplicationRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ApplicationRoles];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[Schools]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Schools];
GO
IF OBJECT_ID(N'[dbo].[Teachers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teachers];
GO
IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO
IF OBJECT_ID(N'[dbo].[StudentClasses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentClasses];
GO
IF OBJECT_ID(N'[dbo].[StudentGrades]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentGrades];
GO
IF OBJECT_ID(N'[dbo].[Cities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cities];
GO
IF OBJECT_ID(N'[dbo].[Inclusions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inclusions];
GO
IF OBJECT_ID(N'[dbo].[Accommodations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accommodations];
GO
IF OBJECT_ID(N'[dbo].[Subjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subjects];
GO
IF OBJECT_ID(N'[dbo].[SchoolYears]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SchoolYears];
GO
IF OBJECT_ID(N'[dbo].[CSPs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CSPs];
GO
IF OBJECT_ID(N'[dbo].[TeacherCSPs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeacherCSPs];
GO
IF OBJECT_ID(N'[dbo].[Locks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locks];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ApplicationUsers'
CREATE TABLE [dbo].[ApplicationUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [LastLogin] datetime  NULL
);
GO

-- Creating table 'ApplicationRoles'
CREATE TABLE [dbo].[ApplicationRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreationDate] datetime  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApplicationUserId] int  NOT NULL,
    [ApplicationRoleId] int  NOT NULL
);
GO

-- Creating table 'Schools'
CREATE TABLE [dbo].[Schools] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [CreatedByUserId] int  NOT NULL
);
GO

-- Creating table 'Teachers'
CREATE TABLE [dbo].[Teachers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [LastLogin] datetime  NULL,
    [IsDeleted] bit  NOT NULL,
    [SchoolId] int  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [CreatedByUserId] int  NOT NULL
);
GO

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Gender] nvarchar(max)  NOT NULL,
    [DOB] datetime  NOT NULL,
    [PrimaryParent] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [State] nvarchar(max)  NOT NULL,
    [MotherCell] nvarchar(max)  NOT NULL,
    [SchoolId] int  NOT NULL,
    [StudentClassId] int  NOT NULL,
    [StudentGradeId] int  NOT NULL,
    [SecondaryParent] nvarchar(max)  NOT NULL,
    [CityId] int  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [FatherCell] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'StudentClasses'
CREATE TABLE [dbo].[StudentClasses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL
);
GO

-- Creating table 'StudentGrades'
CREATE TABLE [dbo].[StudentGrades] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL
);
GO

-- Creating table 'Cities'
CREATE TABLE [dbo].[Cities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL
);
GO

-- Creating table 'Inclusions'
CREATE TABLE [dbo].[Inclusions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(max)  NOT NULL,
    [Classes] nvarchar(max)  NOT NULL,
    [Teacher] nvarchar(max)  NOT NULL,
    [SessionStart] nvarchar(max)  NOT NULL,
    [SessionEnd] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL,
    [StudentId] int  NOT NULL
);
GO

-- Creating table 'Accommodations'
CREATE TABLE [dbo].[Accommodations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL,
    [StudentId] int  NOT NULL,
    [Student_Id] int  NOT NULL
);
GO

-- Creating table 'Subjects'
CREATE TABLE [dbo].[Subjects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL,
    [ParentId] int  NULL
);
GO

-- Creating table 'SchoolYears'
CREATE TABLE [dbo].[SchoolYears] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [CreatedByUserId] int  NOT NULL,
    [SchoolId] int  NOT NULL,
    [IsCurrent] bit  NOT NULL
);
GO

-- Creating table 'CSPs'
CREATE TABLE [dbo].[CSPs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL,
    [StudentId] int  NOT NULL,
    [SubjectId] int  NOT NULL,
    [Materials] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TeacherCSPs'
CREATE TABLE [dbo].[TeacherCSPs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeacherId] int  NOT NULL,
    [CSPId] int  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL
);
GO

-- Creating table 'Locks'
CREATE TABLE [dbo].[Locks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [CreatedByUserId] int  NOT NULL,
    [StudentId] int  NOT NULL,
    [SubjectId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ApplicationUsers'
ALTER TABLE [dbo].[ApplicationUsers]
ADD CONSTRAINT [PK_ApplicationUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ApplicationRoles'
ALTER TABLE [dbo].[ApplicationRoles]
ADD CONSTRAINT [PK_ApplicationRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Schools'
ALTER TABLE [dbo].[Schools]
ADD CONSTRAINT [PK_Schools]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [PK_Teachers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentClasses'
ALTER TABLE [dbo].[StudentClasses]
ADD CONSTRAINT [PK_StudentClasses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentGrades'
ALTER TABLE [dbo].[StudentGrades]
ADD CONSTRAINT [PK_StudentGrades]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [PK_Cities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Inclusions'
ALTER TABLE [dbo].[Inclusions]
ADD CONSTRAINT [PK_Inclusions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Accommodations'
ALTER TABLE [dbo].[Accommodations]
ADD CONSTRAINT [PK_Accommodations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [PK_Subjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SchoolYears'
ALTER TABLE [dbo].[SchoolYears]
ADD CONSTRAINT [PK_SchoolYears]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CSPs'
ALTER TABLE [dbo].[CSPs]
ADD CONSTRAINT [PK_CSPs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TeacherCSPs'
ALTER TABLE [dbo].[TeacherCSPs]
ADD CONSTRAINT [PK_TeacherCSPs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Locks'
ALTER TABLE [dbo].[Locks]
ADD CONSTRAINT [PK_Locks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ApplicationUserId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_ApplicationUserUserRole]
    FOREIGN KEY ([ApplicationUserId])
    REFERENCES [dbo].[ApplicationUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationUserUserRole'
CREATE INDEX [IX_FK_ApplicationUserUserRole]
ON [dbo].[UserRoles]
    ([ApplicationUserId]);
GO

-- Creating foreign key on [ApplicationRoleId] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_ApplicationRoleUserRole]
    FOREIGN KEY ([ApplicationRoleId])
    REFERENCES [dbo].[ApplicationRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ApplicationRoleUserRole'
CREATE INDEX [IX_FK_ApplicationRoleUserRole]
ON [dbo].[UserRoles]
    ([ApplicationRoleId]);
GO

-- Creating foreign key on [SchoolId] in table 'Teachers'
ALTER TABLE [dbo].[Teachers]
ADD CONSTRAINT [FK_SchoolTeacher]
    FOREIGN KEY ([SchoolId])
    REFERENCES [dbo].[Schools]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SchoolTeacher'
CREATE INDEX [IX_FK_SchoolTeacher]
ON [dbo].[Teachers]
    ([SchoolId]);
GO

-- Creating foreign key on [SchoolId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_SchoolStudent]
    FOREIGN KEY ([SchoolId])
    REFERENCES [dbo].[Schools]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SchoolStudent'
CREATE INDEX [IX_FK_SchoolStudent]
ON [dbo].[Students]
    ([SchoolId]);
GO

-- Creating foreign key on [StudentClassId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_StudentClassStudent]
    FOREIGN KEY ([StudentClassId])
    REFERENCES [dbo].[StudentClasses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentClassStudent'
CREATE INDEX [IX_FK_StudentClassStudent]
ON [dbo].[Students]
    ([StudentClassId]);
GO

-- Creating foreign key on [StudentGradeId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_StudentGradeStudent]
    FOREIGN KEY ([StudentGradeId])
    REFERENCES [dbo].[StudentGrades]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentGradeStudent'
CREATE INDEX [IX_FK_StudentGradeStudent]
ON [dbo].[Students]
    ([StudentGradeId]);
GO

-- Creating foreign key on [CityId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_CityStudent]
    FOREIGN KEY ([CityId])
    REFERENCES [dbo].[Cities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityStudent'
CREATE INDEX [IX_FK_CityStudent]
ON [dbo].[Students]
    ([CityId]);
GO

-- Creating foreign key on [StudentId] in table 'Inclusions'
ALTER TABLE [dbo].[Inclusions]
ADD CONSTRAINT [FK_StudentInclusion]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentInclusion'
CREATE INDEX [IX_FK_StudentInclusion]
ON [dbo].[Inclusions]
    ([StudentId]);
GO

-- Creating foreign key on [Student_Id] in table 'Accommodations'
ALTER TABLE [dbo].[Accommodations]
ADD CONSTRAINT [FK_StudentAccommodation]
    FOREIGN KEY ([Student_Id])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentAccommodation'
CREATE INDEX [IX_FK_StudentAccommodation]
ON [dbo].[Accommodations]
    ([Student_Id]);
GO

-- Creating foreign key on [ParentId] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [FK_SubjectSubject]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Subjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectSubject'
CREATE INDEX [IX_FK_SubjectSubject]
ON [dbo].[Subjects]
    ([ParentId]);
GO

-- Creating foreign key on [SchoolId] in table 'SchoolYears'
ALTER TABLE [dbo].[SchoolYears]
ADD CONSTRAINT [FK_SchoolSchoolYear]
    FOREIGN KEY ([SchoolId])
    REFERENCES [dbo].[Schools]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SchoolSchoolYear'
CREATE INDEX [IX_FK_SchoolSchoolYear]
ON [dbo].[SchoolYears]
    ([SchoolId]);
GO

-- Creating foreign key on [StudentId] in table 'CSPs'
ALTER TABLE [dbo].[CSPs]
ADD CONSTRAINT [FK_StudentCSP]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentCSP'
CREATE INDEX [IX_FK_StudentCSP]
ON [dbo].[CSPs]
    ([StudentId]);
GO

-- Creating foreign key on [TeacherId] in table 'TeacherCSPs'
ALTER TABLE [dbo].[TeacherCSPs]
ADD CONSTRAINT [FK_TeacherTeacherCSP]
    FOREIGN KEY ([TeacherId])
    REFERENCES [dbo].[Teachers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeacherTeacherCSP'
CREATE INDEX [IX_FK_TeacherTeacherCSP]
ON [dbo].[TeacherCSPs]
    ([TeacherId]);
GO

-- Creating foreign key on [CSPId] in table 'TeacherCSPs'
ALTER TABLE [dbo].[TeacherCSPs]
ADD CONSTRAINT [FK_CSPTeacherCSP]
    FOREIGN KEY ([CSPId])
    REFERENCES [dbo].[CSPs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CSPTeacherCSP'
CREATE INDEX [IX_FK_CSPTeacherCSP]
ON [dbo].[TeacherCSPs]
    ([CSPId]);
GO

-- Creating foreign key on [SubjectId] in table 'CSPs'
ALTER TABLE [dbo].[CSPs]
ADD CONSTRAINT [FK_SubjectCSP]
    FOREIGN KEY ([SubjectId])
    REFERENCES [dbo].[Subjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectCSP'
CREATE INDEX [IX_FK_SubjectCSP]
ON [dbo].[CSPs]
    ([SubjectId]);
GO

-- Creating foreign key on [StudentId] in table 'Locks'
ALTER TABLE [dbo].[Locks]
ADD CONSTRAINT [FK_StudentLock]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentLock'
CREATE INDEX [IX_FK_StudentLock]
ON [dbo].[Locks]
    ([StudentId]);
GO

-- Creating foreign key on [SubjectId] in table 'Locks'
ALTER TABLE [dbo].[Locks]
ADD CONSTRAINT [FK_SubjectLock]
    FOREIGN KEY ([SubjectId])
    REFERENCES [dbo].[Subjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubjectLock'
CREATE INDEX [IX_FK_SubjectLock]
ON [dbo].[Locks]
    ([SubjectId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------