declare @Id int, 
		@Price decimal, 
		@Name Varchar(255),
		@Amount int, 
		@Sum decimal, 
		@Component_Id int, 
		@Sale_Id int
Set @Id = 1
set @Name = 'qwe'
select
	@Amount = Amount
	--[Name]
From Components

where [Name] = @Name 

select * from components
if(isnull(@Amount,-1)= -1)
	--����� ����� ������� ��� �� ������� � ��. �������� ������ �������� � ����������
	insert into Components 
	([Name], Amount, Supply_Id, [Type_Id])
	values
	('qwe',10,1,1)	
else
	--��������� ����� �������������� � ��. �������� ����������
	update Components set Amount = @Amount+1 where [Name] = @Name
   
--select 
--	Id , 
--	Price , 
--	Amount , 
--	Sum , 
--	Component_Id , 
--	Sale_Id
--from
--SalesCompositions