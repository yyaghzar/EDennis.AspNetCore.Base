﻿use AgencyInvestigatorCheck;

declare @EmployeeId int = 1
declare 
	@Input varchar(max) = 
( 
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-01' DateCompleted
		for json path, include_null_values, without_array_wrapper
);

begin transaction
insert into AgencyInvestigatorCheck(EmployeeId, 
	SysStart, SysEnd, SysUser,
	Status, DateCompleted)
	select
	@EmployeeId EmployeeId,
	'2018-01-01',_.MaxDateTime2(),'moe@tester.org',
	'Fail' Status,
	'2018-12-01' DateCompleted

declare 
@Expected varchar(max) = 
(
	select top 1 * 
	from AgencyInvestigatorCheck
	where EmployeeId = @EmployeeId
	order by DateCompleted desc
	for json path, include_null_values
);

rollback transaction
exec _.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorController', 'GetLastCheck', 'PostAndGet', @EmployeeId, 'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorController', 'GetLastCheck', 'PostAndGet', @EmployeeId, 'EmployeeId', @EmployeeId
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorController', 'GetLastCheck', 'PostAndGet', @EmployeeId, 'Expected', @Expected

exec  _.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'AgencyInvestigatorController', 'GetLastCheck', 'PostAndGet', @EmployeeId
		