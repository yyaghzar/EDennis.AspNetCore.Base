﻿use FederalBackgroundCheck;

declare @EmployeeId int = 2
declare 
	@Input varchar(max) = 
( 
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-02' DateCompleted
		for json path, include_null_values, without_array_wrapper
);

begin transaction
insert into FederalBackgroundCheck(EmployeeId, Status, DateCompleted)
	select
	@EmployeeId EmployeeId,
	'Fail' Status,
	'2018-12-02' DateCompleted

declare 
@Expected varchar(max) = 
(
	select * from FederalBackgroundCheck
	for json path, include_null_values
);

rollback transaction
exec _maintenance.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'FederalBackgroundController', 'Post', 'PostAndGetMultiple',@EmployeeId,'Input', @Input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi2', 'FederalBackgroundController', 'Post', 'PostAndGetMultiple',@EmployeeId,'Expected', @Expected

exec _.GetTestJson 'EDennis.Samples.Hr.InternalApi2', 'FederalBackgroundController', 'Post', 'PostAndGetMultiple', @EmployeeId
		