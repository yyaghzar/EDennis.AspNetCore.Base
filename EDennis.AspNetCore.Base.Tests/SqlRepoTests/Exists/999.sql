﻿use colordb;
declare @Id int = 999;

declare @expected bit = CAST(
   CASE WHEN EXISTS (
	select Id, Name
		from Colors
		where Id = @Id
) THEN 1 ELSE 0 END AS BIT);

exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo',@Id,'Id', @id
exec _.SaveTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo',@Id,'Expected', @expected
exec  _.GetTestJson 'EDennis.Samples.Colors.InternalApi','ColorRepo','Exists','SqlRepo',@Id
