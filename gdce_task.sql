use master
if not exists(select * From sysdatabases WHERE NAME = 'gdce_task')
begin
	create database gdce_task
end
go
use gdce_task
go
 

if (object_id('T_Task_History') is not null)
	drop table T_Task_History;
if OBJECT_ID('T_Task_User') is not null
	drop table T_Task_User;
if OBJECT_ID('T_Task') is not null
	drop table T_Task;
if OBJECT_ID('T_Task_Status') is not null
	drop table T_Task_Status;
if OBJECT_ID('T_User') is not null
	drop table T_User;
if OBJECT_ID('T_Role') is not null
	drop table T_Role;

create table T_Role(
	id int identity(1,1) primary key,
	name nvarchar(255) not null,	
		constraint uq_role_name unique (name),
	descr nvarchar(255),
);

exec sp_addextendedproperty N'MS_Description','角色表', N'user', N'dbo', N'table', N'T_Role', NULL, NULL 
exec sp_addextendedproperty N'MS_Description','角色名',N'user',N'dbo',N'table',N'T_Role',N'column',N'name'
exec sp_addextendedproperty N'MS_Description','角色描述',N'user',N'dbo',N'table',N'T_Role',N'column',N'descr'



create table T_User(
	id varchar(64) primary key default lower(replace(newid(),'-','')),
	account varchar(255) not null,
		constraint uq_user_account unique (account),
	roleId int,		
		constraint fk_user_roleid foreign key (roleid) references T_role(id),
	name nvarchar(255),
	password varchar(64),
	createTime datetime default getdate(),
	phone varchar(50),
	dept nvarchar(255),
	descr nvarchar(255),
	status bit default 1
);



exec sp_addextendedproperty N'MS_Description','用户表', N'user', N'dbo', N'table', N'T_User', NULL, NULL
exec sp_addextendedproperty N'MS_Description','id',N'user',N'dbo',N'table',N'T_User',N'column',N'id'
exec sp_addextendedproperty N'MS_Description','用户帐号',N'user',N'dbo',N'table',N'T_User',N'column',N'account'
exec sp_addextendedproperty N'MS_Description','用户角色',N'user',N'dbo',N'table',N'T_User',N'column',N'roleid'
exec sp_addextendedproperty N'MS_Description','用户名字',N'user',N'dbo',N'table',N'T_User',N'column',N'name'
exec sp_addextendedproperty N'MS_Description','用户密码',N'user',N'dbo',N'table',N'T_User',N'column',N'password'
exec sp_addextendedproperty N'MS_Description','创建时间',N'user',N'dbo',N'table',N'T_User',N'column',N'createTime'
exec sp_addextendedproperty N'MS_Description','部门名字',N'user',N'dbo',N'table',N'T_User',N'column',N'dept'
exec sp_addextendedproperty N'MS_Description','用户描述',N'user',N'dbo',N'table',N'T_User',N'column',N'descr'
exec sp_addextendedproperty N'MS_Description','联系电话',N'user',N'dbo',N'table',N'T_User',N'column',N'phone'
exec sp_addextendedproperty N'MS_Description','状态',N'user',N'dbo',N'table',N'T_User',N'column',N'status'



create table T_Task_Status(
	id int identity(1,1) primary key,
	name nvarchar(255) not null,
		constraint uq_task_status_name unique (name),
	descr nvarchar(255)
);

exec sp_addextendedproperty N'MS_Description','任务状态表', N'user', N'dbo', N'table', N'T_Task_Status', NULL, NULL
exec sp_addextendedproperty N'MS_Description','状态编号',N'user',N'dbo',N'table',N'T_Task_Status',N'column',N'id'
exec sp_addextendedproperty N'MS_Description','状态名字',N'user',N'dbo',N'table',N'T_Task_Status',N'column',N'name'
exec sp_addextendedproperty N'MS_Description','状态描述',N'user',N'dbo',N'table',N'T_Task_Status',N'column',N'descr'
go

insert into T_Task_Status (name,descr)
select 'new','新发起，待审核' union all
select 'pending','已审核发布，待分发' union all
select 'executing','已分发，待完成' union all
select 'UAT','已到期，待验收' union all
select 'accepted','已到期，已验收'union all
select 'denied','已到期，未通过'


create table T_Task(
	id int identity(1,1) primary key,
	userId varchar(64),
		constraint fk_task_user foreign key (userId) references T_User(id),
	title nvarchar(255),
	content nvarchar(4000),
	createTime datetime default getdate(),
	beginTime datetime,
	endTime datetime,
	statusId int,
		constraint fk_task_status foreign key (statusId) references T_Task_Status(id)
);

exec sp_addextendedproperty N'MS_Description','任务信息表', N'user', N'dbo', N'table', N'T_Task', NULL, NULL
exec sp_addextendedproperty N'MS_Description','任务编号',N'user',N'dbo',N'table',N'T_Task',N'column',N'id'
exec sp_addextendedproperty N'MS_Description','发起人',N'user',N'dbo',N'table',N'T_Task',N'column',N'userid'
exec sp_addextendedproperty N'MS_Description','任务标题',N'user',N'dbo',N'table',N'T_Task',N'column',N'title'
exec sp_addextendedproperty N'MS_Description','任务内容',N'user',N'dbo',N'table',N'T_Task',N'column',N'content'
exec sp_addextendedproperty N'MS_Description','创建时间',N'user',N'dbo',N'table',N'T_Task',N'column',N'createTime'
exec sp_addextendedproperty N'MS_Description','开始时间',N'user',N'dbo',N'table',N'T_Task',N'column',N'beginTime'
exec sp_addextendedproperty N'MS_Description','结束时间',N'user',N'dbo',N'table',N'T_Task',N'column',N'endTime'
exec sp_addextendedproperty N'MS_Description','状态标识',N'user',N'dbo',N'table',N'T_Task',N'column',N'statusId'

create table T_Task_User(
	id int identity(1,1) primary key,
	taskId int,
		constraint fk_task_user_taskid foreign key (taskId) references T_Task(id),
	userId varchar(64),
		constraint fk_task_user_uId foreign key (userId) references T_User(id),
		constraint uq_task_user unique (taskId,userid),		
	descr nvarchar(255),
);


exec sp_addextendedproperty N'MS_Description','任务发起角色表', N'user', N'dbo', N'table', N'T_Task_User', NULL, NULL
exec sp_addextendedproperty N'MS_Description','id',N'user',N'dbo',N'table',N'T_Task_User',N'column',N'id'
exec sp_addextendedproperty N'MS_Description','任务编号',N'user',N'dbo',N'table',N'T_Task_User',N'column',N'taskId'
exec sp_addextendedproperty N'MS_Description','角色帐号',N'user',N'dbo',N'table',N'T_Task_User',N'column',N'userId'

create table T_Task_History(
	id int identity(1,1) primary key,
	taskId int,
		constraint fk_task_history_taskId foreign key (taskId) references T_Task(id),
	userName nvarchar(255),
	content nvarchar(4000),
	descr nvarchar(255),
	createTime datetime default getdate(),
	userId varchar(64)
);

exec sp_addextendedproperty N'MS_Description','任务历史表', N'user', N'dbo', N'table', N'T_Task_History', NULL, NULL
exec sp_addextendedproperty N'MS_Description','id',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'id'
exec sp_addextendedproperty N'MS_Description','任务编号',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'taskId'
exec sp_addextendedproperty N'MS_Description','角色名字',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'userName'
exec sp_addextendedproperty N'MS_Description','创建时间',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'createTime'
exec sp_addextendedproperty N'MS_Description','内容',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'content'
exec sp_addextendedproperty N'MS_Description','描述',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'descr'
exec sp_addextendedproperty N'MS_Description','用户id',N'user',N'dbo',N'table',N'T_Task_History',N'column',N'userId'


insert into T_Role (name,descr)
select 'party_A_user','培训处用户' union all
select 'party_A_admin','培训处管理员' union all
select 'party_B_user','邮电学院用户' union all
select 'party_B_admin','邮电学院管理员' 

insert into T_User(account,name,dept,password,roleId)
select '江飞','江飞','培训处','e120012d113ff6ea124a2493453c6dd5',1 union all
select '石茜','石茜','培训处','e120012d113ff6ea124a2493453c6dd5',2 union all
select '吴云飞','吴云飞','网络部','e120012d113ff6ea124a2493453c6dd5',3 union all
select '黄晓明','黄晓明','网络部','e120012d113ff6ea124a2493453c6dd5',3 union all
select '郭树森','郭树森','网络部','e120012d113ff6ea124a2493453c6dd5',4

if  (OBJECT_ID('vw_temp') is not null)
	drop view vw_temp
go

create view vw_temp
as

	select h.taskid,u.userId,h.content,h.descr from T_Task_History h,T_Task_User u
	where h.taskId = u.taskId;
	
go
 
select * from vw_temp;


