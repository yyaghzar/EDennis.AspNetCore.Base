﻿use hr;

declare @firstName varchar(30) = 'Larry'
declare @Input varchar(max) = 
( 
	select @firstName FirstName
	for json path, include_null_values, without_array_wrapper
);


begin transaction
insert into Employee(FirstName)
	values (@firstName);

declare @Expected varchar(max) = 
(
	select * from Employee
	for json path, include_null_values
);
rollback transaction
exec _maintenance.ResetIdentities

exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName,'Input', @input
exec _.SaveTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName,'Expected', @expected

exec _.GetTestJson 'EDennis.Samples.Hr.InternalApi1', 'EmployeeController', 'CreateEmployee','CreateAndGetAll',@firstName
