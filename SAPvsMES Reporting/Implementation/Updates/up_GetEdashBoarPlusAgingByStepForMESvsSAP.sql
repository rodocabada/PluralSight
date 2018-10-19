USE [SiteLogix]
GO
/****** Object:  StoredProcedure [dbo].[up_GetEdashBoarPlusAgingByStepForMESvsSAP]    Script Date: 8/30/2018 11:08:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Diego Richarte>
-- Create date: <26-Mayo-2017>
-- Description:	<GeteDashBoardPlusAigingByStepForMESvsSAP>
-- =============================================
ALTER PROCEDURE [dbo].[up_GetEdashBoarPlusAgingByStepForMESvsSAP] 
	-- Add the parameters for the stored procedure here
	@Customer AS NVARCHAR(MAX)
	,@FromDays AS INT
	,@ToDays AS INT
	,@AssemblyNumber AS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @PK AS BIGINT
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @PK = DATEDIFF(s, '19700101', GETDATE())

	SELECT * FROM
(
SELECT
[PK] = @PK,
 C.[Customer_ID], [Customer], MAR.[Assembly_ID], MAR.[Revision], MAR.[AssemblyNumber],--[CurrentStep] AS [Process], [Route], 
                  [CurrentRouteStep] as [Step]
                  ,SUM([WIP]) AS [Total]
         FROM MESvsSAP.dbo.[MES_Aging_Birth] MAR 
         INNER JOIN eDashboard_CUU.dbo.[MES_Customers] C ON C.[Customer_ID] = MAR.[Customer_ID] 
         LEFT OUTER JOIN eDashboard_CUU.dbo.[CT_MES_ExcludedAssembliesForAgingReport] EA ON (MAR.[AssemblyNumber] LIKE EA.[AssemblyNumber])
		 INNER JOIN [SiteLogix].[dbo].[MES_Assemblies] A ON A.Assembly_ID = MAR.[Assembly_ID] AND A.[Revision] = MAR.[Revision]   
         WHERE [CurrentRouteStep] NOT IN (SELECT [RouteStep] FROM eDashboard_CUU.dbo.[CT_MES_ExcludedRouteStepsForAgingReport])  
         AND [CurrentStep] NOT IN (SELECT [Step] FROM eDashboard_CUU.dbo.[CT_MES_ExcludedStepsForAgingReport])
         AND [Route] NOT IN (SELECT [Route] FROM eDashboard_CUU.dbo.[CT_MES_ExcludedMARouteForAgingReport])
         AND [EPS] = 0  AND EA.[AssemblyNumber] IS NULL
         AND C.[Active] = 1 
		 --AND C.Customer IN ('TESLA')--,'PHOENIX','THERMOFISHER') //Select all available customers
		 AND MAR.AssemblyNumber = CASE WHEN '' = '' THEN MAR.AssemblyNumber ELSE '' END
         GROUP BY C.[Customer_ID], C.[Customer], MAR.[Assembly_ID], MAR.[Revision], MAR.[AssemblyNumber],[CurrentStep],[CurrentRouteStep],[Route]
         --ORDER BY C.[Customer],MAR.[AssemblyNumber],[CurrentStep],[CurrentRouteStep]
) A WHERE Total <> 0
ORDER BY [Customer],[AssemblyNumber],[Step]
END
