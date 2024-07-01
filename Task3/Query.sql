WITH DateIntervals AS (
    SELECT
        Id,
        Dt AS StartDate,
        Dt AS EndDate,
        ROW_NUMBER() OVER (PARTITION BY Id ORDER BY Dt) AS rn
    FROM Dates
),
RecursiveIntervals AS (
    SELECT
        Id,
        StartDate,
        EndDate,
        rn
    FROM DateIntervals
    WHERE rn = 1

    UNION ALL

    SELECT
        di.Id,
        CASE 
            WHEN di.StartDate <= ri.EndDate THEN ri.StartDate
            ELSE di.StartDate
        END,
        di.EndDate,
        di.rn
    FROM DateIntervals di
    JOIN RecursiveIntervals ri ON di.Id = ri.Id AND di.rn = ri.rn + 1
)
, MergedIntervals AS (
    SELECT
        Id,
        StartDate,
        MAX(EndDate) AS EndDate
    FROM RecursiveIntervals
    GROUP BY Id, StartDate
)

SELECT DISTINCT
    Id,
    StartDate AS Sd,
    EndDate AS Ed
FROM MergedIntervals
ORDER BY Id, Sd