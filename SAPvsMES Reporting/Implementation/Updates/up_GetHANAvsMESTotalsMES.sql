USE [SiteLogix]
GO
/****** Object:  StoredProcedure [dbo].[up_GetHANAvsMESTotalsMES]    Script Date: 8/30/2018 11:07:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[up_GetHANAvsMESTotalsMES] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @PK AS BIGINT

	TRUNCATE TABLE MESvsSAP.dbo.IX_AssemblyTotals

	INSERT INTO IX_eDashboardPlusAgingByProcess2
    EXEC dbo.up_GetEdashBoarPlusAgingByStepForMESvsSAP 'ALL', 0, 20, ''

	--SELECT @PK = PK FROM IX_eDashboardPlusAgingByProcess2
    --WHERE Customer IN ('TESLA','PHOENIX')

	INSERT INTO MESvsSAP.dbo.IX_AssemblyTotals 
	SELECT Assembly_ID, AssemblyNumber, Revision, Step, SUM(Total) as Total
    FROM IX_eDashboardPlusAgingByProcess2
    GROUP BY Assembly_ID, AssemblyNumber, Revision, Step
	ORDER BY AssemblyNumber

	DELETE FROM IX_eDashboardPlusAgingByProcess2
    --WHERE
    --PK = @PK

END
