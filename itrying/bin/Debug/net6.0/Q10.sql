WITH t AS(
SELECT distinct nickname,COUNT(nickname) AS num FROM cat 
group by nickname
)
SELECT nickname FROM t WHERE num=(SELECT MAX(num) FROM t)