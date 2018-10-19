USE [SiteLogix]
GO
/****** Object:  StoredProcedure [dbo].[up_GetHANAvsMES_New]    Script Date: 8/30/2018 11:31:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Diego Richarte>
-- Create date: <01-23-2018>
-- Description:	<HanaVsMES New structure>
-- =============================================
ALTER PROCEDURE [dbo].[up_GetHANAvsMES_New]
	-- Add the parameters for the stored procedure here
	@Customer AS NVARCHAR (MAX)
	,@FromDays AS INT
	,@ToDays AS INT
	,@AssemblyNumberVariable AS NVARCHAR (MAX)
	,@CustomerID AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	CREATE TABLE #TempAssemblyRoutesProgress(
			 id_num int IDENTITY(1,1),
			 Assembly_ID int,
			 StepOrder int,
			 AssemblyLevel varchar(max),
			 PKAssemblyLevel_ID int
             )
  
    CREATE TABLE #TempAssemblyStepOrderWIP(
			 id_num int IDENTITY(1,1),
			 AssemblyNumber varchar(max),
			 Assembly_ID int,
			 MES_Qty int,
			 StepName varchar(max),
			 StepOrder int
             )

    INSERT INTO #TempAssemblyRoutesProgress
    SELECT DISTINCT Assembly_ID, StepOrder, B.AssemblyLevel, B.PKAssemblyLevel_ID 
							  FROM
							  [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps] A
							  INNER JOIN [BackFlush].[dbo].[CT_AssemblyLevels] B ON A.StepName = B.BackflushPoint
							  --JOIN (SELECT StepOrder FROM [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps] )
							  ORDER BY Assembly_ID, StepOrder


    INSERT INTO #TempAssemblyStepOrderWIP
    SELECT  
	    	A.AssemblyNumber, A.Assembly_ID, A.Total as MES_Qty,
		    C.StepName, C.StepOrder 
	    FROM MESvsSAP.dbo.IX_AssemblyTotals A
	    LEFT JOIN [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps] C ON C.Assembly_ID = A.Assembly_ID AND C.StepName = A.Step  
	    ORDER BY A.Assembly_ID

    CREATE TABLE #TempTrueLevel(
			id_num int IDENTITY(1,1),
			AssemblyNumber varchar(max),
			Assembly_ID int,
			FKAssemblyMatrix_ID int,
			MES_Qty int,
			StepName varchar(max),
			StepOrder int,
			ActualLevel_ID int,
			TrueLevel_ID int
            )

    CREATE TABLE #TempSubTable(
			 id_num int IDENTITY(1,1),
			 Assembly_ID int,
			 StepOrder int,
			 AssemblyLevel varchar(max),
			 PKAssemblyLevel_ID int
             ) 

    CREATE TABLE #TempAssemblyMatrixvsAssemblies(
			 id_num int IDENTITY(1,1),
			 FKAssemblyMatrix_ID int,
			 FKAssemblyLevel_ID int,
			 AssemblyNumber varchar(50),
			 FinalAssembly varchar(50)
             )

    CREATE TABLE #TempRealAssemblyToLook(
			 RealNumberLook varchar (50),
			 AssemblyNumber varchar (50),
			 MES_Qty int
             )


    DECLARE @id int,
		@id2 int,
		@count int,
		@count2 int,
		@Assembly_ID int,
		@intStepOrder int, 
		@realAssemblyLevel int,
		@intStepOrderProgress int,
		@intNextStepOrderProgress int,
		@ActualAssemblyLevel varchar(50),
		@TruelAssemblyLevel varchar(50),
		@intMesQty int,
		@AssemblyNumber varchar(50),
		@FKAssemblyMatrix_ID int,
		@AssemblyNumberMatrix varchar(50),
		@FinalAssembly varchar(50),
		@NextAssemblyNumber varchar(50)

    SET @id = 1

    SELECT @count = count(*) from #TempAssemblyStepOrderWIP

    WHILE @id <= @count
    BEGIN
	 SELECT @Assembly_ID = Assembly_ID, @intStepOrder = StepOrder FROM #TempAssemblyStepOrderWIP WHERE id_num = @id
	  INSERT INTO #TempSubTable
	   SELECT Assembly_ID, StepOrder, AssemblyLevel, PKAssemblyLevel_ID FROM #TempAssemblyRoutesProgress WHERE Assembly_ID = @Assembly_ID
	--SELECT * FROM #TempSubTable
	 SELECT @count2 = count(*) FROM #TempSubTable
	  SET @id2 = 1
	   WHILE @id2 <= @count2
	    BEGIN
	      SELECT @realAssemblyLevel = PKAssemblyLevel_ID, @intStepOrderProgress = StepOrder FROM #TempSubTable WHERE id_num = @id2
	      SELECT @intNextStepOrderProgress = StepOrder FROM #TempSubTable WHERE id_num = @id2 + 1
 	       IF ((@intStepOrder >= @intStepOrderProgress) AND ((@intStepOrder < @intNextStepOrderProgress) OR (@intNextStepOrderProgress = 0)))
	          BEGIN
		         INSERT INTO #TempTrueLevel 
		         SELECT A.AssemblyNumber, A.Assembly_ID, C.FKAssemblyMatrix_ID, A.MES_Qty, A.StepName, A.StepOrder, C.FKAssemblyLevel_ID , B.PKAssemblyLevel_ID
		         FROM #TempAssemblyStepOrderWIP A
		         INNER JOIN #TempSubTable B ON A.Assembly_ID = B.Assembly_ID
		         INNER JOIN [BackFlush].[dbo].[CT_AssemblyLevelsMatrix] C ON C.AssemblyNumber = A.AssemblyNumber
		         WHERE B.id_num = @id2 AND A.id_num = @id
		       BREAK
	          END
	         SET @id2 = @id2 + 1
	         SET @intNextStepOrderProgress = 0
	       END
	     SET @id = @id + 1
	     SET @id2 = 0
	     TRUNCATE TABLE #TempSubTable
        END 

   SET @id = 1

   SELECT @count = count(*) from #TempTrueLevel
  
   WHILE @id < @count
    BEGIN
      SELECT @AssemblyNumber = AssemblyNumber, @FKAssemblyMatrix_ID = FKAssemblyMatrix_ID, @intMesQty = MES_Qty,  @ActualAssemblyLevel = ActualLevel_ID, @TruelAssemblyLevel = TrueLevel_ID FROM #TempTrueLevel WHERE id_num = @id
        INSERT INTO #TempAssemblyMatrixvsAssemblies
        SELECT 
	           A.[FKAssemblyMatrix_ID]
	          ,A.[FKAssemblyLevel_ID]
	          ,A.[AssemblyNumber] 
	          ,B.[AssemblyNumber] as FinalAssembly
        FROM [BackFlush].[dbo].[CT_AssemblyLevelsMatrix] A
        INNER JOIN [BackFlush].[dbo].[CT_AssembliesMatrix] B ON A.FKAssemblyMatrix_ID = B.[PKAssemblyMatrix_ID]
        WHERE A.AssemblyNumber <> '' AND A.[FKAssemblyMatrix_ID] = @FKAssemblyMatrix_ID

   SELECT @count2 = count(*) FROM #TempAssemblyMatrixvsAssemblies
   SET @id2 = 1
  --IF (@ActualAssemblyLevel >= @TruelAssemblyLevel)
     WHILE @id2 <= @count2
      BEGIN
        IF (@ActualAssemblyLevel >= @TruelAssemblyLevel)
	      BEGIN
	        SELECT  @AssemblyNumberMatrix = AssemblyNumber, @FinalAssembly = FinalAssembly FROM #TempAssemblyMatrixvsAssemblies WHERE id_num = @id2
	        SELECT  @NextAssemblyNumber = AssemblyNumber FROM #TempAssemblyMatrixvsAssemblies WHERE id_num = @id2 + 1
	          IF (@AssemblyNumberMatrix = @AssemblyNumber)
	            BEGIN
		         IF (@id2 = @count2)
		          BEGIN
		           INSERT INTO #TempRealAssemblyToLook
		           VALUES (@FinalAssembly, @AssemblyNumber, @intMesQty)
		          BREAK
		         END
		      ELSE
		       BEGIN
		        INSERT INTO #TempRealAssemblyToLook
		        VALUES (@NextAssemblyNumber, @AssemblyNumber, @intMesQty)
		        BREAK
		       END
	     END
	    SET @id2 = @id2 + 1
	  END
      ELSE
	    BEGIN
	      SELECT  @AssemblyNumberMatrix = AssemblyNumber, @FinalAssembly = FinalAssembly FROM #TempAssemblyMatrixvsAssemblies WHERE id_num = @id2
	      INSERT INTO #TempRealAssemblyToLook
	 	  VALUES (@AssemblyNumberMatrix, @AssemblyNumber, @intMesQty)
	     SET @id2 = @id2 + 1
	     BREAK
	    END
     END
   SET @id = @id + 1  
  END

--SELECT * FROM #TempRealAssemblyToLook

   SELECT A.MANTR , Floor(SUM(A.TOTALUNRESTRICTED)) as SAP_Qty, CASE WHEN (B.MES_Qty IS NULL) THEN 0 ELSE B.MES_Qty END as MESQty, Floor((CASE WHEN (B.MES_Qty IS NULL) THEN 0 ELSE B.MES_Qty END) - SUM(A.TOTALUNRESTRICTED)) as Delta
		   , (CAST((CAST(ABS(CASE WHEN Floor(SUM(A.TOTALUNRESTRICTED)) > (CASE WHEN (B.MES_Qty IS NULL) THEN 0 ELSE B.MES_Qty END) THEN (Floor(SUM(A.TOTALUNRESTRICTED) - (CASE WHEN (B.MES_Qty IS NULL) THEN 0 ELSE B.MES_Qty END)) * 100 / Floor(SUM(A.TOTALUNRESTRICTED))) ELSE ((Floor(SUM(A.TOTALUNRESTRICTED) - (CASE WHEN (B.MES_Qty IS NULL) THEN 0 ELSE B.MES_Qty END)) * 100 / (CASE WHEN (B.MES_Qty IS NULL) THEN 0 ELSE B.MES_Qty END))) END) as INT)) as VARCHAR)) + '%' as [%Dif]
		   ,(SELECT TOP 1 [TIMESTAMP] FROM [MESvsSAP].[dbo].[SY_HanaPartNumbers] ) as [LastUpdated]
	FROM MESvsSAP.dbo.SY_HanaPartNumbers A
	LEFT JOIN (SELECT [RealNumberLook] ,SUM([MES_Qty]) as MES_Qty
				FROM #TempRealAssemblyToLook
				GROUP BY [RealNumberLook]) B ON B.RealNumberLook = A.MANTR
	   WHERE LGORT NOT IN ('0100', '0199', '01RI', '0200', '0299', '02RI', '02TR', '0300', '03RI', '0400', '0499', '04RI', '0500', '05RI', '0700', '0799', '07RI', 'MRBR', 'RIRS')
	AND WERKS in ('US03', 'TX02')
	AND MATKL = 'TESLA'
	GROUP BY A.MANTR, B.MES_Qty
	UNION
   SELECT A.RealNumberLook,   
	   FLOOR(CASE WHEN (B.TOTALUNRESTRICTED IS NULL) THEN 0 ELSE B.TOTALUNRESTRICTED END) as SAP_Qty
	  ,SUM([MES_Qty]) as MES_Qty
	  ,FLOOR(SUM([MES_Qty]) - CASE WHEN (B.TOTALUNRESTRICTED IS NULL) THEN 0 ELSE B.TOTALUNRESTRICTED END) as Delta
	  ,(CAST((CAST(ABS(CASE WHEN ((Floor(CASE WHEN (B.TOTALUNRESTRICTED IS NULL) THEN 0 ELSE B.TOTALUNRESTRICTED END)) > (SUM([MES_Qty]))) 
				  THEN ((FLOOR(SUM([MES_Qty]) - CASE WHEN (B.TOTALUNRESTRICTED IS NULL) THEN 0 ELSE B.TOTALUNRESTRICTED END)) * 100 / (Floor(CASE WHEN (B.TOTALUNRESTRICTED IS NULL) THEN 0 ELSE B.TOTALUNRESTRICTED END)))
				  ELSE ((FLOOR(SUM([MES_Qty]) - CASE WHEN (B.TOTALUNRESTRICTED IS NULL) THEN 0 ELSE B.TOTALUNRESTRICTED END)) * 100 / (SUM([MES_Qty]))) END) as INT)) AS varchar)) + '%' AS [%Dif]
	  ,(SELECT TOP 1 [TIMESTAMP] FROM [MESvsSAP].[dbo].[SY_HanaPartNumbers] ) as [LastUpdated]
	FROM #TempRealAssemblyToLook A
	LEFT JOIN   (SELECT MANTR, SUM(TOTALUNRESTRICTED) AS TOTALUNRESTRICTED
			   FROM MESvsSAP.dbo.SY_HanaPartNumbers
			   WHERE LGORT NOT IN ('0100', '0199', '01RI', '0200', '0299', '02RI', '02TR', '0300', '03RI', '0400', '0499', '04RI', '0500', '05RI', '0700', '0799', '07RI', 'MRBR', 'RIRS')
					  AND WERKS in ('US03', 'TX02')
					  AND MATKL = 'TESLA'
			   GROUP BY MANTR) B ON B.MANTR = A.RealNumberLook
	WHERE A.RealNumberLook <> ''
	--AND (Select TOP 1 FKCustomer from [MESvsSAP].[dbo].CT_TeslaProgressions where Concat(Model, ProgressSMT1, ProgressSMT2, ProgressTH1, ProgressTLA, '', ProgressTLA2) like '%' + A.RealNumberLook + '%') = @CustomerID
	GROUP BY RealNumberLook, B.TOTALUNRESTRICTED
    
END

