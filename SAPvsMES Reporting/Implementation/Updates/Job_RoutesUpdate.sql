TRUNCATE TABLE [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps]
 
  INSERT INTO [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps]
  SELECT DISTINCT A.[PVRoute_ID] 
      ,[Step_ID]
      ,C.Assembly_ID
      ,B.Translation
      ,[StepOrder]
  FROM [MXCHIM0SQLN01C].[JEMS].[dbo].[QM_PVRouteSteps] A
  INNER JOIN [MXCHIM0SQLN01C].[JEMS].[dbo].[QM_PVRouteAssemblies] C ON C.PVRoute_ID = A.PVRoute_ID
  INNER JOIN [MXCHIM0SQLN01C].[JEMS].[dbo].CR_Text B ON B.Text_ID = A.Descr
  ORDER BY StepOrder