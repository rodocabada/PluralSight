SELECT FKAssemblyMatrix_ID
		FROM [BackFlush].[dbo].[CT_AssemblyLevelsMatrix] WITH (NOLOCK)
		WHERE AssemblyNumber = 'TX1004318-00-G_02'

SELECT A.Customer_ID, A.AssemblyNumber, A.Assembly_ID, B.StepOrder, A.CurrentRouteStep, SUM(A.WIP) AS Qty_MES
	FROM [MESvsSAP].[dbo].[MES_Aging_Birth] A WITH (NOLOCK)
	INNER JOIN [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps] B WITH (NOLOCK)
	ON A.CurrentRouteStep = B.StepName AND A.Assembly_ID = B.Assembly_ID
	WHERE A.Customer_ID = 148
	AND A.AssemblyNumber = 'TX1004318-00-G_02'
	AND NOT EXISTS (SELECT C.StepDescription FROM [MESvsSAP].[dbo].[CT_StepsToDiscard] C WITH (NOLOCK) 
					WHERE C.Equals = 0 AND C.Available = 1 AND A.CurrentRouteStep LIKE '%' + C.StepDescription + '%')
	AND NOT EXISTS (SELECT D.StepDescription FROM [MESvsSAP].[dbo].[CT_StepsToDiscard] D WITH (NOLOCK)
					WHERE D.Equals = 1 AND D.Available = 1 AND A.CurrentRouteStep = D.StepDescription)
	GROUP BY A.Customer_ID, A.AssemblyNumber, A.Assembly_ID, A.CurrentRouteStep, B.StepOrder
	ORDER BY A.Assembly_ID, B.StepOrder


SELECT ROW_NUMBER() OVER (ORDER BY B.StepOrder) AS RowNumber, A.Area, A.OutOfPhase, B.StepOrder, D.AssemblyNumber
FROM [BackFlush].[dbo].[CT_BackFlushPoints] A
INNER JOIN [MESvsSAP].[dbo].[QM_PVRouteAssemblySteps] B
ON (A.Area = B.StepName)
INNER JOIN [BackFlush].[dbo].[CT_AssemblyLevels] C
ON (C.AssemblyLevel = A.Area )
INNER JOIN [BackFlush].[dbo].[CT_AssemblyLevelsMatrix] D
ON (D.FKAssemblyLevel_ID = C.PKAssemblyLevel_ID)
WHERE B.Assembly_ID = 39189
AND A.FKWorkCell_ID = 2
AND C.FKWorkCell_ID = 2
AND D.FKAssemblyMatrix_ID = 2076
AND D.AssemblyNumber IS NOT NULL
AND LTRIM(RTRIM(D.AssemblyNumber)) <> ''
ORDER BY B.StepOrder