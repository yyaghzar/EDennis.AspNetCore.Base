﻿use Colors2;
go
sp_configure 'Show Advanced Options', 1
go
reconfigure
go
sp_configure 'Ad Hoc Distributed Queries', 1
go
reconfigure
go
if object_id('tempdb..#SpResults') is not null drop table #SpResults
go

declare @ProjectName varchar(255) = 'EDennis.Samples.Colors2Api'
declare @ClassName varchar(255) = 'HslController'
declare @MethodName varchar(255) = 'GetJsonColumnFromStoredProcedureAsync'
declare @TestScenario varchar(255) = 'RgbJsonByColorName'
declare @TestCase varchar(255) = 'AliceBlue'

declare @SpName varchar(255) = @TestScenario
declare @ColorName varchar(255) = @TestCase


select * into #SpResults 
    from openrowset('SQLNCLI', 
	  'Server=(localdb)\MSSQLLocalDb;Database=Colors2;Trusted_Connection=yes;',
      'EXEC RgbJsonByColorName ''AliceBlue''')

declare 
	@Expected varchar(max) = 
(
	select [Json] from #SpResults
	for json path, without_array_wrapper
);

exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'SpName', @SpName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'ColorName', @ColorName
exec _.SaveTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase,'Expected', @Expected
exec  _.GetTestJson @ProjectName, @ClassName, @MethodName,@TestScenario,@TestCase

if object_id('tempdb..#SpResults') is not null drop table #SpResults
go
