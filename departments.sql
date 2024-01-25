CREATE TEMPORARY TABLE temp_dept_emp AS
SELECT * FROM dept_emp
LIMIT 250;

TRUNCATE TABLE dept_emp;

INSERT INTO dept_emp SELECT * FROM temp_dept_emp;

DROP TEMPORARY TABLE IF EXISTS temp_dept_emp;

SELECT COUNT(*) AS row_count FROM dept_emp;