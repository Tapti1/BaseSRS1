WITH t AS(
SELECT title,COUNT(cat_id) AS num from pride INNER JOIN cat ON
cat.pride_id=pride.pride_id
group by title
)
SELECT title FROM t WHERE num=(SELECT MAX(num) FROM t)