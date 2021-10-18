CREATE DATABASE QuanLyQuanCom
GO
USE QuanLyQuanCom
GO

-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo

CREATE TABLE TableFood
(
	id int IDENTITY (1,1) PRIMARY KEY,
	DisplayName NVARCHAR (max) not null DEFAULT N'Bàn chưa có tên',
	Status NVARCHAR (100) DEFAULT N'Trống'
)
GO

CREATE Table UserRole
(
	Id int IDENTITY (1,1) PRIMARY KEY,
	DisplayName NVARCHAR (max) not null
)

INSERT INTO UserRole(DisplayName) VALUES (N'Admin')
INSERT INTO UserRole(DisplayName) VALUES (N'Staff')
GO


CREATE Table Account
(
	id int IDENTITY (1,1) PRIMARY KEY,
	UserName NVARCHAR (max) not null,
	DisplayName NVARCHAR(max) not null,
	
	Password NVARCHAR(max) not null default 0,
	IdUserRole int not null,

	foreign KEY (IdUserRole) references UserRole(Id)
)

INSERT INTO Account (UserName, DisplayName, Password, IdUserRole) VAlUES (N'admin1', N'Quản lý 1', N'0', 1)
INSERT INTO Account (UserName, DisplayName, Password, IdUserRole) VAlUES (N'staff1', N'Nhân viên 1', N'0', 2)
GO



CREATE Table FoodCategory
(
	id int IDENTITY (1,1) PRIMARY KEY,
	DisplayName NVARCHAR(max) not null
)

INSERT INTO FoodCategory (DisplayName) VALUES (N'Món chiên')
INSERT INTO FoodCategory (DisplayName) VALUES (N'Món Canh')
INSERT INTO FoodCategory (DisplayName) VALUES (N'Món kho')
INSERT INTO FoodCategory (DisplayName) VALUES (N'Món luộc')
INSERT INTO FoodCategory (DisplayName) VALUES (N'Món xào')
GO




CREATE TABLE Food
(
	id int IDENTITY (1,1) PRIMARY KEY,
	DisplayName NVARCHAR(max) not null,
	idCategory int not null,
	Price FLOAT not null default 0,

	foreign Key (idCategory) references FoodCategory(id)
)


INSERT INTO Food (DisplayName, idCategory, Price) VALUES (N'Cá chép rán', 1, 100000)
INSERT INTO Food (DisplayName, idCategory, Price) VALUES (N'Canh cá chép', 2, 100000)
INSERT INTO Food (DisplayName, idCategory, Price) VALUES (N'Cá chép kho', 3, 100000)
INSERT INTO Food (DisplayName, idCategory, Price) VALUES (N'Cá chép luộc', 4, 100000)
INSERT INTO Food (DisplayName, idCategory, Price) VALUES (N'Rau xuống xào tỏi', 5, 30000)
INSERT INTO Food (DisplayName, idCategory, Price) VALUES (N'Rau xuống xào', 5, 25500)

GO



CREATE PROC USP_GetListFood
AS
BEGIN
SELECT Food.id, Food.DisplayName AS [Tên món], FoodCategory.DisplayName AS [Danh mục], Food.Price AS [Giá tiền]  from Food join FoodCategory ON Food.idCategory = FoodCategory.id
END
GO

EXEC USP_GetListFood



CREAte TABLE Bill
(
	id int IDENTITY (1,1) PRIMARY KEY,
	DateCheckIn Date not null default GETDATE(),
	DateCheckOut Date,
	idTable int not null,
	status int not null DEFAULT 0 -- 1: đã thanh toán, 0 : chưa thanh toán

	foreign KEY (idTable) references TableFood(id)
	 
)


INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status) VALUES (GETDATE(), null, 2, 0)
INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status) VALUES (GETDATE(), null, 4, 0)
INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status) VALUES (GETDATE(), GETDATE(), 3, 1)

ALTER TABLE Bill aDD discount int
UPDATE Bill SET discount = 0
GO


CREATE TABLE BillInfo
(
	id int IDENTITY (1,1) PRIMARY KEY,
	idBill int not null,
	idFood int not null,
	count int not null default 0

	foreign KEY (idBill) references Bill(id),
	foreign KEY (idFood) references Food(id)
)


INSErt INTO BillInfo (idBill, idFood, count) VALUEs (1 , 1, 1)
INSErt INTO BillInfo (idBill, idFood, count) VALUEs (1 , 2, 1)
INSErt INTO BillInfo (idBill, idFood, count) VALUEs (2 , 1, 1)
INSErt INTO BillInfo (idBill, idFood, count) VALUEs (2 , 3, 1)
INSErt INTO BillInfo (idBill, idFood, count) VALUEs (3 , 3, 3)
GO


CREATE PROC USP_Login
@userName NVARCHAR(max), @password NVARCHAR(max)
AS
BEGIN
	SELECT * FROM Account Where UserName = @userName AND Password = @password
END
GO

--thêm bàn
DECLARE @i INT = 0
WHILE  @i < = 20

BEGIN 
	INSERT INTO TableFood (DisplayName) VAlues (N'Bàn ' + CASt(@i AS NVARCHAR(100)))
	set @i = @i + 1
END
GO




CREATE PROC USP_GetTableList
AS SELECT * FROM TableFood
GO

 UPDATE TableFood SET Status = N'Có người' where id = 5
go


SELECT f.DisplayName, bi.count, f.Price, f.Price*bi.count AS [Total Price] FROM BillInfo as bi, Bill as b, Food as f 
WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.status = 0 AND b.idTable = 3
GO



create PROC USP_InsertBill
@idTable int
AS
BEGIN 
	INSERT INTO Bill (DateCheckIn, DateCheckOut, idTable, status, discount)
	VALUES (GETDATE(), null, @idTable, 0, 0)
END
GO




alter PROC USP_InsertBillInfo
@idBill int, @idFood int, @count int
AS
BEGIn
	DECLARE @isExistBillInfo int;
	DECLARE @foodCount int = 1;
	SELECT @isExistBillInfo = id, @foodCount = count from BillInfo WHERE idBill = @idBill AND idFood = @idFood

	if(@isExistBillInfo > 0) 
	BEGIN 
		DECLARE @newCount int = @foodCount + @count
		if(@newCount > 0)
			Update BillInfo SET count = @foodCount + @count WHERE idFood = @idFood
		else
			DELETE BillInfo where idBill = @idBill and idFood = @idFood
	END
	else 
		BEGIN
			INSErt INTO BillInfo (idBill, idFood, count) VALUEs (@idBill , @idFood, @count)
		END	
END
GO

UPDATE Bill SET status =1 where id =1
GO



CREATE TRIGGER UTG_UpdateBillInfo
On BillInfo  for insert , update
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM inserted

	DECLARE @idTable int
	SELECT  @idTable = idTable FROM Bill WHERE id = @idBill and status = 0

	Update TableFood SET Status = N'Có người' WHERE id = @idTable
END
GO

CREATE TRIGGER UTG_UpdateBill
ON Bill for UPDATE
AS 
BEGIN
	DECLARE @idBill int

	SELECT @idBill = id FROM inserted

	DECLARE @idTable int
	SELECT  @idTable = idTable FROM Bill WHERE id = @idBill

	DECLARE @count int = 0

	SELECT @count = count (*) FROm Bill Where idTable = @idTable and status = 0

	IF(@count = 0)
		UPDATE TableFood SET Status = N'Trống' WHERE id = @idTable

eND 
GO





CREATE PROC USP_SwitchTable 
 @idTable1 INT, @idTable2 int
AS
BEGIN
	DECLARE @idFirstBill INT
	DECLARE @idSecondBill INT

	DECLARE @isFirtTablEmty INT = 1
	DECLARE @isSecondTableEmty INT = 1

	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
	SELECT @idSecondBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0

	IF(@idFirstBill IS NULL)
	BEGIN
		
		INSERT dbo.Bill
		(
		    DateCheckIn,
		    DateCheckOut,
		    idTable,
		    status
		)
		VALUES
		(   GETDATE(), -- DateCheckIn - date
		    NULL, -- DateCheckOut - date
		    @idTable1,         -- idTable - int
		    0          -- status - int
		    )
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
		
	END

	SELECT @isFirtTablEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill


	IF(@idSecondBill IS NULL)
	BEGIN
		INSERT dbo.Bill
		(
		    DateCheckIn,
		    DateCheckOut,
		    idTable,
		    status
		)
		VALUES
		(   GETDATE(), -- DateCheckIn - date
		    NULL, -- DateCheckOut - date
		    @idTable2,         -- idTable - int
		    0          -- status - int
		    )
		SELECT @idSecondBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
		
	END

	SELECT @isSecondTableEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSecondBill


	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill

	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE idBill = @idFirstBill

	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)

	DROP TABLE dbo.IDBillInfoTable

	IF (@isFirtTablEmty =0)
	UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable2

	IF (@isSecondTableEmty =0)
	UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable1

END

alter table bill add totalPrice float
GO


CREATE PROC USP_GetListBillByDate
@checkIn DATE, @checkOut date
AS
BEGIN
	SELECT t.DisplayName AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày CheckIn], DateCheckOut AS [Ngày CheckOut] , discount AS [Giảm giá]
 FROM Bill as b, TableFood as t
 WHere DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut and b.status = 1 AND t.id = b.idTable 
END 
GO




Create PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0

	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE UserName = @userName AND Password = @password
	IF(@isRightPass =1)
		BEGIN
			IF(@newPassword = NULL OR @newPassword = '')
				BEGIN
					UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
				END
				ELSE
					UPDATE dbo.Account SET DisplayName = @displayName, Password = @newPassword WHERE UserName = @userName
        END
END
GO 


CREATE TRIGGER UTG_DeleteBillInfo ON BillInFo FOR DELETE
AS
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = deleted.idBill FROM DELETED 

	DECLARE @idTable INT
	SELECT @idTable = idTable FROm Bill WHERE id = @idBill

	DECLARE @count INT = 0

	SELECT @count = COUNT(*) FROM BillInfo as bi, Bill AS b WHERE b.id = idBill AND b.id = @idBill AND STATUS = 0
	
	If(@count = 0)
		UPDATE TableFood SET status = N'Trống' WHERE id = @idTable
END
GO



CREATE TRIGGER UTG_ChangeFoodCategory 
ON FoodCategory for DELETE
as
begin 
	DECLARE @id int
	SELECT @id = id FROM inserted

	DECLARE @count int = 0

	SELECT @count = COUNT (*) FROm Food WHERE idCategory = @id

	IF(@count >= 1)
		UPDATE Food SET idCategory = 8 WHERE idCategory = @id

	DELETE FoodCategory where id = @id
	
end
go



CREATE FUNCTION [dbo].[fuConvertToUnsign1] 
( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) 
AS 
BEGIN 
	IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput 
END
go



CREATE PROC USP_InsertAccount
@userName NVARCHAR(max), @displayName NVARCHAR(max), @userRole NVARCHAR(100)
AS 
BEGIN
	DECLARE @idUserRole int
	SELECT @idUserRole = Account.IdUserRole FROM Account join UserRole ON Account.IdUserRole = UserRole.id AND UserRole.DisplayName = N'@userRole'
	INSERT INTO Account (UserName, DisplayName, IdUserRole) Values (N'@userName', N'@displayName', @idUserRole)

END
GO

CREATE PROC USP_InsertAccounts1
@userName NVARCHAR(max), @displayName NVARCHAR(max), @userRole int
AS 
	INSERT INTO Account (UserName, DisplayName, IdUserRole, Password) Values ( @userName  ,@displayName , @userRole ,N'0')
GO
INSERT INTO Account (UserName, DisplayName, IdUserRole, Password) Values ( N'admin2'  ,N'Quan ly 2' , 2 ,N'0')
go


CREATE PROC USP_GetListBillByDateAndPage
@checkIn DATE, @checkOut date, @page int
AS
BEGIN
	DECLARE @pageRow INT = 10
	DECLARE @selectRow int = @pageRow
	DECLARE @exceptRow int = (@page - 1) * @pageRow

	;WITH BillShow AS ( SELECT b.id, t.DisplayName AS [Tên bàn], b.totalPrice AS [Tổng tiền], DateCheckIn AS [Ngày CheckIn], DateCheckOut AS [Ngày CheckOut] , discount AS [Giảm giá]
	FROM Bill as b, TableFood as t
	WHere DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut and b.status = 1 AND t.id = b.idTable)

	SELECT TOp (@selectRow) * FROM BillShow WHERE id NOT IN (SELECT TOP (@exceptRow) id  FROM BillShow)
	
END 
GO

CREATE PROC USP_GetNumBillByDate
@checkIn DATE, @checkOut date
AS
BEGIN
	SELECT COUNT(*) FROM Bill as b, TableFood as t
    WHere DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut and b.status = 1 AND t.id = b.idTable 
END 
GO   


SELECT id, DisplayName AS [Tên danh mục] FROM FoodCategory
SELECT id, DisplayName AS [Tên bàn], Status AS [Trạng thái] FROM TableFood
GO


SELECT Account.id, Account.UserName AS[Tên đăng nhập], Account.DisplayName AS [Tên hiển thị], UserRole.DisplayName AS[Loại tài khoản] FROm Account join UserRole ON Account.idUserrole = UserRole.id WHERE dbo.fuConvertToUnsign1(Account.DisplayName) like N'%' + dbo.fuConvertToUnsign1(N'{nhân}') + '%' 

SELECT Account.id, Account.UserName AS[Tên đăng nhập], Account.DisplayName AS [Tên hiển thị], UserRole.DisplayName AS[Loại tài khoản] FROm Account join UserRole ON Account.idUserrole = UserRole.id WHERE Account.DisplayName = N'Nhân viên 1'

SELECT * FROM TableFood