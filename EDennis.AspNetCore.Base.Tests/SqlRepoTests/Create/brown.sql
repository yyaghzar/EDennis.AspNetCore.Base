﻿use colordb;
begin transaction
declare @Color varchar(30) = 'brown'
declare @Input varchar(max) = (
select @Color Name
	for json path, without_array_wrapper
);
insert into Colors(name) values (@Color);
declare @Expected varchar(max) = (
	select Id, Name
		from Colors
		for json path
);
rollback transaction
exec _maintenance.ResetIdentities;
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Create','SqlRepo',@Color,'Input',@Input
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Create','SqlRepo',@Color,'Expected',@Expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Create','SqlRepo',@Color
