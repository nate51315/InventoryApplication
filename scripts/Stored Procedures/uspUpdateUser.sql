GO
/****** Object:  StoredProcedure [dbo].[uspUpdateUser]    Script Date: 2/15/2020 10:09:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[uspUpdateUser]
	@pUserID INT,
	@pPassword NVARCHAR(50),
	@responseMessage NVARCHAR(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @salt UNIQUEIDENTIFIER=NEWID()
	BEGIN TRY

		UPDATE dbo.[Users]
		SET PasswordHash = HASHBYTES('SHA2_512', @pPassword+CAST(@salt AS NVARCHAR(36))),
			Salt = @salt
		WHERE UserID = @pUserID

		SET @responseMessage='Successfully Updated Password'

	END TRY
	BEGIN CATCH
		SET @responseMessage=ERROR_MESSAGE() 
	END CATCH

END
GO