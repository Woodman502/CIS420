Create Table Events (
[id] int IDENTITY (1,1) not null,
[text] nvarchar(250) null,
[start_date] datetime not null,
[end_date] datetime not null,
Primary Key (id)
)