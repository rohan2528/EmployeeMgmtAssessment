CREATE TEMPORARY TABLE temp_employees AS
SELECT DISTINCT * FROM employees
LIMIT 250;

TRUNCATE TABLE employees;

INSERT INTO employees SELECT * FROM temp_employees;

DROP TEMPORARY TABLE IF EXISTS temp_employees;

SELECT COUNT(*) AS row_count FROM employees;