CREATE TEMPORARY TABLE temp_salaries AS
SELECT * FROM salaries
LIMIT 250;

TRUNCATE TABLE salaries;

INSERT INTO salaries SELECT * FROM temp_salaries;

DROP TEMPORARY TABLE IF EXISTS temp_salaries;

SELECT COUNT(*) AS row_count FROM salaries;