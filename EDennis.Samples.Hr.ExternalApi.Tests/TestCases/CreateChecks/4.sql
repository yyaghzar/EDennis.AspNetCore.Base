﻿use AgencyInvestigatorCheck;
begin transaction
declare @Id int = 4;
declare @Input varchar(max) = (
select @Id EmployeeId, '2018-12-04' DateCompleted, 'Fail' Status
	for json path, without_array_wrapper
);
insert into AgencyInvestigatorCheck(EmployeeId,DateCompleted,Status) 
	values (4,'2018-12-04','Fail');
declare @Expected varchar(max) = (
	select 
		a.DateCompleted as [AgencyInvestigatorCheck.DateCompleted],
		a.Status as [AgencyInvestigatorCheck.Status],
		b.DateCompleted as [AgencyOnlineCheck.DateCompleted],
		b.Status as [AgencyOnlineCheck.Status],
		c.DateCompleted as [FederalBackgroundCheck.DateCompleted],
		c.Status as [FederalBackgroundCheck.Status],
		d.DateCompleted as [StateBackgroundCheck.DateCompleted],
		d.Status as [StateBackgroundCheck.Status]
		from 
		(select @Id EmployeeId) emps
		cross join
		(select top 1 DateCompleted, Status 
			from AgencyInvestigatorCheck..AgencyInvestigatorCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) a
		cross join 
		(select top 1 DateCompleted, Status 
			from AgencyOnlineCheck..AgencyOnlineCheck
			where EmployeeId = @Id
			order by DateCompleted desc) b
		cross join 
		(select top 1 DateCompleted, Status 
			from FederalBackgroundCheck..FederalBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) c
		cross join 
		(select top 1 DateCompleted, Status 
			from StateBackgroundCheck..StateBackgroundCheck 
			where EmployeeId = @Id
			order by DateCompleted desc) d
		for json path);

rollback transaction
exec _maintenance.ResetIdentities;
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id,'Id',@Id
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id,'Input',@Input
exec _maintenance.SaveTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id,'Expected',@Expected

exec  _maintenance.GetTestJson 'EDennis.Samples.Hr.ExternalApi','EmployeeController','CreateChecks','IntegrationTests',@Id
