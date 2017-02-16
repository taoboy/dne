/*
if (object_id('sp_Paging', 'P') is not null) drop proc sp_paging
*/
create proc [dbo].[sp_Paging](
	@viewName varchar(max),
	@pageNum int = 1,
	@pageSize int = 10,
	@strWhere nvarchar(800) = '',
	@orderBy  varchar(500) = 'TemporaryColumn'
)
as

--declare @viewName varchar(max) = 'dept'
--declare @pageSize int = 10
--declare @pageNum int = 1
--declare @strWhere nvarchar(800) = 'and deptname like ''%ϵ%'''
--declare @orderBy  varchar(50) = 'TemporaryColumn' 

declare @rowStart varchar(10) = (@pageNum-1) * @pageSize 
declare @rowEnd varchar(10) = (@pageNum-1) * @pageSize + @pageSize

declare @sql nvarchar(max)

set @sql = '
select * 
from (	
	select row_number() over (order by TemporaryColumn) as RowNumber,*
	from (
		select top '+@rowEnd+' *,TemporaryColumn=0 from '+@viewName+' where 1=1 
		'+@strWhere+'
		order by '+@orderBy+' 
	) as a
) as b
where RowNumber > '+@rowStart

declare @totalRecord varchar(50)

set @sql = 'select @count = count(*) from '+@viewName+' where 1=1 '+@strWhere
 --select @sql
exec sp_executesql @sql,N'@count int output',@totalRecord output  

set @sql = '
select '+@totalRecord+' as TotalRecord,* 
from (	
	select row_number() over (order by TemporaryColumn) as RowNumber,*
	from (
		select top '+@rowEnd+' *,TemporaryColumn=0 from '+@viewName+' where 1=1 
		'+@strWhere+'
		order by '+@orderBy+' 
	) as a
) as b
where RowNumber > '+@rowStart

 --select @sql

exec sp_executesql @sql


/*

-- example: 

exec sp_paging 'dept',1,10,'and deptname like N''%ϵ%'' ' , 'deptid desc,createtime asc' 



*/

